using NUnit.Framework;
using FluentAssertions;
using ResponsibleChains.Tests.Links;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ResponsibleChains.Tests
{
    public class ResponsibleChainBuilderBuildingTests
    {
        [Test]
        public void ShouldBuildSimpleLink()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            ITestLink builtChain = responsibleChainBuilder.WithLink<TestEndLink>().Build();

            // Assert
            builtChain.Should().BeOfType<TestEndLink>();
        }

        [Test]
        public void ShouldBuildMultipleLinks()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink<TestLink>()
                .WithLink<TestEndLink>()
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestLink>();
            builtChain.As<TestLink>().Next.Should().BeOfType<TestEndLink>();
        }

        [Test]
        public void ShouldBuildLinksWithOtherDependencies()
        {
            // Arrange
            TestDependency testDependency = new TestDependency { Value = "foo" };
            IServiceCollection services = new ServiceCollection().AddSingleton(testDependency);
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>(services);

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink<TestLinkWithDependencies>()
                .WithLink<TestEndLink>()
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestLinkWithDependencies>();
            builtChain.As<TestLinkWithDependencies>().TestDependency.Should().Be(testDependency);
            builtChain.As<TestLinkWithDependencies>().Next.Should().BeOfType<TestEndLink>();
        }

        [Test]
        public void ShouldBuildLinksWithPrivateConstructor()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            ITestLink builtChain = responsibleChainBuilder
                .WithLink<TestLinkWithPrivateConstructor>()
                .WithLink<TestEndLink>()
                .Build();

            // Assert
            builtChain.Should().BeOfType<TestLinkWithPrivateConstructor>();
            builtChain.As<TestLinkWithPrivateConstructor>().Result(0).Should().Be(42);
        }

        [Test]
        public void GivenLinkHasNoPublicConstructor_ShouldThrowException()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            responsibleChainBuilder
                .WithLink<TestLinkWithNoPublicConstructor>()
                .WithLink<TestEndLink>();

            Action Act = () => responsibleChainBuilder.Build();

            // Assert
            Act.Should().Throw<ArgumentException>().WithMessage($"No public constructors found for type '{typeof(TestLinkWithNoPublicConstructor).FullName}'.");
        }

        [Test]
        public void GivenLinkHasMultiplePublicConstructor_ShouldThrowException()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            responsibleChainBuilder
                .WithLink<TestLinkWithMultiplePublicConstructors>()
                .WithLink<TestEndLink>();

            Action Act = () => responsibleChainBuilder.Build();

            // Assert
            Act.Should().Throw<ArgumentException>().WithMessage($"Multiple public constructors found for type '{typeof(TestLinkWithMultiplePublicConstructors).FullName}'.");
        }

        [Test]
        public void GivenLinkHasNoEndLink_ShouldThrowException()
        {
            // Arrange
            IResponsibleChainBuilder<ITestLink> responsibleChainBuilder = new ResponsibleChainBuilder<ITestLink>();

            // Act
            responsibleChainBuilder
                .WithLink<TestLink>();

            Action Act = () => responsibleChainBuilder.Build();

            // Assert
            Act.Should().Throw<ArgumentException>().WithMessage($"Final link of type '{typeof(TestLink).FullName}' expects next link in constructor, but no link was provided.");
        }
    }
}
