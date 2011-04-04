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

using System.IO;

using Ionic.Zip;
using Newtonsoft.Json.Linq;

namespace RatCow.ComicReader.API
{

  /// <summary>
  /// Because comicbookinfo only really exists in CBZ this is
  /// CBZ specific.
  /// 
  /// This kind of works, but the spec is so vague, I've got a lot more work to do on it...
  /// </summary>
  public class ComicBookInfo
  {

    Ionic.Zip.ZipFile archive = null;

    JObject json = null;

    public string GetValue(string name)
    {
      string value = Convert.ToString(json[name]);
      return value;
    }

    public void SetValue(string name, string value)
    {    
      json[name] = value;
    }

    public void SetValue(string name, int value)
    {
      json[name] = value;
    }


    public string CreateDefaultInfo()
    {
      string defaultJSON = 
        "{appID:\"ComicBookLover/888\"," +
        " lastModified:\"" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss +zzz") + "\", " +
        " \"ComicBookInfo/1.0\" : " +
        " { " +
        "   series : \"None\", " +
        "   title : \"None\", " +
        "   publisher : \"Unknown\", " +
        "   issue : 0, " +
        "   numberOfIssues : 0 " +
        " } " +
        "}";

      return defaultJSON;
    }

    /// <summary>
    /// We read the file info
    /// </summary>
    /// <param name="fileName"></param>
    public void Init(string fileName)
    {
      archive = null;

      archive = ZipFile.Read(fileName);

      string data = archive.Comment;

      archive = null;

      try
      {
        if (data != null && data != String.Empty)
        {
          json = JObject.Parse(data);
        }
        else
          json = JObject.Parse(CreateDefaultInfo());

      }
      catch( Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.ToString());
      }
    }

    /// <summary>
    /// Writes the info back to the file
    /// </summary>
    /// <param name="fileName"></param>
    public void Update(string fileName)
    {
      archive = null;

      archive = ZipFile.Read(fileName);

      string data = json.ToString(); //converts info back to string

      archive.Comment = data;

      archive.Save();

      archive = null;
    }
  }
}

/**
 {"appID":"ComicBookLover/888",
 "lastModified":"2009-10-25 14:51:31 +0000",
 "ComicBookInfo/1.0":
    {
    "series" : "Watchmen",
    "title" : "At Midnight, All the Agents",
    "publisher" : "DC Comics",
    "publicationMonth" : 9,
    "publicationYear" : 1986,
    "issue" : 1,
    "numberOfIssues" : 12,
    "volume" : 1,
    "numberOfVolumes" : 1,
    "rating" : 5,
    "genre" : "Superhero"
    "language":"English",
    "country":"United States",

    "credits" : [
        {
          "person" : "Moore, Alan",
          "role" : "Writer",
          "primary" : YES

        },
        {
          "person" : "Gibbons, Dave",
          "role" : "Artist"
          "primary" : YES

        },
        {
          "person" : "Gibbons, Dave",
          "role" : "Letterer"
        },
        {
          "person" : "Gibbons, John",
          "role" : "Colorer"
        },
        {
          "person" : "Wein, Len",
          "role" : "Editor"
        },
        {
          "person" : "Kesel, Barbara",
          "role" : "Editor"
        },
      ],
    "tags":[
        "Rorschach",
        "Ozymandias",
        "Nite Owl",
      ],
    "comments" : "Tales of the Black Freighter...",
    }
"x-ComicBookLover":{
        key : value
        ...
    }
"x-LinuxComicReader":{
        key : value
        ...
    }
}
 */
