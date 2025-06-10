using Malshinon.DB;
using Malshinon.models;
using Malshinon.Utils;
using Malshinon.DAL;
using ZstdSharp.Unsafe;

namespace Malshinon.Services
{
    public class ReportService
    {
        private PeopleDAL _peopleDal = new PeopleDAL();
        private IntelReportsDAL _intelReportsDal = new IntelReportsDAL();

        // Submit report - receives ready People objects
        public void SubmitReport(People reporter, People target, string reportText)
        {
            IntelReport report = new IntelReport
            {
                ReporterId = reporter.Id,
                TargetId = target.Id,
                Text = reportText,
                Timestamp = DateTime.Now
            };
            _intelReportsDal.AddNewIntelReport(report);

            _peopleDal.IncrementReportCount(reporter.Id);
            _peopleDal.IncrementMentionCount(target.Id);

            Console.WriteLine("Report saved successfully.");
        }

        // Helper: identify or create person by secret code, first name, last name, and type
        public People? GetOrCreatePerson(string? code, string? firstName, string? lastName, string type)
        {
            People? person = null;

            // 1. Try to find by secret code
            if (!string.IsNullOrEmpty(code))
            {
                person = _peopleDal.GetPersonBySecretCode(code);

                // If found, update type to "both" if needed
                if (person != null && person.Type != type && person.Type != "both")
                {
                    person = _peopleDal.UpdateType(person.Id, "both");
                }
            }

            // 2. If not found, ask for details and create new
            if (person == null)
            {
                if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
                {
                    Console.WriteLine($"Secret code not found. Please enter first name and last name of {type}:");
                    Console.Write("Enter first name: ");
                    firstName = Console.ReadLine();
                    Console.Write("Enter last name: ");
                    lastName = Console.ReadLine();
                }

                person = new People
                {
                    FirstName = firstName,
                    LastName = lastName,
                    SecetCode = TextAnalysisHelper.GenerateSecretCode(),
                    Type = type,
                    NumReports = 0,
                    NumMentions = 0
                };

                person = _peopleDal.AddNewPeople(person);
                Console.WriteLine($"New user created. Secret code: {person?.SecetCode}");
            }

            return person;
        }

    }
}
