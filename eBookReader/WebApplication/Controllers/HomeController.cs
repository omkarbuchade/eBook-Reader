///////////////////////////////////////////////////////////////
// HomeController.cs - Home controllerclass for eBook reader.//
//                                                           //
// Omkar Buchade, CSE686 - Internet Programming, Spring 2019 //
///////////////////////////////////////////////////////////////
/*
 * 
 * This package implements Controller for eBook Reader.
 * The HomeController application:
 * - Create a index method to display the homepage of the website
 * 
 * 
 */
using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Data;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context_;
        private const string sessionId_ = "SessionId";
        public HomeController(ApplicationDbContext context)
        {
            context_ = context;
        }
        
        //------------< Index method for HomeController; This is the homepage of the website ----->//
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
