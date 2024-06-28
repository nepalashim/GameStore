
namespace GameStore.Api.Dtos;

//This is the contract or DTOS we are using for returning or posting data into the api

public record class CreateGameDto(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
     
);
