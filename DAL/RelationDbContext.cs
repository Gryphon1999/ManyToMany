using ManyToManyRelation.Models;
using Microsoft.EntityFrameworkCore;

namespace ManyToManyRelation.DAL
{
    public class RelationDbContext: DbContext
    {
        public RelationDbContext(DbContextOptions<RelationDbContext>options):base(options)
        {

        }
        public DbSet<Post> posts { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<PostCategory>().HasKey(pc=>new {pc.PostId,pc.CategoryId});
            modelBuilder.Entity<Post>().Property(p=>p.Title).IsRequired();
            modelBuilder.Entity<Category>().Property(p=>p.Title).IsRequired();
        }
    }
}
