using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddDbContext<SystemizeContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8; // Minimum password length of 8
    options.Password.RequireNonAlphanumeric = true; // At least one special character
    options.Password.RequireUppercase = true; // At least one uppercase letter
    options.Password.RequireLowercase = true; // At least one lowercase letter
})
.AddEntityFrameworkStores<SystemizeContext>()
.AddDefaultTokenProviders();

// Configure the application cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    // Redirect unauthorized users to the custom Unauthorized page
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToAccessDenied = context =>
        {
            context.Response.Redirect("/Home/Unauthorized");
            return Task.CompletedTask;
        },
        OnRedirectToLogin = context =>
        {
            context.Response.Redirect("/Account/Login");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}




app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Workflow}/{action=Index}/{id?}")
    .WithStaticAssets();






app.Run();
