///////////////////////////////////////////////////////////////
// roles.cs - RoleManager class for eBook reader.            //
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/*
 * - This package is a manager to create Roles for users who register
 * - There are 2 roles defned: admin and user
 * - Admin has all the priviliges that a super user has
 * - User however is given restricted access
 * - This code is responsible to create a new account for the admin if it does not exist
 * - The credential for the admin account are located under the appsettings.json file 
 */


using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication.Models;

namespace WebApplication.Data
{
    public static class roles
    {
    //------------< Method to create roles: user and admin and create a admin account > ---------------------//
        public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        {
            //adding customs roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var SignInManager = serviceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //creating a super user who could maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = Configuration.GetSection("AppSettings")["UserEmail"],
                Email = Configuration.GetSection("AppSettings")["UserEmail"],
                Name = Configuration.GetSection("AppSettings")["FirstName"]
        };
            string userPassword = Configuration.GetSection("AppSettings")["UserPassword"];
            var user = await UserManager.FindByEmailAsync(Configuration.GetSection("AppSettings")["UserEmail"]);

            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
