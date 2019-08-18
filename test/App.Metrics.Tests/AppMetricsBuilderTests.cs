using System;
using System.Collections.Generic;
using System.Diagnostics;
using App.Metrics;
using App.Metrics.Health;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Rocket.Surgery.Extensions.App.Metrics.Tests
{
    public class AppMetricsBuilderTests : AutoTestBase
    {
        public AppMetricsBuilderTests(ITestOutputHelper outputHelper) : base(outputHelper) { }

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
    public class UseAppMetricsTests : AutoTestBase
    {
        public UseAppMetricsTests(ITestOutputHelper outputHelper) : base(outputHelper, LogLevel.Trace)
        {
        }

        class HostBuilder : ConventionHostBuilder<HostBuilder>
        {
            public HostBuilder(IConventionScanner scanner, IAssemblyCandidateFinder assemblyCandidateFinder, IAssemblyProvider assemblyProvider, DiagnosticSource diagnosticSource, IServiceProviderDictionary serviceProperties) : base(scanner, assemblyCandidateFinder, assemblyProvider, diagnosticSource, serviceProperties)
            {
            }
        }

        [Fact]
        public void AddsAsAConvention()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            var finder = AutoFake.Resolve<IAssemblyCandidateFinder>();

            A.CallTo(() => finder.GetCandidateAssemblies(A<IEnumerable<string>>._))
                .Returns(new[] { typeof(AppMetricsHostBuilderExtensions).Assembly });

            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);
            var services = new ServiceCollection();
            AutoFake.Provide<IServiceCollection>(services);

            var builder = AutoFake.Resolve<HostBuilder>();
            var sb = AutoFake.Resolve<ServicesBuilder>();

            var sp = sb.Build();

            services.Should().Contain(x => x.ServiceType == typeof(IHealth));
            services.Should().Contain(x => x.ServiceType == typeof(IMetrics));
        }
        
        [Fact]
        public void AddsAppMetrics()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);
            var services = new ServiceCollection();
            AutoFake.Provide<IServiceCollection>(services);

            var builder = AutoFake.Resolve<HostBuilder>();
            var sb = AutoFake.Resolve<ServicesBuilder>();

            builder.UseAppMetrics();

            var sp = sb.Build();

            services.Should().Contain(x => x.ServiceType == typeof(IHealth));
            services.Should().Contain(x => x.ServiceType == typeof(IMetrics));
        }

        [Fact]
        public void AddsDefaultAppMetrics()
        {
            var properties = new ServiceProviderDictionary();
            AutoFake.Provide<IServiceProviderDictionary>(properties);
            AutoFake.Provide<IDictionary<object, object?>>(properties);
            AutoFake.Provide<IServiceProvider>(properties);
            properties[typeof(ILogger)] = Logger;
            var scanner = AutoFake.Resolve<SimpleConventionScanner>();
            AutoFake.Provide<IConventionScanner>(scanner);
            var services = new ServiceCollection();
            AutoFake.Provide<IServiceCollection>(services);

            var builder = AutoFake.Resolve<HostBuilder>();
            var sb = AutoFake.Resolve<ServicesBuilder>();

            builder.UseDefaultAppMetrics();

            var sp = sb.Build();

            services.Should().Contain(x => x.ServiceType == typeof(IHealth));
            services.Should().Contain(x => x.ServiceType == typeof(IMetrics));
        }
    }
}
