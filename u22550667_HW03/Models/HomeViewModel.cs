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
}