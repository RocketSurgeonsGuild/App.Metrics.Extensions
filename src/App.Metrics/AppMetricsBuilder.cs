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
    /// Implements the <see cref="Rocket.Surgery.Conventions.ConventionBuilder{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder, Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConvention, Rocket.Surgery.Extensions.App.Metrics.AppMetricsConventionDelegate}" />
    /// Implements the <see cref="Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder" />
    /// Implements the <see cref="Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConventionContext" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Conventions.ConventionBuilder{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder, Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConvention, Rocket.Surgery.Extensions.App.Metrics.AppMetricsConventionDelegate}" />
    /// <seealso cref="Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder" />
    /// <seealso cref="Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConventionContext" />
    public class AppMetricsBuilder : ConventionBuilder<IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate>, IAppMetricsBuilder, IAppMetricsConventionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppMetricsBuilder"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="assemblyProvider">The assembly provider.</param>
        /// <param name="assemblyCandidateFinder">The assembly candidate finder.</param>
        /// <param name="metricsBuilder">The metrics builder.</param>
        /// <param name="healthBuilder">The health builder.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="diagnosticSource">The diagnostic source.</param>
        /// <param name="properties">The properties.</param>
        /// <exception cref="ArgumentNullException">
        /// environment
        /// or
        /// metricsBuilder
        /// or
        /// healthBuilder
        /// or
        /// configuration
        /// or
        /// diagnosticSource
        /// </exception>
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

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// Gets the metrics builder.
        /// </summary>
        /// <value>The metrics builder.</value>
        public IMetricsBuilder MetricsBuilder { get; }
        /// <summary>
        /// Gets the health builder.
        /// </summary>
        /// <value>The health builder.</value>
        public IHealthBuilder HealthBuilder { get; }
        /// <summary>
        /// A logger that is configured to work with each convention item
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; }
        /// <summary>
        /// Gets the assembly provider.
        /// </summary>
        /// <value>The assembly provider.</value>
        public IAssemblyProvider AssemblyProvider { get; }
        /// <summary>
        /// Gets the assembly candidate finder.
        /// </summary>
        /// <value>The assembly candidate finder.</value>
        public IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        /// <summary>
        /// The environment that this convention is running
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        /// <value>The environment.</value>
        public IRocketEnvironment Environment { get; }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        /// <returns>System.ValueTuple&lt;IMetricsRoot, IHealthRoot&gt;.</returns>
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
