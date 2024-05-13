using ebooks_dotnet7_api;
using Microsoft.AspNetCore.Http.HttpResults;
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
ebooks.MapGet("/genre={genre}&author={author}&format={format}", FilterEBooksAsync);
ebooks.MapGet("/{id}", ObtainEBookByIDAsync);
ebooks.MapPut("/{id}", UpdateEBookAsync);
ebooks.MapPut("/{id}/change-availability", ProvideEBookAsync);
ebooks.MapPut("/{id}/increment-stock", IncrementStockAEbookAsync);
ebooks.MapPost("/purchase", BuyEBookAsync);
ebooks.MapDelete("/{id}", DeleteEBookAsync);


app.Run();

// TODO: Add more methods
async Task<IResult> CreateEBookAsync([FromBody] AddEbookDTO addEbookByDTO , DataContext context)
{
    EBook? eBook = await context.EBooks.Where(e => e.Title == addEbookByDTO.Title && e.Author == addEbookByDTO.Author).FirstOrDefaultAsync();
    if (eBook != null) return Results.BadRequest("Ya existe un libro como este");

    EBook newEBook = new EBook{
        Title = addEbookByDTO.Title,
        Author = addEbookByDTO.Author,
        Genre = addEbookByDTO.Genre,
        Format = addEbookByDTO.Format,
        Price = addEbookByDTO.Price,
        IsAvailable = true,
        Stock = 0
    };
    await context.AddAsync(newEBook);
    await context.SaveChangesAsync();
    
    return Results.Ok(newEBook);
}


async Task<List<EBook>> ObtainEBooksAsync(DataContext context){

    List<EBook> eBooks = await context.EBooks.OrderBy(e => e.Title).ToListAsync();

    return eBooks;
    
}

async Task<List<EBook>> FilterEBooksAsync(string Author, string Format, string Genre, DataContext context){

    List<EBook> eBooks = await context.EBooks
        .Where(e => e.Format.Contains(Format) &&
         e.Genre.Contains(Genre) && e.Author.Contains(Author))
        .OrderBy(e => e.Title)
        .ToListAsync();

    return eBooks;
}

async Task<IResult> ObtainEBookByIDAsync(int id,DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(id);
    if(eBook == null) return Results.BadRequest("ese libro no existe");
   
    return Results.Ok(eBook);
}

async Task<IResult> UpdateEBookAsync(int id, AddEbookDTO ebookDTO, DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(id);
    if(eBook == null) return Results.BadRequest("ese libro no existe");

    eBook.Title = ebookDTO.Title;
    eBook.Author = ebookDTO.Author;
    eBook.Genre = ebookDTO.Genre;
    eBook.Format = ebookDTO.Format;
    eBook.Price = ebookDTO.Price;

    await context.SaveChangesAsync();
    return Results.Ok("Ese Ebook ha sido actualizado");
}

async Task<IResult> ProvideEBookAsync(int id, DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(id);
    if(eBook == null) return Results.BadRequest("ese libro no existe");
    eBook.IsAvailable = !eBook.IsAvailable;

    await context.SaveChangesAsync();
    return Results.Ok("Ese Ebook ha cambiado su disponibilidad: " + eBook.IsAvailable);
}

async Task<IResult> IncrementStockAEbookAsync(int id, stockDTO stockDTO, DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(id);
    if(eBook == null) return Results.BadRequest("ese libro no existe");
    eBook.Stock = eBook.Stock + stockDTO.Stock;

    await context.SaveChangesAsync();
    return Results.Ok("Stock añadido");
}

async Task<IResult> BuyEBookAsync([FromBody] PurchaseDTO purchaseDTO, DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(purchaseDTO.Id);
    if(eBook == null || eBook.IsAvailable == false) return Results.BadRequest("ese libro no esta disponible");

    if(eBook.Stock < purchaseDTO.quantity) return Results.BadRequest("No se puede comprar más de lo que hay");
    int total = eBook.Price * purchaseDTO.quantity;
    if(purchaseDTO.pay < total) return Results.BadRequest("Tiene que pagar esa cantidad, no hay descuento");

    int returned = purchaseDTO.pay - total;

    eBook.Stock = eBook.Stock - purchaseDTO.quantity;

    await context.SaveChangesAsync();
    return Results.Ok("Libro comprado, tu vuelto es: " + returned);
}

async Task<IResult> DeleteEBookAsync(int id, DataContext context){

    EBook? eBook = await context.EBooks.FindAsync(id);
    if(eBook == null) return Results.BadRequest("ese libro no existe o fue eliminado");
    context.EBooks.Remove(eBook);
    await context.SaveChangesAsync();
    return Results.Ok("El objeto ha sido eliminado");
}