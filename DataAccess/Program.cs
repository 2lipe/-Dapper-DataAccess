using Dapper;
using DataAccess.Mocks;
using DataAccess.Model;
using DataAccess.Queries;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=reallyStrongPwd!";

            using (var connection = new SqlConnection(connectionString))
            {
                //CategoryQueries.UpdateCategory(connection);

                //CategoryQueries.InsertManyCategories(connection, CategoryMocks.Categories());

                //CategoryQueries.ExecuteDeleteProcedure(connection);

                //CategoryQueries.SelectCategory(connection);

                //CategoryQueries.ExecuteReadProcedure(connection);

                //CategoryQueries.InsertCategory(connection);

                //CategoryQueries.ExecuteScalar(connection, CategoryMocks.CategoryScalar());

                //CategoryQueries.ReadView(connection);

                //CategoryQueries.OneToOne(connection);

                //CategoryQueries.OneToMany(connection);

                //CategoryQueries.QueryMutiple(connection);

                //CategoryQueries.SelectIn(connection);

                //CategoryQueries.SelectLike(connection, "front");

                CategoryQueries.Transaction(connection, CategoryMocks.CategoryTransaction());
            }            
        }
    }
}
