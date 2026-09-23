var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient(); // ✅ Required for API calls
builder.Services.AddSession();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<AdminService>(); // Lisätty AdminService
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TekosyyService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    dbInitializer.InitializeDatabase();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();


app.MapGet("/", context =>
{

    if (context.Session.GetString("Username") != null)
    {

        context.Response.Redirect("/TekosyyGeneraattori");
        return Task.CompletedTask;
    }
    else
    {

        context.Response.Redirect("/Login");
        return Task.CompletedTask;
    }
});

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();