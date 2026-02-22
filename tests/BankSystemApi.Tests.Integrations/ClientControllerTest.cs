using BankSystemApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemApi.Tests.Integrations
{
    public class ClientControllerTest:IClassFixture<WebApplicationFactory<IApiMarker>>,IAsyncLifetime
    {
        //arrange
        
       
        private readonly HttpClient _httpClient;

        public ClientControllerTest(WebApplicationFactory<IApiMarker> appFactory)
        {
            _httpClient = appFactory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsOk_WhenClientExist()
        {
            //act
            var responseMessage= await _httpClient.GetAsync("/api/client/22222222-2222-2222-2222-222222222222");
            //assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            var text = await responseMessage.Content.ReadAsByteArrayAsync();
        }

        
        [Fact]
        public async Task Get_ReturnsOk_WhenClientExist3()
        {
            //act
            var responseMessage = await _httpClient.GetAsync("/api/client/22222222-2222-2222-2222-222222222222");
            //assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            //i have to give the type when i want as json format 
            //for 400 the type must be of <ValidationProblemDetails> and in 200 it should be <theDtoClass>
            var text = await responseMessage.Content.ReadFromJsonAsync<ClientDto>();
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenClientDoesNotExist()
        {
            //act
            var responseMessage = await _httpClient.GetAsync($"/api/client/{Guid.NewGuid()}");
            //assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var problem = await responseMessage.Content.ReadFromJsonAsync<ValidationProblemDetails>();
            problem.Status.Should().Be(404);
        }

        [Theory]
        [ClassData(typeof(ClassData))]
        public async Task Get_ReturnsOk_WhenClientExist2(string guidAsString)
        {
            //act
            var responseMessage = await _httpClient.GetAsync($"/api/client/{guidAsString}");
            //assert
            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public async Task InitializeAsync()
        {
            await Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public class ClassData : IEnumerable<Object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { "22222222-2222-2222-2222-222222222222" };
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
