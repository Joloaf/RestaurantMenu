using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Service.Interfaces;
using RestaurantMenu.API.Tests.Fixtures;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Tests.TestData;

public class MenuAPIDBTests : IClassFixture<WebclassFixture<Program>>
{
    /// <summary>
    /// Add all migration and db files into gitignore 
    /// </summary>
    private readonly WebclassFixture<Program> _fixture;
    private const string url = "/Menu";
    public MenuAPIDBTests(WebclassFixture<Program> fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task PostMenu_AssertCreatedInDb()
    {
        //create signed in userclient
        var fixt = await _fixture.CreateSignedInClient();
        var client = fixt.client; //split out the client
        //instantiate creationobj
        var toCreate = new MenuDto(0, "Defaultname", "User", Guid.NewGuid().ToString(), " ");
        
        var created = await client.PostAsJsonAsync("/Menu", toCreate);
        if(!created.IsSuccessStatusCode)
            throw new Exception($"Failed to create menu: {created.ReasonPhrase} " +
                                $"With StatusCode: {created.StatusCode}" +
                                $"With ReasonPhrase: {created.ReasonPhrase}" +
                                $"With StringContent: {await created.Content.ReadAsStringAsync()}");
        //fetch db
        var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();
        //get value from post
        var resultMenu = await created.Content.ReadFromJsonAsync<MenuDto>();
        
        //asserts
        Assert.NotNull(resultMenu);
        Assert.NotNull(resultMenu.Id);
        Assert.Equal(toCreate.Menu_name, resultMenu.Menu_name);
        Assert.Equal(toCreate.Theme, resultMenu.Theme);
        Assert.Equal(toCreate.User_name, resultMenu.User_name);
        
        var menusList = await ctx.Menus.
            Select(x => new MenuDto(x.Id, x.MenuName, x.UserName, x.Theme, " "))
            .ToListAsync();

        Assert.Contains(menusList, m => m.Id == resultMenu.Id 
                                        && m.Menu_name == resultMenu.Menu_name
                                        && m.Theme == resultMenu.Theme
                                        && m.User_name == resultMenu.User_name);
    }

    [Fact]
    public async Task UpdateMenu_AssertUpdateInDb()
    {
        var fixt = await _fixture.CreateSignedInClient();
        var client = fixt.client;
        var clientGuid = fixt.uid;
         var toCreate = new MenuDto(0, "Defaultname", "User", Guid.NewGuid().ToString(), " "); 
        var created = await client.PostAsJsonAsync("/Menu", toCreate);
        if(!created.IsSuccessStatusCode)
            throw new Exception($"Failed to create menu: {created.ReasonPhrase} " +
                                $"With StatusCode: {created.StatusCode}" +
                                $"With ReasonPhrase: {created.ReasonPhrase}" +
                                $"With StringContent: {await created.Content.ReadAsStringAsync()}");
        //fetch db
        var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();
        //get value from post
        var resultMenu = await created.Content.ReadFromJsonAsync<MenuDto>();
        Assert.NotNull(resultMenu);
        
        var updatedMenu = new MenuDto(resultMenu.Id, "Othername", "Otheruser", Guid.NewGuid().ToString(), " ");
        var updated = await client.PatchAsJsonAsync($"/Menu/{resultMenu.Id}", updatedMenu);
        var updatedResultMenu = await updated.Content.ReadFromJsonAsync<MenuDto>();
        
        if(!updated.IsSuccessStatusCode)
            throw new Exception($"Failed to UPDATE menu: {created.ReasonPhrase} " +
                                $"With StatusCode: {created.StatusCode}" +
                                $"With ReasonPhrase: {created.ReasonPhrase}" +
                                $"With StringContent: {await created.Content.ReadAsStringAsync()}");
        
        Assert.NotNull(updatedResultMenu);
        Assert.NotNull(updatedResultMenu.Id);
        
        //looking to assert updatedMenu, updatedResulMenu and db excavated entity are equal in all manner
        Assert.Equal(updatedMenu.Menu_name, updatedResultMenu.Menu_name);
        Assert.Equal(updatedMenu.Theme,     updatedResultMenu.Theme);
        Assert.Equal(updatedMenu.User_name, updatedResultMenu.User_name);
        Assert.Equal(updatedMenu.Id,        updatedResultMenu.Id);
        
        //excavate entity
        var dbEntity = await ctx.Menus.Where(x => x.Id == updatedMenu.Id)
            .Include(x => x.User)
            .Select(x => new MenuDto(x.Id, x.MenuName, x.UserName, x.Theme, x.User.Id))
            .SingleOrDefaultAsync();
        
        
        Assert.Equal(dbEntity.Menu_name, updatedMenu.Menu_name);
        Assert.Equal(dbEntity.Theme, updatedMenu.Theme);
        Assert.Equal(dbEntity.User_name, updatedMenu.User_name);
            //assert no upsert (cus we anal like that)
        Assert.Equal(dbEntity.Id, updatedMenu.Id);
        
        //looking to assert resultMenu, updatedResultMenu are NOT equal in all fields except id
        Assert.NotEqual(dbEntity.Menu_name, resultMenu.Menu_name);
        Assert.NotEqual(dbEntity.Theme, resultMenu.Theme);
        Assert.NotEqual(dbEntity.User_name, resultMenu.User_name);
        Assert.Equal(  dbEntity.Id, resultMenu.Id);
    }

    [Fact]
    public async Task DeleteMenu_AssertDeleteInDb()
    {
        var fixt = await _fixture.CreateSignedInClient();
        var client = fixt.client;
        var clientGuid = fixt.uid;
        
        var toCreate = new MenuDto(0, "Defaultname", "User", Guid.NewGuid().ToString(), " ");
        
        var created = await client.PostAsJsonAsync("/Menu", toCreate);
        if(!created.IsSuccessStatusCode)
            throw new Exception($"Failed to create menu: {created.ReasonPhrase} " +
                                $"With StatusCode: {created.StatusCode}" +
                                $"With ReasonPhrase: {created.ReasonPhrase}" +
                                $"With StringContent: {await created.Content.ReadAsStringAsync()}");
        //fetch db
        var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();
        //get value from post
        var resultMenu = await created.Content.ReadFromJsonAsync<MenuDto>();
        Assert.NotNull(resultMenu); 
        Assert.NotNull(resultMenu.Id);
        
        var deleteResult =  await client.DeleteAsync($"/Menu/{resultMenu.Id.ToString()}");
        if(!deleteResult.IsSuccessStatusCode)
            throw new Exception($"Failed to DELETE menu: {created.ReasonPhrase} " +
                                $"With StatusCode: {created.StatusCode}" +
                                $"With ReasonPhrase: {created.ReasonPhrase}" +
                                $"With StringContent: {await created.Content.ReadAsStringAsync()}");
        var presentInDB = await ctx.Menus.Where(x => x.Id == resultMenu.Id).Where(x=> x.Id == resultMenu.Id).SingleOrDefaultAsync();
        
        Assert.Null(presentInDB);
    }
    
}