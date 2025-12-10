using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Service.Interfaces;

public interface IMenuValidator
{
    public bool ModelValid(MenuDto dto);
}