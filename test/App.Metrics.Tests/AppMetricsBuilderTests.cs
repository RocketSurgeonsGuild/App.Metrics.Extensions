using System;
using App.Metrics;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Extensions.App.Metrics.Tests
{
    public class ConfigurationBuilderTests : AutoTestBase
    {
        public ConfigurationBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

        [Fact]
        public void Constructs()
        {
            var metricsBuilder = AutoFake.Resolve<IMetricsBuilder>();
            var builder = AutoFake.Resolve<AppMetricsBuilder>();

            builder.MetricsBuilder.Should().BeSameAs(metricsBuilder);
            Action a = () => { builder.AppendConvention(A.Fake<IAppMetricsConvention>()); };
            a.Should().NotThrow();
            a = () => { builder.AppendDelegate(delegate { }); };
            a.Should().NotThrow();
        }
    }
}
