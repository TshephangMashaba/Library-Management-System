using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using u22550667_HW03.Models;

namespace u22550667_HW03.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();

        public async Task<ActionResult> Index()
        {
            var students = await db.students.ToListAsync();
            var books = await db.books.ToListAsync();

            var viewModel = new HomeViewModel
            {
                Students = students,
                Books = books
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}