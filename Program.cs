using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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
app.MapGet("games/{id}",(int id)=>games.Find(game => game.Id ==id))
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
    


// app.MapGet("/", () => "Hello World!");

app.Run();
