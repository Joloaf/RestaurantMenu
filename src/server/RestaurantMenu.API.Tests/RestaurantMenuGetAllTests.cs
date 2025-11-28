using System.Net.Http.Json;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Tests.Fixtures;

namespace RestaurantMenu.API.Tests;

public class RestaurantMenuGetAllTests : IClassFixture<WebclassFixture<Program>>
{
    public const string base_url = "/Menu/";
    private readonly WebclassFixture<Program> _fixture;
    public readonly HttpClient _client;
    public RestaurantMenuGetAllTests(WebclassFixture<Program> fixture)
    {
        _client = fixture.CreateClient();
        _fixture = fixture;
    }
        
    [Fact]
    public async Task ReturnListOfMenu_ValidResponseCode()
    {
        var newListMenu = new List<MenuModel>();
         
        var userId = await _fixture.AddUsers();
            
        //Arrange
        for (int i = 0; i < 3; i++)
        {
            var newMenu = new MenuModel(0,
                $"Read test manu name {i}",
                $"Skdjfsl",
                Guid.NewGuid().ToString(),
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