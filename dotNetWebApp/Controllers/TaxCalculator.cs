using dotNetWebApp.Models;
using dotNetWebApp.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace dotNetWebApp.Controllers
{
    public static class TaxCalculator
    {
        private const double _tax = 9.85;
        private const double _pensionContribution = 19.2;
        private const double _healthCareContribution = 7.06;
        private const double _unemployementContribution = 1.02;
        private const double _brutto1 = 37.14;

        private const double _pensionContributionBrutto2 = 11;
        private const double _healthCareContributionBrutto2 = 7.06;
        private const double _brutto2 = 16.15;

        public static EmployeeVM? BrutoSalaryCalculation(Employee employee, string currency)
        {
            string Selectedcurrency;
            API_Obj currencyList;

            try
            {
                currencyList = FetchCurrency();
            }
            catch (Exception)
            {
                return null;
            }

            if(currency == "RSD")
            {
                Selectedcurrency = "RSD";
            }
            else if (currency == "USD")
            {
                employee.NettoSalary = Math.Round(employee.NettoSalary*currencyList.conversion_rates.USD,2);
                Selectedcurrency = "USD";
            }
            else
            {
                employee.NettoSalary = Math.Round(employee.NettoSalary*currencyList.conversion_rates.EUR,2);
                Selectedcurrency = "EUR";
            }

            return new EmployeeVM(employee, new string[]{
                Selectedcurrency,
            "Netto Salary = " + Math.Round(employee.NettoSalary,2),
            "Tax = " + Math.Round(employee.NettoSalary * (_tax / 100),2),
            "PensionContribution = " + Math.Round(employee.NettoSalary * (_pensionContribution / 100),2),
            "HealthCareContribution = " + Math.Round(employee.NettoSalary * (_healthCareContribution / 100),2),
            "UnemployementContribution = " + Math.Round(employee.NettoSalary * (_unemployementContribution / 100),2),
            "Taxes (1) (Tax + PensionContribution + HealthCareContribution + UnemployementContribution) = "
            + Math.Round(employee.NettoSalary * _brutto1/100),
            "Brutto (1) (Netto Salary + Taxes (1)) = " + Math.Round(employee.NettoSalary* (1 + _brutto1 / 100),2),
            "PensionContribution (2) = " + Math.Round(employee.NettoSalary * (_pensionContributionBrutto2 / 100),2),
            "HealthCareContribution (2) = " + Math.Round(employee.NettoSalary * (_healthCareContributionBrutto2 / 100),2),
            "Taxes (2) (PensionContribution (2) + HealthCareContribution (2)) = "
            + Math.Round(employee.NettoSalary * (1+_brutto1/100)*_brutto2/100),
            "Brutto (2) (Netto Salary + Taxes (1) + Taxes (2)) = " + BruttoSalary(employee.NettoSalary)
            });
        }

        private static double BruttoSalary(double NettoSalary)
        {
            return Math.Round(NettoSalary * (1 + _brutto1 / 100) * (1 + _brutto2 / 100), 2);
        }

        private static API_Obj FetchCurrency()
        {
            const string URL = "https://v6.exchangerate-api.com/v6/";
            const string urlParameters = "f391eefcfb2e65bb67201959/latest/RSD";

            String URLString = "https://v6.exchangerate-api.com/v6/" + urlParameters;
            using (var webClient = new System.Net.WebClient())
            {
                var json = webClient.DownloadString(URLString);
                return JsonConvert.DeserializeObject<API_Obj>(json);
            }
        }
    }
}

