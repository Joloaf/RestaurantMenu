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
            var id = await _fixture.AddUsers(_fixture);
            //arrange
            var obj = new MenuModel(0, "standard menu", "Sara", "spiderman", id);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            //act
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var res = await created.Content.ReadFromJsonAsync<MenuModel>();
            var edit = new MenuModel(res.Id, "SpidermanMenu", "Bartek", "Princess", res.User_id);
            
            //assert
            var response = await _client.PatchAsJsonAsync(base_url, edit);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"PATCH Failed with status {response.StatusCode}: {errorContent}");
            }
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MenuDelete_RemoveExistingMenuForValidUser_ReceiveValidResponseCode()
        {
            var userId = await _fixture.AddUsers(_fixture);

            //arrange
            var obj = new MenuModel(0, "menu to delete", "Patrick", "spongebob", userId);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var createdMenu = await created.Content.ReadFromJsonAsync<MenuModel>();
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{createdMenu.Id}?userId={userId}");
            
            //assert
            if (!deleteResponse.IsSuccessStatusCode)
            {
                var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"DELETE Failed with status {deleteResponse.StatusCode}: {errorContent}");
            }
            deleteResponse.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MenuDelete_NotFound_NonExistentMenu()
        {
            var userId = await _fixture.AddUsers(_fixture);

            //arrange
            var nonExistentMenuId = 99999;
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{nonExistentMenuId}?userId={userId}");
            
            //assert
            if (deleteResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"Expected NotFound but got {deleteResponse.StatusCode}: {errorContent}");
            }
            Assert.Equal(System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }

        [Fact]
        public async Task MenuDelete_NotFound_WrongUser()
        {
            var userId1 = await _fixture.AddUsers(_fixture);

            //arrange
            var obj = new MenuModel(0, "menu owned by user1", "Patrick", "spongebob", userId1);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var createdMenu = await created.Content.ReadFromJsonAsync<MenuModel>();

            var wrongUserId = "wrong-user-id-123";
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{createdMenu.Id}?userId={wrongUserId}");
            
            //assert
            if (deleteResponse.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"Expected NotFound but got {deleteResponse.StatusCode}: {errorContent}");
            }
            Assert.Equal(System.Net.HttpStatusCode.NotFound, deleteResponse.StatusCode);
        }
        
    }


}
