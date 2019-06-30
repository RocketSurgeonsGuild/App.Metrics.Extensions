using Rocket.Surgery.Conventions;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    /// <summary>
    /// Interface ILoggingConvention
    /// Implements the <see cref="Rocket.Surgery.Conventions.IConvention{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConventionContext}" />
    /// </summary>
    /// <seealso cref="Rocket.Surgery.Conventions.IConvention{Rocket.Surgery.Extensions.App.Metrics.IAppMetricsConventionContext}" />
    public interface IAppMetricsConvention : IConvention<IAppMetricsConventionContext>
    {
        
    }
}
