using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace u22550667_HW03.Models
{
    public class HomeViewModel
    {
        public List<student> Students { get; set; }

        public List<book> Books { get; set; }   
    }

    public class MaintainViewModel
    {
        public List<author> Author { get; set; }

        public List<type> Type { get; set; }

        public List<borrow> Borrow { get; set; }
    }

    public class ReportViewModel
    {
        public List<BorrowingRanking> BorrowingRanking { get; set; }
        public List<BorrowHistory> BorrowHistory { get; set; }
        public List<PopularBook> PopularBooks { get; set; }
        public List<BorrowingFrequency> BorrowingFrequencies { get; set; } // New property for borrowing frequency
        public List<DocumentViewModel> Documents { get; set; }
        public List<Report> SavedReports { get; set; }
    }

    public class Report
    {
        public int ReportId { get; set; } // Unique identifier for the report
        public string Title { get; set; }  // Title of the report
        public string Content { get; set; } // Content of the report
        public DateTime CreatedDate { get; set; } // Date the report was created
        public string FileData { get; set; } // Base64 
    }
    public class BorrowingFrequency
    {
        public string BookName { get; set; }  // Name of the book
        public int Frequency { get; set; }     // Number of times the book has been borrowed
    }


    public class BorrowHistory
    {
        public int BorrowId { get; set; }
        public string BookName { get; set; }
        public DateTime? TakenDate { get; set; }
        public DateTime? BroughtDate { get; set; }
    }

    public class BorrowingRanking
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }
        public int BookCount { get; set; }   // This should be non-nullable int
    }

    public class OverdueBook
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    public class PopularBook
    {
        public string BookName { get; set; }
        public int Count { get; set; }
    }


    public class DocumentViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public string Description { get; set; } // Add a property for the descriptio
    }
}