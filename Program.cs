using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);


//actually connecting to sqlite

// var connString = "Data Source = GameStore.db"; //connection string

//appsettings.json file ma Connection String define garepachi we are using builder to get the key of Connection Settings from appsettings.json file

var connString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext> (connString);   // this line is registering  our DbContext / GameStoreContext into the service provider, which is a service container and it is doing so with a Scoped life time

// As we are adding .AddSqlite here, entity framework is going to take care of connString
//then it is going to create and instance of GameStoreContext and it is going to pass in DB Context options over there, that are going to contain all of the details that are in that connection string that can connect to our Database and map the entities to the table

var app = builder.Build();

//Cleaning things just because we moved this into the GamesEndpoints file

/*
const string GetGameEndpointName ="GetGame";

//defining a very simple list of games
List<GameDto> games = [
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
//GET /games
app.MapGet("games",()=>games);

//create a request that can be used to retrieve not all games but just one games by ID
 
//GET /games/1
app.MapGet("games/{id}",(int id)=>
{
    GameDto? game = games.Find(game => game.Id ==id);

    return game is null ? Results.NotFound() : Results.Ok(game);
    
    })
    .WithName(GetGameEndpointName);

//endpoint for posting into games
//POST/games
//also converting CreateGameDto into normal GameDto to ad it in our in-memory list
app.MapPost("games",(CreateGameDto newGame) =>
{ 
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

//define our put endpoint for updating the resources

//Put/games/1
app.MapPut("games/{id}",(int id, UpdateGameDto updatedGame )=>
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
app.MapDelete("games/{id}",(int id)=>{
    games.RemoveAll(game=> game.Id ==id); 

    return Results.NoContent();
}
);
    */


// app.MapGet("/", () => "Hello World!");
app.MapGamesEndpoints();
app.MapGenresEndpoints();
await app.MigrateDbAsync();
app.Run();
