using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Schema;

namespace RatCow.XmlValidation
{
    public class XmlValidator
    {
        #region Properties and fields

        public string XmlFilePath { get; private set; }
        public string XsdFilePath { get; private set; }
        public string FileExtension { get; set; }

        private bool fXmlPathIsDirectory = false;

        public List<ValidationEventArgs> Errors { get; private set; }

        public List<String> Files { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public XmlValidator(string xmlPath, string xsdPath, bool treatXmlPathAsDirtectory = false)
        {
            fXmlPathIsDirectory = treatXmlPathAsDirtectory;
            XmlFilePath = xmlPath;
            XsdFilePath = xsdPath;

            Errors = new List<ValidationEventArgs>();
            Files = new List<string>();
        }

        #endregion

        #region Validation implementation
        /// <summary>
        /// 
        /// </summary>
        public bool Validate(string xmlfile)
        {


            var settings = new XmlReaderSettings();
            try
            {
                settings.Schemas.Add(null, XsdFilePath);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(settings_ValidationEventHandler);

                using (var reader = XmlReader.Create(xmlfile, settings))
                {
                    while (reader.Read())
                    {

                    }

                    reader.Close();
                }
            }
            finally
            {
                settings = null;
            }

            return Errors.Count == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Validate()
        {
            bool result = true;
            Errors.Clear();
            Files.Clear();

            if (fXmlPathIsDirectory || !File.Exists(XmlFilePath))
            {
                string[] files = Directory.GetFiles(XmlFilePath, "*.xml");

                foreach (var file in files)
                {
                    Files.Add(file);
                    result = Validate(file);
                }
            }
            else
            {
                Files.Add(XmlFilePath);
                result = Validate(XmlFilePath);
            }

            return result;
        }

        #endregion

        #region Report implementation

        /// <summary>
        /// Generates a report file
        /// </summary>
        public void Report()
        {
            var filePath = Path.Combine(System.Environment.CurrentDirectory, String.Format("rpt{0}.html", DateTime.Now.Ticks));
            using (var outfile = File.CreateText(filePath))
            {
                WriteReportHeader(outfile, String.Format("Report for validations against {0}", Path.GetFileName(XsdFilePath)));

                WriteReportSummary(outfile);

                WriteReportTableStart(outfile);

                foreach (var error in Errors)
                {
                    WriteReportTableRow(outfile, error);
                }

                WriteReportTableEnd(outfile);

                WriteReportFooter(outfile);
            }
        }

        #region WriteReportXxxxxx implementation

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportHeader(StreamWriter outfile, string title)
        {
            outfile.WriteLine("<html>");
            outfile.WriteLine("<head>");
            outfile.WriteLine(String.Format("  <title>{0}</title>", title));
            outfile.WriteLine("  <style type=\"text/css\">");
            outfile.WriteLine("    .tableStyle");
            outfile.WriteLine("    {");
            outfile.WriteLine("      border-width: medium; font-size: small; table-layout: auto; border-collapse: collapse; border-spacing: 1px; empty-cells: show; border-style: solid; height: 20px; width: auto; caption-side: top; text-align: justify; background-color: #E4E4E4;");
            outfile.WriteLine("    }");
            outfile.WriteLine();
            outfile.WriteLine("    .headerCellStyle");
            outfile.WriteLine("    {");
            outfile.WriteLine("      padding: 2px 5px 2px 5px; text-align: justify; border-right-style: solid; border-bottom-style: solid; border-bottom-width: 2px; border-bottom-color: #808080; ");
            outfile.WriteLine("    }");
            outfile.WriteLine();
            outfile.WriteLine("    .cellStyle");
            outfile.WriteLine("    {");
            outfile.WriteLine("      padding: 2px 5px 2px 5px; text-align: justify; border-right-style: solid; border-bottom-style: solid; border-bottom-width: 2px;");
            outfile.WriteLine("    }");
            outfile.WriteLine();
            outfile.WriteLine("  </style>");
            outfile.WriteLine("</head>");
            outfile.WriteLine("<body>");
            outfile.WriteLine(String.Format("  <h1>{0}</h1>", title));
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportFooter(StreamWriter outfile)
        {
            outfile.WriteLine("</body>");
            outfile.WriteLine("</html>");
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportSummary(StreamWriter outfile)
        {
            outfile.WriteLine("<div>");
            DateTime dt = DateTime.Now;
            outfile.WriteLine(String.Format("Test date/time: {0} {1}<br />", dt.ToShortDateString(), dt.ToLongTimeString()));
            outfile.WriteLine("Files tested: <br />");
            outfile.WriteLine("<table cellpadding=\"0\" cellspacing=\"0\">");

            foreach (var file in Files)
            {
                outfile.WriteLine("<tr>");
                outfile.WriteLine(String.Format("<td style=\"padding-left: 20px; padding-right: 5px\">{0}</td>", Path.GetFileName(file)));
                outfile.WriteLine(String.Format("<td style=\"padding-left: 5px; padding-right: 5px\">(from \"{0}\")</td>", Path.GetFullPath(file)));
                outfile.WriteLine("</tr>");
            }

            outfile.WriteLine("</table>");
            outfile.WriteLine("</div>");
            outfile.WriteLine("<br />");
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportTableStart(StreamWriter outfile)
        {
            outfile.WriteLine("<table cellpadding=\"0\" cellspacing=\"0\" frame=\"box\" class=\"tableStyle\">");
            outfile.WriteLine("<tr>");
            outfile.WriteLine("<th class=\"headerCellStyle\">Severity</th>");
            outfile.WriteLine("<th class=\"headerCellStyle\">Line</th>");
            outfile.WriteLine("<th class=\"headerCellStyle\">Message</th>");
            outfile.WriteLine("<th class=\"headerCellStyle\">File</th>");
            outfile.WriteLine("</tr>");
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportTableRow(StreamWriter outfile, ValidationEventArgs error)
        {
            outfile.WriteLine("<tr>");
            outfile.WriteLine(String.Format("<td class=\"cellStyle\">{0}</td>", error.Severity.ToString()));
            outfile.WriteLine(String.Format("<td class=\"cellStyle\">{0}</td>", error.Exception.LineNumber.ToString()));
            outfile.WriteLine(String.Format("<td class=\"cellStyle\">{0}</td>", error.Message));
            outfile.WriteLine(String.Format("<td class=\"cellStyle\">{0} ({1}, {2})</td>", error.Exception.SourceUri, error.Exception.LineNumber, error.Exception.LinePosition));
            outfile.WriteLine("</tr>");
        }

        /// <summary>
        /// 
        /// </summary>
        private void WriteReportTableEnd(StreamWriter outfile)
        {
            outfile.WriteLine("</table>");
        }

        #endregion

        #endregion

        #region ReadXml event code

        public delegate void ReadXmlDelegate(XmlValidator sender, XmlReader reader);
        public event ReadXmlDelegate ReadXml = null;
        private void DoReadXml(XmlReader reader)
        {
            if (ReadXml != null)
            {
                ReadXml(this, reader);
            }
        }

        #endregion

        #region XmlReaderSettings ValidationEventHandler implementation

        /// <summary>
        /// This is a basic implementation
        /// </summary>
        protected virtual void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            Errors.Add(e);
        }

        #endregion

        #region Static public methods

        /// <summary>
        /// 
        /// </summary>
        public static bool Validate(string xmlPath, string xsdPath, string extension, out string[] errors)
        {
            var v = new XmlValidator(xmlPath, xsdPath, Directory.Exists(xmlPath));

            v.FileExtension = (extension.StartsWith(".") ? String.Empty : ".") + extension;

            var result = v.Validate();

            v.Report();

            errors = v.Errors.Select(e => String.Format("{0}, line {1} : {2}", e.Severity, e.Exception.LineNumber, e.Message)).ToArray();

            return result;
        }

        #endregion
    }
}
