using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.Metrics
{
    /// <summary>
    ///  IMetricsBuilder
    /// Implements the <see cref="IConventionBuilder{IMetricsBuilder, IMetricsConvention, MetricsConventionDelegate}" />
    /// </summary>
    /// <seealso cref="IConventionBuilder{IMetricsBuilder, IMetricsConvention, MetricsConventionDelegate}" />
    public interface IMetricsBuilder : IConventionBuilder<IMetricsBuilder, IMetricsConvention, MetricsConventionDelegate> { }
}
