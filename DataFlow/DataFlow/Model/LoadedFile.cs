using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Model
{
    public class LoadedFile
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public ICollection<FinancialClass> FinancialClasses { get; set; }

    }
}
