using System;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Extensions.Metrics;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;
using AppMetricsBuilder = App.Metrics.MetricsBuilder;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Rocket.Surgery.Metrics.Hosting.Tests
{
    public class MetricsBuilderTests : AutoFakeTest
    {
        [Fact]
        public void Constructs()
        {
            var metricsBuilder = AutoFake.Resolve<IAppMetricsBuilder>();
            var builder = AutoFake.Resolve<MetricsBuilder>();

            builder.AppMetricsBuilder.Should().BeSameAs(metricsBuilder);
            Action a = () => { builder.AppendConvention(A.Fake<IMetricsConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.AppendDelegate(delegate { }); };
            a.Should().NotThrow();
        }

        public MetricsBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper) => Disposable.Add(
            System.Reactive.Disposables.Disposable.Create(
                () => typeof(MetricsConfigureHostBuilderExtensions).GetField(
                    "_metricsBuilt",
                    BindingFlags.Static | BindingFlags.NonPublic
                )!.SetValue(null, false)
            )
        );
    }
}