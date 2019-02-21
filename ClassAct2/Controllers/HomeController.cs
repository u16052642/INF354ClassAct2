using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassAct2.Models;
using System.Data;
using System.IO;
using ClassAct2.ViewModel;

namespace ClassAct2.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Report()
        {

            ReportVM vm = new ReportVM();

            vm.Employees = GetEmployees(0);

            vm.DateFrom = new DateTime(2011, 01, 1);
            vm.DateTo = new DateTime(2011, 01, 31);

            return View(vm);
        }

        private SelectList GetEmployees(int selected)
        {
            using (HardwareDBEntities1 db = new HardwareDBEntities1())
            {
                db.Configuration.ProxyCreationEnabled = false;


                var employees = db.lgemployees.Select(x => new SelectListItem
                {
                    Value = x.dept_num.ToString(),
                    Text = x.emp_fname
                }).ToList();

                //If selected pearameter has a value, configure the SelectList so that the apporiate item is preselected
                if (selected == 0)
                    return new SelectList(employees, "Value", "Text");
                else
                    return new SelectList(employees, "Value", "Text", selected);
            }
        }
        [HttpPost]
        public ActionResult Report(ReportVM vm)
        {
            using (HardwareDBEntities1 db = new HardwareDBEntities1())
            {
                db.Configuration.ProxyCreationEnabled = false;

                vm.Employees = GetEmployees(vm.SelectedEmployeeNum);
                vm.employee = db.lgemployees.Where(x => x.emp_num == vm.SelectedEmployeeNum).FirstOrDefault();

                //For each of the results, load data into a new ReportRecord object
                var list = db.lginvoices.Include("employee name").Where(pp => pp.cust_code == vm.employee.emp_num && pp.inv_DATETIME >= vm.DateFrom && pp.inv_DATETIME <= vm.DateTo).ToList().Select(rr => new ReportRecord
                {
                    InvoiceDate = rr.inv_DATETIME.ToString(),
                    InvoiceNumber = Convert.ToInt32(rr.inv_num),
                    Amount = Convert.ToDouble(rr.inv_total),
                    CustomerID = Convert.ToInt32(rr.cust_code),
                    CustomerName = db.lgcustomers.Where(pp => pp.cust_code == rr.cust_code).Select(x => x.cust_code + " " + x.cust_lname).FirstOrDefault(),
                });

                //Load the list of ReportRecords returned by the above query into a new list grouped by Shipment Method
                vm.results = list.GroupBy(g => g.CustomerName).ToList();

                //Load the list of ReportRecords returned by the above query into a new dictionary grouped by customer
                //This will be used to generate the chart on the View through the MicroSoft Charts helper
                vm.chartData = list.GroupBy(g => g.CustomerName).ToDictionary(g => g.Key, g => g.Sum(v => v.Amount));

                //Store the chartData dictionary in temporary data so that it can be accessed by the EmployeeOrdersChart action resonsible for generating the chart
                TempData["chartData"] = vm.chartData;
                TempData["records"] = list.ToList();
                TempData["employee"] = vm.employee;

                return View(vm);
            }
        }


             public ActionResult InvoiceChart()
        {
            //Load the chartData from temporary memory
            var data = TempData["chartData"];

            //Return the EmployeeOrdersChart temporary view, pass through the required chartData
            return View(TempData["chartData"]);
        }

    }
}

