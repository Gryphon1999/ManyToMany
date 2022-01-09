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
            var post = context.posts.Include(p => p.PostCategories)
            .ThenInclude(c => c.Category)
            .ToList();
            return View(post);
        }

        public IActionResult Create()
        {
            ViewData["categories"] = context.categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Post post, int[] SelectedCategoryIds)
        {
            foreach (var item in SelectedCategoryIds)
            {
                post.PostCategories.Add(new PostCategory { CategoryId = item });
            }
            context.Add(post);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            ViewData["categories"] = context.categories.ToList();
            var post = context.posts.Include(p => p.PostCategories).FirstOrDefault(x => x.Id == id);
            return View(post);
        }

        [HttpPost]
        public IActionResult Edit(int id,Post post, int[] SelectedCategoryIds)
        {
            var model = context.posts.Include(p=>p.PostCategories).FirstOrDefault(x=>x.Id==id);
            //remove selected category
            var removeCategories = new List<PostCategory>();
            foreach (var postCategories in model.PostCategories)
            {
                if (!SelectedCategoryIds.Contains(postCategories.CategoryId))
                {
                    removeCategories.Add(postCategories);
                }
            }
            //remove oldselect
            foreach (var postCategory in removeCategories)
            {
                context.PostCategories.Remove(postCategory);
            }
            //add new selected Category
            foreach (var item in SelectedCategoryIds)
            {
                if (!model.PostCategories.Any(pc => pc.CategoryId == item))
                    model.PostCategories.Add(new PostCategory { CategoryId = item, PostId = post.Id });
            }
            model.Title = post.Title;
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var post = context.posts.Find(id);
            return View(post);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var post = context.posts.Find(id);
            if (post == null)
                return RedirectToAction(nameof(Index));
            context.Remove(post);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}