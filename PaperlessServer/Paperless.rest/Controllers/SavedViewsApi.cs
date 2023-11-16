using Microsoft.AspNetCore.Mvc;
using Paperless.rest.Attributes;

namespace Paperless.rest.Controllers
{
    [ApiController]
    public class SavedViewsApi : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/saved_views/")]
        [Consumes("application/json", "text/json", "application/*+json")]
        [ValidateModelState]
        public virtual IActionResult GetUISettings()
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            return StatusCode(200);
        }
    }
}
