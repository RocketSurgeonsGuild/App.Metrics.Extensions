using System;
using System.Reflection;
using App.Metrics;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.Testing;
using Rocket.Surgery.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Metrics.AspNetCore.Tests
{
    public class UseMetricsTests : AutoFakeTest
    {
        public UseMetricsTests(ITestOutputHelper outputHelper) : base(outputHelper, LogLevel.Trace)
        {
            Disposable.Add(System.Reactive.Disposables.Disposable.Create(() => typeof(MetricsAspNetWebHostBuilderExtensions).GetField("_metricsBuilt", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, false)));
        }

        [Fact]
        public void AddsAsAConvention()
        {
            using var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureRocketSurgery(builder => { })
                .Build();

            host.Services.GetService<IMetrics>().Should().NotBeNull();
        }

        [Fact]
        public void AddsMetrics()
        {
            using var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureRocketSurgery(builder => builder.UseScanner(new BasicConventionScanner(builder.ServiceProperties)).UseMetrics())
                .Build();

            host.Services.GetService<IMetrics>().Should().NotBeNull();
        }

        [Fact]
        public void AddsDefaultMetrics()
        {
            using var host = Host.CreateDefaultBuilder(Array.Empty<string>())
                .ConfigureRocketSurgery(builder => builder.UseScanner(new BasicConventionScanner(builder.ServiceProperties)).UseMetricsWithDefaults())
                .Build();

            host.Services.GetService<IMetrics>().Should().NotBeNull();
        }
    }
}
