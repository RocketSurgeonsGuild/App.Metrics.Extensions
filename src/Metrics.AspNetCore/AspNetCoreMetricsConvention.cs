using System;
using App.Metrics;
using App.Metrics.AspNetCore;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.AspNetCore.Metrics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.Metrics;
using Rocket.Surgery.Hosting;
using MetricsBuilder = Rocket.Surgery.Extensions.Metrics.MetricsBuilder;

[assembly: Convention(typeof(AspNetCoreMetricsConvention))]

namespace Rocket.Surgery.AspNetCore.Metrics
{
    /// <summary>
    /// MetricsConvention.
    /// Implements the <see cref="IHostingConvention" />
    /// </summary>
    /// <seealso cref="IHostingConvention" />
    public class AspNetCoreMetricsConvention : IHostingConvention
    {
        private readonly IConventionScanner _scanner;
        private readonly ILogger _diagnosticSource;
        private readonly RocketMetricsOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreMetricsConvention"/> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        /// <param name="diagnosticSource">The diagnostic source logger.</param>
        /// <param name="options">The options.</param>
        public AspNetCoreMetricsConvention(IConventionScanner scanner, ILogger diagnosticSource, RocketMetricsOptions? options = null)
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

            context.Builder.UseMetrics();
            context.Builder.ConfigureServices((_, services) => services.AddAppMetricsHealthPublishing());
        }
    }
}
