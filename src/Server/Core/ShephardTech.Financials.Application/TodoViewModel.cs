using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Application
{
    public  class TodoViewModel
    {
        public string title { get; set; }

        public string content { get; set; }

        public bool completed { get; set; }

        public DateTime dueDate { get; set; }
    }
}
