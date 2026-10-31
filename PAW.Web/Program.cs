using APW.Architecture;
using PAW.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRestProvider, RestProvider>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserActionService, UserActionService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

builder.Services.AddHttpClient<TipoCambioClient>(client =>
{
    client.BaseAddress =
        new Uri("https://api.hacienda.go.cr/");

    client.Timeout = TimeSpan.FromSeconds(10);

    client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "Mozilla/5.0 (compatible; PAW/1.0)");

    client.DefaultRequestHeaders.Accept.ParseAdd(
        "application/json");

    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(
        "es-CR,es;q=0.9");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//Nuevo clase para el servicio de tipo de cambio: Clase 4


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapGet(
    "/exchange-rate",
    async (
        IExchangeRateService service,
        CancellationToken ct) =>
    {
        var result =
            await service.GetExchangeRateAsync(ct);

        return Results.Ok(result);
    });

app.Run();
