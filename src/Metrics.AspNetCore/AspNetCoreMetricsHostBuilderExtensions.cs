using App.Metrics;
using Rocket.Surgery.AspNetCore.Metrics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Metrics;
using AppMetricsBuilder = App.Metrics.MetricsBuilder;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// AspNetCoreMetricsHostBuilderExtensions.
    /// </summary>
    public static class AspNetCoreMetricsHostBuilderExtensions
    {
        /// <summary>
        /// Uses the application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMetrics(
            this IConventionHostBuilder container)
        {
            container.Set(new RocketMetricsOptions() { UseDefaults = false });
            container.Scanner.PrependConvention<AspNetCoreMetricsConvention>();
            return container;
        }

        /// <summary>
        /// Uses the default application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMetricsWithDefaults(this IConventionHostBuilder container)
        {
            container.Set(new RocketMetricsOptions() { UseDefaults = true });
            container.Scanner.PrependConvention<AspNetCoreMetricsConvention>();
            return container;
        }
    }
}
