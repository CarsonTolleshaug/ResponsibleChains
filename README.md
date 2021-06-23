# Responsible Chains
### A library for easily implementing a chain of responsibility pattern

[![Build and Release](https://github.com/CarsonTolleshaug/ResponsibleChains/actions/workflows/build.yml/badge.svg)](https://github.com/CarsonTolleshaug/ResponsibleChains/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/v/ResponsibleChains.svg)](https://nuget.org/packages/ResponsibleChains) 
[![Nuget](https://img.shields.io/nuget/dt/ResponsibleChains.svg)](https://nuget.org/packages/ResponsibleChains)

![Logo](./Logo/logo.png)

This library can help you instantiate a chain of responsibility pattern in a number of convenient ways. 
See [this Wikipedia entry](https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern) for an 
explaination of the Chain of Responsibility pattern in Object Oriented design.

## Getting Started

ResponsibleChains can be installed using the Nuget package manager or the dotnet CLI

```
dotnet add package ResponsibleChains
```

## Using the package

The [Examples](./Examples) folder contains two implementations of FizzBuzz using the chain of responsibility pattern with ResponsibleChains.

Your chain should consist of objects which each have reference to the next object in the chain. Such
objects are refered to as **links** in this package.

To construct a chain, use the `.AddResponsibleChain<YOUR_INTERFACE_TYPE>()` method in `ConfigureServices` 
in `Startup` to add your links in order:

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddResponsibleChain<IMyChain>()
        .WithLink<MyFirstLink>()
        .WithLink<MySecondLink>()
        .WithLink<DefaultLink>();

    ...
}
```

## No DI? No Problem.

If you don't have access to a service collection or want to manually construct your chain, you can 
do so using the `ResponsibleChainBuilder` class:

```c#
IMyChain chain = new ResponsibleChainBuilder<IMyChain>()
    .WithLink<MyFirstLink>()
    .WithLink<MySecondLink>()
    .WithLink<DefaultLink>()
    .Build();
```


## Creating Link Classes

An example link might look like this:

```c#
public class MyFirstLink : IMyChain
{
    private readonly IMyChain _nextLink;

    public MyFirstLink(IMyChain nextLink)
    {
        _nextLink = nextLink;
    }

    public string DoSomething(string input)
    {
        if (input == "foo")
        {
            return "bar";
        }

        return _nextLink.DoSomething(input);
    }
}
```

note that the link's constructor takes a reference to the next link in the chain. The ResponsibleChain 
framework will handle instantiating this class with the correct `nextLink` reference passed in.

### Handling Dependencies

Let's say your link class needs another dependency. Your constructor might look something like this:

```c#
 public MyFirstLink(IMyChain nextLink, IMyOtherDependency myOtherDependency)
{
    _nextLink = nextLink;
    _myOtherDependency = myOtherDependency;
}
```

If you're using the service collection injection method of creating your chain, then you just need to
make sure that an instance of `IMyOtherDependency` is added to the service collection

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddResponsibleChain<IMyChain>()
        .WithLink<MyFirstLink>()
        .WithLink<MySecondLink>()
        .WithLink<DefaultLink>();

    // Add IMyOtherDependency to services
    services.AddTransient<IMyOtherDendency, MyOtherDependency>();
    ...
}
```

if you're instantiating the chain manually, you can provide an expression to the `WithLink` Method

```c#
IMyOtherDependency dep = new MyOtherDependency();

IMyChain chain = new ResponsibleChainBuilder<IMyChain>()
    .WithLink<MyFirstLink>(nextLink => new MyFirstLink(nextLink, dep))
    .WithLink<MySecondLink>()
    .WithLink<DefaultLink>()
    .Build();
```

## Constraints

1. Chains should end with a "default" link, which does not take a next link dependency
2. All links must have exactly 1 `public` constructor (or the default constructor)
3. All links must implement the same interface, or inherit from the same base class

## Q & A

### Question: Do my links need to implement a specific interface?

Answer: No, you can make your own interface or base class.

### Question: Why not just instantiate my chain the old fashioned way?

```c#
IMyChain chain = new DefaultLink(
                    new MySecondLink(
                        new MyFirstLink(new MyOtherDependency())));
```

Answer: 
1. This doesn't work well within a dependency injection framework
2. You have to instantiate the chain backwards, which is confusing to read
3. It looks gross.

---

ResponsibleChains is maintained by @CarsonTolleshaug