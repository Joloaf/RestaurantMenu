namespace RestaurantMenu.API.Service.DTOs.Models;

    
public record EditMenuModel(int id, string? menu_name, string? user_account_name, string? theme, string user_id);
