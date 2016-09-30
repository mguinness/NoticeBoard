using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoticeBoard.Models
{
    public class ContentContext : DbContext
    {
        public ContentContext(DbContextOptions<ContentContext> options)
          : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>().ToTable("Content").HasIndex(c => new { c.Path });
        }

        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
    }
}
