using Microsoft.AspNetCore.Mvc;
using FizzWare.NBuilder;
using System.Linq;

namespace Paperless.rest.Controllers;

[ApiController]
[Route("/api/search/")]
public partial class SearchController : ControllerBase
{
    /*
    [HttpGet("autocomplete/", Name = "AutoComplete")]
    public IActionResult AutoComplete([FromQuery] string term, [FromQuery] int limit)
    {
        var generator = new RandomGenerator();

        return Ok(
            Enumerable.Range(0, 10)
                .Select(el => generator.Phrase(10)
        ));
    }
    */
}
