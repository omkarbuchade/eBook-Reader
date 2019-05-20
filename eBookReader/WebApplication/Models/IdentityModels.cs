///////////////////////////////////////////////////////////////
// IdentityModel.cs - IdentityModel class for eBook reader.  //
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/*
 * - This package help us to create our own
 *   identity model by inheriting the IdentityUser
 *   and simply add the fields which we need.
 *     
 */
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace WebApplication.Models
{
    public class ApplicationUser : IdentityUser

    {
        //Adding a string type new field while creating a new account. 
        [Required]
        public string Name { get; set; }
    }
}
