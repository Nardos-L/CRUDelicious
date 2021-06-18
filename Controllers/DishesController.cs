using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CRUDelicious.Models;

namespace CRUDelicious.Controllers
{
    public class DishesController : Controller
    {
        private readonly ILogger<DishesController> _logger;

        private CRUDeliciousContext db;


        public DishesController(CRUDeliciousContext context)
        {
            db = context;
        }

        [HttpGet("/add")]
        public IActionResult Add()
        {
            return View("CreateDish");
        }

        [HttpPost("/dishes/create")]
        public IActionResult Create(Dish newDish)
        {
            if (ModelState.IsValid == false)
            {
                // Send back to the page with the form to show errors.
                return View("CreateDish");
            }
            // ModelState IS valid...
            Console.WriteLine($"{newDish.Calories} : {newDish.Chef} : {newDish.Name}: {newDish.Description} {newDish.Tastiness}");

            db.Dishes.Add(newDish);
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }


        /* [HttpGet("/dishes")]
        public IActionResult All()
        {
            List<Dish> AllDishes = db.Dishes.ToList();
            return View("Index","Home",AllDishes);
        }
 */
        
        [HttpGet("/dishes/{dishId}")]
        public IActionResult Details(int dishId)
        {
            Dish dish = db.Dishes.FirstOrDefault(d => d.DishId == dishId);

            // if (dish == null)
            // {
            //     return RedirectToAction("Index","Home");
            // }

            return View("Details", dish);
        } 

        [HttpGet("/dishes/{dishId}/edit")]
        public IActionResult Edit(int dishId)
        {
            Dish dish = db.Dishes.FirstOrDefault(d => d.DishId == dishId);

            if (dish == null)
            {
                return RedirectToAction("Index","Home");
            }

            return View("Edit", dish);
        }


        [HttpPost("/dishes/{dishId}")]
        public IActionResult Delete(int dishId)
        {
            Dish dish = db.Dishes.FirstOrDefault(d => d.DishId == dishId);

            if (dish != null)
            {
                db.Dishes.Remove(dish);
                db.SaveChanges();
            }

            return RedirectToAction("Index","Home");
        }

        [HttpPost("/dishes/{dishId}/update")]
        public IActionResult Update(int dishId, Dish editedDish)
        {
            if (ModelState.IsValid == false)
            {
                editedDish.DishId = dishId;
                return View("Edit", editedDish);
            }

            Dish dbDish= db.Dishes.FirstOrDefault(d => d.DishId == dishId);

            if (dbDish == null)
            {
                return RedirectToAction("Index","Home");
            }

            dbDish.Name = editedDish.Name;
            dbDish.Chef = editedDish.Chef;
            dbDish.Tastiness = editedDish.Tastiness;
            dbDish.Calories = editedDish.Calories;
            dbDish.Description = editedDish.Description;
            dbDish.UpdatedAt = DateTime.Now;

            db.Dishes.Update(dbDish);
            db.SaveChanges();

            // Dict matches Details params     new { paramName = paramValue }
            return RedirectToAction("Details", new { dishId = dbDish.DishId });
        }
    }
}
