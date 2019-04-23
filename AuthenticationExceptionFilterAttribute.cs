using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Web.Http;
using System.Web.Http.Filters;

namespace MyWebsite
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is AuthenticationException)
            {
                actionExecutedContext.Response = GetResponse(HttpStatusCode.Unauthorized, actionExecutedContext.Exception.Message);
            } else if (actionExecutedContext.Exception is AuthorizationException)
            {
                actionExecutedContext.Response = GetResponse(HttpStatusCode.Forbidden, actionExecutedContext.Exception.Message);
            }
        }

        private HttpResponseMessage GetResponse(HttpStatusCode status, string message)
        {
            return new HttpResponseMessage
            {
                StatusCode = status,
                Content = new ObjectContent(typeof(ErrorResponse),
                    new ErrorResponse {ErrorMessage = message}, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
        }
    }
}