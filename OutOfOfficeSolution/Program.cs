using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutOfOfficeSolution.Mappings;
using OutOfOfficeSolution.Models;
using OutOfOfficeSolution.Models.Context;
using OutOfOfficeSolution.Services.Implementations;
using OutOfOfficeSolution.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IUserAndRoleManagementService, UserAbdRoleManagementService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IApprovalRequestService, ApprovalRequestService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<ISortService, SortService>();

builder.Services.AddDbContext<OutOfOfficeSolutionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "OutOfOfficeSolutionCookie";
    options.LoginPath = "/Account/Login";
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
})
  .AddEntityFrameworkStores<OutOfOfficeSolutionDbContext>()
  .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.Seed(services);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
