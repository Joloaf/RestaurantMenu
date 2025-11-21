using RestaurantMenu.Core.Models;
using RestaurantMenu.API.Service;

namespace RestaurantMenu.API.Tests;
public class ModelMenuTest
{

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("Bartek", true)]
    [InlineData("           ", false)]
    public void ValidatUsername_NotNull(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var res = sut.ValidUsername(name);

        //assert
        Assert.Equal(res, expected);
    }


    [Fact]
    public void MenuStandardName_ShouldBeNotNull()
    {
        
    }
}


//public static class Use