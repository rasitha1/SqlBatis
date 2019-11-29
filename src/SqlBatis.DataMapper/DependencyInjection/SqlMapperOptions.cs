using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        /// or, for an embedded file, use embedded://assembly.qualified.resource.name
        /// </remarks>
        [Required(AllowEmptyStrings = false, ErrorMessage = "A value for 'Resource' must be provided")]
        public string Resource { get; set; }

        /// <summary>
        /// Gets the optional map of key-value parameters
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

        internal bool ConfigurationComplete { get; set; }
    }
}