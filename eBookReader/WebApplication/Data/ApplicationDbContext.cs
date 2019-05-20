///////////////////////////////////////////////////////////////
// ApplicationDbContext.cs -DBContext class for eBook reader.//
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/* 
 * This code creates a DbSet property for each (Genre, Book and Book Comment) entity set. 
 */
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication.Models;
namespace WebApplication.Data

{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }









        public DbSet<Genre> Genres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookComment> BookComments { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
