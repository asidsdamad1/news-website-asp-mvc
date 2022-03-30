using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebsite.ViewModel
{
    public class UploadViewModel
    {
        [Required]
        public int id { get; set; }
        public string title { get; set; }
    }
}