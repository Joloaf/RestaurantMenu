using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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
    public int? Id { get; init; } = id;
    public string? Menu_name { get; init; } = menuName;
    public string? User_name { get; init; } = userName;
    public string? Theme { get; init; } = theme;
    public string? User_id { get; init; } = userId;
}

