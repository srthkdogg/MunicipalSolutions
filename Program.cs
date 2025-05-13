// Required namespaces for Identity and Entity Framework
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MyWebApp;  // Make sure you are using the namespace for your ApplicationDbContext and ApplicationUser

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with your connection string (SQLite or SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity services
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container
builder.Services.AddControllersWithViews();

// Add session services for user sessions
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Use session to handle user data between requests
app.UseSession();

// Enable static file serving (for images, CSS, JS, etc.)
app.UseStaticFiles();

// Set up routing
app.UseRouting();

// Set up authentication and authorization (important for login, register)
app.UseAuthentication();
app.UseAuthorization();

// Map the default route (you can customize the default controller and action)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (enables Identity's default login and register pages)
app.MapRazorPages();  // This is where Identity routes for login and register will be mapped

app.Run();