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
        public string UserGuid { get; set; }

        public readonly HttpClient _client;

        public RestaurantMenuAPITests(WebclassFixture<Program> fixture)
        {
            _client = fixture.CreateClient();
            UserGuid = fixture.UserGuid;
        }
        public async Task MenuAdd_ValidResponseCode()
        {
            var obj = new MenuModel(null, "standard menu", "Sara", "spiderman", this.UserGuid);
            
            //act
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //assert
            created.EnsureSuccessStatusCode();
        }

        public async Task MenuPatch_ValidResponseCode()
        {
            //arrange
            var obj = new MenuModel(null, "standard menu", "Sara", "spiderman", this.UserGuid);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //act
            var res = await created.Content.ReadFromJsonAsync<MenuModel>();
            var edit = new MenuModel(res.id, "SpidermanMenu", "Bartek", "Princess", res.user_id);
            
            //assert
            var response = await _client.PutAsJsonAsync(base_url, edit);
            response.EnsureSuccessStatusCode();
        }
        
    }


}
