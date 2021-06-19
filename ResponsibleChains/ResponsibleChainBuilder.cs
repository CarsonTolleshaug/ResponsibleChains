using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace ResponsibleChains
{
    public interface IResponsibleChainBuilder<T> where T : class
    {
        IResponsibleChainBuilder<T> WithLink<TLink>() where TLink : T;
        IResponsibleChainBuilder<T> WithLink(Type type);
        IResponsibleChainBuilder<T> WithLink(Func<T, T> linkInstantiationFactory);
        IResponsibleChainBuilder<T> WithLinks(IEnumerable<Type> types);
        T Build();
    }

    public class ResponsibleChainBuilder<T> : IResponsibleChainBuilder<T> where T : class
    {
        private readonly Stack<Func<T, T>> _linkBuilders;
        private readonly Lazy<IServiceProvider> _serviceProvider;

        public ResponsibleChainBuilder() : this(new ServiceCollection()) { }

        public ResponsibleChainBuilder(IServiceCollection serviceCollection) : this(new Lazy<IServiceProvider>(serviceCollection.BuildServiceProvider)) { }

        public ResponsibleChainBuilder(IServiceProvider serviceProvider) : this(new Lazy<IServiceProvider>(() => serviceProvider)) { }

        private ResponsibleChainBuilder(Lazy<IServiceProvider> serviceProvider)
        {
            _linkBuilders = new Stack<Func<T, T>>();
            _serviceProvider = serviceProvider;
        }

        public IResponsibleChainBuilder<T> WithLink<TLink>() where TLink : T
        {
            return WithLink(typeof(TLink));
        }

        public IResponsibleChainBuilder<T> WithLink(Type type)
        {
            return WithLink(next => BuildLink(type, next));
        }

        public IResponsibleChainBuilder<T> WithLink(Func<T, T> linkInstantiationFactory)
        {
            _linkBuilders.Push(linkInstantiationFactory);
            return this;
        }

        public IResponsibleChainBuilder<T> WithLinks(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                WithLink(type);
            }

            return this;
        }

        public T Build()
        {
            T nextLink = null;

            foreach (Func<T,T> linkBuilder in _linkBuilders)
            {
                nextLink = linkBuilder(nextLink);
            }

            return nextLink;
        }

        private T BuildLink(Type linkType, T nextLink)
        {
            ConstructorInfo[] constructors = linkType.GetConstructors();

            if (constructors.Length == 0)
            {
                throw new ArgumentException($"No public constructors found for type '{linkType.FullName}'.");
            }

            if (constructors.Length > 1)
            {
                throw new ArgumentException($"Multiple public constructors found for type '{linkType.FullName}'.");
            }

            ConstructorInfo constructor = constructors.Single();

            object[] parameters = constructor.GetParameters().Select(parameterInfo =>
            {
                if (parameterInfo.ParameterType == typeof(T))
                {
                    return nextLink ?? throw new ArgumentException($"Final link of type '{linkType.FullName}' expects next link in constructor, but no link was provided."); ;
                }

                return _serviceProvider.Value.GetRequiredService(parameterInfo.ParameterType);

            }).ToArray();

            return (T)constructor.Invoke(parameters);
        }
    }
}
