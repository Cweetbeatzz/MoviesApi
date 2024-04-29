using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using MoviesApi.Controllers;
using Newtonsoft.Json;
using System.Net;
using static MoviesApi.Model.IMDBModel;

namespace MoviesApi.Tests
{
    public class TestMovies
    {


        [Fact]
        public async Task Search_Title_ReturnsOkResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"Search\":[{\"Title\":\"Avengers\",\"Year\":\"2022\"},{\"Title\":\"Dr Strange\",\"Year\":\"2021\"}]}")
            };
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.omdbapi.com/")
            };

            var controller = new MoviesController(client);

            // Act
            var result = await controller.Search("movie") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var searchResponse = result.Value as SearchResponse;
            Assert.NotNull(searchResponse);
            Assert.Equal(2, searchResponse.Search.Count);
        }

        [Fact]
        public async Task Search_Title_ReturnsBadRequestResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("") 
            };

            
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ThrowsAsync(new HttpRequestException("OMDB API Unavailable"));

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.omdbapi.com/")
            };

            var controller = new MoviesController(client);

            // Act
            var result = await controller.Search("movie") as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("OMDB API Unavailable", result.Value); 
        }


        [Fact]
        public async Task GetMovie_Id_ReturnsOkResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"Title\":\"Avengers\",\"Year\":\"2022\"}")
            };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.omdbapi.com/") 
            };

            var controller = new MoviesController(client);

            // Act
            var result = await controller.GetMovie("movieId") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var movie = result.Value as Movie;
            Assert.NotNull(movie);
            Assert.Equal("Avengers", movie.Title);
        }


        [Fact]
        public async Task GetMovie_Id_ReturnsBadRequestResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError };

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response);

            var client = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://www.omdbapi.com/") 
            };

            var controller = new MoviesController(client);

            // Act
            var actionResult = await controller.GetMovie("movieId");

            // Assert
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

    }
}