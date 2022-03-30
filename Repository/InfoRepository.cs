using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewsWebsite.Repository
{
    public class InfoRepository
    {
        private NewsDbContext entity;// = new DVCPContext();
        public InfoRepository(NewsDbContext context)
        {
            this.entity = context;
        }

        public WebInfo FindByID(int id = 1)
        {
            WebInfo u = entity.infoes.Find(id);
            return u;
        }

        public void Update(WebInfo u)
        {
            entity.Entry(u).State = EntityState.Modified;
        }
    }
}