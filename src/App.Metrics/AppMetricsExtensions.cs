using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// AppMetricsExtensions.
    /// </summary>
    public static class AppMetricsExtensions
    {
        /// <summary>
        /// Uses the application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="metricsBuilder">The metrics builder.</param>
        /// <param name="healthBuilder">The health builder.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseAppMetrics(
            this IConventionHostBuilder container, IMetricsBuilder metricsBuilder = null, IHealthBuilder healthBuilder= null)
        {
            if (metricsBuilder == null) metricsBuilder = new MetricsBuilder();
            if (healthBuilder == null) healthBuilder = new HealthBuilder();
            container.Scanner.PrependConvention(new AppMetricsConvention(container.Scanner, container.DiagnosticSource, metricsBuilder, healthBuilder));
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
            container.Scanner.PrependConvention(new AppMetricsConvention(container.Scanner, container.DiagnosticSource, AppMetrics.CreateDefaultBuilder(), AppMetricsHealth.CreateDefaultBuilder()));
            return container;
        }
    }
}
