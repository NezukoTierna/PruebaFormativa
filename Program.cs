using ebooks_dotnet7_api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("ebooks"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var ebooks = app.MapGroup("api/ebook");

// TODO: Add more routes
ebooks.MapPost("/", CreateEBookAsync);
ebooks.MapGet("/", ObtainEBooksAsync);
ebooks.MapGet("/?genre={genre}&author={author}&format={format}", FilterEBooksAsync);
ebooks.MapGet("/{id}", ObtainEBookByIDAsync);
ebooks.MapPut("/{id}", UpdateEBookAsync);
ebooks.MapPut("/{id}/change-availability", ProvideEBookAsync);
ebooks.MapPut("/{id}/increment-stock", IncrementStockAEbookAsync);
ebooks.MapPost("/purchase", BuyEBookAsync);
ebooks.MapDelete("/{id}", DeleteEBookAsync);


app.Run();

// TODO: Add more methods
async Task<IResult> CreateEBookAsync(DataContext context)
{
    return Results.Ok();
}

async Task<IResult> ObtainEBooksAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> FilterEBooksAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> ObtainEBookByIDAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> UpdateEBookAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> ProvideEBookAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> IncrementStockAEbookAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> BuyEBookAsync(DataContext context){

    return Results.Ok();
}

async Task<IResult> DeleteEBookAsync(DataContext context){

    return Results.Ok();
}