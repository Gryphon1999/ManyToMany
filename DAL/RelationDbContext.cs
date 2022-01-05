using ManyToManyRelation.Models;
using Microsoft.EntityFrameworkCore;

namespace ManyToManyRelation.DAL
{
    public class RelationDbContext: DbContext
    {
        public RelationDbContext(DbContextOptions<RelationDbContext>options):base(options)
        {

        }
        public DbSet<Student> students { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
    }
}
