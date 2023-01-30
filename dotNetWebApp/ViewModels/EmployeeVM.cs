using dotNetWebApp.Models;

namespace dotNetWebApp.ViewModels
{
    public class EmployeeVM
    {
        public string[] TaxCalculation { get; set; }
        public Employee employee{ get; set; }

        public EmployeeVM(Employee employee, string[] taxCalculation)
        {
            TaxCalculation = taxCalculation;
            this.employee = employee;
        }
    }
}
