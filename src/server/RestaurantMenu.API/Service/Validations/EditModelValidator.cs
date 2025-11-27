using RestaurantMenu.API.Service.DTOs.Models;

namespace RestaurantMenu.API.Service.Interfaces;

public class EditModelValidator : IEditModelValidator
{
    private readonly IValidations _v;
    
    public EditModelValidator(IValidations validations)
    {
        _v = validations;
    }
    public bool EditModelValid(MenuModel model)
    {
        
        return (_v.ValidThemeName(model.Theme)
                && _v.ValidUserName(model.User_name)
                && _v.ValidMenuName(model.Menu_mame)
                && _v.ValidId(model.Id)
                && _v.ValidThemeName(model.User_id));
    }
}