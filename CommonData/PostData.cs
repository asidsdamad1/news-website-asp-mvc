using NewsWebsite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewsWebsite.CommonData
{
    public class PostData
    {
        public static List<SelectListItem> getTagList()
        {
            UnitOfWork db = new UnitOfWork(new Models.NewsDbContext());
            List<SelectListItem> listTag = db.tagRepository.AllTags()
                .Select(m => new SelectListItem
                {
                    Text = m.tag_name,
                    Value = m.tag_id.ToString()
                }).ToList();
            return listTag;
        }
    }
}