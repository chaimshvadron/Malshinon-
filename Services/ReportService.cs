using Malshinon.DB;
using Malshinon.models;
using Malshinon.Utils;
using Malshinon.DAL;

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
            CheckAndUpgradePotentialAgent(PersonReorter);
            CheckAndMarkDangerous(PersonTaeget);
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

        // בודק אם צריך לשדרג את ה-reporter ל"potential_agent"
        private void CheckAndUpgradePotentialAgent(People? person)
        {
            if (person != null && person.NumReports >= 10 && person.Type == "reporter" && _intelReportsDal.GetAverageReportLengthByReporterId(person.Id) > 100)
            {
                Console.WriteLine($"Warning: {person.FirstName} {person.LastName} has been reported {person.NumReports} times!");
                _peopleDal.UpdateType(person.Id, "potential_agent");
            }
        }

        // בודק אם צריך לסמן את ה-target כ-dangerous
        private void CheckAndMarkDangerous(People? person)
        {
            if (person != null && person.NumMentions >= 20 && !person.DangerStatus)
            {
                _peopleDal.UpdateDangerStatus(person.Id, true);
                Console.WriteLine($"Warning: {person.FirstName} {person.LastName} is now marked as dangerous!");
            }
        }

    }
}
