using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ManyToManyRelation.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }         
        public virtual ICollection<PostCategory> PostCategories{get;set;}
        public Post(){
            PostCategories = new Collection<PostCategory>();
        }
        
    }
}