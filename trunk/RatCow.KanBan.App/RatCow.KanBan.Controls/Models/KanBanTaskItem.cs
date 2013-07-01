using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace RatCow.KanBan.Controls.Models
{
    public class KanBanTaskItem : DependencyObject
    {

        /// <summary>
        /// status of what is done
        /// </summary>
        public bool Flag
        {
            get { return (bool)GetValue(FlagProperty); }
            set { SetValue(FlagProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Flag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FlagProperty =
            DependencyProperty.Register("Flag", typeof(bool), typeof(KanBanTaskItem), new UIPropertyMetadata());


        /// <summary>
        /// Description of the task
        /// </summary>
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(KanBanTaskItem), new UIPropertyMetadata());


    }
}
