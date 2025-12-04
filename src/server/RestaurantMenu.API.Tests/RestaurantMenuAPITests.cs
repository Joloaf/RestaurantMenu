using System.Net;
using RestaurantMenu.API.Tests.Fixtures;
using RestaurantMenu.API.Service.DTOs.Models;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using RestaurantMenu.API.Tests.TestData;

namespace RestaurantMenu.API.Tests
{
    public class RestaurantMenuAPITests : IClassFixture<WebclassFixture<Program>>
    {
        public const string base_url = "/Menu/";
        private readonly WebclassFixture<Program> _fixture;
        
        public RestaurantMenuAPITests(WebclassFixture<Program> fixture)
        {
            _fixture = fixture;
        }
       
         
        [Theory]
        [ClassData(typeof(PatchMenuNameTestData))]
        public async Task Patch_ChangeMenuName(MenuModel model, string expectedMenuNameChange)
        {
            //arrange
            var cli = await _fixture.CreateSignedInClient();
            var _client = cli.client;
            var createModel = new MenuModel(model.Id, model.Menu_name, model.User_name, model.Theme, cli.uid);
            var created = await _client.PostAsJsonAsync(base_url, createModel);
            
            //act
            var result =  await created.Content.ReadFromJsonAsync<MenuModel>();
            var modified = new MenuModel(result.Id, expectedMenuNameChange, result.User_name, result.Theme, result.User_id);
            var actualResponse = await _client.PatchAsJsonAsync(base_url+$"{result.Id}",  modified);
            var actual = await actualResponse.Content.ReadFromJsonAsync<MenuModel>();
            
            //assert
            created.EnsureSuccessStatusCode();
            actualResponse.EnsureSuccessStatusCode();
            Assert.Equal(modified.Id, actual.Id);
            Assert.Equal(modified.User_id, actual.User_id);
            Assert.Equal(expectedMenuNameChange, actual.Menu_name);
            Assert.Equal(modified.Theme, actual.Theme);
        }
        [Theory]
        [ClassData(typeof(PatchThemeTestData))]
        public async Task Patch_ChangeTheme(MenuModel model, string expectedThemeChange)
        {
            //arrange
            var userclient = await _fixture.CreateSignedInClient();
            var _client = userclient.client;
            
            var createModel = new MenuModel(model.Id, model.Menu_name, model.User_name, model.Theme, userclient.uid);
            var created = await _client.PostAsJsonAsync(base_url, createModel);
            
            //act
            var result =  await created.Content.ReadFromJsonAsync<MenuModel>();
            var modified = new MenuModel(result.Id, result.Menu_name, result.User_name, expectedThemeChange, result.User_id);
            var actualResponse = await _client.PatchAsJsonAsync(base_url+$"{result.Id}",  modified);
            var actual = await actualResponse.Content.ReadFromJsonAsync<MenuModel>();
            
            //assert
            created.EnsureSuccessStatusCode();
            actualResponse.EnsureSuccessStatusCode();
            Assert.Equal(modified.Id, actual.Id);
            Assert.Equal(modified.User_id, actual.User_id);
            Assert.Equal(modified.Menu_name, actual.Menu_name);
            Assert.Equal(actual.Theme, expectedThemeChange);
        }
        
        
        [Theory]
        [ClassData(typeof(PatchUserNameTestData))]
        public async Task Patch_ChangesUserName(MenuModel model, string nameChange)
        {
            //arrange
            var userId = await _fixture.CreateSignedInClient();
            var _client = userId.client;
            var createModel = new MenuModel(model.Id, model.Menu_name, model.User_name, model.Theme, userId.uid);
            var created = await _client.PostAsJsonAsync(base_url, createModel);
            
            //act
            var result =  await created.Content.ReadFromJsonAsync<MenuModel>();
            var modified = new MenuModel(result.Id, result.Menu_name, nameChange, result.Theme, result.User_id);
            var actualResponse = await _client.PatchAsJsonAsync(base_url+$"{result.Id}",  modified);
            var actual = await actualResponse.Content.ReadFromJsonAsync<MenuModel>();
            
            //assert
            created.EnsureSuccessStatusCode();
            actualResponse.EnsureSuccessStatusCode();
            Assert.Equal(modified.Id, actual.Id);
            Assert.Equal(nameChange, actual.User_name);
            Assert.Equal(modified.Theme, actual.Theme);
            Assert.Equal(modified.User_id, actual.User_id);
        }
        
        [Fact]
        public async Task MenuAdd_ValidResponseCode()
        {
            var userClient = await _fixture.CreateSignedInClient();
            var _client = userClient.client;
            var obj = new MenuModel(0, "standard menu", "Sara", Guid.NewGuid().ToString(), userClient.uid);

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
            var userClient = await _fixture.CreateSignedInClient();
            var _client = userClient.client;
            //arrange
            var obj = new MenuModel(0, "standard menu", "Sara", Guid.NewGuid().ToString(),  userClient.uid);
            var created = await _client.PostAsJsonAsync(base_url, obj);

            //act
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }

            var res = await created.Content.ReadFromJsonAsync<MenuModel>();
            var edit = new MenuModel(res.Id, "SpidermanMenu", "Bartek", Guid.NewGuid().ToString(), res.User_id);
            
            //assert
            var response = await _client.PatchAsJsonAsync(base_url+$"{res.Id}", edit);
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
            var userClient = await _fixture.CreateSignedInClient();
            var _client = userClient.client;

            //arrange
            var obj = new MenuModel(0, "menu to delete", "Patrick", Guid.NewGuid().ToString(), userClient.uid);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var createdMenu = await created.Content.ReadFromJsonAsync<MenuModel>();
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{createdMenu.Id}");
            
            //assert
            if (!deleteResponse.IsSuccessStatusCode)
            {
                var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                throw new Exception($"DELETE Failed with status {deleteResponse.StatusCode}: {errorContent}");
            }
            deleteResponse.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task MenuDelete_RemoveExistingMenuWithDishesForValidUser()
        {
            var userClient = await _fixture.CreateSignedInClient();
            var _client = userClient.client;

            //arrange
            var obj = new MenuModel(0, "menu to delete", "Patrick", Guid.NewGuid().ToString(), userClient.uid);
            List<DishModel> dishes =
            [
                new DishModel(0, "SpidermanMenu", "Bartek"),
                new DishModel(0, "SpidermanMen2u", "Bartek")
            ];
            
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var createdMenu = await created.Content.ReadFromJsonAsync<MenuModel>();
            var resdish  = await _client.PostAsJsonAsync($"/Dish/{createdMenu.Id.ToString()}", dishes[0]);
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{createdMenu.Id}");
            
            //assert
            resdish.EnsureSuccessStatusCode();
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
            var userClient= await _fixture.CreateSignedInClient(); var _client = userClient.client;

            //arrange
            var nonExistentMenuId = 99999;
            
            //act
            var deleteResponse = await _client.DeleteAsync($"{base_url}{nonExistentMenuId}?userId={userClient.uid}");
            
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
            var userClient = await _fixture.CreateSignedInClient(); var _client = userClient.client;

            //arrange
            var obj = new MenuModel(0, "menu owned by user1", "Patrick", Guid.NewGuid().ToString(), userClient.uid);
            var created = await _client.PostAsJsonAsync(base_url, obj);
            
            if (!created.IsSuccessStatusCode)
            {
                var errorContent = await created.Content.ReadAsStringAsync();
                throw new Exception($"POST Failed with status {created.StatusCode}: {errorContent}");
            }
            var createdMenu = await created.Content.ReadFromJsonAsync<MenuModel>();

            var unsignedInClient = _fixture.CreateClient();
            //act
            var deleteResponse = await unsignedInClient.DeleteAsync($"{base_url}{createdMenu.Id}?userId={userClient.uid}");
            
            //assert
            Assert.Equal(System.Net.HttpStatusCode.MethodNotAllowed, deleteResponse.StatusCode);
        }
        
        [Fact]
        public async Task CanReadMenu()
        {
            var userClient = await _fixture.CreateSignedInClient();
            var _client = userClient.client;
            //Arrange
            var newMenu = new MenuModel(0, "Readtestmanuname", "Readtestmenuusername", Guid.NewGuid().ToString(),
                userClient.uid);
            var createResp = await _client.PostAsJsonAsync(base_url, newMenu);
            var created = await createResp.Content.ReadFromJsonAsync<MenuModel>();

            //Act
            var getResp = await _client.GetAsync($"{base_url}single/{created.Id}");

            //Assert
            getResp.EnsureSuccessStatusCode();
            var fetched = await getResp.Content.ReadFromJsonAsync<MenuModel>();
            Assert.NotNull(fetched);
            Assert.Equal(created.Id, fetched.Id);
            Assert.Equal(created.Menu_name, fetched.Menu_name);
            Assert.Equal(created.User_name, fetched.User_name);
            Assert.Equal(created.Theme, fetched.Theme);
        }

    }
}