namespace GameStore.Api.Dtos;

public record class GameDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate);//declaring out properties inside the parenthesis, that could be id, name or the genre of game like: racing, sports, fighting, Dateonly cause we are keeping the release date of our game not caring the time