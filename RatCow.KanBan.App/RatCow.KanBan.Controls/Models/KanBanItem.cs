using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace RatCow.KanBan.Controls.Models
{
    public class KanBanItem: DependencyObject, RatCow.KanBan.Controls.Models.IKanBanItem
    {

        public KanBanItem()
        {
            IsActive = true; //default for a new item
            IsComplete = false;

            Title = "None";
            Created = DateTime.Now;
            Updated = DateTime.Now;

            Author = System.Environment.MachineName; //TODO - make this something more sensible
            Owners = new ObservableCollection<string>();

            Tasks = new ObservableCollection<KanBanTaskItem>();
            Comments = new ObservableCollection<KanBanCommentItem>();
        }

        /// <summary>
        /// Title of the item
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(KanBanItem), new UIPropertyMetadata());


        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(KanBanItem), new UIPropertyMetadata());


        public DateTime Created
        {
            get { return (DateTime)GetValue(CreatedProperty); }
            set { SetValue(CreatedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CreateDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CreatedProperty =
            DependencyProperty.Register("Created", typeof(DateTime), typeof(KanBanItem), new UIPropertyMetadata());



        public DateTime? Updated
        {
            get { return (DateTime?)GetValue(UpdatedProperty); }
            set { SetValue(UpdatedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Updated.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UpdatedProperty =
            DependencyProperty.Register("Updated", typeof(DateTime?), typeof(KanBanItem), new UIPropertyMetadata());



        public string Author
        {
            get { return (string)GetValue(AuthorProperty); }
            protected set { SetValue(AuthorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(KanBanItem), new UIPropertyMetadata());



        public ObservableCollection<string> Owners
        {
            get { return (ObservableCollection<string>)GetValue(OwnersProperty); }
            set { SetValue(OwnersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Owners.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OwnersProperty =
            DependencyProperty.Register("Owners", typeof(ObservableCollection<string>), typeof(KanBanItem), new UIPropertyMetadata());



        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(KanBanItem), new UIPropertyMetadata());



        public bool IsComplete
        {
            get { return (bool)GetValue(IsCompleteProperty); }
            set { SetValue(IsCompleteProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsComplete.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCompleteProperty =
            DependencyProperty.Register("IsComplete", typeof(bool), typeof(KanBanItem), new UIPropertyMetadata());
       

        /// <summary>
        /// List of tasks
        /// </summary>
        public ObservableCollection<KanBanTaskItem> Tasks
        {
            get { return (ObservableCollection<KanBanTaskItem>)GetValue(TasksProperty); }
            set { SetValue(TasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Tasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TasksProperty =
            DependencyProperty.Register("Tasks", typeof(ObservableCollection<KanBanTaskItem>), typeof(KanBanItem), new UIPropertyMetadata());


        public ObservableCollection<KanBanCommentItem> Comments
        {
            get { return (ObservableCollection<KanBanCommentItem>)GetValue(CommentsProperty); }
            set { SetValue(CommentsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Comments.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommentsProperty =
            DependencyProperty.Register("Comments", typeof(ObservableCollection<KanBanCommentItem>), typeof(KanBanItem), new UIPropertyMetadata(0));
        
    }
}
