using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace RatCow.XmlValidation
{
    public class XmlValidator
    {
        public string XmlFilePath { get; private set; }
        public string XsdFilePath { get; private set; }

        private bool fXmlPathIsDirectory = false;

        public List<string> Errors { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public XmlValidator(string xmlPath, string xsdPath, bool treatXmlPathAsDirtectory = false)
        {
            fXmlPathIsDirectory = treatXmlPathAsDirtectory;
            XmlFilePath = xmlPath;
            XsdFilePath = xsdPath;

            Errors = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Validate(string xmlfile)
        {
            Errors.Clear();

            var settings = new XmlReaderSettings();
            settings.Schemas.Add(null, XsdFilePath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(settings_ValidationEventHandler);

            var reader = XmlReader.Create(xmlfile, settings);

            while (reader.Read())
            {
                // ??
            }

            return Errors.Count == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Validate()
        {
            bool result = true;

            if (fXmlPathIsDirectory || !File.Exists(XmlFilePath))
            {
                string[] files = Directory.GetFiles(XmlFilePath, "*.xml");

                foreach (var file in files)
                {
                    result = Validate(file);
                }
            }
            else
            {
                result = Validate(XmlFilePath);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private void settings_ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
        {
            Errors.Add(e.Severity.ToString() + ", line " + e.Exception.LineNumber.ToString() + ": " + e.Message);
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool Validate(string xmlPath, string xsdPath, out string[] errors)
        {
            var v = new XmlValidator(xmlPath, xsdPath, Directory.Exists(xmlPath));

            var result = v.Validate();

            errors = v.Errors.ToArray();

            return result;
        }
    }
}
