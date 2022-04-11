using ImageShareWithLikes.Data;
using ImageShareWithLikes.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;




namespace ImageShareWithLikes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            List<Image> images = repo.GetAll();
            var vm = new ImagesViewModel()
            {
                Images = images
            };
            return View(vm);
        }
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(string title, IFormFile image)
        {
            string fileName = $"{Guid.NewGuid()}-{image.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            image.CopyTo(fs);
            Image img = new Image()
            {
                Title = title,
                ImageLocation = fileName,
                DateUploaded = DateTime.Now,
                Likes = 0
            };
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            repo.UploadImage(img);
            return Redirect("/");
        }
        public IActionResult ViewImage(int id)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            var image = repo.GetImageById(id);
            var likedImage = HttpContext.Session.Get<List<int>>("LikedImage");
            if(likedImage == null)
            {
                likedImage = new List<int>();
            }
            var liked = likedImage.Contains(id);
            ViewImageViewModel vm = new ViewImageViewModel()
            {
                Image = image,
                Liked = liked
            };
            return View(vm);

        }
        public IActionResult GetLikes(int imageId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            int likes = repo.GetLikesPerImage(imageId);
            return Json(likes);
        }
        [HttpPost]
        public void AddLike(int imageId)
        {
            var connectionString = _configuration.GetConnectionString("ConStr");
            var repo = new ImagesRepository(connectionString);
            var likedImage = HttpContext.Session.Get<List<int>>("LikedImage");
            if (likedImage == null)
            {
                likedImage = new List<int>();
            }
            likedImage.Add(imageId);
            HttpContext.Session.Set("LikedImage", likedImage);
            
            repo.AddLikes(imageId);

        }
    }
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonConvert.DeserializeObject<T>(value);
        }

    }
}
