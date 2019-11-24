using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;

namespace Rocket.Surgery.Extensions.Metrics
{
    /// <summary>
    /// Logging Builder
    /// Implements the <see cref="ConventionBuilder{TBuilder,TConvention,TDelegate}" />
    /// Implements the <see cref="IMetricsBuilder" />
    /// Implements the <see cref="IMetricsConvention" />
    /// Implements the <see cref="IMetricsConventionContext" />
    /// </summary>
    /// <seealso cref="ConventionBuilder{IMetricsBuilder, IMetricsConvention, MetricsConventionDelegate}" />
    /// <seealso cref="IMetricsBuilder" />
    /// <seealso cref="IMetricsConvention" />
    /// <seealso cref="IMetricsConventionContext" />
    public class MetricsBuilder : ConventionBuilder<IMetricsBuilder, IMetricsConvention, MetricsConventionDelegate>,
                                  IMetricsBuilder,
                                  IMetricsConventionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MetricsBuilder" /> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="assemblyProvider">The assembly provider.</param>
        /// <param name="assemblyCandidateFinder">The assembly candidate finder.</param>
        /// <param name="appMetricsBuilder">The metrics builder.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="diagnosticSource">The diagnostic source.</param>
        /// <param name="properties">The properties.</param>
        /// <exception cref="ArgumentNullException">
        /// environment
        /// or
        /// metricsBuilder
        /// or
        /// configuration
        /// or
        /// diagnosticSource
        /// </exception>
        public MetricsBuilder(
            IConventionScanner scanner,
            IAssemblyProvider assemblyProvider,
            IAssemblyCandidateFinder assemblyCandidateFinder,
            IAppMetricsBuilder appMetricsBuilder,
            IRocketEnvironment environment,
            IConfiguration configuration,
            ILogger diagnosticSource,
            IDictionary<object, object?> properties
        ) : base(scanner, assemblyProvider, assemblyCandidateFinder, properties)
        {
            Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            AppMetricsBuilder = appMetricsBuilder ?? throw new ArgumentNullException(nameof(appMetricsBuilder));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            Logger = diagnosticSource ?? throw new ArgumentNullException(nameof(diagnosticSource));
        }

        /// <summary>
        /// Builds this instance.
        /// </summary>
        public void Build() => Composer.Register(
            Scanner,
            this,
            typeof(IMetricsConvention),
            typeof(MetricsConventionDelegate)
        );

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the metrics builder.
        /// </summary>
        /// <value>The metrics builder.</value>
        public IAppMetricsBuilder AppMetricsBuilder { get; }

        /// <summary>
        /// A logger that is configured to work with each convention item
        /// </summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; }

        /// <summary>
        /// The environment that this convention is running
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        /// <value>The environment.</value>
        public IRocketEnvironment Environment { get; }
    }
}