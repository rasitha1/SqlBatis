namespace SqlBatis.DataMapper
{
    /// <summary>
    /// A factory for getting an instance of <see cref="ISqlMapper"/>.
    /// </summary>
    public interface ISqlMapperFactory
    {
        /// <summary>
        /// Gets the default instance of <see cref="ISqlMapper"/>.
        /// </summary>
        /// <returns></returns>
        ISqlMapper GetMapper();

        /// <summary>
        /// Gets a named instance of <see cref="ISqlMapper"/>
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ISqlMapper GetMapper(string name);
    }
}