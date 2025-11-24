using RestaurantMenu.API.Tests.Fixtures;
using RestaurantMenu.API;
using RestaurantMenu.API.Service.DTOs.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;

namespace RestaurantMenu.API.Tests
{
    internal class RestaurantMenuAPITests : IClassFixture<WebclassFixture<Program>>
    {
        public const string base_url = "/Menu/";

        public readonly HttpClient _client;

        public RestaurantMenuAPITests(WebclassFixture<Program> fixture)
        {
            _client = fixture.CreateClient();
        }

        public async Task MenuPatch_ValidResponseCode()
        {
            //arrange
            var obj = new MenuModel(null, "standard menu", "Sara", "spiderman", 0);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            var res = created.Content.ReadFromJsonAsync<MenuModel>();

            var response = await _client.PutAsJsonAsync(base_url, )
            
            response.
            //act
            
            
            
            //assert
        }
        
    }


}
