using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PeopleAPI.Data;

namespace PeopleAPI.Models
{
    public static class PrepDB
    {
        
        public static void PrepPopulation(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());        
            }

        }

        public static void SeedData(AppDbContext context)
        {
            System.Console.WriteLine("Applying Migrations... ");
            context.Database.Migrate();

            if(!context.Persons.Any())
            {
                 System.Console.WriteLine("Adding data - seeding...");
                 context.AddRange(

                     new Person(){Name="Tim", Gender="male"}

                 );
                 context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data - not seeding");
            }

        }


    }
}