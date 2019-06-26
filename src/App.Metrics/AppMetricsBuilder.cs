using System;
using System.Collections.Generic;
using System.Diagnostics;
using App.Metrics;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// Logging Builder
    /// </summary>
    public class AppMetricsBuilder : ConventionBuilder<IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate>, IAppMetricsBuilder, IAppMetricsConventionContext
    {
        public AppMetricsBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IMetricsBuilder metricsBuilder,
            IHealthBuilder healthBuilder,
            IRocketEnvironment environment,
            IConfiguration configuration,
            DiagnosticSource diagnosticSource,
            IDictionary<object, object> properties) : base(scanner, assemblyProvider, assemblyCandidateFinder, properties)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            MetricsBuilder = metricsBuilder ?? throw new ArgumentNullException(nameof(metricsBuilder));
            HealthBuilder = healthBuilder ?? throw new ArgumentNullException(nameof(healthBuilder));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var diagnosticSource1 = diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource));
            Logger = new DiagnosticLogger(diagnosticSource1);
        }

        public IConfiguration Configuration { get; }
        public IMetricsBuilder MetricsBuilder { get; }
        public IHealthBuilder HealthBuilder { get; }
        public ILogger Logger { get; }
        public IAssemblyProvider AssemblyProvider { get; }
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        public IRocketEnvironment Environment { get; }

        public (IMetricsRoot metrics, IHealthRoot health) Build()
        {
            new ConventionComposer(Scanner)
                .Register(
                    this,
                    typeof(IAppMetricsConvention),
                    typeof(AppMetricsConventionDelegate)
                );

            return (MetricsBuilder.Build(), HealthBuilder.Build());
        }
    }
}
