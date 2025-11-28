using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Service.Interfaces;

public class EditModelValidator : IEditModelValidator
{
    private readonly IValidations _v;
    
    public EditModelValidator(IValidations validations)
    {
        _v = validations;
    }

    /*  TODO: Expand the validations to get more specific error messages.
        We wanna make it easier to pinpoint what is wrong if we ever get an error. */

    public bool EditModelValid(MenuModel model)
    {
        
        return (_v.ValidThemeName(model.Theme)
                && _v.ValidUserName(model.User_name)
                && _v.ValidMenuName(model.Menu_name)
                && _v.ValidId(model.Id)
                && _v.ValidThemeName(model.User_id));
    }
}