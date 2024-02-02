using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using FilmRecensioner.Configuration;
using FilmRecensioner.Data;

var app = new FilmRecensionApp(args);

app.Run();