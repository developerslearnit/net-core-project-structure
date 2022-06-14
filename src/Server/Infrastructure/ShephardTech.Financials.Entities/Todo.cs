using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Entities
{
    public partial class Todo
    {
        public int TodoId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool Completed { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
