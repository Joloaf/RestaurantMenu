public interface IEndpoint
{
    /// <summary>
    /// //this gets called as the registrator function itself after the group definition
    ///
    /// * inherited by the endpoint itself used to configure the endpoint as the endpoint
    ///     itself sees fit. 
    /// </summary>
    /// <returns></returns>
    public static abstract void Map(IEndpointRouteBuilder config);
}