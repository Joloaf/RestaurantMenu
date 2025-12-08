using RestaurantMenu.API.Tests.Fixtures;

namespace RestaurantMenu.API.Tests.TestData;

public class MenuAPIDBTests : IClassFixture<WebclassFixture<Program>>
{
    private readonly WebclassFixture<Program> _fixture;
    private const string url = "/Menu";
    public MenuAPIDBTests(WebclassFixture<Program> fixture)
    {
        _fixture = fixture;
    }
    
    
}