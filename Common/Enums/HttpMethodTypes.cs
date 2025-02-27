using Microsoft.AspNetCore.Http;

namespace Common.Enums
{
    public enum HttpMethodTypes
    {
        Get = 1, 
        Post = 2, 
        Put = 3, 
        Delete = 4
    }

    public static class HttpMethodTypesExtensions
    {
        public static HttpMethodTypes GetRequestMethodType(this HttpRequest request)
        {
            switch (request.Method)
            {
                case "POST":
                    return HttpMethodTypes.Post;
                case "GET":
                    return HttpMethodTypes.Get;
                case "PUT":
                    return HttpMethodTypes.Put;
                case "DELETE":
                    return HttpMethodTypes.Delete;
                default:
                    return HttpMethodTypes.Get;
            }
        }
    }
}
