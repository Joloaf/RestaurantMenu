public static class EndpointRegistration
{

    public static void MapApplicationEndPoints(this IEndpointRouteBuilder config)
    {
        config.MapMenuEndpoints();
        config.MapDishEndpoint();
    }
    public static void MapMenuEndpoints(this IEndpointRouteBuilder config)
    {
        var group = config.MapGroup("/Menu")
            .MapEndPoint<MapPatch>();

    }

    public static void MapDishEndpoint(this IEndpointRouteBuilder config)
    {
        
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