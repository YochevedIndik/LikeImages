﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageShareWithLikes.Data
{
   public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }
        public int Likes { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
