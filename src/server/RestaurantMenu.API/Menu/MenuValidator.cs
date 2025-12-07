using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Service.Interfaces;

public class MenuValidator : IMenuValidator
{
    private readonly IValidations _v;
    
    public MenuValidator(IValidations validations)
    {
        _v = validations;
    }

    /*  TODO: Expand the validations to get more specific error messages.
        We wanna make it easier to pinpoint what is wrong if we ever get an error. */

    public bool EditModelValid(MenuDto dto)
    {
        
        return (_v.ValidThemeName(dto.Theme)
                && _v.ValidUserName(dto.User_name)
                && _v.ValidMenuName(dto.Menu_name)
                && _v.ValidId(dto.Id)
                && _v.ValidThemeName(dto.User_id));
    }
}