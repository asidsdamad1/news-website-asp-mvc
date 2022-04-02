using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NewsWebsite.ViewModel
{
    public class ListTagViewModel
    {
        public int TagID { get; set; }

        [StringLength(50)]
        [Display(Name = "TagName", ResourceType= typeof(StaticResources.Resources))]
        public string TagName { get; set; }
        public int PostCount { get; set; }
    }
}