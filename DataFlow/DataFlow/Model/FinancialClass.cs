using System.ComponentModel.DataAnnotations;

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
