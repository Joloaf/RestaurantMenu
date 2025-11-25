using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RestaurantMenu.API.Service.DTOs.Models;

public class MenuModel(int? id, string? menu_mame, string? user_name, string? theme, string user_id)
{
    public MenuModel() : 
        this(-1, string.Empty, string.Empty, string.Empty, string.Empty)
    { 
    }
    
    //copy ctor
    public MenuModel(MenuModel model) : 
        this(model.Id, model.Menu_mame, model.User_name, model.Theme, model.User_id)
    {
        
    }
    public int? Id { get; init; } = id;
    public string? Menu_mame { get; init; } = menu_mame;
    public string? User_name { get; init; } = user_name;
    public string? Theme { get; init; } = theme;
    public string? User_id { get; init; } = user_id;
}

