namespace succotashBE.CustomerManagement
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            return services;
        }
    }
}
