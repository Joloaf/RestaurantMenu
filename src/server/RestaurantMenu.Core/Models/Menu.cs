namespace RestaurantMenu.Core.Models;

public class Menu
{
    public int Id { get; set; }
    public string MenuName { get; set; } // Menu name
    public string UserName { get; set; }  // Kids name
    public string Theme { get; set; }
    public User Users { get; set; }
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}