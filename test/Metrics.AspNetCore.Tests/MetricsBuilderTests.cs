using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Metrics;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;
using AppMetricsBuilder = App.Metrics.MetricsBuilder;
using IAppMetricsBuilder = App.Metrics.IMetricsBuilder;
using IMetricsBuilder = Rocket.Surgery.Extensions.Metrics.IMetricsBuilder;
using MetricsBuilder = Rocket.Surgery.Extensions.Metrics.MetricsBuilder;
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Rocket.Surgery.Metrics.AspNetCore.Tests
{
    public class MetricsBuilderTests : AutoFakeTest
    {
        public MetricsBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper)
        {
            Disposable.Add(System.Reactive.Disposables.Disposable.Create(() => typeof(MetricsAspNetWebHostBuilderExtensions).GetField("_metricsBuilt", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, false)));
        }

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
    }
}
