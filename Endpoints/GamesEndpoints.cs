using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Api.Dtos;

namespace GameStore.Api.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName ="GetGame";

        //defining a very simple list of games
        private static readonly List<GameDto> games = [
            new(
                1,
                "St",
                "Fight",
                19.99M,
                new DateOnly(1992, 7, 15)),

            new(
                2,
                "Final Fantasy",
                "RoleplayGenre",
                59.99M,
                new DateOnly(2022, 12, 11)),

            new(
                3,
                "FIFA",
                "Sports",
                67.77M,
                new DateOnly(2021, 9, 23)
            )  
        ];

        public static RouteGroupBuilder MapGamesEndpoints( this WebApplication app)
        {
            var group= app.MapGroup("games")
                        .WithParameterValidation();
            //GET /games
            group.MapGet("/",()=>games);

            //create a request that can be used to retrieve not all games but just one games by ID
            
            //GET /games/1
            group.MapGet("/{id}",(int id)=>
            {
                GameDto? game = games.Find(game => game.Id ==id);

                return game is null ? Results.NotFound() : Results.Ok(game);
                
                })
                .WithName(GetGameEndpointName);

            //endpoint for posting into games
            //POST/games
            //also converting CreateGameDto into normal GameDto to ad it in our in-memory list
            group.MapPost("",(CreateGameDto newGame) =>
            { 
                // if(string.IsNullOrEmpty(newGame.Name)){
                //     return Results.BadRequest("Name Field is Required");
                // }
                // if(string.IsNullOrEmpty(newGame.Genre)){
                //     return Results.BadRequest("Genre Field is Required");
                // }
                GameDto game = new(
                    games.Count +1, //this is going to be our game ID one additional from current count
                    newGame.Name,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate);

                games.Add(game);

                // to return response
                return Results.CreatedAtRoute(GetGameEndpointName,new{id=game.Id},game);


            });
            // .WithParameterValidation();

            //define our put endpoint for updating the resources

            //Put/games/1
            group.MapPut("/{id}",(int id, UpdateGameDto updatedGame )=>
            {
                var index= games.FindIndex(game => game.Id == id);

                if (index== -1)
                {
                    return Results.NotFound();
                }
                
                games[index] = new GameDto(
                    id,
                    updatedGame.Name,
                    updatedGame.Genre,
                    updatedGame.Price,
                    updatedGame.ReleaseDate);

                return Results.NoContent();


            });


            //DELETE/games/1
            group.MapDelete("/{id}",(int id)=>{
                games.RemoveAll(game=> game.Id ==id); 

                return Results.NoContent();
            }
            );

            return group;

        }
        
    }
}