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
    public void ValidatUserName_NotNull(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var res = sut.ValidUserName(name);

        //assert
        Assert.Equal(res, expected);
    }


    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("StandardMenu", true)]
    [InlineData("           ", false)]
    public void MenuStandardName_ShouldBeNotNull(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var res = sut.ValidMenuName(name);

        //assert
        Assert.Equal(res, expected);
        
    }

    [Theory]
    [InlineData("", false)]
    [InlineData(null, false)]
    [InlineData("Hamburger", true)]
    [InlineData("           ", false)]
    public void MenuDishName_ShouldBeNotNull(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var res = sut.ValidDishName(name);

        //assert
        Assert.Equal(res, expected);
    }

    [Theory]
    [InlineData("123", false)]
    [InlineData("Test123", false)]
    [InlineData("TestUserName", true)]
    [InlineData("Test 123", false)]
    public void ValidateUserName_NoNumbersInName(string? name, bool expected)
    {
        // Arrange
        var sut = new Validations();
        // Act
        var actual = sut.ValidUserName(name);

        // Assert
        Assert.Equal(actual, expected);
    }
}


//public static class Use