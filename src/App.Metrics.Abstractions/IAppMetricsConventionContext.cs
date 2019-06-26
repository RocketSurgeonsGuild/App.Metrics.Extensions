using System;
using System.Collections.Generic;
using App.Metrics;
using App.Metrics.Health;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;

namespace Rocket.Surgery.Extensions.App.Metrics
{
    public interface IAppMetricsConventionContext : IConventionContext
    {
        IAssemblyProvider AssemblyProvider { get; }
        IAssemblyCandidateFinder AssemblyCandidateFinder { get; }
        IConfiguration Configuration { get; }
        IMetricsBuilder MetricsBuilder { get; }
        IHealthBuilder HealthBuilder { get; }

        /// <summary>
        /// The environment that this convention is running
        ///
        /// Based on IHostEnvironment / IHostingEnvironment
        /// </summary>
        IRocketEnvironment Environment { get; }
    }
}
