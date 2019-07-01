using System.Diagnostics;
using System.Linq;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using App.Metrics.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.App.Metrics;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(AppMetricsConvention))]

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// AppMetricsConvention.
    /// Implements the <see cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Extensions.DependencyInjection.IServiceConvention" />
    public class AppMetricsConvention : IServiceConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly ILogger _diagnosticSource;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly IHealthBuilder _healthBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppMetricsConvention"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="diagnosticSource">The diagnostic source logger.</param>
        /// <param name="metricsBuilder">The metrics builder.</param>
        /// <param name="healthBuilder">The health builder.</param>
        public AppMetricsConvention(IConventionScanner scanner, ILogger diagnosticSource, IMetricsBuilder metricsBuilder = null, IHealthBuilder healthBuilder = null)
        {
            _scanner = scanner;
            _diagnosticSource = diagnosticSource;
            _metricsBuilder = metricsBuilder ?? AppMetrics.CreateDefaultBuilder();
            _healthBuilder = healthBuilder ?? AppMetricsHealth.CreateDefaultBuilder();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register(IServiceConventionContext context)
        {
            _metricsBuilder.Configuration.ReadFrom(context.Configuration);
            _healthBuilder.Configuration.ReadFrom(context.Configuration);

            var builder = new AppMetricsBuilder(_scanner, context.AssemblyProvider, context.AssemblyCandidateFinder,
                _metricsBuilder, _healthBuilder, context.Environment, context.Configuration, _diagnosticSource,
                context.Properties);

            var (metrics, health) = builder.Build();

            // NOTE: This does not scan for all the health checks across assemblies

            if (metrics.Options.ReportingEnabled && metrics.Reporters != null && metrics.Reporters.Any())
            {
                context.Services.AddMetricsReportingHostedService();
            }

            if (health.Options.ReportingEnabled && health.Reporters != null && health.Reporters.Any())
            {
                context.Services.AddHealthReportingHostedService();
            }

            context.Services.AddMetrics(metrics);
            context.Services.AddHealth(health);
            context.Services.AddAppMetricsHealthPublishing();
        }
    }
}
