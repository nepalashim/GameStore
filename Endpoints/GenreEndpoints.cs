using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Data;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using GameStore.Api.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;


namespace GameStore.Api.Endpoints;

public static class GenreEndpoints
{
    public static RouteGroupBuilder MapGenresEndpoints( this WebApplication app)
    {
        var group = app.MapGroup("genres");

        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                        .Select( genre => genre.ToDto())
                        .AsNoTracking()
                        .ToListAsync());
        return group;
    }
}