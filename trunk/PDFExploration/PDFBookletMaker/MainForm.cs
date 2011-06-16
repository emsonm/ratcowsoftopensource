using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDFBookletMaker
{

  using PdfSharp.Drawing;
  using PdfSharp.Pdf;
  using PdfSharp.Pdf.IO;

  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      //show oepndialog

      using (OpenFileDialog dlg = new OpenFileDialog())
      {
        dlg.Filter = "*.pdf|*.pdf";
        dlg.FileName = "";
        if (dlg.ShowDialog() == DialogResult.OK)
        {

          //we go...
          string newfilename = System.IO.Path.GetDirectoryName(dlg.FileName) + "\\"+ System.IO.Path.GetFileNameWithoutExtension(dlg.FileName) + "_combined.pdf";
          ProcessFile(dlg.FileName, newfilename, checkBox1.Checked);
        }
      }
    }


    /// <summary>
    /// This is a very messy page reorder that will create a file with pages ready to 
    /// print as a booklet in "A5 folded A4" print order.
    /// 
    /// Really, it's just me messing about, but I do have a few practical applications
    /// for this too.
    /// 
    /// NO... this is not "model code" hahahah!!
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="outfilename"></param>
    /// <param name="pad"></param>
    void ProcessFile(string filename, string outfilename, bool pad)
    {
      //make a temp dir
      string path = String.Format("{0}{1}", System.IO.Path.GetTempPath(), System.IO.Path.GetFileNameWithoutExtension(filename));
      
      //create directory if it doesn't already exist
      if (!System.IO.Directory.Exists(path))
      {
        System.IO.Directory.CreateDirectory(path);
      }

      List<string> pageList = new List<string>(); //lazy

      //now we split the file
      //open
      PdfDocument inputDocument = PdfReader.Open(filename, PdfDocumentOpenMode.Import);
      PdfDocument outputDocument = null;

      progressBar1.Minimum = 0;
      progressBar1.Maximum = inputDocument.PageCount;
      progressBar1.Step = 1;

      string name = System.IO.Path.GetFileNameWithoutExtension(filename);
      for (int idx = 0; idx < inputDocument.PageCount; idx++)
      {
        // Create new document
        outputDocument = new PdfDocument();
        outputDocument.Version = inputDocument.Version;
        outputDocument.Info.Title =
          String.Format("Page {0} of {1}", idx + 1, inputDocument.Info.Title);
        outputDocument.Info.Creator = inputDocument.Info.Creator;

        // Add the page and save it
        outputDocument.AddPage(inputDocument.Pages[idx]);

        string fileToCreate = String.Format("{0}\\{1}{2}.pdf", path,name, idx + 1);
        pageList.Add(fileToCreate);
        outputDocument.Save(fileToCreate);
        progressBar1.PerformStep();
        Application.DoEvents();
      }

      int target = inputDocument.PageCount;

      if (pad)
      {
        progressBar1.Value = 0;
        label1.Text = "Padding";

        //create a blank dummy pad page
        PdfDocument document = new PdfDocument();
        document.Info.Title = "Pad page";
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);
        gfx.DrawString("Page is blank", font, XBrushes.Black, new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);
        string blank_page = String.Format("{0}\\{1}{2}.pdf", path, name, "blank");
        document.Save(blank_page);

        while (target % 4 != 0)
        {
          pageList.Add(blank_page);  //pad
          target++;
          Application.DoEvents();
        }
      }

      int splittarget = target / 2;


      //sort pages
      List<string> sortedPages = new List<string>();

      for (int i = 0; i < splittarget; i++)
      {
        sortedPages.Add(pageList[i]);
        sortedPages.Add(pageList[target - (i+ 1)]);
      }

      progressBar1.Value = 0;
      label1.Text = "Render";

      outputDocument = new PdfDocument();
      foreach (string file in sortedPages)
      {
        inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);
        int count = inputDocument.PageCount;
        for (int idx = 0; idx < count; idx++)
        {
          PdfPage npage = inputDocument.Pages[idx];
          outputDocument.AddPage(npage);
        }
        progressBar1.PerformStep();
        Application.DoEvents();
      }

      outputDocument.Save(outfilename);

      //clean up
      System.IO.Directory.Delete(path, true);

    }

    /// <summary>
    /// Make a booklet from simple markup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button2_Click(object sender, EventArgs e)
    {
        //format [ ] <- chapter seperator
        //       ^   <- page section
        //       @   <- title section
        //       - range
        //       , list
        // so, for a 20 paged pdf
        // [@chapter1@^1,3,5-7^][@chapter2@^10-15]
      string raw = textBox1.Text;
      string[] list = raw.Split(']');
      foreach (var s in list)
      {

      }
    }
  }
}
