using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Paperless.rest.Attributes;
using System;

namespace Paperless.rest.Controllers
{
    [ApiController]
    public class SettingsApi : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Route("/api/ui_settings/")]
        [Consumes("application/json", "text/json", "application/*+json")]
        [ValidateModelState]
        public virtual IActionResult GetUISettings()
        {
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200);

            throw new NotImplementedException();
        }
    }
}
