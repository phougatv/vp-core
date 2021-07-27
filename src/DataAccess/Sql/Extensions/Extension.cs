namespace VP.Core.DataAccess.Sql.Extensions
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using VP.Core.DataAccess.Sql.Attributes;

    public static class Extension
    {
        private static readonly BindingFlags PublicNonPublicInstanceBindingFlag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        #region Entity Extension Methods
        /// <summary> Adds instance property name as stored procedure parameter with value. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity, <see cref="T"/>.</param>
        /// <param name="performParamBinding">The perform param binding delegate, <see cref="Action{SqlParameterCollection}"/>.</param>
        /// <returns></returns>
        public static Action<SqlParameterCollection> AddPropertiesAsParametersWithValues<T>(this T entity, Action<SqlParameterCollection> performParamBinding)
            where T : Entity
        {
            performParamBinding = p =>
            {
                foreach (var property in entity.GetType().GetProperties(PublicNonPublicInstanceBindingFlag))
                {
                    var value = property.GetValue(entity, null);
                    p.AddWithValue($"@{property.Name}", value);
                }
            };

            return performParamBinding;
        }

        /// <summary> Adds the specified parameter, using <see cref="PropertyParameterBinderAttribute"/>, as stored procedure parameter with value. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="performParamBinding"></param>
        /// <returns></returns>
        public static Action<SqlParameterCollection> AddSpecifiedParametersWithValues<T>(this T entity, Action<SqlParameterCollection> performParamBinding)
            where T : Entity
        {
            performParamBinding = p =>
            {
                foreach (var property in entity.GetType().GetProperties(PublicNonPublicInstanceBindingFlag))
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
        #endregion Entity Extension Methods

        #region IDataReader Extension Methods
        /// <summary> Reads the value of the specified column name. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader">The data reader, <see cref="IDataReader"/>.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns></returns>
        public static T Read<T>(this IDataReader dataReader, string columnName)
        {
            if (null == dataReader)
                throw new ArgumentNullException(nameof(dataReader));
            if (null == columnName)
                throw new ArgumentNullException(nameof(columnName));
            if (string.IsNullOrEmpty(columnName))
                throw new ArgumentException($"{nameof(columnName)} cannot be empty.");

            var value = dataReader[columnName];
            if (null == value || DBNull.Value == value)
                return default;
            return (T)value;
        }

        /// <summary> Reads the value of the specified column index. </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataReader">The data reader, <see cref="IDataReader"/>.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns></returns>
        public static T Read<T>(this IDataReader dataReader, int columnIndex)
        {
            if (null == dataReader)
                throw new ArgumentNullException(nameof(dataReader));
            if (0 > columnIndex)
                throw new ArgumentException($"{nameof(columnIndex)} must be zero or +ve int.");

            var value = dataReader[columnIndex];
            if (null == value || DBNull.Value == value)
                return default;
            return (T)value;
        }
        #endregion IDataReader Extension Methods
    }
}
