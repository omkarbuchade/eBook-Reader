//////////////////////////////////////////////////////////////////////////
// FilesController.cs - Web Api for handling Files                      //
// Author - Omkar Buchade, CSE686 - Internet Programming, Spring 2019   //                                                          //
// Source - Jim Fawcett, CSE686 - Internet Programming, Spring 2019     //
//////////////////////////////////////////////////////////////////////////
/*
 * This package implements Controller for Files Web Api.
 * The web api application:
 * - uploads files to wwwroot/FileStore
 * - displays all files in FileStore
 * 
 * Note that Web Api applications don't use action names in their urls.
 * Instead, they use GET, POST, PUT, and DELETE based on the type of
 * the HTTP Request Message.  Also, they don't return views.  They
 * return data.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using WebApplication.Models;
using WebApplication.Data;

namespace WebApplication.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class FilesController : ControllerBase
  {
    private readonly IHostingEnvironment hostingEnvironment_;
        private readonly ApplicationDbContext context_;
        private string webRootPath = null;
    private string filePath = null;

    public FilesController(IHostingEnvironment hostingEnvironment, ApplicationDbContext context)
    {
      hostingEnvironment_ = hostingEnvironment;
      webRootPath = hostingEnvironment_.WebRootPath;
      filePath = Path.Combine(webRootPath, "FileStorage");
      context_ = context;

        }
       

        // GET: api/<controller>
        [HttpGet]
    public IEnumerable<object> Get()
    {
            List<object> files= new List<object>();
            try
            {
                var data = context_.Genres.ToList<Genre>();
                foreach (var item in data)
                {
                    files.Add(item.GenreName);
                    files.Add(item.ID);
                }
            }
            catch
            {
                files.Add("404 - Not Found");
            }
            return files;
        }
    //----< download single file in wwwroot\FileStorage >------
    // GET api/<controller>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Download(int id)
    {
      List<string> files = null;
      string file = "";
      try
      {
        files = Directory.GetFiles(filePath).ToList<string>();
        if (0 <= id && id < files.Count)
          file = Path.GetFileName(files[id]);
        else
          return NotFound();
      }
      catch
      {
        return NotFound();
      }
      var memory = new MemoryStream();
      file = files[id];
      using (var stream = new FileStream(file, FileMode.Open))
      {
        await stream.CopyToAsync(memory);
      }
      memory.Position = 0;
      return File(memory, GetContentType(file), Path.GetFileName(file));
    }
    private string GetContentType(string path)
    {
      var types = GetMimeTypes();
      var ext = Path.GetExtension(path).ToLowerInvariant();
      return types[ext];
    }

    private Dictionary<string, string> GetMimeTypes()
    {
      return new Dictionary<string, string>
      {
        {".cs", "application/C#" },
        {".txt", "text/plain"},
        {".pdf", "application/pdf"},
        {".doc", "application/vnd.ms-word"},
        {".docx", "application/vnd.ms-word"},
        {".xls", "application/vnd.ms-excel"},
        {".xlsx", "application/vnd.openxmlformatsofficedocument.spreadsheetml.sheet"},
        {".png", "image/png"},
        {".jpg", "image/jpeg"},
        {".jpeg", "image/jpeg"},
        {".gif", "image/gif"},
        {".csv", "text/csv"}
      };
    }
    //----< upload file >--------------------------------------

    // POST api/<controller>
    [HttpPost]
    public async Task<IActionResult> Upload()
    {
      var request = HttpContext.Request;
            foreach (var file in request.Form.Files)
            {
                var ext=System.IO.Path.GetExtension(file.FileName);
                if (file.Length > 0 && (System.IO.Path.GetExtension(file.FileName) ==".pdf"))
                {
                    var path = Path.Combine(filePath, file.FileName);
                    string title = request.Form["title"];
                    string author = request.Form["author"];
                    string isbnStr = request.Form["isbn"];
                    int isbn = Int32.Parse(isbnStr);
                    string privStr = request.Form["private"];
                    bool priv;
                    if (privStr == "1")
                        priv = true;
                    else
                        priv = false;
                    string genreidStr = request.Form["genreid"];
                    int genreid = Int32.Parse(genreidStr);
                    string name = file.FileName;

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var book = new Book { Title = title, Author=author, Isbn=isbn,Private=priv,GenreID=genreid,BookName= name };
                    try{
                        context_.Books.Add(book);
                        context_.SaveChanges();
                    }
                    catch(Exception)
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }
      }
      return Ok();
    }
  }
}
