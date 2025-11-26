using System.Collections;

namespace RestaurantMenu.API.Tests.TestData;

public class PatchThemeTestData : IEnumerable<object[]>
{
    public List<object[]> _testData;

    public PatchThemeTestData()
    {
        SetModelData();
    }

    private void SetModelData()
    {
        List<object[]> models = [];
        for (int i = 0; i < 10; i++)
            models.Add(CreateDataModel());
        
        _testData = models;
    }

    private object[] CreateDataModel()
    {
        var builder = new MenuModelBuilder();

        var obj = builder.WithId(true)
            .WithIdentityUserId(true)
            .WithName(true)
            .WithUserName(true)
            .WithThemeName(true)
            .Build();

        var changeObject = builder.WithThemeName(true).Build();
        
        return  new[] { obj, (object)obj.Theme };
    }
    
    public IEnumerator<object[]> GetEnumerator()
    {
        return _testData.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}