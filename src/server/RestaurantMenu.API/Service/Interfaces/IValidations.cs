namespace RestaurantMenu.API.Service.Interfaces;

public interface IValidations
{
   bool  TicketValidIdNumber(int? id);
   bool  ValidDishName(string? username);
   bool  ValidId(int? id);
   bool  ValidMenuName(string? username);
   bool  ValidThemeName(string? username);
   bool  ValidUserName(string? username);
}