using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace RestaurantMenu.API.Service.DTOs.Models;

public class MenuModel(Nullable<int> id, string? menu_mame, string? user_name, string? theme, string user_id)
{
    [AllowNull]
    public Nullable<int> Id { get; init; } = id;
    [AllowNull]
    public string? Menu_mame { get; init; } = menu_mame;
    [AllowNull]
    public string? User_name { get; init; } = user_name;
    [AllowNull]
    public string? Theme { get; init; } = theme;
    [AllowNull]
    public string? User_id { get; init; } = user_id;
}

