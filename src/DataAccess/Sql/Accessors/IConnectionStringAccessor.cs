namespace VP.Core.DataAccess.Sql.Accessors
{
    /// <summary> IConnectionStringAccessor interface. </summary>
    internal interface IConnectionStringAccessor
    {
        /// <summary>
        /// Gets the connection-string based on the key.
        /// CAUTION: If key is not found, null is returned.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetConnectionString(string key);
    }
}
