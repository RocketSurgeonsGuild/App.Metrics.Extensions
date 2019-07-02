using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// AppMetricsExtensions.
    /// </summary>
    public static class AppMetricsHostBuilderExtensions
    {
        /// <summary>
        /// Uses the application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="metricsBuilder">The metrics builder.</param>
        /// <param name="healthBuilder">The health builder.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseAppMetrics(
            this IConventionHostBuilder container, IMetricsBuilder metricsBuilder = null, IHealthBuilder healthBuilder = null)
        {
            if (metricsBuilder == null) metricsBuilder = container.ServiceProperties.TryGetValue(typeof(IMetricsBuilder), out var metricsBuilderObject) ? metricsBuilderObject as IMetricsBuilder : new MetricsBuilder();
            if (healthBuilder == null) healthBuilder = container.ServiceProperties.TryGetValue(typeof(IHealthBuilder), out var healthBuilderObject) ? healthBuilderObject as IHealthBuilder : new HealthBuilder();

            container.ServiceProperties[typeof(IMetricsBuilder)] = metricsBuilder;
            container.ServiceProperties[typeof(IHealthBuilder)] = healthBuilder;
            container.Scanner.PrependConvention<AppMetricsConvention>();
            return container;
        }

        /// <summary>
        /// Uses the default application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseDefaultAppMetrics(
            this IConventionHostBuilder container)
        {
            container.ServiceProperties[typeof(IMetricsBuilder)] = AppMetrics.CreateDefaultBuilder();
            container.ServiceProperties[typeof(IHealthBuilder)] = AppMetricsHealth.CreateDefaultBuilder();
            container.Scanner.PrependConvention<AppMetricsConvention>();
            return container;
        }
    }
}
