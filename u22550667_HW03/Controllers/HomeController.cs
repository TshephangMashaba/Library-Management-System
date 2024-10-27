using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using u22550667_HW03.Models;

namespace u22550667_HW03.Controllers
{
    public class HomeController : Controller
    {
        private LibraryEntities db = new LibraryEntities();
        // Define the number of elements per page
        // Define the number of elements per page
        int numberElementsPerPage = 10;


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



        public async Task<ActionResult> Maintain(int? authorPage, int? typePage, int? borrowPage)
        {
            // Determine the current page numbers for authors, types, and borrows, defaulting to 1 if null
            int authorPageNumber = authorPage ?? 1;
            int typePageNumber = typePage ?? 1;
            int borrowPageNumber = borrowPage ?? 1;

            // Get all authors, types, and borrows as IQueryable to support pagination
            var authorsQuery = db.authors.AsQueryable();
            var typesQuery = db.types.AsQueryable();
            var borrowsQuery = db.borrows.AsQueryable();

            // Define the number of elements per page
            int numberElementsPerPage = 10;

            // Get the total count of authors, types, and borrows
            int totalAuthorCount = await authorsQuery.CountAsync();
            int totalTypeCount = await typesQuery.CountAsync();
            int totalBorrowCount = await borrowsQuery.CountAsync();

            // Create a paginated list of authors
            var authors = await authorsQuery
                .OrderBy(a => a.authorId)
                .Skip((authorPageNumber - 1) * numberElementsPerPage)
                .Take(numberElementsPerPage)
                .ToListAsync();

            // Create a paginated list of types
            var types = await typesQuery
                .OrderBy(t => t.typeId)
                .Skip((typePageNumber - 1) * numberElementsPerPage)
                .Take(numberElementsPerPage)
                .ToListAsync();

            // Create a paginated list of borrows
            var borrows = await borrowsQuery
                .OrderBy(b => b.borrowId)
                .Skip((borrowPageNumber - 1) * numberElementsPerPage)
                .Take(numberElementsPerPage)
                .ToListAsync();

            // Create the view model
            var viewModel = new MaintainViewModel
            {
                Author = authors,
                Type = types,
                Borrow = borrows
            };

            // Pass the total count and current page numbers to the view
            ViewBag.TotalAuthorCount = totalAuthorCount;
            ViewBag.CurrentAuthorPage = authorPageNumber;
            ViewBag.TotalAuthorPages = (int)Math.Ceiling((double)totalAuthorCount / numberElementsPerPage);

            ViewBag.TotalTypeCount = totalTypeCount;
            ViewBag.CurrentTypePage = typePageNumber;
            ViewBag.TotalTypePages = (int)Math.Ceiling((double)totalTypeCount / numberElementsPerPage);

            ViewBag.TotalBorrowCount = totalBorrowCount;
            ViewBag.CurrentBorrowPage = borrowPageNumber;
            ViewBag.TotalBorrowPages = (int)Math.Ceiling((double)totalBorrowCount / numberElementsPerPage);

            return View(viewModel);
        }

        public ActionResult Report(int? studentId, DateTime? startDate, DateTime? endDate, string frequency = "monthly")
        {
            int studentIdValue = studentId ?? 0; // Assign 0 if studentId is null

            // Set default date range if not provided
            if (!startDate.HasValue)
            {
                startDate = DateTime.MinValue;
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.MaxValue;
            }


            // Get Student Borrowing Ranking
            var studentBorrowingRanking = db.borrows
           .Where(b => b.takenDate >= startDate && b.takenDate <= endDate)
           .GroupBy(b => b.studentId) // Group by student ID
           .Select(group => new
           {
               StudentId = group.Key, // Get the student ID
               BookCount = group.Count() // Count of books borrowed
           })
           .Join(db.students, // Assuming 'db.students' is your students table
               ranking => ranking.StudentId, // Join on the studentId from the borrows group
               student => student.studentId, // Change to student.studentId
               (ranking, student) => new BorrowingRanking // Create the BorrowingRanking object
               {
                   StudentId = ranking.StudentId ?? 0, // Ensure no null values
                   StudentName = student.name, // Change to student.name
                   BookCount = ranking.BookCount // Get the book count
               })
           .OrderByDescending(x => x.BookCount) // Order by the count of books borrowed in descending order
           .Take(10) // Take only the top 10 results
           .ToList();

            // Log Student Borrowing Ranking Count
            System.Diagnostics.Debug.WriteLine($"Student Borrowing Ranking Count: {studentBorrowingRanking.Count}");


            // Get Borrowing Frequency Report
            var borrowFrequency = db.borrows
                .GroupBy(b => b.book.name)
                .Select(group => new BorrowingFrequency
                {
                    BookName = group.Key,
                    Frequency = group.Count() // Ensure Count() is invoked correctly
                })
                .OrderByDescending(x => x.Frequency)
                .ToList();

            // Get Popular Books Data
            var popularBooks = db.borrows
                .Where(b => b.takenDate >= startDate && b.takenDate <= endDate)
                .GroupBy(b => b.book.name)
                .Select(group => new PopularBook
                {
                    BookName = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            // Combine data into ViewModel
            var reportViewModel = new ReportViewModel
            {
                BorrowingRanking = studentBorrowingRanking,
                BorrowHistory = null,
                PopularBooks = popularBooks,
                BorrowingFrequencies = borrowFrequency,
                Documents = null
            };

            // Serialize data for JavaScript
           
            ViewBag.PopularBooksData = JsonConvert.SerializeObject(popularBooks);
            ViewBag.BorrowingFrequencyData = JsonConvert.SerializeObject(borrowFrequency);
            ViewBag.StudentBorrowingData = JsonConvert.SerializeObject(studentBorrowingRanking);

            return View(reportViewModel);
        }


        [HttpPost]
        public ActionResult SaveReport(string filename, string fileType)
        {
            string reportContent = "This is a sample report content."; // Replace with your report data

            string path = Server.MapPath("~/Reports/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filePath = Path.Combine(path, $"{filename}.{fileType}");

            System.IO.File.WriteAllText(filePath, reportContent);

            return RedirectToAction("Report"); // Redirect back to the report page
        }

       
      



    }
}