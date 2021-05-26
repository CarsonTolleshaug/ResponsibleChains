using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ResponsibleChains;
using ResponsibleChains.Tests.Links;
using FluentAssertions;

namespace Tests
{
    public class ServiceCollectionExtensionsTests
    {
        private IServiceCollection services;

        [SetUp]
        public void Setup()
        {
            services = new ServiceCollection();
        }

        [Test]
        public void ShouldAddChainToServices()
        {
            // Act
            services.AddResponsibleChain<ITestLink>()
                .WithLink<TestEndLink>();

            // Assert
            services.Should().Contain(serviceDescriptor => serviceDescriptor.ServiceType == typeof(ITestLink));
        }

        [Test]
        public void ShouldAddChainBuilderToServices()
        {
            // Act
            services.AddResponsibleChain<ITestLink>()
                .WithLink<TestEndLink>();

            // Assert
            services.Should().Contain(serviceDescriptor => serviceDescriptor.ServiceType == typeof(IResponsibleChainBuilder<ITestLink>));
        }

        [Test, Category("integration")]
        public void ShouldConstructChainProperly()
        {
            // Arrange
            TestDependency testDependency = new TestDependency { Value = "bar" };
            services.AddSingleton(testDependency);

            // Act
            services.AddResponsibleChain<ITestLink>()
                .WithLink<TestLink>()
                .WithLink<TestLinkWithDependencies>()
                .WithLink<TestEndLink>();

            // Assert
            ITestLink chain = services.BuildServiceProvider().GetRequiredService<ITestLink>();
            chain.Should().BeOfType<TestLink>();
            chain.As<TestLink>().Next.Should().BeOfType<TestLinkWithDependencies>();
            chain.As<TestLink>().Next.As<TestLinkWithDependencies>().TestDependency.Value.Should().Be("bar");
            chain.As<TestLink>().Next.As<TestLinkWithDependencies>().Next.Should().BeOfType<TestEndLink>();

            chain.Result(0).Should().Be(0);
            chain.Result(1).Should().Be(1337);
        }
    }
}