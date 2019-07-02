using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    ///  IAppMetricsBuilder
    /// Implements the <see cref="IConventionBuilder{IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate}" />
    /// </summary>
    /// <seealso cref="IConventionBuilder{IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate}" />
    public interface IAppMetricsBuilder : IConventionBuilder<IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate> { }
}
