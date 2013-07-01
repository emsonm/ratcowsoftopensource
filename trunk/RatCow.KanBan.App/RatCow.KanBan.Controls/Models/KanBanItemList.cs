using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.ObjectModel;

namespace RatCow.KanBan.Controls.Models
{
    public class KanBanItemList: DependencyObject
    {
        public KanBanItemList()
        {
            Name = "New List";
            Items = new ObservableCollection<KanBanItem>();
        }


        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(KanBanItem), new UIPropertyMetadata());


        public ObservableCollection<KanBanItem> Items
        {
            get { return (ObservableCollection<KanBanItem>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<KanBanItem>), typeof(KanBanItemList), new UIPropertyMetadata());

        
    }
}
