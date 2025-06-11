namespace Malshinon.models
{
    public class People
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecetCode { get; set; }
        public string? Type { get; set; }
        public int? NumReports { get; set; }
        public int? NumMentions { get; set; }
        public bool DangerStatus { get; set; } = false;
    }
}