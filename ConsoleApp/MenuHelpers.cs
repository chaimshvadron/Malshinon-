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
    }
}
