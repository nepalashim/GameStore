using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
//manually database update naagarnu paros bhanera we created this file
namespace GameStore.Api.Data
{
    public static class DataExtensions// this method is going to extend webapplication object /class, to migrate our database
    {
        public static void MigrateDb(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
            dbContext.Database.Migrate();
            // We are now ready to execute migration on Startup


        }

        
    }
}