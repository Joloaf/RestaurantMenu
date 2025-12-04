using System.Collections;

namespace RestaurantMenu.API.Tests.TestData;

public class  PatchMenuNameTestData: IEnumerable<object[]>
{
    public List<object[]> _testData;

    public PatchMenuNameTestData()
    {
        SetModelData();
    }

    private void SetModelData()
    {
        List<object[]> models = [];
        for (int i = 0; i < 10000; i++)
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

        var changeObject = builder.WithName(true).Build();
        return  new[] { obj, (object)changeObject.Menu_name };
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