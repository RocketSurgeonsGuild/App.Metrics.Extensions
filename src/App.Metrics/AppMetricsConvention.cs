using System.Diagnostics;
using System.Linq;
using App.Metrics;
using App.Metrics.Extensions.Configuration;
using App.Metrics.Health;
using App.Metrics.Health.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.DependencyInjection;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    public class AppMetricsConvention : IServiceConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly DiagnosticSource _diagnosticSource;
        private readonly IMetricsBuilder _metricsBuilder;
        private readonly IHealthBuilder _healthBuilder;

        public AppMetricsConvention(IConventionScanner scanner, DiagnosticSource diagnosticSource, IMetricsBuilder metricsBuilder, IHealthBuilder healthBuilder)
        {
            _scanner = scanner;
            _diagnosticSource = diagnosticSource;
            _metricsBuilder = metricsBuilder;
            _healthBuilder = healthBuilder;
        }

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
