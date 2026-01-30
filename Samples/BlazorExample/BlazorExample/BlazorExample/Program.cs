using BlazorExample.Client.Components.Pages;
using BlazorExample.Components;
using Csla.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddCascadingAuthenticationState();

// CSLA requires AddHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Add CSLA
builder.Services.AddCsla(o => o
  .AddAspNetCore()
  .AddServerSideBlazor(o => o.UseInMemoryApplicationContextManager = false)
  .DataPortal(o => o.AddServerSideDataPortal()));

// configure DAL services for EF or Mock:

//for EF Db
//builder.Services.AddTransient(typeof(DataAccess.IPersonDal), typeof(DataAccess.EF.PersonEFDal));
//builder.Services.AddDbContext<DataAccess.EF.PersonDbContext>(
//options => options.UseSqlServer(builder.Configuration.GetConnectionString("PersonDB") ?? throw new InvalidOperationException("Connection string 'PersonDB' not found.")));

// for Mock Db
builder.Services.AddTransient(typeof(DataAccess.IPersonDal), typeof(DataAccess.Mock.PersonDal));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseWebAssemblyDebugging();
}
else
{
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.Run();
