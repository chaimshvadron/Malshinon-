using System;
using Malshinon.DAL;
using Malshinon.models;
using System.Collections.Generic;

namespace Malshinon.ConsoleApp
{
    public static class MenuHelpers
    {
        public static void ShowPotentialAgents()
        {
            var dal = new PeopleDAL();
            List<People> agents = dal.GetPeopleByType("potential_agent");
            Console.WriteLine("\nPotential Agents:");
            if (agents.Count == 0)
            {
                Console.WriteLine("None found.");
                return;
            }
            foreach (var p in agents)
            {
                Console.WriteLine($"Name: {p.FirstName} {p.LastName}, Report Count: {p.NumReports}");
            }
        }

        public static void ShowDangerousTargets()
        {
            var dal = new PeopleDAL();
            List<People> targets = dal.GetPeoplesByDangerStatus(true);
            Console.WriteLine("\nDangerous Targets:");
            if (targets.Count == 0)
            {
                Console.WriteLine("None found.");
                return;
            }
            foreach (var p in targets)
            {
                Console.WriteLine($"Name: {p.FirstName} {p.LastName}, Mention Count: {p.NumMentions}");
            }
        }
        public static void ShowAlerts()
        {
            var alerts = Malshinon.Services.AlertService.GetAlerts();
            Console.WriteLine("\nCurrent Alerts:");
            if (alerts.Count == 0)
            {
                Console.WriteLine("No alerts found.");
                return;
            }
            foreach (var alert in alerts)
            {
                Console.WriteLine($"Alert #{alert.Id}: TargetId={alert.TargetId}, Window=({alert.WindowStart:HH:mm} - {alert.WindowEnd:HH:mm}), Reason={alert.Reason}, CreatedAt={alert.CreatedAt:yyyy-MM-dd HH:mm}");
            }
        }
    }
}
