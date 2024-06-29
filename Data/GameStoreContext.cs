using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data
{
    public class GameStoreContext (DbContextOptions<GameStoreContext> options) : DbContext(options) //GameStoreContext should be inherited from Db context
    {
        public DbSet<Game> Games => Set<Game>();

        public DbSet<Genre> Genre => Set<Genre>();
        
    }
}