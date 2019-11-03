namespace Rocket.Surgery.Extensions.Metrics
{
    /// <summary>
    /// Options for configuration Metrics
    /// </summary>
    public class RocketMetricsOptions
    {
        /// <summary>
        /// Use the default metrics configuration from App.Metrics
        /// </summary>
        public bool UseDefaults { get; set; } = true;
    }
}
