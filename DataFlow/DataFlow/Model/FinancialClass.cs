using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Model
{
    public class FinancialClass
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public int LoadedFileId {  get; set; }  
        public LoadedFile LoadedFile { get; set; }
        public ICollection<BankAccount> BankAccounts { get; set; }
    }
}
