using System;

namespace SqlBatis.DataMapper.DependencyInjection
{
    /// <summary>
    /// A dependency resolver that can create a type from the default <see cref="IServiceProvider"/>
    /// using named <see cref="ISqlMapper"/> registrations
    /// </summary>
    public interface INamedMapperDependencyResolver
    {
        /// <summary>
        /// Gets an instance of <typeparamref name="TImplementation"/> using a named <see cref="ISqlMapper"/>
        /// instance (registered in DI via <code>services.AddSqlMapper(name)</code>) 
        /// </summary>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        TImplementation GetInstance<TImplementation>(string name);
    }
}