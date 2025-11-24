using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantMenu.Core.Models;

public class Menu
{
    //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string MenuName { get; set; } // Menu name
    public string UserName { get; set; }  // Kids name
    public string Theme { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}