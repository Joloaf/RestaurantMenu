using RestaurantMenu.API.Features;

public static class Endpoints
{

    public static void MapApplicationEndPoints(this IEndpointRouteBuilder config)
    {
        config.MapMenuEndpoints();
        config.MapDishEndpoint();
        config.AccountEndpoints();
        
    }
    public static void MapMenuEndpoints(this IEndpointRouteBuilder config)
    {
        var group = config.MapGroup("/Menu")
            .RequireAuthorization()
            .MapEndPoint<GetMenu>()
            .MapEndPoint<GetAllMenus>()
            .MapEndPoint<CreateMenu>()
            .MapEndPoint<UpdateMenu>()
            .MapEndPoint<DeleteMenu>();

    }

    public static void AccountEndpoints(this IEndpointRouteBuilder config)
    {
        var group = config.MapGroup("/Account");
        group.AddAccountFeatures();
    }

    public static void MapDishEndpoint(this IEndpointRouteBuilder config)
    {
        var group = config.MapGroup("/Dish");

        group.MapEndPoint<GetMenuDishes>();
    }
    //why do we do the TEndpoint constraint of IEndPoint ??
    // the type definition is not 
    //  possibly coviariance issue -> try to get difintion of static method (resolved by type)
    //  resolve higher up in the hirearchy.
    public static IEndpointRouteBuilder MapEndPoint<TEndPoint>(this IEndpointRouteBuilder config) where TEndPoint : IEndpoint
    {
        TEndPoint.Map(config);
        return config;
    }
}

public class GetMenuDishes : IEndpoint
{
    public static void Map(IEndpointRouteBuilder config)
        => config.MapGet("/DishesPerMenuId/{MenuId}", Handler);

    private static Task<IResult> Handler(int MenuId, HttpContext context)
    {
        throw new NotImplementedException();
    }
}