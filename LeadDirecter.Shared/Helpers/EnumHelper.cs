using LeadDirecter.Shared.Enums;

namespace LeadDirecter.Shared.Helpers
{
    public static class EnumHelper
    {
        public static string ConvertContentTypeToString(ContentType v) => v switch
        {
            ContentType.ApplicationJson => "application/json",
            ContentType.FormUrlEncoded => "application/x-www-form-urlencoded",
            ContentType.MultipartFormData => "multipart/form-data",
            ContentType.TextPlain => "text/plain",
            ContentType.ApplicationXml => "application/xml",
            ContentType.TextXml => "text/xml",
            _ => "application/json" // default fallback
        };

        public static ContentType ConvertStringToContentType(string v) => v switch
        {
            "application/json" => ContentType.ApplicationJson,
            "application/x-www-form-urlencoded" => ContentType.FormUrlEncoded,
            "multipart/form-data" => ContentType.MultipartFormData,
            "text/plain" => ContentType.TextPlain,
            "application/xml" => ContentType.ApplicationXml,
            "text/xml" => ContentType.TextXml,
            _ => ContentType.ApplicationJson // default fallback
        };
    }
}
