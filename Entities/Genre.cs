using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace GameStore.Api.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public required string Name { get; set; } // Name can't be nullable so I made this name field to be mandatory. They have to provide the value for name when they construct the object
        
        
    }
}