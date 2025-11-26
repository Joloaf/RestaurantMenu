using RestaurantMenu.API.Tests.Fixtures;
using RestaurantMenu.API.Service.DTOs.Models;
using System.Net.Http.Json;

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
        public async Task CanReadMenu()
        {
            var id = await _fixture.AddUsers(_fixture);
            //Arrange
            var newMenu = new MenuModel(0, "Read test manu name", "Read test menu username", "Read test menu theme",
                id);
            var createResp = await _client.PostAsJsonAsync(base_url, newMenu);
            var created = await createResp.Content.ReadFromJsonAsync<MenuModel>();

            //Act
            var getResp = await _client.GetAsync($"{base_url}single?id={created.Id}");

            //Assert
            getResp.EnsureSuccessStatusCode();
            var fetched = await getResp.Content.ReadFromJsonAsync<MenuModel>();
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal(created.Menu_mame, fetched.Menu_mame);
            Assert.Equal(created.User_name, fetched.User_name);
            Assert.Equal(created.Theme, fetched.Theme);
        }

        [Fact]
        public async Task ReturnListOfMenu_ValidResponseCode()
        {
            var newListMenu = new List<MenuModel>();
            var userId = await _fixture.AddUsers(_fixture);
            
            //Arrange
            for (int i = 0; i < 3; i++)
            {
                var newMenu = new MenuModel(0, $"Read test manu name {i}", $"Read test menu username {i}", $"Read test menu theme {i}",
                    userId);
                
                var createResp = await _client.PostAsJsonAsync(base_url, newMenu);
                var created = await createResp.Content.ReadFromJsonAsync<MenuModel>();
                newListMenu.Add(created);
            }

            //Act
            var getResp = await _client.GetAsync($"{base_url}all?userId={userId}");

            //Assert
            getResp.EnsureSuccessStatusCode();
            var fetched = await getResp.Content.ReadFromJsonAsync<List<MenuModel>>();
            Assert.NotNull(fetched);
            Assert.NotEmpty(fetched);
            Assert.Equal(newListMenu.Count, fetched.Count);
            
        }
    }
}