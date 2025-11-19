namespace RestaurantMenu.Core.Models;

public class Ticket
{
    public int Id { get; set; }
    public int TicketNumber { get; set; }
    public Menu Menu { get; set; }
    
    
}
