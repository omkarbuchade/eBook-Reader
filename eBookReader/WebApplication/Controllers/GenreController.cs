///////////////////////////////////////////////////////////////
// GenreController.cs - Controller class for eBook reader.   //
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/*
 * This package implements Controller for eBook Reader.
 * The GenreController application:
 * - CRUD operation from genre table
 * - CRUD operation from books table
 * - CRUD operation from comments table
 * 
 * The GenreController generates corresponding views for 
 * CRUD operation for each of the genre, book and comment model.
 */
 

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace WebApplication.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext context_;
        private const string sessionId_ = "SessionId";
        public GenreController(ApplicationDbContext context)
        {
            context_ = context;
        }

       //-----< Index view for genreController; Returns the Genre DBset >------//
        public IActionResult Index()
        {
            return View(context_.Genres.ToList<Genre>());
        }
        //-----< Returns the Books DBset >------//
        public IActionResult Books()
        {
            return View(context_.Books.ToList<Book>());
        }

        //-----< Get method to Create a new object of genre class and return it to the view >------//
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGenre(int id)
        {
            var model = new Genre();
            return View(model);
        }
        //-----< Method to add the genre class object to genres dbset >------//
        [HttpPost]
        public IActionResult AddGenre(int id, Genre usr)
        {
            context_.Genres.Add(usr);
            context_.SaveChanges();
            return RedirectToAction ("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditGenre(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Genre usr = context_.Genres.Find(id);
            if (usr == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(usr);
        }

        //-----< Method to update the DBset for genre >------//
        [HttpPost]
        public IActionResult EditGenre(int? id, Genre usr)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var userEdit = context_.Genres.Find(id); 
            if (usr != null)
            {
                userEdit.GenreName = usr.GenreName;
                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // error message since data not saved into database
                }
            }
            return RedirectToAction("Index");
        }
        //-----< Method to display the DBset for genres >------//
        public ActionResult GenreDetails(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Genre usr = context_.Genres.Find(id);

            if (usr == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            var bks = context_.Books.Where(l => l.Genre == usr);
            usr.Books = bks.ToList<Book>();
            return View(usr);
        }
        //-----< Method to delete a entry in the genre DBset >------//
        public IActionResult DeleteGenre(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var genre = context_.Genres.Find(id);
                if (genre != null)
                {
                    var bookList = context_.Books.Where(s => s.GenreID == id);

                    if (bookList.Any())
                    {
                        //ViewBag.MyErrorMessage = "The genre is not empty. There are books associated with it. Please delete all books first to remove this genre";
                        return View();
                    }
                    if (!bookList.Any())
                    {
                        context_.Genres.Remove(genre);
                        context_.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                // nothing for now
            }
            return RedirectToAction("Index");
        }
        //-----< Method to display public books in the Book DBset >------//
        public IActionResult BookDet(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Genre usr = context_.Genres.Find(id);

            var bks = context_.Books.Where(l => l.Genre==usr);
            return View(bks);
        }
        //-----< Method to display private books in the Book DBset >------//
        public IActionResult BookDetPrivate(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Genre usr = context_.Genres.Find(id);

            var bks = context_.Books.Where(l => l.Genre == usr);
            return View(bks);
        }
        //-----< Method to create new data for the Book model class >------//
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddBook(int id)
        {
            HttpContext.Session.SetInt32(sessionId_, id);
            Genre usr = context_.Genres.Find(id);
            if (usr == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Book bk = new Book();
            return View(bk);
        }
        //-----< Method that adds a Book object to the books Dbset >------//
        [HttpPost]
        public IActionResult AddBook(int? id, Book bk)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            int? userID = HttpContext.Session.GetInt32(sessionId_);
            var usr = context_.Genres.Find(userID);

            if (usr != null)
            {
                if (usr.Books == null)
                {
                    List<Book> lectures = new List<Book>();
                    usr.Books = lectures;
                }
                usr.Books.Add(bk);

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // throw error message here. Data not saved to DB
                }
            }
            return RedirectToAction("Index");
        }
        //-----< Method to edit a object in the book DBset >------//
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditBook(int? id)
        {
            if (id == null)
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest);
            }
            Book bk = context_.Books.Find(id);

            if (bk == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(bk);
        }
        //-----< Method to edit a object in the book DBset >------//
        [HttpPost]
        public IActionResult EditBook(int? id, Book bk)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            var origBk = context_.Books.Find(id);

            if (origBk != null)
            {
                origBk.Title = bk.Title;
                origBk.Author = bk.Author;
                origBk.Isbn = bk.Isbn;
                origBk.Private = bk.Private;

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // Error Could not write back to the table in database
                }
            }
            //return RedirectToAction("Index");
            if (bk.Private)
                return Redirect("/Genre/BookDetPrivate/" + origBk.GenreID);
            return Redirect("/Genre/BookDet/" + origBk.GenreID);
        }
        //-----< Method to delete selected object from the book DBset >------//
        public IActionResult DeleteBook(int? id)
        {
            bool access = false ;
            int genId=0;
            string name;
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var book = context_.Books.Find(id);
                access = book.Private;
                genId = book.GenreID;
                name = book.BookName;
                if (book != null)
                {
                //Delete existing comments if there are any for the book; Or else we cannot delete the book
                    var comments = context_.BookComments.Where(s => s.BookID == book.BookID);
                    if (comments != null)
                    {
                        foreach (var comment in comments)
                        {
                            context_.BookComments.Remove(comment);
                        }
                    }
                    context_.Remove(book);
                    context_.SaveChanges();
                    try
                    {
                        string filename = "../../../wwwroot/FileStorage/" + name;
                        var basedir = System.AppDomain.CurrentDomain.BaseDirectory;
                        var fullPath = System.IO.Path.Combine(basedir, filename);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                        else
                        {
                            Debug.WriteLine("File does not exist.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception)
            {
                // show error message here
            }
            if(access)
                return Redirect("/Genre/BookDetPrivate/"+genId);
            return Redirect("/Genre/BookDet/" + genId);

            //return RedirectToAction("Index");
        }
        //-----< Method to read the selected book from the book DBset >------//
        public IActionResult ReadBook(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var bk = context_.Books.Find(id);
                if (bk != null)
                {                    
                    return View(bk);
                }
            }
            catch (Exception)
            {
                // show error message here
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        //-----< Method to create a object of BookComment class >------//
        [HttpGet]
        public IActionResult AddComment(int id)
        {
            HttpContext.Session.SetInt32(sessionId_, id);
            Book bk = context_.Books.Find(id);
            if (bk == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            BookComment bkComment = new BookComment();
            bkComment.BookID = id;
            return View(bkComment);
        }
        //-----< Method to add a object of BookComment class to BookComment DB set >------//
        [HttpPost]
        public IActionResult AddComment(int? id, BookComment bkComment)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            int? bookID = HttpContext.Session.GetInt32(sessionId_);
            var bk = context_.Books.Find(bookID);

            if (bk != null)
            {
                if (bk.BookComments == null)
                {
                    List<BookComment> bkComments = new List<BookComment>();
                    bk.BookComments = bkComments;
                }
                bkComment.Username = User.Identity.Name;
                bkComment.Date = DateTime.Now.Date;
                bk.BookComments.Add(bkComment);

                try
                {
                    context_.SaveChanges();
                }
                catch (Exception)
                {
                    // throw error message here. Data not saved to DB
                }
            }
            return Redirect("/Genre/ReadBook/" + bkComment.BookID);
            //return RedirectToAction("ReadBook", bkComment.BookID);// ("ReadBook");
        }
        //-----< Method to display the objects of the BookComments DB set >------//
        public IActionResult BookComments(int? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Book usr = context_.Books.Find(id);

            var bks = context_.BookComments.Where(l => l.Book == usr);
            return View(bks);
        }
        //-----< Method to delete a selected object from BookComment DB set >------//
        public IActionResult DeleteComment(int? id)
        {
            var redirectTo="";
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            try
            {
                var bkComm = context_.BookComments.Find(id);
                redirectTo = "/Genre/BookComments/" + bkComm.BookID;
                if (bkComm != null)
                {
                    context_.Remove(bkComm);
                    context_.SaveChanges();
                }
            }
            catch (Exception)
            {
                redirectTo = "/Genre/";
            }
            //return RedirectToAction("Index");
            return Redirect(redirectTo);

        }
    }
}