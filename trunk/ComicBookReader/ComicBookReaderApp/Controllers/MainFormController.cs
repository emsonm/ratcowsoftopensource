/*
 * Copyright 2011 Rat Cow Software and Matt Emson. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used 
 *    to endorse or promote products derived from this software without specific prior written 
 *    permission.
 *    
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RatCow.MvcFramework;
using RatCow.ComicReader.API;
using System.Drawing;

namespace cbr
{
  partial class MainFormController : AbstractMainFormController
  {
    ComicBook comicBook = null;

    protected override void ViewLoad()
    {
      this.AddModalSubController("InfoFORM", new InfoFormController());
    }

    /// <summary>
    /// Select a new image
    /// </summary>
    void PageListIndexChanged()
    {
      IComicPage page = ((IComicPage)pageList.SelectedItem); //image from the list
      page.Activate();  //activate the internal counter
      var image = Image.FromStream(page.RawData); //load data
      pageImage.Image = image; //display
    }

    /// <summary>
    /// The image was clicked, so we advance it by one frame
    /// </summary>
    void PageImageClicked()
    {
      IComicPage page = comicBook.Next();
      var image = Image.FromStream(page.RawData);
      pageImage.Image = image;
    }

    /// <summary>
    /// Click ze "open"
    /// </summary>
    protected override void openButtonClick()
    {
      pageList.BeginUpdate();
      try
      {
        openFileDialog.Filter = "*.cbz|*.cbz|*.cbr|*.cbr|*.*|*.*";
        openFileDialog.FileName = "";

        //we should ask for a file
        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          //Open(openFileDialog1.FileName);
          ComicBookReader reader = new ComicBookReader(openFileDialog.FileName);
          comicBook = new ComicBook(reader);

          foreach (var page in comicBook)
          {
            pageList.Items.Add( /*reference*/ page);
          }
        }
      }
      finally
      {
        pageList.EndUpdate();
      }
    }

    protected override void infoButtonClick()
    {
      openFileDialog.Filter = "*.cbz|*.cbz";
      openFileDialog.FileName = "";

      //we should ask for a file
      if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        this.ExecuteModalControllerWithData<string>("InfoFORM", openFileDialog.FileName); 
      }
    }
  }
}
