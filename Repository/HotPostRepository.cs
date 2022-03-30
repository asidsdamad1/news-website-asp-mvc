using NewsWebsite.Models;
using System.Linq;
using System.Data.Entity;
using System;

namespace NewsWebsite.Repository
{
    public class HotPostRepository
    {
        private NewsDbContext entity;
        public HotPostRepository(NewsDbContext context)
        {
            this.entity = context;
        }
        public void AddHotPost(StickyPost post)
        {
            entity.StickyPosts.Add(post);
        }
        public IQueryable<StickyPost> AllPosts()
        {
            IQueryable<StickyPost> query = entity.StickyPosts;
            return query.AsQueryable();
        }
        public void DeletePost(StickyPost post)
        {
            entity.StickyPosts.Remove(post);

        }
        public void UpdatePost(StickyPost post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public StickyPost FindByID(int id)
        {
            return entity.StickyPosts.Find(id);
        }
        public void SaveChanges()
        {
            entity.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}