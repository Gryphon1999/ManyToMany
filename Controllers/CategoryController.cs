using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ManyToManyRelation.DAL;
using ManyToManyRelation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ManyToManyRelation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly RelationDbContext context;

        public CategoryController(RelationDbContext _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var category = context.categories.ToList();
            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            context.categories.Add(category);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            var category = context.categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id, Category category)
        {
            var data = context.categories.Find(id);
            data.Title = category.Title;
            context.categories.Update(data);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var data = context.categories.Find(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = context.categories.Find(id);
            context.categories.Remove(category);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}