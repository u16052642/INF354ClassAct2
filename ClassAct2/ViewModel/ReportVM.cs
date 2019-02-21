using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClassAct2.Models;
using System.Web.Mvc;

namespace ClassAct2.ViewModel
{
    public class ReportVM
    {
        public IEnumerable<SelectListItem> Employees { get; set; }
        public int SelectedEmployeeNum { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        //Fields for report data
        public lgemployee employee { get; set; }
        public List<IGrouping<string,ReportRecord>> results { get; set; }
        public Dictionary<string, double> chartData { get; set; }
    }

    public class ReportRecord
    {
        public string InvoiceDate { get; set; }
        public double InvoiceNumber { get; set; }
        public double Amount { get; set; }
        //public string Departmant { get; set; }
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }

        
    }
}
