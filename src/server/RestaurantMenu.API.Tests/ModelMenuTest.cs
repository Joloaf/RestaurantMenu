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
    [Theory]
    [InlineData(null)]
    public void ValidId_NotNull(int id)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual =  sut.ValidId(id);

        //Assert
        Assert.False(actual);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValidId_NotNegative(int id)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual = sut.ValidId(id);

        //Assert
        Assert.False(actual);
    }

    [Fact]
    public void ValidThemeName_NotNull(string? name)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.False(actual);
    }
    [Theory]
    [InlineData("testName", false)]
    [InlineData("TestName", true)]
    public void ValidUserName_StartWithCapital(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("testName", false)]
    [InlineData("TestName", false)]
    [InlineData("Testname", true)]
    public void ValidUserName_NoInWordCapital(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("Jr", true)]
    [InlineData("Testname", true)]
    [InlineData("J", false)]
    [InlineData("", false)]
    public void ValidUserName_UserNameMinLen(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("   Jr", false)]
    [InlineData("Testname     ", false)]
    [InlineData("    Jr    ", false)]
    [InlineData("Testname", true)]
    public void ValidUserName_UserNameMinLen(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("   Jr", false)]
    [InlineData("Testname     ", false)]
    [InlineData("    Jr    ", false)]
    [InlineData("Tesname", true)]
    public void ValidUserName_UserNameMinLen(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }

    [Theory]
    [InlineData("Jr.", false)]
    [InlineData(",Testname", false)]
    [InlineData("#Jr", false)]
    [InlineData("Tesname", true)]
    [InlineData("Tes//name",  false )]
    [InlineData("Tes/?name",  false )]
    [InlineData("Tes/!name",  false )]
    [InlineData("Tes><>name", false )]
    [InlineData("Tes|}name",  false )]
    [InlineData("Tes{{?/name",false )]
    [InlineData("Tesname🥷",  true )]
    [InlineData("Tesn[ame",   false )]
    [InlineData("Tesn{ame",   false )]
    public void ValidUserName_NoSpecialCharacters(string? name, bool expected)
    {

        var c = ((char)a);


        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(name);

        //assert
        Assert.Equal(actual, expected);
    }
}


public class ISpecialCharacterTestData : IEnumerable<object[]>
{

    public List<object[]> _testData = [];

    public ModelMenuTest()
    {
        _testData = TestDataGenerator();
    }
    public IEnumerator<object[]> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private List<object[]> TestDataGenerator()
    {
       // int a = 65;
       // int b = 90;
       // int l = 97;
       // int le = 122;
    }
}


//public static class Use