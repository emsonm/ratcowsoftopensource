using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//3rd Party
using RatCow.MvcFramework;

namespace testapp2
{
  partial class Form1Controller
  {
    partial void button1Click(EventArgs e)
    {
      MessageBox.Show(textBox1.Text);
      if (checkBox1.Checked)
        label1.Text = textBox1.Text;
    }

    partial void listView1GetItem(ref ListViewItem item, RetrieveVirtualItemEventArgs e)
    {
      if (checkBox2.Checked)
      {
        item = new ListViewItem();
        item.Text = data[e.ItemIndex].One;
        item.SubItems.Add(data[e.ItemIndex].Two);
        item.SubItems.Add(data[e.ItemIndex].Three);
        item.SubItems.Add(data[e.ItemIndex].Four);
      }
    }

    partial void checkBox2Click(EventArgs e)
    {
      listView1.Refresh();
    }

    internal class Form1Data
    {
      public string One { get; set; }

      public string Two { get; set; }

      public string Three { get; set; }

      public string Four { get; set; }
    }

    List<Form1Data> data = new List<Form1Data>();
    ListViewHelper<Form1Data> helper = null;

    protected override void ViewLoad()
    {
      data.Add(new Form1Data() { One = "1", Two = "2", Three = "3", Four = "4" });
      data.Add(new Form1Data() { One = "2", Two = "3", Three = "4", Four = "5" });
      data.Add(new Form1Data() { One = "3", Two = "4", Three = "5", Four = "6" });
      data.Add(new Form1Data() { One = "4", Two = "5", Three = "6", Four = "7" });

      helper = GetlistView1Helper<Form1Data>();

      SetData<Form1Data>(helper, data);
    }
  }
}