using NUnit.Framework;
using FluentAssertions;
using ResponsibleChains.Tests.Links;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ResponsibleChains.Tests
{
    public class ResponsibleChainBuilderLinkingTests
    {
        [Test]
        public void ShouldAddLinksWithTypes()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink(typeof(TestEndLink))
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestEndLink>();
        }

        [Test]
        public void ShouldAddLinksWithCustomSetup()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink(nextLink => new TestLink(nextLink)
                {
                    ListensFor = 2,
                    RespondsWith = 15
                })
                .WithLink(nextLink => new TestEndLink()
                {
                    RespondsWith = 12
                })
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestLink>();

            builtChain.Result(1).Should().Be(12);
            builtChain.Result(2).Should().Be(15);
        }

        [Test]
        public void KitchenSinkTest()
        {
            // Arrange
            TestDependency testDependency = new TestDependency { Value = "foo" };
            IServiceCollection services = new ServiceCollection().AddSingleton(testDependency);
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>(services);

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink<TestLinkWithDependencies>()
                .WithLink<TestLink>()
                .WithLink(nextLink => new TestLink(nextLink)
                {
                    ListensFor = 1,
                    RespondsWith = 100
                })
                .WithLink(nextLink => new TestLink(nextLink)
                {
                    ListensFor = 2,
                    RespondsWith = 200
                })
                .WithLink(typeof(TestLinkWithPrivateConstructor))
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestLinkWithDependencies>();

            builtChain.Result(0).Should().Be(0);
            builtChain.Result(1).Should().Be(100);
            builtChain.Result(2).Should().Be(200);
            builtChain.Result(3).Should().Be(42);
        }
    }
}
