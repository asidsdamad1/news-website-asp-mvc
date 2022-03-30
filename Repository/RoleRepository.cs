using NewsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NewsWebsite.Repository
{
    public class RoleRepository
    {
        private NewsDbContext entity;// = new ();
        public RoleRepository(NewsDbContext context)
        {
            this.entity = context;
        }

        public Role FindByID(int id)
        {
            Role u = entity.Roles.Find(id);
            return u;
        }

        public void Update(Role u)
        {
            entity.Entry(u).State = EntityState.Modified;
        }
    }
}