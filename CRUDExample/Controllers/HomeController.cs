using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            IExceptionHandlerPathFeature? exceptionHandlingpathfeatures =
                HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlingpathfeatures != null && exceptionHandlingpathfeatures.Error != null)
            {
                ViewBag.ErrorMessage = exceptionHandlingpathfeatures.Error;
            }
            return View();
        }
    }
}
