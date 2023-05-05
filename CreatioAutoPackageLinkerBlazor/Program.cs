using CreatioAutoPackageLinkerBlazor;
using CreatioAutoPackageLinkerBlazor.Data;
using CreatioAutoPackageLinkerBlazor.Data.DbClass.Repository;
using CreatioAutoPackageLinkerBlazor.Data.Rest;
using CreatioAutoPackageLinkerBlazor.Services;
using Hangfire;
using Hangfire.SQLite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddScoped<ICreatioService, CreatioRestService>();
builder.Services.AddScoped<LinkerService>();
builder.Services.AddSingleton<SignalRService>();
builder.Services.AddSingleton<SignalRHub>();
builder.Services.AddResponseCompression();
builder.Services.AddHangfire(config => config
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireConnection"),
        new SQLiteStorageOptions {
            InvisibilityTimeout = TimeSpan.FromDays(365 * 100),
            JobExpirationCheckInterval = TimeSpan.FromHours(1),
            CountersAggregateInterval = TimeSpan.FromHours(1)
        })
);
builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseResponseCompression();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHangfireDashboard();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<SignalRHub>("/myHub");
app.MapFallbackToPage("/_Host");

app.Run();