using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using static MoviesApi.Model.IMDBModel;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {

        private readonly HttpClient _httpClient;
        //#############################################################################################
        //#############################################################################################

        public MoviesController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //#############################################################################################
        //#############################################################################################
        [HttpGet("search/{title}")]
        public async Task<IActionResult> Search(string title)
        {
            try
            {
                var API_KEY = "6a95ad42";
                var IMDB_API = $"http://www.omdbapi.com/?s={title}&apikey={API_KEY}";
                var response = await _httpClient.GetAsync(IMDB_API);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var searchResponse = JsonConvert.DeserializeObject<SearchResponse>(data);
                return Ok(searchResponse);

            }
            catch (HttpRequestException httpex)
            {

                return BadRequest("OMDB API Unavailable");
            }
            catch (Exception)
            {

                return StatusCode(500, "An unknowwn error occured");
            }

        }
        //#############################################################################################
        //#############################################################################################

        [HttpGet("movie/{id}")]
        public async Task<IActionResult> GetMovie(string id)
        {
            try
            {
                var API_KEY = "6a95ad42";
                var IMDB_API = $"http://www.omdbapi.com/?i={id}&apikey={API_KEY}";
                var response = await _httpClient.GetAsync(IMDB_API);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var movie = JsonConvert.DeserializeObject<Movie>(data);
                return Ok(movie);
            }
            catch (HttpRequestException httpex)
            {

                return BadRequest("OMDB API Unavailable");
            }
            catch (Exception)
            {

                return StatusCode(500, "An unknowwn error occured");
            }

        }
    }
}
