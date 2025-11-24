using RestaurantMenu.Core.Models;

public class MenuFactory : IFactory<Menu>
{
    public Menu Create()
    {
        //object creation.

        //Mutate and insert default values here.

        //insert default values here.
        return new Menu();
    }
}