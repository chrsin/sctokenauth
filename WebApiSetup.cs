using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Sitecore.Pipelines;
using TdcErhverv.Website.Code.Pipelines;

namespace MyWebsite
{
    [ExcludeFromCodeCoverage]
    public class CustomMvcRoutes
    {
        public void Process(PipelineArgs args)
        {
            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Filters.Add(new AuthenticationExceptionFilterAttribute()); //This handles authentication exceptions and creates a proper http response.
            config.Filters.Add(new RemoveCookiesFilterAttribute()); //This removes any cookies set if an endpoint in our namespace is being hit.
        }
    }
}