using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManyToManyRelation.DAL;
using ManyToManyRelation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ManyToManyRelation.Controllers
{
    public class PostController : Controller
    {
        private readonly RelationDbContext context;

        public PostController(RelationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var post = context.posts.Include(p=>p.PostCategories)
            .ThenInclude(c=>c.Category)
            .OrderByDescending(p=>p.Id);
            return View(post);
        }        

        public IActionResult Create()
        {
            ViewData["categories"]= context.categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post post, int[] SelectedCategoryIds)
        {
            foreach(var item in SelectedCategoryIds){
                post.PostCategories.Add(new PostCategory{CategoryId = item});
            }
            context.Add(post);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ViewData["categories"] = context.categories.ToList();
            var post = context.posts.Include(p=>p.PostCategories).FirstOrDefault(x=>x.Id==id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(Post post, int[] SelectedCategoryIds)
        {
            //remove selected category
            var removeSelected = new List<PostCategory>();
            foreach(var postCategories in context.PostCategories){
                if(!SelectedCategoryIds.Contains(postCategories.CategoryId)){
                removeSelected.Add(postCategories);
                }
            }
            foreach(var postCategory in removeSelected){
                context.PostCategories.Remove(postCategory);
            }
            //add new selected Category
            foreach(var item in SelectedCategoryIds.Where(item=>!context.PostCategories.Any(pc=>pc.CategoryId==item))){
                context.PostCategories.Add(new PostCategory{CategoryId=item,PostId=post.Id});
            }
            context.Update(post);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var post = context.posts.Find(id);
            context.posts.Remove(post);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}