namespace LeadDirecter.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : Attribute
    {
        public Type ModelType { get; }

        public ValidateModelAttribute(Type modelType)
        {
            ModelType = modelType;
        }
    }
}