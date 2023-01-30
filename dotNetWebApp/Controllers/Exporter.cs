using ClosedXML.Excel;
using dotNetWebApp.Models;
using dotNetWebApp.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;
using System.Text;

namespace dotNetWebApp.Controllers
{
    public static class Exporter
    {
        public static MemoryStream exportToPdf(EmployeeVM employee)
        {
            Employee emp = employee.employee;
            Document document = new Document();

            MemoryStream stream = new MemoryStream();

            try
            {
                PdfWriter pdfWriter = PdfWriter.GetInstance(document, stream);
                pdfWriter.CloseStream = false;

                document.Open();

                document.Add(new Paragraph("First Name: " + emp.FirstName));
                document.Add(new Paragraph("Last Name: " + emp.LastName));
                document.Add(new Paragraph("Address: " + emp.Address));
                document.Add(new Paragraph("Email: " + emp.Email));
                document.Add(new Paragraph("Position: " + emp.Position));
                document.Add(new Paragraph("--------------------------"));
                document.Add(new Paragraph("Currency: " + employee.TaxCalculation[0]));

                foreach(var calculation in employee.TaxCalculation.Skip(1))
                {
                    document.Add(new Paragraph(calculation));
                }
            }
            catch (DocumentException de)
            {
                Console.Error.WriteLine(de.Message);
            }
            catch (IOException ioe)
            {
                Console.Error.WriteLine(ioe.Message);
            }

            document.Close();

            stream.Flush();
            stream.Position = 0;

            return stream;
        }

        public static string exportToCSV(IEnumerable<Employee> EmployeeList)
        {

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("Id,FirstName,LastName,Address,Email,NettoSalary,Position");

            foreach (Employee employee in EmployeeList)
            {
                builder.AppendLine($"{employee.Id},{employee.FirstName}, " +
                    $"{employee.LastName},{employee.Address},{employee.Email},{employee.NettoSalary},{employee.Position}");
            }

            return builder.ToString();
        }

        public static byte[] exportToXLSX(IEnumerable<Employee> EmployeeList)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "First Name";
                worksheet.Cell(currentRow, 3).Value = "Last Name";
                worksheet.Cell(currentRow, 4).Value = "Address";
                worksheet.Cell(currentRow, 5).Value = "Email";
                worksheet.Cell(currentRow, 6).Value = "NettoSalary";
                worksheet.Cell(currentRow, 7).Value = "Position";

                foreach (var employee in EmployeeList)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = employee.Id;
                    worksheet.Cell(currentRow, 2).Value = employee.FirstName;
                    worksheet.Cell(currentRow, 3).Value = employee.LastName;
                    worksheet.Cell(currentRow, 4).Value = employee.Address;
                    worksheet.Cell(currentRow, 5).Value = employee.Email;
                    worksheet.Cell(currentRow, 6).Value = employee.NettoSalary;
                    worksheet.Cell(currentRow, 7).Value = employee.Position;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }
        }

        public static void EmailEmployeeReport(EmployeeVM employeeVM)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("testVladimirMaksimovic@gmail.com","Vladimir Maksimovic");
            mail.To.Add(new MailAddress(employeeVM.employee.Email));
            mail.Subject = "Employee Report";
            mail.IsBodyHtml = true;
            mail.Attachments.Add(new Attachment(Exporter.exportToPdf(employeeVM), "Employee Report.pdf", "application/pdf"));

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential("testVladimirMaksimovic@gmail.com", "jjxgibosghncrzti");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}

