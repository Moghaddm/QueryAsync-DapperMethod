using System;
using Microsoft.Data.SqlClient;

namespace query;

public static class DapperExtensions<T>
    where T : new()
{
    public static async Task<IEnumerable<T>> QueryAsync(string sql, params object[] paramters)
    {
        using (
            var connection = new SqlConnection(
                "Server = localhost;"
                    + "Database = Dapperdb;"
                    + "User Id = sa;"
                    + "Password = mohammad2005;"
                    + "TrustServerCertificate = true"
            )
        )
        {
            connection.Open();
            var parameterNames = new List<string>();
            for (int i = 0; i < sql.Length; i++)
            {
                if (sql[i] == '@')
                {
                    var spaceFound = sql.IndexOf(" ", i + 1);
                    var sub = sql.Substring(i, spaceFound - i);
                    parameterNames.Add(sub);
                }
            }
            var sqlCommand = new SqlCommand(sql);
            for (int i = 0; i < paramters.Count(); i++)
            {
                // sqlCommand.CreateParameter();
                sqlCommand.Parameters.AddWithValue(parameterNames[i], paramters[i].ToString());
            }
            var dataReader = await sqlCommand.ExecuteReaderAsync();
            List<T> objects = new List<T>();
            var properties = dataReader.GetType().GetProperties();
            while (dataReader.Read())
            {
                var item = new T();
                foreach (var property in properties)
                {
                    if (dataReader.IsDBNull(dataReader.GetOrdinal(property.Name)))
                    {
                        property.SetValue(item, dataReader[property.Name]);
                    }
                }
                objects.Add(item);
            }
            return objects;
        }
    }

    // public static async Task<IEnumerable<T>> QueryAsync(string sql)
    // {
    //     using (
    //         var connection = new SqlConnection(
    //             "Server = localhost;"
    //                 + "Database = Dapperdb;"
    //                 + "User Id = sa;"
    //                 + "Password = mohammad2005;"
    //                 + "TrustServerCertificate = true"
    //         )
    //     )
    //     {
    //         connection.Open();
    //         var sqlCommand = new SqlCommand(sql);
    //         var dataReader = await sqlCommand.ExecuteReaderAsync();
    //         List<T> objects = new List<T>();
    //         var properties = dataReader.GetType().GetProperties();
    //         while (dataReader.Read())
    //         {
    //             var item = new T();
    //             foreach (var property in properties)
    //             {
    //                 if (dataReader.IsDBNull(dataReader.GetOrdinal(property.Name)))
    //                 {
    //                     property.SetValue(item, dataReader[property.Name]);
    //                 }
    //             }
    //             objects.Add(item);
    //         }
    //         return objects;
    //     }
    // }
}
