using dotNetWebApp.Data;
using dotNetWebApp.Models;
using dotNetWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text;

namespace dotNetWebApp.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public EmployeeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> employeeList = _db.Employees;
            return View(employeeList);
        }

        public IActionResult Show(int id, string currency)
        {
            try
            {
            Employee employee = _db.Employees.Find(id);
            EmployeeVM? _currentEmployee = TaxCalculator.BrutoSalaryCalculation(employee, currency);
            return View(_currentEmployee);
            }
            catch(Exception ex)
            {
                TempData["Message"] = ex.ToString();
                return View("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee emp)
        {
            if(emp.FirstName == emp.LastName)
            {
                ModelState.AddModelError("firstName", "First and Last Name shouldn't be equal");
            }

            if (ModelState.IsValid)
            {
                _db.Employees.Add(emp);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(emp);
        }

        public IActionResult CSV()
        {
            try
            {
                return File(Encoding.UTF8.GetBytes(Exporter.exportToCSV(_db.Employees)),
                    "text/csv", "EmployeeInfo.csv");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.ToString();
                return View("Index");
            }
        }

        public IActionResult XLSX()
        {
            try
            {
            return File(Exporter.exportToXLSX(_db.Employees),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","EmployeeInfo.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.ToString();
                return View("Index");
            }
        }

        public IActionResult Email(int id, string currency)
        {
            try
            {
                Employee employee = _db.Employees.Find(id);
                EmployeeVM _currentEmployee = TaxCalculator.BrutoSalaryCalculation(employee, currency);
                Exporter.EmailEmployeeReport(_currentEmployee);
                TempData["Message"] = "Report successfully sent to the employee email";
                return View("Show", _currentEmployee);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Failed to send the email";
                return RedirectToAction("Index");
            }
        }
    }
}