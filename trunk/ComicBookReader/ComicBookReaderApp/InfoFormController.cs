using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RatCow.MvcFramework;
using RatCow.ComicReader.API;

namespace cbr
{
  class InfoFormController: AbstractInfoFormController, RatCow.MvcFramework.IModalSubFormContainer
  {

    string sdata = null;
    ComicBookInfo info = null;

    protected override void ViewLoad()
    {
      ReadData();
    }

    void ReadData()
    {
      titleEdit.Text = info.GetValue("title");
      seriesEdit.Text = info.GetValue("series");
      issueNumEdit.Text = info.GetValue("issue");
      numOfIssuesEdit.Text = info.GetValue("numberOfIssues");
    }

    void WriteData()
    {
      info.SetValue("title", titleEdit.Text);
      info.SetValue("series", seriesEdit.Text);
      info.SetValue("issue", Convert.ToInt32(issueNumEdit.Text));
      info.SetValue("numberOfIssues", numOfIssuesEdit.Text);
    }


    #region IModalSubFormContainer Members

    public bool PerformModalTask<T>(T data)
    {
      Type t = typeof(T);

      if (t == typeof(String))
      {
        sdata = (string)((object)data);

        //here we load that JSON
        info = new ComicBookInfo();
        info.Init(sdata);

        System.Windows.Forms.DialogResult dr = View.ShowDialog();

        bool result = (dr == System.Windows.Forms.DialogResult.OK);

        if (result)
        {
          //save the data
          WriteData();
          
          info.Update(sdata);
        }

        return result;
      }
      else
        return false;
    }

    public bool PerformModalTask()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
