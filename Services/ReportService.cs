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

            var PersonReorter = _peopleDal.IncrementReportCount(reporter.Id);
            var PersonTaeget = _peopleDal.IncrementMentionCount(target.Id);

            if (PersonReorter != null)
                EnsureRoleUpgrade(PersonReorter, "reporter");
            if (PersonTaeget != null)
                EnsureRoleUpgrade(PersonTaeget, "target");

            Console.WriteLine("Report saved successfully.");
            if (PersonReorter != null && PersonReorter.NumReports >= 10 && PersonReorter.Type != "potential_agent")
            {
                Console.WriteLine($"Warning: {PersonReorter.FirstName} {PersonReorter.LastName} has been reported {PersonReorter.NumReports} times!");
                _peopleDal.UpdateType(PersonReorter.Id, "potential_agent");
            }

            if (PersonTaeget != null && PersonTaeget.NumMentions >= 20 && !PersonTaeget.DangerStatus)
            {
                _peopleDal.UpdateDangerStatus(PersonTaeget.Id, true);
                Console.WriteLine($"Warning: {PersonTaeget.FirstName} {PersonTaeget.LastName} is now marked as dangerous!");
            }
        }

        public People? GetOrCreatePerson(string? code, string? firstName, string? lastName, string type)
        {
            People? person = null;

            // 1. Try to find by secret code
            if (!string.IsNullOrEmpty(code))
            {
                person = _peopleDal.GetPersonBySecretCode(code);
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
        private void EnsureRoleUpgrade(People person, string roleToAdd)
        {
            if (person.Type == "both" || person.Type == "potential_agent")
                return;

            if ((person.Type == "reporter" && roleToAdd == "target") ||
                (person.Type == "target" && roleToAdd == "reporter"))
            {
                _peopleDal.UpdateType(person.Id, "both");
            }
        }


    }
}
