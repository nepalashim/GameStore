
using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

//This is the contract or DTOS we are using for returning or posting data into the api

public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(15)] string Genre,
    [Range(1,100)] decimal Price,
    DateOnly ReleaseDate
     
);
