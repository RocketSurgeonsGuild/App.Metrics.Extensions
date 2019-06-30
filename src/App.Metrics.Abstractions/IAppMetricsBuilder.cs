using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// Interface ILoggingConvention
    /// Implements the <see cref="Rocket.Surgery.Conventions.IConventionBuilder{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder, Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConvention, Rocket.Surgery.Extensions.App.Metrics.AppMetricsConventionDelegate}" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Conventions.IConventionBuilder{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsBuilder, Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConvention, Rocket.Surgery.Extensions.App.Metrics.AppMetricsConventionDelegate}" />
    public interface IAppMetricsBuilder : IConventionBuilder<IAppMetricsBuilder, IAppMetricsConvention, AppMetricsConventionDelegate> { }
}
