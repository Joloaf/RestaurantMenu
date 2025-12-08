using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantMenu.API.Service.DTOs.Models;
using RestaurantMenu.API.Tests.Fixtures;
using RestaurantMenu.Core.Models;
using RestaurantMenu.Infrastructure.Data;

namespace RestaurantMenu.API.Tests;

public class DishAPIDBTests : IClassFixture<WebclassFixture<Program>>
{
   public const string baseUrl = "/Dish";
   private readonly WebclassFixture<Program> _fixture;
   
   public DishAPIDBTests(WebclassFixture<Program> fixture)
   {
      _fixture = fixture;
   }

   
   
   [Theory]
   [MemberData(nameof(CreateDishDto))]
   public async Task Post_AssertCreated(DishDto dishDto)
   {
      var fixt = await _fixture.CreateSignedInClient();
      var client = fixt.client;
      var res = await client.PostAsJsonAsync(baseUrl+"/"+ (await CreateMenu(client)).ToString(),  dishDto);
      
      var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();

      if(!res.IsSuccessStatusCode)
         throw new HttpRequestException($"POST FAILED: {res.ReasonPhrase}," +
                                        $"  StatusCode: {res.StatusCode}," +
                                        $" ReasonPhrase: {res.ReasonPhrase}, "+
                                        $" Content: {res.Content.ReadAsStringAsync().Result}");

      var creation = await res.Content.ReadFromJsonAsync<DishDto>();
      
      Assert.NotNull(creation);
      Assert.NotEqual(0, creation.Id);
      var dbList = await ctx.Dishes.ToListAsync();
      
      
      Assert.NotNull(creation);
      Assert.Equal(dishDto.DishName, creation.DishName);
      Assert.Equal(dishDto.DishPicture, creation.DishPicture);
      Assert.Contains(new DishDto(creation.Id, dishDto.DishName, dishDto.DishPicture),
         dbList.Select(x=> new DishDto(x.Id, x.Name, x.FoodPicture)));
   }
   
   [Theory]
   [MemberData(nameof(CreateDishDto))]
   public async Task Patch_AssertUpdated(DishDto dishDto)
   {
      var fixt = await _fixture.CreateSignedInClient();
      var client = fixt.client;

      var createRes = await client.PostAsJsonAsync(baseUrl+"/"+(await CreateMenu(client)).ToString(), dishDto);

      if (!createRes.IsSuccessStatusCode)
         throw new HttpRequestException($"POST FAILED: {createRes.ReasonPhrase}," +
                                        $"  StatusCode: {createRes.StatusCode}," +
                                        $" ReasonPhrase: {createRes.ReasonPhrase}, " +
                                        $" Content: {createRes.Content.ReadAsStringAsync().Result}");

      var created = await createRes.Content.ReadFromJsonAsync<DishDto>();

      Assert.NotNull(created);
      Assert.NotEqual(0, created.Id);

      var updatedDto = new DishDto(
         created.Id,
         dishDto.DishName + "Updated",
         Guid.NewGuid().ToString()
      );

      var patchRes = await client.PatchAsJsonAsync(baseUrl, updatedDto);

      if (!patchRes.IsSuccessStatusCode)
         throw new HttpRequestException($"PATCH FAILED: {patchRes.ReasonPhrase}," +
                                        $"  StatusCode: {patchRes.StatusCode}," +
                                        $" ReasonPhrase: {patchRes.ReasonPhrase}, " +
                                        $" Content: {patchRes.Content.ReadAsStringAsync().Result}");

      var patched = await patchRes.Content.ReadFromJsonAsync<DishDto>();
      var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();
      var dishInDb = await ctx.Dishes.SingleOrDefaultAsync(x => x.Id == created.Id);

      Assert.NotNull(patched);
      Assert.Equal(created.Id, patched.Id);
      Assert.Equal(updatedDto.DishName, patched.DishName);
      Assert.Equal(updatedDto.DishPicture, patched.DishPicture);

      Assert.NotNull(dishInDb);
      Assert.Equal(updatedDto.DishName, dishInDb.Name);
      Assert.Equal(updatedDto.DishPicture, dishInDb.FoodPicture);

      var countWithId = await ctx.Dishes.CountAsync(x => x.Id == created.Id);
      Assert.Equal(1, countWithId);
   }

   [Theory]
   [MemberData(nameof(CreateDishDto))]
   public async Task Delete_AssertRemoved(DishDto dishDto)
   {
      var fixt = await _fixture.CreateSignedInClient();
      var client = fixt.client;
      var ctx = _fixture.Services.CreateScope().ServiceProvider.GetRequiredService<RestaurantDbContext>();

      var initialCount = await ctx.Dishes.CountAsync();

      var createRes = await client.PostAsJsonAsync(baseUrl+"/"+(await CreateMenu(client)).ToString(), dishDto);

      if (!createRes.IsSuccessStatusCode)
         throw new HttpRequestException($"POST FAILED: {createRes.ReasonPhrase}," +
                                        $"  StatusCode: {createRes.StatusCode}," +
                                        $" ReasonPhrase: {createRes.ReasonPhrase}, " +
                                        $" Content: {createRes.Content.ReadAsStringAsync().Result}");

      var created = await createRes.Content.ReadFromJsonAsync<DishDto>();

      Assert.NotNull(created);
      Assert.NotEqual(0, created.Id);

      var countAfterCreate = await ctx.Dishes.CountAsync();
      Assert.Equal(initialCount + 1, countAfterCreate);

      var deleteRes = await client.DeleteAsync(baseUrl +"/"+ created.Id);

      if (!deleteRes.IsSuccessStatusCode)
         throw new HttpRequestException($"DELETE FAILED: {deleteRes.ReasonPhrase}," +
                                        $"  StatusCode: {deleteRes.StatusCode}," +
                                        $" ReasonPhrase: {deleteRes.ReasonPhrase}, " +
                                        $" Content: {deleteRes.Content.ReadAsStringAsync().Result}");

      var deletedEntity = await ctx.Dishes.SingleOrDefaultAsync(x => x.Id == created.Id);
      Assert.Null(deletedEntity);

      var countAfterDelete = await ctx.Dishes.CountAsync();
      Assert.Equal(initialCount, countAfterDelete);

      var dtoList = await ctx.Dishes
         .Select(x => new DishDto(x.Id, x.Name, x.FoodPicture))
         .ToListAsync();

      Assert.DoesNotContain(
         dtoList,
         x => x.Id == created.Id
      );
   }
   private async Task<int> CreateMenu(HttpClient client)
   {
      var menuObj = new MenuDto(0,
         "Defaultmenu",
         "Username", 
         Guid.NewGuid().ToString(),
         " ");
      var menuCreation = await client.PostAsJsonAsync("/Menu/", menuObj);
      if(!menuCreation.IsSuccessStatusCode)
         throw new Exception("MenuCreation FAILED");
      
      var menu = await menuCreation.Content.ReadFromJsonAsync<MenuDto>();
      if (menu is null)
         throw new Exception("MenuCreation Failed");
      
      return menu.Id ?? 0;
   }
   public static IEnumerable<object[]> CreateDishDto()
   {
      return [[new DishDto(0, "defaultName", Guid.NewGuid().ToString())]];
   }
}