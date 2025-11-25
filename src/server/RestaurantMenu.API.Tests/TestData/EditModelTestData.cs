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
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
                models.Add([(object)CreateInvalidModel(), (object)false]);
            models.Add([(object)CreateValidModel(), (object)true]);
        }

        return models;
    }

    private MenuModel CreateInvalidModel()
    {
        var rand = Random.Shared.Next(0, 5);
        switch (rand)
        {
            case 0:
                return _builder.WithId(false)
                .WithIdentityUserId(true)
                .WithName(true)
                .WithUserName(true)
                .WithThemeName(true)
                .Build();
            case 1:
                return _builder.WithId(true)
                .WithIdentityUserId(false)
                .WithName(true)
                .WithUserName(true)
                .WithThemeName(true)
                .Build();
            case 2:
                return _builder.WithId(true)
                .WithIdentityUserId(true)
                .WithName(false)
                .WithThemeName(true)
                .WithUserName(true)
                .Build();
            
            case 3:
                return _builder.WithId(true)
                .WithIdentityUserId(true)
                .WithName(true)
                .WithThemeName(true)
                .WithUserName(false)
                .Build();
            case 4:
                return _builder.WithId(true)
                    .WithIdentityUserId(true)
                    .WithName(true)
                    .WithThemeName(false)
                    .WithUserName(true)
                    .Build();
            
        }

        return null;
    }

    private MenuModel CreateValidModel()
    {
        return _builder.WithId(true)
            .WithIdentityUserId(true)
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