using System.Collections.Specialized;

namespace SqlBatis.DataMapper.Configuration
{
    /// <summary>
    /// Represents a builder that can configure an <see cref="ISqlMapper"/> instance
    /// </summary>
    public interface IDomSqlMapBuilder
    {
        /// <summary>
        /// Allow properties to be set before configuration.
        /// </summary>
        NameValueCollection Properties { set; }

        /// <summary>
        /// Configure an ISqlMapper object from a file path.
        /// </summary>
        /// <param name="resource">
        /// A relative resource path from your Application root 
        /// or a absolute file path file:\\c:\dir\a.config
        /// </param>
        /// <returns>An ISqlMapper instance.</returns>
        ISqlMapper Configure(string resource);
    }
}