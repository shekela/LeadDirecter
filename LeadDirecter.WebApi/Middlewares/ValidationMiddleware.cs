using System.Text.Json;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using LeadDirecter.WebApi.Attributes;

namespace LeadDirecter.WebApi.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Find endpoint and check for [ValidateModel]
            var endpoint = context.GetEndpoint();
            var attribute = endpoint?.Metadata.GetMetadata<ValidateModelAttribute>();

            if (attribute == null)
            {
                await _next(context);
                return;
            }

            // Only read body for POST/PUT/PATCH
            if (context.Request.Method is not ("POST" or "PUT" or "PATCH"))
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();

            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;

            if (string.IsNullOrWhiteSpace(body))
            {
                await _next(context);
                return;
            }

            // Deserialize into specified model type
            var model = JsonSerializer.Deserialize(body, attribute.ModelType, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (model == null)
            {
                await _next(context);
                return;
            }

            // Resolve validator dynamically
            var validatorType = typeof(IValidator<>).MakeGenericType(attribute.ModelType);
            var validator = context.RequestServices.GetService(validatorType);

            if (validator == null)
            {
                await _next(context);
                return;
            }

            // Run async validation
            var validateMethod = validatorType.GetMethod("ValidateAsync", new[] { attribute.ModelType, typeof(CancellationToken) });
            if (validateMethod == null)
            {
                await _next(context);
                return;
            }

            var task = (Task)validateMethod.Invoke(validator, new[] { model, CancellationToken.None })!;
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            var result = resultProperty?.GetValue(task);
            var isValid = (bool)result?.GetType().GetProperty("IsValid")?.GetValue(result)!;

            if (!isValid)
            {
                var errors = (IEnumerable<object>)result?.GetType().GetProperty("Errors")?.GetValue(result)!;

                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(errors));
                return;
            }

            // ✅ Continue pipeline if validation passed
            await _next(context);
        }
    }
}
