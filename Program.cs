var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//redirige automaticamente todas las peticiones HTTP a HTTPS
app.UseHttpsRedirection();
//permite servir archivos estaticos como CSS, JS, imagenes, etc. desde la carpeta wwwroot.
app.UseStaticFiles();

//habilita el enrutamiento de la aplicacion (mapeo URLs a endpoints)
app.UseRouting();

//habilita la autorizacion (control de acceso a paginas o acciones protegidas)
app.UseAuthorization();

//mapeo de las paginas razor al pipeline de la app.
app.MapRazorPages();

//inicia la app y hace conexion con las solicitudes HTTP.
app.Run();
