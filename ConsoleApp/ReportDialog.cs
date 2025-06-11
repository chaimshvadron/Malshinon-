using Malshinon.Services;
using Malshinon.models;
using Malshinon.DAL;
using Malshinon.Utils;


namespace Malshinon.ConsoleApp
{
    public static class ReportDialog
    {
        public static void Show()
        {
            var reportService = new ReportService();

            // Get reporter details
            People? reporter = GetPersonFromUser("reporter", reportService);
            if (reporter == null)
            {
                Console.WriteLine("Could not identify or create reporter.");
                return;
            }

            // Get target details
            People? target = GetPersonFromUser("target", reportService);
            if (target == null)
            {
                Console.WriteLine("Could not identify or create target.");
                return;
            }

            // Get report text directly here
            string reportText = GetReportTextFromUser();
            reportService.SubmitReport(reporter, target, reportText);
        }

        // All user input logic is now here for clarity
        private static People? GetPersonFromUser(string role, ReportService reportService)
        {
            Console.Write($"Enter secret code for {role} (or leave blank): ");
            string? code = Console.ReadLine();
            string? firstName = null;
            string? lastName = null;

            if (string.IsNullOrWhiteSpace(code))
            {
                Console.Write($"Enter first name for {role}: ");
                firstName = Console.ReadLine();
                Console.Write($"Enter last name for {role}: ");
                lastName = Console.ReadLine();
            }
            return reportService.GetOrCreatePerson(code, firstName, lastName, role);
        }

        private static string GetReportTextFromUser()
        {
            string? input;
            do
            {
                Console.Write("Enter report text: ");
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }
    }
}
