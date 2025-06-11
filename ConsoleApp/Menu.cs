using Malshinon.Services;
using Malshinon.models;
using Malshinon.ConsoleApp;


namespace Malshinon.ConsoleApp
{
    public static class Menu
    {
        public static void Show()
        {
            while (true)
            {
                Console.WriteLine("\n===== Main Menu =====");
                Console.WriteLine("1. Report");
                Console.WriteLine("2. List all potential agents");
                Console.WriteLine("3. List all dangerous targets");
                Console.WriteLine("4. Show current alerts");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");
                var input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        ReportDialog.Show();
                        break;
                    case "2":
                        MenuHelpers.ShowPotentialAgents();
                        break;
                    case "3":
                        MenuHelpers.ShowDangerousTargets();
                        break;
                    case "4":
                        MenuHelpers.ShowAlerts();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

    }
}
