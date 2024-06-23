using Microsoft.AspNetCore.Identity;
using OutOfOfficeSolution.Models;

public class DbInitializer
{
    public static async Task Seed(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        string[] roleNames = ["Admin", "PM", "HR", "Employee"];

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<int>(roleName));
            }
        }

        if (userManager.Users.Count() == 0)
        {
            var user = new User
            {
                UserName = $"admin@a.com",
                Email = $"admin@a.com",
                EmailConfirmed = true,
                Employee = new Employee
                {
                    FullName = "Євгеній Палуб",
                    Subdivision = "IT",
                    Position = "Admin",
                    Status = "Active",
                    PeoplePartner = null,
                    OutOfOfficeBalance = 10,
                    Photo = string.Empty
                }
            };

            var userResult = await userManager.CreateAsync(user, "12345678");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            user = new User
            {
                UserName = $"userPM@a.com",
                Email = $"userPM@a.com",
                EmailConfirmed = true,
                Employee = new Employee
                {
                    FullName = "Віктор Дудка",
                    Subdivision = "IT",
                    Position = "Project Manager",
                    Status = "Active",
                    PeoplePartner = null,
                    OutOfOfficeBalance = 15,
                    Photo = string.Empty
                }
            };

            userResult = await userManager.CreateAsync(user, "12345678");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "PM");
            }

            user = new User
            {
                UserName = $"userHR@a.com",
                Email = $"userHR@a.com",
                EmailConfirmed = true,
                Employee = new Employee
                {
                    FullName = "Сергій Притула",
                    Subdivision = "HR",
                    Position = "HR Manager",
                    Status = "Active",
                    PeoplePartner = null,
                    OutOfOfficeBalance = 12,
                    Photo = string.Empty
                }
            };

            userResult = await userManager.CreateAsync(user, "12345678");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "HR");
            }

            user = new User
            {
                UserName = $"user@a.com",
                Email = $"user@a.com",
                EmailConfirmed = true,
                Employee = new Employee
                {
                    FullName = "Володимир Зеленський",
                    Subdivision = "IT",
                    Position = "Developer",
                    Status = "Active",
                    PeoplePartner = 3,
                    OutOfOfficeBalance = 5,
                    Photo = string.Empty
                }
            };

            userResult = await userManager.CreateAsync(user, "12345678");

            if (userResult.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Employee");
            }
        }
    }
}
