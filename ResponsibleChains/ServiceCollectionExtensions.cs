using Microsoft.Extensions.DependencyInjection;

namespace ResponsibleChains
{
    public static class ServiceCollectionExtensions
    {
        public static IResponsibleChainBuilder<T> AddResponsibleChain<T>(this IServiceCollection services) where T : class
        {
            IResponsibleChainBuilder<T> builder = new ResponsibleChainBuilder<T>(services);

            services.AddSingleton(builder);
            services.AddTransient(_ => builder.Build());

            return builder;
        }
    }
}
