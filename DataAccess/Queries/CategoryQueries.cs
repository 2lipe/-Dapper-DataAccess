using Dapper;
using DataAccess.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccess.Queries
{
    public static class CategoryQueries
    {
        public static void SelectCategory(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        [Id],
                        [Title]
                    FROM 
                        [Category]";

            var categories = connection.Query<Category>(query);

            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        public static void InsertCategory(SqlConnection connection)
        {
            var query = @"
                    INSERT INTO 
                        [Category] 
                    VALUES
                        (
                            @Id,
                            @Title,
                            @Url,
                            @Summary,
                            @Order,
                            @Description,
                            @Featured
                        )";

            var category = new Category();

            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon-aws";
            category.Summary = "AWS Cloud";
            category.Order = 8;
            category.Description = "Categoria destinada a serviços do AWS";
            category.Featured = false;

            var rows = connection.Execute(query, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });

            Console.WriteLine($"{rows} linhas afetadas");
        }

        public static void UpdateCategory(SqlConnection connection)
        {
            var query = @"
                    UPDATE
                        [Category]
                    SET 
                        [Title] = @title
                    WHERE
                        [Id] = @Id";

            var rows = connection.Execute(query, new
            {
                id = new Guid("1bfd7b85-6f0f-416c-9c31-355d394dc48e"),
                title = "Banckend NodeJS"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }

        public static void InsertManyCategories(SqlConnection connection, IList<Category> categories)
        {
            var query = @"
                    INSERT INTO 
                        [Category] 
                    VALUES
                        (
                            @Id,
                            @Title,
                            @Url,
                            @Summary,
                            @Order,
                            @Description,
                            @Featured
                        )";

            var rows = connection.Execute(query, categories);

            Console.WriteLine($"{rows} linhas afetadas");
        }

        public static void ExecuteDeleteProcedure(SqlConnection connection)
        {
            var procedure = "spDeleteStudent";
            var data = new { StudentId = "a6a95223-d084-4026-b75d-1f77621f55ef" };
            
            var rows = connection.Execute(procedure, data, commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{rows} linhas afetadas");
        }

        public static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "spGetCoursesByCategory";
            var data = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

            var courses = connection.Query<Course>(procedure, data, commandType: CommandType.StoredProcedure);

            foreach (var course in courses)
            {
                Console.WriteLine
                    ($@"
                        Category:                        
                            Id - {course.Id}
                            Tag - {course.Tag}
                            Title - {course.Title}
                            Summary - {course.Summary}
                            Url - {course.Url}
                            Level - {course.Level}
                            DurationInMinutes - {course.DurationInMinutes}
                            CreateDate - {course.CreateDate}
                            LastUpdate - {course.LastUpdate}
                            Active - {course.Active}
                            Free - {course.Free}
                            Featured - {course.Featured}
                            AuthorId - {course.AuthorId}
                            CategoryId - {course.CategoryId}
                            Tags - {course.Tags}
                       "
                    );
            }
        }

        public static void ExecuteScalar(SqlConnection connection, Category category)
        {
            var query = @"
                    INSERT INTO
                        [Category] 
                    OUTPUT
                        inserted.[Id]
                    VALUES
                        (
                            NEWID(), 
                            @Title, 
                            @Url, 
                            @Summary, 
                            @Order, 
                            @Description, 
                            @Featured
                        )";

            var id = connection.ExecuteScalar<Guid>(query, category);

            Console.WriteLine($"A categoria inserida foi: {id}");
        }

        public static void ReadView(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        * 
                    FROM 
                        [vwCourses]";

            var courses = connection.Query<Course>(query);

            foreach (var course in courses)
            {
                Console.WriteLine($"{course.Id} - {course.Title}");
            }
        }

        public static void OneToOne(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        * 
                    FROM 
                        [CareerItem] 
                    INNER JOIN 
                        [Course] ON [CareerItem].[CourseId] = [Course].[Id]";

            var careerItens = connection.Query<CareerItem, Course, CareerItem>(
                query,
                (careerItem, course) =>
                {
                    careerItem.Course = course;
                    return careerItem;
                }, splitOn: "Id");

            foreach (var careerItem in careerItens)
            {
                Console.WriteLine($"{careerItem.Title} - Curso: {careerItem.Course.Title}");
            }
        }

        public static void OneToMany(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        [Career].[Id],
                        [Career].[Title],
                        [CareerItem].[CareerId],
                        [CareerItem].[Title]
                    FROM 
                        [Career] 
                    INNER JOIN 
                        [CareerItem] ON [CareerItem].[CareerId] = [Career].[Id]
                    ORDER BY
                        [Career].[Title]";

            var careers = new List<Career>();

            var items = connection.Query<Career, CareerItem, Career>(
                query,
                (career, item) =>
                {
                    var car = careers.Where(x => x.Id == career.Id).FirstOrDefault();
                    if (car == null)
                    {
                        car = career;
                        car.CareerItems.Add(item);
                        careers.Add(car);
                    }
                    else
                    {
                        car.CareerItems.Add(item);
                    }

                    return career;
                }, splitOn: "CareerId");

            foreach (var career in careers)
            {
                Console.WriteLine($"{career.Title}");
                foreach (var item in career.CareerItems)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }
        }

        public static void QueryMutiple(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        * 
                    FROM 
                        [Category];

                    SELECT 
                         * 
                    FROM 
                        [Course]";

            using (var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>();
                var courses = multi.Read<Course>();

                foreach (var item in categories)
                {
                    Console.WriteLine(item.Title);
                }

                foreach (var item in courses)
                {
                    Console.WriteLine(item.Title);
                }
            }
        }

        public static void SelectIn(SqlConnection connection)
        {
            var query = @"
                    SELECT 
                        * 
                    FROM 
                        [Career] 
                    WHERE 
                        [Id] IN @Id";

            var careers = connection.Query<Career>(query, new
            {
                Id = new[]{
                    "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                    "e6730d1c-6870-4df3-ae68-438624e04c72"
                }
            });

            foreach (var career in careers)
            {
                Console.WriteLine(career.Title);
            }

        }

        public static void SelectLike(SqlConnection connection, string term)
        {
            var query = @"
                    SELECT 
                        * 
                    FROM 
                        [Course]
                    WHERE 
                        [Title] LIKE @exp";

            var courses = connection.Query<Course>(query, new
            {
                exp = $"%{term}%"
            });

            foreach (var course in courses)
            {
                Console.WriteLine(course.Title);
            }
        }

        public static void Transaction(SqlConnection connection, Category category)
        {
            var query = @"
                    INSERT INTO 
                        [Category] 
                    VALUES
                        (
                            @Id, 
                            @Title, 
                            @Url, 
                            @Summary, 
                            @Order, 
                            @Description, 
                            @Featured
                        )";

            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(query, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);

                transaction.Commit();
                //transaction.Rollback();

                Console.WriteLine($"{rows} linhas inseridas");
            }
        }
    }
}
