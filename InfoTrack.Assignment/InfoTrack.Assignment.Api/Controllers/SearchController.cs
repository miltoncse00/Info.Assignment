using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InfoTrack.Assignment.Application;
using InfoTrack.Assignment.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace InfoTrack.Assignment.Api.Controllers
{
    [Route("api/Search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<ActionResult> Search( SearchInput searchInput)
        {
            try
            {
                var result = await _searchService.Search(searchInput);
                return Ok(result);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}
