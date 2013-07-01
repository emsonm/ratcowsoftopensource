using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RatCow.KanBan.Controls.Models
{
    public class KanBanCommentItem: DependencyObject
    {
        public KanBanCommentItem()
        {
            Author = System.Environment.MachineName;
        }

        public string Author
        {
            get { return (string)GetValue(AuthorProperty); }
            protected set { SetValue(AuthorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Author.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AuthorProperty =
            DependencyProperty.Register("Author", typeof(string), typeof(KanBanCommentItem), new UIPropertyMetadata());


        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(KanBanCommentItem), new UIPropertyMetadata());


        public DateTime? TimeStamp
        {
            get { return (DateTime?)GetValue(TimeStampProperty); }
            set { SetValue(TimeStampProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeStamp.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeStampProperty =
            DependencyProperty.Register("TimeStamp", typeof(DateTime?), typeof(KanBanCommentItem), new UIPropertyMetadata());
        
    }
}
