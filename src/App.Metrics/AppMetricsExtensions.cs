using App.Metrics;
using App.Metrics.Health;
using App.Metrics.Health.Builder;
using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    public static class AppMetricsExtensions
    {
        public static IConventionHostBuilder UseAppMetrics(
            this IConventionHostBuilder container, IMetricsBuilder metricsBuilder = null, IHealthBuilder healthBuilder= null)
        {
            if (metricsBuilder == null) metricsBuilder = new MetricsBuilder();
            if (healthBuilder == null) healthBuilder = new HealthBuilder();
            container.Scanner.PrependConvention(new AppMetricsConvention(container.Scanner, container.DiagnosticSource, metricsBuilder, healthBuilder));
            return container;
        }

        public static IConventionHostBuilder UseDefaultAppMetrics(
            this IConventionHostBuilder container)
        {
            container.Scanner.PrependConvention(new AppMetricsConvention(container.Scanner, container.DiagnosticSource, AppMetrics.CreateDefaultBuilder(), AppMetricsHealth.CreateDefaultBuilder()));
            return container;
        }
    }
}