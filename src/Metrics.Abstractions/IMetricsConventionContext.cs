using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
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
        /// Gets the assembly provider.
        /// </summary>
        /// <value>The assembly provider.</value>
        [NotNull]
        IAssemblyProvider AssemblyProvider { get; }

        /// <summary>
        /// Gets the assembly candidate finder.
        /// </summary>
        /// <value>The assembly candidate finder.</value>
        [NotNull]
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }

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
        IRocketEnvironment Environment { get; }
    }
}