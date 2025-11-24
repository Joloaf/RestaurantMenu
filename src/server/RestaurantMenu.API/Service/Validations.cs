using System.Text.RegularExpressions;

namespace RestaurantMenu.API.Service;

public class Validations
{
    public bool ValidUserName(string? username)
    {
        if(string.IsNullOrEmpty(username)
           || string.IsNullOrWhiteSpace(username))
            return false;
        
        if(username.Length < 2)
            return false;
        
        var UppRegex = new Regex(@"^[A-Z]{1}[a-z]+$");        
        if(!UppRegex.IsMatch(username))
            return false;
        
        var regex = new Regex(@"^[\p{L}\p{So}\p{Sk}\s]*$");
        return regex.IsMatch(username);
    }
    public bool ValidDishName(string? username)
    {
        if(string.IsNullOrEmpty(username)
           || string.IsNullOrWhiteSpace(username)) 
            return false;
            
        var regex = new Regex(@"^[\p{L}\p{So}\p{Sk}\s]*$");
        return regex.IsMatch(username);
    }
    public bool ValidId(int? id)
    {
        return !(id == null || id < 0);
    }
    public bool ValidThemeName(string? username)
    {
        if(string.IsNullOrEmpty(username)
           || string.IsNullOrWhiteSpace(username))
            return false;

        var regex = new Regex(@"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$");
        return regex.IsMatch(username);
    }
    public bool TicketValidIdNumber(int id)
    {
        return !(id == null || id < 0);
    }
    public bool ValidMenuName(string? username)
    {
        if(string.IsNullOrEmpty(username)
           || string.IsNullOrWhiteSpace(username))
            return false;

        var pattern = @"^(?:[a-zA-Z0-9]|[\u00a9\u00ae]|[\u2000-\u3300]|[\ud83c-\ud83e][\ud800-\udfff])*$";
        var regex = new Regex(pattern);
        return regex.IsMatch(username);
    }
    
}