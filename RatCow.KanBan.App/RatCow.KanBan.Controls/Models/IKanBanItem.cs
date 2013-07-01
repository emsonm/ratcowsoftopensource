using System;
using System.Collections.ObjectModel;

namespace RatCow.KanBan.Controls.Models
{
    public interface IKanBanItem
    {
        string Author { get; }
        ObservableCollection<global::RatCow.KanBan.Controls.KanBanCommentItem> Comments { get; set; }
        DateTime Created { get; set; }
        string Description { get; set; }
        bool IsActive { get; set; }
        bool IsComplete { get; set; }
        ObservableCollection<string> Owners { get; set; }
        ObservableCollection<global::RatCow.KanBan.Controls.KanBanTaskItem> Tasks { get; set; }
        string Title { get; set; }
        DateTime? Updated { get; set; }
    }
}
