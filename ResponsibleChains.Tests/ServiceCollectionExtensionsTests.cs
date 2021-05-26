using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ResponsibleChains;
using ResponsibleChains.Tests.Links;

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
        public void GivenBasicLinks_ShouldBuildLink()
        {
            // Arrange

            // Act
            services.AddResponsibleChain<ITestLink>()
                .WithLink<TestEndLink>();

            // Assert
        }
    }
}