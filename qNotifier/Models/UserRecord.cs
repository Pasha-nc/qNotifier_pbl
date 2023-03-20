namespace qNotifier.Models
{
    public class UserRecord
    {
        public int Id { get; set; }
        public DateTime? AppDateTime { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public User? AppUser { get; set; }

        public RecordStatus? Status { get; set; }

        public UserRecord(User appUser, DateTime appDateTime, string title, RecordStatus status)
        {
            AppUser = appUser;
            AppDateTime = appDateTime;
            Title = title;
            Status = status;
            Description = String.Empty;
        }

        public UserRecord()
        {
        }
    }
    public enum RecordStatus
    {
        ToStart, InProgress, Done
    }
}
