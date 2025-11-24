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
    public class RestaurantMenuAPITests : IClassFixture<WebclassFixture<Program>>
    {
        public const string base_url = "/Menu/";
        public string UserGuid { get; set; }

        public readonly HttpClient _client;

        
        public RestaurantMenuAPITests(WebclassFixture<Program> fixture)
        {
            _client = fixture.CreateClient();
            UserGuid = fixture.UserGuid;
        }
        [Fact]
        public async Task MenuAdd_ValidResponseCode()
        {
            var obj = new MenuModel(null, "standard menu", "Sara", "spiderman", this.UserGuid);
            
            //act
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //assert
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"Failed with status {created.StatusCode}: {errorContent}");
            }
            created.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MenuPatch_ValidResponseCode()
        {
            //arrange
            var obj = new MenuModel(null, "standard menu", "Sara", "spiderman", this.UserGuid);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //act
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var res = await created.Content.ReadFromJsonAsync<MenuModel>();
            var edit = new EditMenuModel(res.id.Value, "SpidermanMenu", "Bartek", "Princess", res.user_id);
            
            //assert
            var response = await _client.PatchAsJsonAsync(base_url, edit);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"PATCH Failed with status {response.StatusCode}: {errorContent}");
            }
            response.EnsureSuccessStatusCode();
        }
        
    }


}
