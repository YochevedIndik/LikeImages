using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageShareWithLikes.Data
{
    public class ImagesRepository
    {
        private string _connectionString;
        public ImagesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Image> GetAll()
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.OrderByDescending(image => image.DateUploaded).ToList();
        }
        public void UploadImage(Image image)
        {
            using var context = new ImageDataContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }
        public Image GetImageById(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);

        }
        public int GetLikesPerImage(int id)
        {
            using var context = new ImageDataContext(_connectionString);
            Image image = context.Images.FirstOrDefault(i => i.Id == id);
            return image.Likes;
        }
        public void AddLikes(int id)
        {
            
            using var context = new ImageDataContext(_connectionString);
            Image image = context.Images.FirstOrDefault(i => i.Id == id);
            image.Likes++;
            context.SaveChanges();
        }
    }
}
