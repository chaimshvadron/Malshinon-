using System;
using System.Collections.Generic;
using System.Linq;
using Malshinon.models;

namespace Malshinon.Services
{
    public static class AlertService
    {
        private static List<Alert> alerts = new List<Alert>();
        private static int nextId = 1;

        public static void AddAlert(int targetId, DateTime windowStart, DateTime windowEnd, string reason)
        {
            alerts.Add(new Alert
            {
                TargetId = targetId,
                WindowStart = windowStart,
                WindowEnd = windowEnd,
                CreatedAt = DateTime.Now,
                Reason = reason
            });
        }

        public static List<Alert> GetAlerts()
        {
            return alerts.ToList();
        }

        public static void ClearAlerts()
        {
            alerts.Clear();
            nextId = 1;
        }
    }
}
