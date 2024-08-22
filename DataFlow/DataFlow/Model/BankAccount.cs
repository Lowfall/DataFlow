using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Model
{
    public class BankAccount
    {
        public int Id { get; set; } 
        public decimal ActiveOpenningBalance {  get; set; }
        public decimal PassiveOpenningBalance {  get; set; }
        public decimal DebitTurnover {  get; set; }
        public decimal CreditTurnover {  get; set; }
        public decimal ActiveClosingBalance { get; set; }
        public decimal PassiveClosingBalance { get; set; }
        public int FinancialClassId { get; set; }
        public FinancialClass FinancialClass { get; set; }
    }
}
