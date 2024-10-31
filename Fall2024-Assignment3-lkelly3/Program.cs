using Fall2024_Assignment3_lkelly3.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Fall2024_Assignment3_lkelly3.Models;
using Azure.AI.OpenAI;
using Microsoft.Build.Framework;
using OpenAI.Chat;
using NuGet.ProjectModel;
using Fall2024_Assignment3_lkelly3.Services;
using static Fall2024_Assignment3_lkelly3.Services.OpenAIhit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<OpenAIhit>();


OpenAIhit openAI = new OpenAIhit(builder.Configuration);

//var secret = builder.Configuration["ConnectionStrings:DefaultConnection"];
// Add services to the container.


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."); ;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

await seedActors(app.Services);
await seedMovies(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

var OpenAIKey = app.Configuration["OpenAIKey"];

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

async Task seedActors(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "ActorPhotos");

        if (!context.Actors.Any()) {
            context.Actors.AddRange(
                new Actors
                {
                    Name = "Chris Pratt",
                    Gender = "Male",
                    Age = 45,
                    Link = "https://www.imdb.com/name/nm0695435/",
                    Photo = File.Exists(Path.Combine(basePath, "Pratt.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Pratt.jpeg"))
                    : null

                },

                new Actors
                {
                    Name = "Tom Cruise",
                    Gender = "Male",
                    Age = 62,
                    Link = "https://www.imdb.com/name/nm0000129/",
                    Photo = File.Exists(Path.Combine(basePath, "Cruise.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Cruise.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Daniel Craig",
                    Gender = "Male",
                    Age = 56,
                    Link = "https://www.imdb.com/name/nm0185819/",
                    Photo = File.Exists(Path.Combine(basePath, "Craig.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Craig.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Christian Bale",
                    Gender = "Male",
                    Age = 56,
                    Link = "https://www.imdb.com/name/nm0000288/",
                    Photo = File.Exists(Path.Combine(basePath, "Bale.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Bale.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Javier Bardem",
                    Gender = "Male",
                    Age = 55,
                    Link = "https://www.imdb.com/name/nm0000849/",
                    Photo = File.Exists(Path.Combine(basePath, "Bardem.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Bardem.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Dave Bautista",
                    Gender = "Male",
                    Age = 55,
                    Link = "https://www.imdb.com/name/nm1176985/",
                    Photo = File.Exists(Path.Combine(basePath, "Bautista.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Bautista.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Tom Hardy",
                    Gender = "Male",
                    Age = 47,
                    Link = "https://www.imdb.com/name/nm0362766/",
                    Photo = File.Exists(Path.Combine(basePath, "Hardy.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Hardy.jpeg"))
                    : null
                },

                new Actors
                {
                    Name = "Austin Butler",
                    Gender = "Male",
                    Age = 33,
                    Link = "https://www.imdb.com/name/nm2581521/",
                    Photo = File.Exists(Path.Combine(basePath, "Butler.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Butler.jpeg"))
                    : null
                }
            );

            await context.SaveChangesAsync();

        }
    }
}



async Task seedMovies(IServiceProvider serviceProvider)
{
    using (var scope = serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        string basePath = Path.Combine(Directory.GetCurrentDirectory(), "MoviePosters");

        //context.Movies.AddRange(
        //        new Movies
        //        {
        //            Title = "Skyfall",
        //            Link = "https://www.imdb.com/title/tt1074638/",
        //            Genre = "Action",
        //            Year = 2012,
        //            Poster = File.Exists(Path.Combine(basePath, "Skyfall.jpeg"))
        //            ? File.ReadAllBytes(Path.Combine(basePath, "Skyfall.jpeg"))
        //            : null

        //        }
        //        );
        //await context.SaveChangesAsync();



        if (!context.Movies.Any())
        {
            context.Movies.AddRange(
                new Movies
                {
                    Title = "Guardians of the Galaxy",
                    Link = "https://www.imdb.com/title/tt2015381/",
                    Genre = "Action",
                    Year = 2014,
                    Poster = File.Exists(Path.Combine(basePath, "Guardians.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Guardians.jpeg"))
                    : null

                },

                new Movies
                {
                    Title = "The Dark Knight Rises",
                    Link = "https://www.imdb.com/title/tt1345836/",
                    Genre = "Action",
                    Year = 2012,
                    Poster = File.Exists(Path.Combine(basePath, "DarkKnight.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "DarkKnight.jpeg"))
                    : null

                },

                new Movies
                {
                    Title = "The Bikeriders",
                    Link = "https://www.imdb.com/title/tt21454134/",
                    Genre = "Crime Drama",
                    Year = 2023,
                    Poster = File.Exists(Path.Combine(basePath, "Bikeriders.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Bikeriders.jpeg"))
                    : null

                },

                new Movies
                {
                    Title = "Dune: Part Two",
                    Link = "https://www.imdb.com/title/tt15239678/",
                    Genre = "Action",
                    Year = 2024,
                    Poster = File.Exists(Path.Combine(basePath, "DuneTwo.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "DuneTwo.jpeg"))
                    : null

                },

                new Movies
                {
                    Title = "Terminator Salvation",
                    Link = "https://www.imdb.com/title/tt0438488/",
                    Genre = "Action",
                    Year = 2009,
                    Poster = File.Exists(Path.Combine(basePath, "Terminator.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Terminator.jpeg"))
                    : null

                },

                new Movies
                {
                    Title = "Mission Impossible",
                    Link = "https://www.imdb.com/title/tt0117060/",
                    Genre = "Action",
                    Year = 1996,
                    Poster = File.Exists(Path.Combine(basePath, "Mission.jpeg"))
                    ? File.ReadAllBytes(Path.Combine(basePath, "Mission.jpeg"))
                    : null

                }


            );

            await context.SaveChangesAsync();

        }
    }
}




