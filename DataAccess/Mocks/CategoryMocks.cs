using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Mocks
{
    public static class CategoryMocks
    {
        public static IList<Category> Categories()
        {
            var categories = new List<Category>();

            categories.Add(new Category()
            {
                Id = Guid.NewGuid(),
                Title = "Azure",
                Url = "azure",
                Summary = "Administrador Azure",
                Order = 9,
                Description = "Categoria destinada a serviços do Azure",
                Featured = true
            });
            categories.Add(new Category()
            {
                Id = Guid.NewGuid(),
                Title = "QA Tester",
                Url = "qa-tester",
                Summary = "Quality Tester",
                Order = 10,
                Description = "Categoria destinada a Teste de Qualidade de Softwares",
                Featured = true
            });

            return categories;
        }

        public static Category CategoryScalar()
        {
            var category = new Category();

            category.Title = "SQL Server";
            category.Url = "sql-server";
            category.Description = "Categoria destinada a serviços SQL Server";
            category.Order = 11;
            category.Summary = "SQL Server";
            category.Featured = false;

            return category;
        }

        public static Category CategoryTransaction()
        {
            var category = new Category();

            category.Id = Guid.NewGuid();
            category.Title = "Teste";
            category.Url = "categoria-teste";
            category.Summary = "Categoria Teste";
            category.Order = 12;
            category.Description = "Categoria destinada a Teste";
            category.Featured = true;

            return category;
        }
    }
}
