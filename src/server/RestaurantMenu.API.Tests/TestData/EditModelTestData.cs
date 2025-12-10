using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Tests.TestData;

public class EditModelTestData : IEnumerable<object[]>
{
    public EditModelTestData()
    {
        _testData = GetTestData();
    }
    
    private MenuModelBuilder _builder = new MenuModelBuilder();
    private List<object[]> _testData;
    public List<object[]> GetTestData()
    {
        List<object[]> models = [];
        for (int i = 0; i < 100; i++)
        {
            if (i % 2 == 0)
            {
                foreach (var menuModel in CreateInvalidModel())
                {
                   models.Add([(object)menuModel, (object)false]); 
                }
            }
            models.Add([(object)CreateValidModel(), (object)true]);
        }

        return models;
    }

    private IEnumerable<MenuDto> CreateInvalidModel()
    {
        for(int i = 0; i < 4; i++)
            switch (i)
            {
                case 0:
                    yield return _builder.WithId(false)
                    .WithName(true)
                    .WithUserName(true)
                    .WithThemeName(true)
                    .Build();
                    break;
                case 1:
                    yield return _builder.WithId(true)
                    .WithName(false, Spaces.Ending | Spaces.Start)
                    .WithThemeName(true)
                    .WithUserName(true)
                    .Build();
                    break;
                case 2:
                    yield return _builder.WithId(true)
                    .WithName(true)
                    .WithThemeName(true)
                    .WithUserName(false)
                    .Build();
                    break;
                case 3:
                    yield return _builder.WithId(true)
                        .WithName(true)
                        .WithThemeName(false)
                        .WithUserName(true)
                        .Build();
                    break;
            
        }
    }

    private MenuDto CreateValidModel()
    {
        return _builder.WithId(true)
            .WithThemeName(true)
            .WithUserName(true)
            .WithName(true)
            .Build();
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