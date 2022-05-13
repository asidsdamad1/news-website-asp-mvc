using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace NewsWebsite.ViewModel
{
    public class ListPostViewModel
    {
        [Key]
        public int post_id { get; set; }

        public int? userid { get; set; }

        [Required]
        [StringLength(200)]
        public string post_title { get; set; }

        [Required]
        [StringLength(500)]
        public string post_teaser { get; set; }

        [StringLength(500)]
        public string post_review { get; set; }


        public int post_type { get; set; }


        public DateTime? create_date { get; set; }

        public DateTime? edit_date { get; set; }


        public int ViewCount { get; set; }

        public int Rated { get; set; }

        [StringLength(200)]
        public string AvatarImage { get; set; }

        public bool status { get; set; }
        public string slug { get; set; }
        public string tagsname { get; set; }
    }
}