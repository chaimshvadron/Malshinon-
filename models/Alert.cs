using System;

namespace Malshinon.models
{
    public class Alert
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public DateTime WindowStart { get; set; }
        public DateTime WindowEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; }
    }
}
