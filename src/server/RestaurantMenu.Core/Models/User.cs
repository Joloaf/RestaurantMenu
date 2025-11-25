using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RestaurantMenu.Core.Models;

public class User : IdentityUser
{
    public ICollection<Menu> Menus { get; set; } = [];
}