namespace RestaurantMenu.Core.DTO;

public class UpdateDTO
{
    public string Type { get; set; }
    public int? Id {get;set;}
    public string Field { get; set; }
    public string Value { get;set;}
}