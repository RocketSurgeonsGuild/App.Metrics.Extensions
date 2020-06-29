using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;

namespace Rocket.Surgery.Extensions.Metrics
{
    /// <summary>
    /// IMetricsConventionContext
    /// Implements the <see cref="IConventionContext" />
    /// </summary>
    /// <seealso cref="IConventionContext" />
    public interface IMetricsConventionContext : IConventionContext
    {
        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        [NotNull]
        IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the metrics builder.
        /// </summary>
        /// <value>The metrics builder.</value>
        [NotNull]
        IAppMetricsBuilder AppMetricsBuilder { get; }

        /// <summary>
        /// The environment that this convention is running
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        /// <value>The environment.</value>
        [NotNull]
        IHostEnvironment Environment { get; }
    }
}