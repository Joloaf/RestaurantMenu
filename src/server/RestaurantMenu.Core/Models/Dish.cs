namespace RestaurantMenu.Core.Models;

public class Dish
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string FoodPicture { get; set; } // set defualt picture
    public Menu Menu { get; set; }
}