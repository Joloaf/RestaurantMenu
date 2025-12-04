namespace RestaurantMenu.API.Common.Api;

public interface IDevelopmentEndPoint
{
    public static abstract void MapDevEndPoint(IEndpointRouteBuilder config, IWebHostEnvironment env);
}