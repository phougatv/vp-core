namespace VP.Core.DataAccess.Sql.Extensions
{
    using System;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using VP.Core.DataAccess.Sql.Attributes;

    public static class EntityExtension
    {
        private static readonly BindingFlags BindingPublicNonPublicInstanceFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static Action<SqlParameterCollection> AddPropertiesAsParametersWithValues<T>(this T entity, Action<SqlParameterCollection> performParamBinding)
            where T : Entity
        {
            performParamBinding = p =>
            {
                foreach (var property in entity.GetType().GetProperties(BindingPublicNonPublicInstanceFlag))
                {
                    var value = property.GetValue(entity, null);
                    p.AddWithValue($"@{property.Name}", value);
                }
            };

            return performParamBinding;
        }

        public static Action<SqlParameterCollection> AddSpecifiedParametersWithValues<T>(this T entity, Action<SqlParameterCollection> performParamBinding)
        {
            performParamBinding = p =>
            {
                foreach (var property in entity.GetType().GetProperties(BindingPublicNonPublicInstanceFlag))
                {
                    var attribute = (PropertyParameterBinderAttribute)property.GetCustomAttributes().FirstOrDefault(p => p is PropertyParameterBinderAttribute);
                    if (null == attribute)
                        continue;

                    var value = property.GetValue(entity, null);
                    p.AddWithValue(attribute.StoredProcParamName, value);
                }
            };

            return performParamBinding;
        }
    }
}
