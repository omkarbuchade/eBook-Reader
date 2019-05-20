///////////////////////////////////////////////////////////////
// ConsoleClient.cs - Client for WebApi FilesController      //
//                                                           //
// Jim Fawcett, CSE686 - Internet Programming, Spring 2019   //
///////////////////////////////////////////////////////////////
/*
 * - Based on Asp.Net Core Framework, this client project generates
 *   dynamic link library that can be hosted by Visual Studio or
 *   dotnet CLI.
 * - It provides options via its command line, e.g.:
 *   - url /fl            displays list of files in server's FileStore
 *   - url /up fileSpec   uploades fileSpec to FileStore
 *   - url /dn n          downloads nth file in FileStore
 *   - url /dl n          deletes nth file in FileStore
 */
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;  // must install Newtonsoft package from Nuget
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ConsoleClient
{
  class CoreConsoleClient
  {
    public HttpClient client { get; set; }

    private string baseUrl_;

    CoreConsoleClient(string url)
    {
      baseUrl_ = url;
      client = new HttpClient();
    }
    //----< upload file >--------------------------------------

    public async Task<HttpResponseMessage> SendFile(String Title, String Author, String Isbn, String Priv, String GenreID, String fileSpec)
    {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            byte[] data = File.ReadAllBytes(fileSpec);
            ByteArrayContent bytes = new ByteArrayContent(data);
            string fileName = Path.GetFileName(fileSpec);
            multiContent.Add(bytes, "files", fileName);

            byte[] title = Encoding.ASCII.GetBytes(Title);
            ByteArrayContent titleBytes = new ByteArrayContent(title);

            byte[] author = Encoding.ASCII.GetBytes(Author);
            ByteArrayContent authorBytes = new ByteArrayContent(author);

            byte[] isbn = Encoding.ASCII.GetBytes(Isbn);
            ByteArrayContent isbnBytes = new ByteArrayContent(isbn);

            byte[] priv = Encoding.ASCII.GetBytes(Priv);
            ByteArrayContent privBytes = new ByteArrayContent(priv);

            byte[] genreId = Encoding.ASCII.GetBytes(GenreID);
            ByteArrayContent genreIdBytes = new ByteArrayContent(genreId);

            multiContent.Add(titleBytes, "title");
            multiContent.Add(authorBytes, "author");
            multiContent.Add(isbnBytes, "isbn");
            multiContent.Add(privBytes, "private");
            multiContent.Add(genreIdBytes, "genreid");
            return await client.PostAsync(baseUrl_, multiContent);
            ///call some method here
        }
    //----< get list of files in server FileStorage >----------

    public async Task<IEnumerable<object>> GetFileList()
    {
      HttpResponseMessage resp = await client.GetAsync(baseUrl_);
      var files = new List<object>();
      if (resp.IsSuccessStatusCode)
      {
        var json = await resp.Content.ReadAsStringAsync();
        JArray jArr = (JArray)JsonConvert.DeserializeObject(json);
        foreach (var item in jArr)
          files.Add(item.ToString());
      }
      return files;
    }
    //----< download the id-th file >--------------------------

    public async Task<HttpResponseMessage> GetFile(int id)
    {
      return await client.GetAsync(baseUrl_ + "/" + id.ToString());
    }
    //----< delete the id-th file from FileStorage >-----------

    public async Task<HttpResponseMessage> DeleteFile(int id)
    {
      return await client.DeleteAsync(baseUrl_ + "/" + id.ToString());
    }
    //----< usage message shown if command line invalid >------

    static void showUsage()
    {
      Console.Write("\n  Command line syntax error: expected usage:\n");
      Console.Write("\n    http[s]://machine:port /option [filespec]\n\n");
    }

    static void Main(string[] args)
    {

      Console.Write("\n  CoreConsoleClient");
      Console.Write("\n ===================\n");
      string url = args[0];
      int ch = 1;
            do
            {
                Console.Write("\nChoices \n 1.Upload file \n 2. Exit \n");
                Console.Write("Enter choice: ");
                int choice = Int32.Parse(Console.ReadLine());
                CoreConsoleClient client = new CoreConsoleClient(url);
                switch (choice)
                {

                    case 1:
                        try
                        {
                            Console.Write("\n  sending request to {0}\n", url);
                            Task<IEnumerable<object>> tfl = client.GetFileList();
                            var resultfl = tfl.Result;
                            int i = 0;
                            foreach (var item in resultfl)
                            {
                                if (i % 2 == 0)
                                {
                                    Console.Write("\n Genre Name: {0}  ", item);
                                    i++;
                                }
                                else if (i % 2 != 0)
                                {
                                    Console.Write("GenreID: {0} ", item);
                                    i++;
                                }
                            }
                            Console.Write("\nEnter GenreId in which you wish to add book: ");
                            string genreId = Console.ReadLine();

                            Console.Write("\nEnter book title: ");
                            string title = Console.ReadLine();

                            Console.Write("\nEnter book author: ");
                            string author = Console.ReadLine();

                            Console.Write("\nEnter book ISBN: ");
                            string isbn = Console.ReadLine();

                            Console.Write("\nBook Access (Enter 1 for private, or 0 to keep book public): ");
                            string priv = Console.ReadLine();

                            Console.Write("\nEnter book path (only pdf files supported): ");
                            string path = Console.ReadLine();

                            Task<HttpResponseMessage> tup = client.SendFile(title, author, isbn, priv, genreId, path);
                            Console.Write(tup.Result);
                        }
                        catch (Exception)
                        {
                            Console.Write("\nError...");
                        }
                        break;
                    case 2:
                        break;
                    default:
                        Console.Write("\nIncorrect choice, try again...");
                        break;
                }
            Console.Write("\nPress 1 to continue, 0 to quit: ");
            ch =Int32.Parse(Console.ReadLine());
            } while (ch == 1);
    }
  }
}
