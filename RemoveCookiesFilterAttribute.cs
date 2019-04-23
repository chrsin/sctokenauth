using System.Diagnostics.CodeAnalysis;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using ActionFilterAttribute = System.Web.Http.Filters.ActionFilterAttribute;

namespace MyWebsite
{
    [ExcludeFromCodeCoverage]
    public class RemoveCookiesFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            IHttpController controller = actionExecutedContext.ActionContext.ControllerContext.Controller;

            if (controller != null)
            {
                string ns = controller.GetType().Namespace;
                if(ns != null && ns.StartsWith("MyWebsite"))
                    HttpContext.Current.Response.Cookies.Clear();
            }
        }
    }
}