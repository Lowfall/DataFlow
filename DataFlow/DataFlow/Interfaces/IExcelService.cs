using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFlow.Interfaces
{
    public interface IExcelService
    {
        BankAccount ParseRow(int row, FinancialClass financialClass);
        List<FinancialClass> ParseFile(int rowsAmount);
    }
}
