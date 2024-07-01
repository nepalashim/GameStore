using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Mapping;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName ="GetGame";

        //defining a very simple list of games

        // private static readonly List<GameSummaryDto> games = [
        //     new(
        //         1,
        //         "St",
        //         "Fight",
        //         19.99M,
        //         new DateOnly(1992, 7, 15)),

        //     new(
        //         2,
        //         "Final Fantasy",
        //         "RoleplayGenre",
        //         59.99M,
        //         new DateOnly(2022, 12, 11)),

        //     new(
        //         3,
        //         "FIFA",
        //         "Sports",
        //         67.77M,
        //         new DateOnly(2021, 9, 23)
        //     )  
        // ];
        // as we no longer require the in- memory list of games
        public static RouteGroupBuilder MapGamesEndpoints( this WebApplication app)
        {
            var group= app.MapGroup("games")
                        .WithParameterValidation();
            //GET /games
            group.MapGet("/",async (GameStoreContext dbContext) => await dbContext.Games
                            .Include(game => game.Genre)
                            .Select(game => game.ToGameSummaryDto())
                            .AsNoTracking()
                            .ToListAsync());

            //create a request that can be used to retrieve not all games but just one games by ID
            
            //GET /games/1
            group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                // GameDto? game = games.Find(game => game.Id ==id);

                Game? game = await dbContext.Games.FindAsync(id);



                return game is null ? Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
                
                })
                .WithName(GetGameEndpointName);

            //endpoint for posting into games
            //POST/games
            //also converting CreateGameDto into normal GameDto to ad it in our in-memory list
            group.MapPost("/",async (CreateGameDto newGame, Data.GameStoreContext dbContext) =>
            { 
                // if(string.IsNullOrEmpty(newGame.Name)){
                //     return Results.BadRequest("Name Field is Required");
                // }
                // if(string.IsNullOrEmpty(newGame.Genre)){
                //     return Results.BadRequest("Genre Field is Required");
                // }

                Game game = newGame.ToEntity();
                // game.Genre = dbContext.Genre.Find(newGame.GenreId);


                // Game game = new()
                // {
                //     Name = newGame.Name,
                //     Genre = dbContext.Genre.Find(newGame.GenreId),
                //     GenreId = newGame.GenreId,
                //     Price = newGame.Price,
                //     ReleaseDate = newGame.ReleaseDate

                // };// the above is the creation of our entity

                /*GameDto game = new(
                    games.Count +1, //this is going to be our game ID one additional from current count
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate);   */

                // games.Add(game);

                dbContext.Games.Add(game); // adding to dbContext so Entity framework and Db Context keep track of this brand new entity that will need to be stored in the database
                await dbContext.SaveChangesAsync();// insert new record into the games table in sql database



                // GameDto gameDto = new(
                //     game.Id,
                //     game.Name,
                //     game.Genre!.Name,
                //     game.Price,
                //     game.ReleaseDate);


                // to return response
                return Results.CreatedAtRoute(GetGameEndpointName,new{id=game.Id},game.ToGameDetailsDto());


            });
            // .WithParameterValidation();

            //define our put endpoint for updating the resources

            //Put/games/1
            group.MapPut("/{id}",async (int id, UpdateGameDto updatedGame, GameStoreContext dbContext )=>
            {
                // var index= games.FindIndex(game => game.Id == id);

                var existingGame =await dbContext.Games.FindAsync(id);


                if (existingGame is null)
                {
                    return Results.NotFound();
                }
                
                // games[existingGame] = new GameSummaryDto(
                //     id,
                //     updatedGame.Name,
                //     updatedGame.Genre,
                //     updatedGame.Price,
                //     updatedGame.ReleaseDate);

                dbContext.Entry(existingGame)
                            .CurrentValues
                            .SetValues(updatedGame.ToEntity(id));

                await dbContext.SaveChangesAsync();

                return Results.NoContent();


            });


            //DELETE/games/1
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext)=>{
                // 
                await dbContext.Games
                        .Where(game => game.Id == id)
                        .ExecuteDeleteAsync();// go into database, find the entity and delete it right away

                return Results.NoContent();
            }
            );

            return group;

        }
        
    }
}