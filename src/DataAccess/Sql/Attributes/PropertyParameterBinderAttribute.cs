namespace VP.Core.DataAccess.Sql.Attributes
{
    using System;

    /// <summary>
    /// PropertyParameterBinderAttribute class.
    /// Binds a property of the class to the specified stored-procedure parameter name.
    /// </summary>
    public class PropertyParameterBinderAttribute : Attribute
    {
        public string StoredProcParamName { get; }

        public PropertyParameterBinderAttribute(string storedProcParamName)
        {
            StoredProcParamName = storedProcParamName;
        }
    }
}
