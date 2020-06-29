using System;
using App.Metrics;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.AspNetCore.Metrics;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Metrics;
using AppMetricsBuilder = App.Metrics.MetricsBuilder;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Conventions
{
    /// <summary>
    /// AspNetCoreMetricsHostBuilderExtensions.
    /// </summary>
    [PublicAPI]
    public static class AspNetCoreMetricsHostBuilderExtensions
    {
        /// <summary>
        /// Uses the application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IHostBuilder.</returns>
        public static IHostBuilder UseMetrics(
            [NotNull] this IHostBuilder container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.GetConventions().UseMetrics();
            return container;
        }
        /// <summary>
        /// Uses the application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMetrics(
            [NotNull] this IConventionHostBuilder container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Set(new RocketMetricsOptions() { UseDefaults = false });
            container.Scanner.PrependConvention<AspNetCoreMetricsConvention>();
            return container;
        }

        /// <summary>
        /// Uses the default application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IHostBuilder.</returns>
        public static IHostBuilder UseMetricsWithDefaults([NotNull] this IHostBuilder container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.GetConventions().UseMetricsWithDefaults();
            return container;
        }

        /// <summary>
        /// Uses the default application metrics.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>IConventionHostBuilder.</returns>
        public static IConventionHostBuilder UseMetricsWithDefaults([NotNull] this IConventionHostBuilder container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Set(new RocketMetricsOptions() { UseDefaults = true });
            container.Scanner.PrependConvention<AspNetCoreMetricsConvention>();
            return container;
        }
    }
}
