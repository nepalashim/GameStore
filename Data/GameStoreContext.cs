using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options) //GameStoreContext should be inherited from Db context
    {
        public DbSet<Game> Games => Set<Game>();

        public DbSet<Genre> Genres=> Set<Genre>();
        

        //public IEnumerable<object> Genres { get; internal set; }

        //this is one of the methods that is going to be executed as soon as the migration is executed

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genre>().HasData(
                new {Id=1, Name="Fighting"},
                new {Id=2, Name="Roleplaying"},
                new {Id=3, Name="Sports"},
                new {Id=4, Name="Racing"},
                new {Id=5, Name="Kids and Family"}
                
            );
        }

        
    }
}