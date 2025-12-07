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
        var newListMenu = new List<MenuDto>();
         
        var signedInClient = await _fixture.CreateSignedInClient();
            
        //Arrange
        for (int i = 0; i < 3; i++)
        {
            var newMenu = new MenuDto(0,
                $"Read test manu name {i}",
                $"Skdjfsl",
                Guid.NewGuid().ToString(),
                signedInClient.uid);
                
            var createResp = await signedInClient.client.PostAsJsonAsync(base_url, newMenu);
            var created = await createResp.Content.ReadFromJsonAsync<MenuDto>();
            newListMenu.Add(created);
        }

        //Act
        var getResp = await signedInClient.client.GetAsync($"{base_url}all");

        //Assert
        getResp.EnsureSuccessStatusCode();
        var fetched = await getResp.Content.ReadFromJsonAsync<List<MenuDto>>();
        Assert.NotNull(fetched);
        Assert.NotEmpty(fetched);
        Assert.Equal(newListMenu.Count, fetched.Count);
    }
}