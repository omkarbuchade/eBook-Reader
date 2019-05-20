# eBook-Reader

Website built using ASP.Net Core MVC for reading eBooks in pdf format.

The website consists of a public static section using HTML5, CSS3, and ES6 and a private password protected section using ASP.Net Core MVC. 
Users have the ability to view eBooks that are available publicly and to view private eBooks the user needs to create their own private account. Admin has the ability to upload, delete or modify eBooks and make them available publicly or privately.

User is provided with the functionality to change the viewing scale of the eBook and to iterate through pages of the book. Authenticted users are provided with an additional functionality to comment on these eBooks for review. 

The Webservice is built using Asp.Net Web API, to upload the eBooks from local filesystem to the site and the client technology uses the .Net Core HttpClient class.
