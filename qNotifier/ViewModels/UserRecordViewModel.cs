using qNotifier.Models;

namespace qNotifier.ViewModels
{
    public class UserRecordViewModel
    {
        public int Id { get; set; }
        public DateTime AppDateTime { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }        

        public RecordStatus Status { get; set; }

        public UserRecordViewModel(DateTime appDateTime, string title, RecordStatus status)
        {            
            AppDateTime = appDateTime;
            Title = title;
            Status = status;
            Description = String.Empty;
        }

        public UserRecordViewModel()
        {
        }
    }
}
