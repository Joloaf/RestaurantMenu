using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace RestaurantMenu.API.Service.DTOs.Models;

public class MenuModel(int? id, string? menuName, string? userName, string? theme, string userId)
{
    public MenuModel() : 
        this(-1, string.Empty, string.Empty, string.Empty, string.Empty)
    { 
    }
    
    //copy ctor
    public MenuModel(MenuModel model) : 
        this(model.Id, model.Menu_name, model.User_name, model.Theme, model.User_id)
    {
        
    }
    [JsonPropertyName("menuId")]
    public int? Id { get; init; } = id;
    [JsonPropertyName("menuName")]
    public string? Menu_name { get; init; } = menuName;
    [JsonPropertyName("userName")]
    public string? User_name { get; init; } = userName;
    [JsonPropertyName("theme")]
    public string? Theme { get; init; } = theme;
    [JsonPropertyName("userId")]
    public string? User_id { get; init; } = userId;
}

