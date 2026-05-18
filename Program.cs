using Microsoft.EntityFrameworkCore;
using pryLPWeb_DisTerminal.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TerminalDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ConexionTerminal")));

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
