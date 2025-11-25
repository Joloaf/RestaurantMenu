using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Service.Interfaces;

public interface IEditModelValidator
{
    public bool EditModelValid(MenuModel model);
}