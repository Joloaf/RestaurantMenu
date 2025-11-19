using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace RestaurantMenu.Core.Models;

public class User : IdentityUser
{
    public Menu Menu { get; set; }
    [ForeignKey(nameof(Menu.Id))]
    public int MenuId { get; set; }
}