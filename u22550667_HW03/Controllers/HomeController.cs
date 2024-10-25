using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using u22550667_HW03.Models;

namespace u22550667_HW03.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // Define the number of elements per page
        // Define the number of elements per page
        int numberElementsPerPage = 10;

        // GET: students and books
        // GET: students
        public async Task<ActionResult> Index(int? page, int? bookPage)
        {
            // Determine the current page number for students and books, defaulting to 1 if null
            int studentPageNumber = page ?? 1;
            int bookPageNumber = bookPage ?? 1;

            // Get all students and books as IQueryable to support pagination
            var studentsQuery = db.students.AsQueryable();
            var booksQuery = db.books.AsQueryable();

            // Define the number of elements per page
            int numberElementsPerPage = 10;

            // Get the total count of students and books
            int totalStudentCount = await studentsQuery.CountAsync();
            int totalBookCount = await booksQuery.CountAsync();

            // Create a paginated list of students
            var students = await studentsQuery
                .OrderBy(s => s.studentId) // Order by some field, e.g., studentId
                .Skip((studentPageNumber - 1) * numberElementsPerPage) // Skip the previous pages
                .Take(numberElementsPerPage) // Take the number of elements per page
                .ToListAsync();

            // Create a paginated list of books
            var books = await booksQuery
                .OrderBy(b => b.bookId) // Order by some field, e.g., bookId
                .Skip((bookPageNumber - 1) * numberElementsPerPage) // Skip the previous pages
                .Take(numberElementsPerPage) // Take the number of elements per page
                .ToListAsync();

            // Create your view model
            var viewModel = new HomeViewModel
            {
                Students = students,
                Books = books
            };

            // Pass the total count and current page numbers to the view for both students and books
            ViewBag.TotalStudentCount = totalStudentCount;
            ViewBag.CurrentStudentPage = studentPageNumber;
            ViewBag.TotalStudentPages = (int)Math.Ceiling((double)totalStudentCount / numberElementsPerPage);

            ViewBag.TotalBookCount = totalBookCount;
            ViewBag.CurrentBookPage = bookPageNumber;
            ViewBag.TotalBookPages = (int)Math.Ceiling((double)totalBookCount / numberElementsPerPage);

            return View(viewModel);
        }



        public ActionResult Maintain()
        {
            
            return View();
        }

        public ActionResult Report()
        {
           

            return View();
        }
    }
}