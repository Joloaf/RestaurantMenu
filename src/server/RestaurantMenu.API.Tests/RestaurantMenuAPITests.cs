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
using Microsoft.AspNetCore.Hosting;

namespace RestaurantMenu.API.Tests
{
    public class RestaurantMenuAPITests : IClassFixture<WebclassFixture<Program>>
    {
        public const string base_url = "/Menu/";
        private readonly WebclassFixture<Program> _fixture;

        public readonly HttpClient _client;

        public RestaurantMenuAPITests(WebclassFixture<Program> fixture)
        {
            _client = fixture.CreateClient();
            _fixture = fixture;
        }
        [Fact]
        public async Task MenuAdd_ValidResponseCode()
        {
            var id = await _fixture.AddUsers(_fixture);
            var obj = new MenuModel(0, "standard menu", "Sara", "spiderman", id);
            
            //act
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //assert
            created.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task MenuPatch_ValidResponseCode()
        {
            var id = await _fixture.AddUsers(_fixture);
            //arrange
            var obj = new MenuModel(0, "standard menu", "Sara", "spiderman", id);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //act
            var res = await created.Content.ReadFromJsonAsync<MenuModel>();
            var edit = new MenuModel(res.Id, "SpidermanMenu", "Bartek", "Princess", res.User_id);
            
            //assert
            var response = await _client.PatchAsJsonAsync(base_url, edit);
            response.EnsureSuccessStatusCode();
        }
        
    }


}
