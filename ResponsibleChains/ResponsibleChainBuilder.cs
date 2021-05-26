using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ResponsibleChains
{
    public interface IResponsibleChainBuilder<T> where T : class
    {
        IResponsibleChainBuilder<T> WithLink<TLink>() where TLink : T;
        IResponsibleChainBuilder<T> WithLink(Type type);
        IResponsibleChainBuilder<T> WithLinks(IEnumerable<Type> types);
        T Build();
    }

    public class ResponsibleChainBuilder<T> : IResponsibleChainBuilder<T> where T : class
    {
        private readonly Stack<Type> _chainLinks;
        private readonly IServiceCollection _serviceCollection;

        public ResponsibleChainBuilder() : this(new ServiceCollection()) { }

        public ResponsibleChainBuilder(IServiceCollection serviceCollection) 
        {
            _chainLinks = new Stack<Type>();
            _serviceCollection = serviceCollection;
        }

        public IResponsibleChainBuilder<T> WithLink<TLink>() where TLink : T
        {
            return WithLink(typeof(TLink));
        }

        public IResponsibleChainBuilder<T> WithLink(Type type)
        {
            _chainLinks.Push(type);

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
            T nextLink = BuildLink(_chainLinks.Pop(), null);
            foreach (Type type in _chainLinks)
            {
                nextLink = BuildLink(type, nextLink);
            }
            return nextLink;
        }

        private T BuildLink(Type linkType, T nextLink)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
