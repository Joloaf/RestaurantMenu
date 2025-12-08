using Newtonsoft.Json;
using RestaurantMenu.Core.Models;
using RestaurantMenu.API.Service;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.API.Service.Validations;
using RestaurantMenu.API.Tests.TestData;
using System.Text.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace RestaurantMenu.API.Tests;




public class ModelMenuTest
{
    /// <summary>
    /// Trailing and leading whitespaces will not be allowed.
    /// neither will more than one space between words.
    ///
    /// The menu name needs to have atleast one character now too
    ///
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="expected"></param>
    [Theory]
    [ClassData(typeof(EditModelTestData))] //define
    public void EditModelValid_ModelCombinations(MenuDto dto, bool expected)
    /*
     * Fixed the Generation, issue wasn't actually the generation but the addition of
     * whitespaces characters inside the range of allowed characters
     * (:trustNothingButMe:)
     *
     * the larger issue wasnt the testdatas random nature but the fact that
     *  the reason why the test failed wasn't visible, not a helpful test.
     *      fixed that with a true assertion with a helper message.
     *  
     */
    {
        
       //arrange 
       var sut = new MenuValidator(new Validations());
       
       //act
       var actual = sut.EditModelValid(dto);

       //assert
       Assert.True(actual == expected,
           $"Validation Failed: \n" +
           $"\texpected:\t{expected} |\n \tactual:\t${actual} \n" +
           $"\tTestObject:\t {System.Text.Json.JsonSerializer.Serialize(dto)}");
    }

    public class localModel(MenuDto dto, bool exp, bool act)
    {
        public MenuDto Dto { get; set; } = dto;
        public bool expected { get; set; } = exp;
        public bool actual { get; set; } = act;
    }
    [Fact]
    public void EditModelValid_Inline()
    {
        //arrange 
        var sut = new MenuValidator(new Validations());
       
        //act
        var actual = sut.EditModelValid(new MenuDto(0, "StandardMenu", "Sara",Guid.NewGuid().ToString()+".webp", Guid.NewGuid().ToString()));
       
        //assert
        Assert.True(actual);
    }
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
    [InlineData("Testusername", true)]
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
    public void ValidId_NotNull(int? id)
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

    [Theory]
    [InlineData("ada650cd-8835-40eb-9e15-7dc9d2f362a5.webp", true)]
    [InlineData("e7c265e6-cd31-468b-a619-5c10210866dd.jpeg", true)]
    [InlineData("74e51634-1cfe-4ab3-9a8b-5054b8aa84c6.jpg", true)]
    [InlineData("f72f3330-82e5-4dbe-ac5a-03cd9b25fe84.webp", true)]
    [InlineData("j72f3330-82e5-4dbe-ac5a-03cd9b25fe84.jpeg", false)]

    public void ValidThemeName_IsValidGuid(string? theme, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidThemeName(theme);

        //assert
        Assert.Equal(expected, actual);
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
        var actual  = sut.ValidUserName(name);

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
        var actual  = sut.ValidUserName(name);

        //assert
        Assert.Equal(actual, expected);
    }
    
    [Theory]
    [InlineData("   Jr", false)]
    [InlineData("Testname     ", false)]
    [InlineData("    Jr    ", false)]
    [InlineData("Testname", true)]
    public void ValidUserName_UserNameNoWhiteSpaces(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidUserName(name);

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
    [InlineData("Tesname🌳",  true )]
    [InlineData("Tesname😊",  true )]

    [InlineData("Tesn[ame",   false )]
    [InlineData("Tesn{ame",   false )]
    public void ValidUserName_NoSpecialCharacters(string? name, bool expected)
    {
        //arrange
        var sut = new Validations();

        //act
        var actual  = sut.ValidMenuName(name);

        //assert
        Assert.Equal(actual, expected);
    }
    /// <summary>
    /// Added cus since the last push where whitespace was added to the regex,
    /// the menuname could suddenly be start with a bunch of
    ///  whitespace and the regex would pass, this was also why the generation
    ///  tests were failing... when doing a debug of the tests the testdata showed
    ///   eg: menu_name: "     sdfsdfsdfs" expected: false, actual: true
    /// </summary>
    /// <param name="name"></param>
    /// <param name="expected"></param>
    [Theory]
    [InlineData("sdkfj   ", false)]
    [InlineData("    sdkfj", false)]
    [InlineData("    sdkfj sdkfjs", false)]
    [InlineData("    sdkfj sdkfjs      ", false)]
    [InlineData("sd   kfj", false)]
    [InlineData("sdkfj", true)]
    [InlineData("sdkfj dlkfjs", true)]
    [InlineData("sdkfj dlkfjs ", false)]
    [InlineData("sdkfj dlkfjs   ", false)]
    [InlineData("sdkfj    dlkfjs   ", false)]
    public void MenuName_TrimmedOfWhiteSpaces(string? name,  bool expected)
    {
        
        //arange
        var sut = new Validations();
        
        //assert
        Assert.Equal(expected, sut.ValidMenuName(name));
    }
}