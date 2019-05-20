///////////////////////////////////////////////////////////////
// Genre.cs - Model class for eBook reader.                  //
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/*
 * - This package implements 3 classes.
 *   Each class is a model for the 
 *   Genre, Book and Book comments
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace WebApplication.Models
{
//-----------< Genre class that stores the GenreName and one to many relation to books model >-----------
    public class Genre
    {
        public int ID { get; set; }
        [Required, Display(Name="Genre Name")]
        [RegularExpression(@"[a-zA-Z]+[ ]*[a-zA-Z]*", ErrorMessage = "Please enter valid genre name")]
        public string GenreName { get; set; }
        public ICollection<Book> Books { get; set; }
    }
    //-----------< Book class that stores the book details and one to many relation to comments model >-----------
    //-----------< It also establishes a one to one relation to the genre model >-----------
    public class Book
    {
        public int BookID { get; set; }

        [Required, Display(Name = "Book title")]
        [RegularExpression(@"[a-zA-Z]+[ ]*[a-zA-Z]*", ErrorMessage = "Please enter valid Book title")]
        public string Title { get; set; }

        [Required, Display(Name = "Book Author")]
        [RegularExpression(@"[a-zA-Z]+[ ]*[a-zA-Z]*", ErrorMessage = "Please enter valid Book Author")]
        public string Author { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Please enter ISBN")]
        public long Isbn { get; set; }

        public bool Private { get; set; }

        [Required]
        public string BookName { get; set; }
        public int GenreID { get; set; }
        //public int? GenreID { get; set; }

        public Genre Genre { get; set; }
        public ICollection<BookComment> BookComments { get; set; }
    }
    //-----------< Book Comment class that stores the comment details and one to many relation to book model >-----------
    public class BookComment
    {
        public int BookCommentID { get; set; }
        public string Username { get; set; }
        [Required, Display(Name = "comment")]
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public int? BookID { get; set; }
        public Book Book { get; set; }
    }
}
