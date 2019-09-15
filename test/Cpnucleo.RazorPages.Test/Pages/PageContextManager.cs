using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace Cpnucleo.RazorPages.Test.Pages
{
    internal class PageContextManager
    {
        public static PageContext CreatePageContext()
        {
            DefaultHttpContext httpContext = new DefaultHttpContext();
            ModelStateDictionary modelState = new ModelStateDictionary();
            ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new PageActionDescriptor(), modelState);
            EmptyModelMetadataProvider modelMetadataProvider = new EmptyModelMetadataProvider();
            ViewDataDictionary viewData = new ViewDataDictionary(modelMetadataProvider, modelState);
            PageContext pageContext = new PageContext(actionContext)
            {
                ViewData = viewData
            };

            return pageContext;
        }
    }
}
