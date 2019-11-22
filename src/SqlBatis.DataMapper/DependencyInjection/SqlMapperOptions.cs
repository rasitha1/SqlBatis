using System.Collections.Generic;

namespace IBatisNet.DataMapper.DependencyInjection
{
    /// <summary>
    /// Options for configuring an <see cref="ISqlMapper"/>
    /// </summary>
    public class SqlMapperOptions
    {
        /// <summary>
        /// Gets or sets the Resource
        /// </summary>
        /// <remarks>
        /// A relative resource path from your Application root 
        /// or a absolute file path file:\\c:\dir\a.config
        /// </remarks>
        public string Resource { get; set; }

        /// <summary>
        /// Gets the optional map of key-value parameters
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}