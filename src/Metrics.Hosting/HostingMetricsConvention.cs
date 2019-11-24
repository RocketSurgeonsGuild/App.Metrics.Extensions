using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.Metrics;
using Rocket.Surgery.Hosting.Metrics;
using MetricsBuilder = Rocket.Surgery.Extensions.Metrics.MetricsBuilder;

[assembly: Convention(typeof(HostingMetricsConvention))]

namespace Rocket.Surgery.Hosting.Metrics
{
    /// <summary>
    /// MetricsConvention.
    /// Implements the <see cref="IHostingConvention" />
    /// </summary>
    /// <seealso cref="IHostingConvention" />
    public class HostingMetricsConvention : IHostingConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly ILogger _diagnosticSource;
        private readonly RocketMetricsOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="HostingMetricsConvention"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="diagnosticSource">The diagnostic source logger.</param>
        /// <param name="options">The options.</param>
        public HostingMetricsConvention(IConventionScanner scanner, ILogger diagnosticSource, RocketMetricsOptions? options = null)
        {
            _scanner = scanner;
            _diagnosticSource = diagnosticSource;
            _options = options ?? new RocketMetricsOptions();
        }

        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public void Register([NotNull] IHostingConventionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (_options.UseDefaults)
            {
                context.Builder.ConfigureMetricsWithDefaults((ctx, builder) =>
                {
                    new MetricsBuilder(_scanner, context.AssemblyProvider, context.AssemblyCandidateFinder,
                        builder, ctx.HostingEnvironment.Convert(), ctx.Configuration, _diagnosticSource,
                        context.Properties).Build();
                });
            }
            else
            {
                context.Builder.ConfigureMetrics((ctx, builder) =>
                {
                    new MetricsBuilder(_scanner, context.AssemblyProvider, context.AssemblyCandidateFinder,
                        builder, ctx.HostingEnvironment.Convert(), ctx.Configuration, _diagnosticSource,
                        context.Properties).Build();
                });
            }

            context.Builder.ConfigureServices((_, services) => services.AddAppMetricsHealthPublishing());
        }
    }
}
