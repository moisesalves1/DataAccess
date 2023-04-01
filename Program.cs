using System;
using Dapper;
using Microsoft.Data.SqlClient;
using DataAccess.Models;

namespace DataAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;password=1q2w3e4r@#$; TrustServerCertificate=True";

            
            using (var connection = new SqlConnection(connectionString))
            {         
                
                // CreateCategory(connection);  
                // CreateManyCategory(connection);  
                // UpdateCategory(connection); 
                // DeleteCategory(connection); 
                // ListCategories(connection);
                // GetCategory(connection);  
                // ExecuteProcedure(connection);
                // ExecuteReadProcedure(connection);
                // ExecuteScalar(connection);
                ReadView(connection);
                
                
            }

        }

        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id], [Title] FROM [Category]");
                foreach(var categoria in categories)
                {
                    Console.WriteLine($"{categoria.Id} - {categoria.Title}");
                }
        }
    
        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO
                [Category] 
             VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)";

                var rows = connection.Execute(insertSql, new {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }); 

                Console.WriteLine($"{rows} linhas inseridas");
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection
                .QueryFirstOrDefault<Category>(
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                    new
                    {
                        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                    });
            Console.WriteLine($"{category.Id} - {category.Title}");

        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title]=@title WHERE [Id]=@id";
            var rows = connection.Execute(updateQuery, new {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2023"
            });

            Console.WriteLine($"{rows} registros atualizados");

        }

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE FROM [Category] WHERE [Id]=@id";
            var rows = connection.Execute(deleteQuery, new {
                id = new Guid("4e827812-52d8-4bb9-9626-5b87581855bd")
            });

            Console.WriteLine($"{rows} registros excluídos");

        }

        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria";
            category2.Url = "categoria";
            category2.Description = "Categoria nova";
            category2.Order = 9;
            category2.Summary = "Nova Categoria";
            category2.Featured = false;

            var insertSql = @"INSERT INTO
                [Category] 
             VALUES(
                @Id, 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)";

                var rows = connection.Execute(insertSql, new[]{ 
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }}); 

                Console.WriteLine($"{rows} linhas inseridas");
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var sql = "[spDeleteStudent]";
            var pars = new { StudentId = "3af85085-5f74-49ab-b4a3-0a040a748aec"};
            var affectedRows = connection.Execute(sql, pars, commandType: System.Data.CommandType.StoredProcedure);
            Console.WriteLine($"{affectedRows} linhas afetadas");
        }

        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var sql = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142"};
            var courses = connection.Query(sql, pars, commandType: System.Data.CommandType.StoredProcedure);
            
            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO
                [Category] 
            OUTPUT inserted.[Id]
             VALUES(
                NEWID(), 
                @Title, 
                @Url, 
                @Summary, 
                @Order, 
                @Description, 
                @Featured)";

                var id = connection.ExecuteScalar<Guid>(insertSql, new {
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }); 

                Console.WriteLine($"A cateogira inserida foi {id}");
        }

        static void ReadView(SqlConnection connection)
        {
            var sql = "SELECT * FROM [vwCourses]";
            var courses = connection.Query(sql);
                foreach(var item in courses)
                {
                    Console.WriteLine($"{item.Id} - {item.Title}");
                }
        }

        

    }
}