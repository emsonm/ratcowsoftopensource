using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using System.IO;
using System.Xml;
using BrendanGrant.Helpers.FileAssociation;

namespace Outliner
{
  public partial class MainForm : Form
  {

    public MainForm()
    {
      InitializeComponent();

      InitFileAssociations();
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      TreeNode node = mainView.SelectedNode; //selected item

      if (node == null) //parent
      {
        mainView.Nodes.Add("New Item");
      }
      else             //child
      {
        node.Nodes.Add("New child Item");
      }

      mainView.ExpandAll();
    }

    private void removeButton_Click(object sender, EventArgs e)
    {
      TreeNode node = mainView.SelectedNode; //selected item

      if (node != null)
      {
        mainView.Nodes.Remove(node);
      }
    }

    private void insertBeforeToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode node = mainView.SelectedNode; //selected item

      if (node != null)
      {
        if (node.Parent != null)
        {
          node.Parent.Nodes.Insert(node.Index, "New child Item");
        }
        else
        {
          mainView.Nodes.Insert(node.Index, "New Item");
        }
      }

      mainView.ExpandAll();

    }

    private void insertAtBeginningToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode node = mainView.SelectedNode; //selected item

      if (node != null)
      {
        if (node.Parent != null)
        {
          node.Parent.Nodes.Insert(0, "New child Item");
        }
        else
        {
          mainView.Nodes.Insert(0, "New Item");
        }
      }

      mainView.ExpandAll();
    }

    private void insertAfterSelectedToolStripMenuItem_Click(object sender, EventArgs e)
    {
      TreeNode node = mainView.SelectedNode; //selected item

      if (node != null)
      {
        if (node.Parent != null)
        {
          node.Parent.Nodes.Insert(node.Index + 1, "New child Item");
        }
        else
        {
          mainView.Nodes.Insert(node.Index + 1, "New Item");
        }
      }

      mainView.ExpandAll();
    }

    string loadedFileName = "default.meo";

    private void MainForm_Load(object sender, EventArgs e)
    {
      //did we get loaded with a param?
      string[] items = Environment.GetCommandLineArgs();


      if (items.Length > 1)
      {
        //Assume items[1] is passed path
        LoadData(items[1]);
      }
      else if (File.Exists("default.meo"))
      {
        //load data
        LoadData("default.meo");
      }
      else
      {
        //we do nothing
      }
      mainView.ExpandAll();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      //save
      SaveData(loadedFileName);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileToLoad"></param>
    void LoadData(string fileToLoad)
    {
      mainView.Nodes.Clear(); // to be sure

      XmlTextReader reader = null;
      try
      {
        // disabling re-drawing of treeview till all nodes are added

        mainView.BeginUpdate();
        reader = new XmlTextReader(fileToLoad);
        TreeNode parentNode = null;
        while (reader.Read())
        {
          if (reader.NodeType == XmlNodeType.Element)
          {
            if (reader.Name == XmlNodeTag)
            {
              TreeNode newNode = new TreeNode();
              bool isEmptyElement = reader.IsEmptyElement;

              // loading node attributes

              int attributeCount = reader.AttributeCount;
              if (attributeCount > 0)
              {
                for (int i = 0; i < attributeCount; i++)
                {
                  reader.MoveToAttribute(i);
                  SetAttributeValue(newNode, reader.Name, reader.Value);
                }
              }
              // add new node to Parent Node or TreeView

              if (parentNode != null)
                parentNode.Nodes.Add(newNode);
              else
                mainView.Nodes.Add(newNode);

              // making current node 'ParentNode' if its not empty

              if (!isEmptyElement)
              {
                parentNode = newNode;
              }
            }
          }
          // moving up to in TreeView if end tag is encountered

          else if (reader.NodeType == XmlNodeType.EndElement)
          {
            if (reader.Name == XmlNodeTag)
            {
              parentNode = parentNode.Parent;
            }
          }
          else if (reader.NodeType == XmlNodeType.XmlDeclaration)
          {
            //Ignore Xml Declaration                    

          }
          else if (reader.NodeType == XmlNodeType.None)
          {
            return;
          }
          else if (reader.NodeType == XmlNodeType.Text)
          {
            parentNode.Nodes.Add(reader.Value);
          }

        }
      }
      catch(System.Xml.XmlException)
      {
        //ignore
      }
      finally
      {
        // enabling redrawing of treeview after all nodes are added

        mainView.EndUpdate();
        reader.Close();
        loadedFileName = fileToLoad;
        this.Text = String.Format("Outliner - {0}", Path.GetFileName(loadedFileName));
      }


    }

    private void SetAttributeValue(TreeNode node, string propertyName, string value)
    {
      if (propertyName == XmlNodeTextAtt)
      {
        node.Text = value;
      }
      else if (propertyName == XmlNodeImageIndexAtt)
      {
        node.ImageIndex = int.Parse(value);
      }
      else if (propertyName == XmlNodeTagAtt)
      {
        node.Tag = value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    void SaveData(string fileName)
    {
      XmlTextWriter textWriter = new XmlTextWriter(fileName, System.Text.Encoding.ASCII);
      // writing the xml declaration tag

      textWriter.WriteStartDocument();
      //textWriter.WriteRaw("\r\n");

      // writing the main tag that encloses all node tags

      textWriter.WriteStartElement("TreeView");

      // save the nodes, recursive method

      SaveNodes(mainView.Nodes, textWriter);

      textWriter.WriteEndElement();

      textWriter.Close();
    }

    private const string XmlNodeTag = "outline";

    // Xml attributes for node e.g. <node text="Asia" tag="" 
    // imageindex="1"></node>
    private const string XmlNodeTextAtt = "text";
    private const string XmlNodeTagAtt = "tag";
    private const string XmlNodeImageIndexAtt = "imageindex";

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nodesCollection"></param>
    /// <param name="textWriter"></param>
    private void SaveNodes(TreeNodeCollection nodesCollection, XmlTextWriter textWriter)
    {
      for (int i = 0; i < nodesCollection.Count; i++)
      {
        TreeNode node = nodesCollection[i];
        textWriter.WriteStartElement(XmlNodeTag);
        textWriter.WriteAttributeString(XmlNodeTextAtt, node.Text);
        textWriter.WriteAttributeString(XmlNodeImageIndexAtt, node.ImageIndex.ToString());
        if (node.Tag != null)
          textWriter.WriteAttributeString(XmlNodeTagAtt, node.Tag.ToString());
        // add other node properties to serialize here  

        if (node.Nodes.Count > 0)
        {
          SaveNodes(node.Nodes, textWriter);
        }
        textWriter.WriteEndElement();
      }
    }

    void InitFileAssociations()
    {
      FileAssociationInfo fai = new FileAssociationInfo(".meo");
      if (!fai.Exists)
      {
        fai.Create("MattsExperimentalOutliner");

        //Specify MIME type (optional)

        fai.ContentType = "application/outliner-data";

        //Programs automatically displayed in open with list

        fai.OpenWithList = new string[] { "outliner.exe", "notepad.exe", "someotherapp.exe" };
      }

      ProgramAssociationInfo pai = new ProgramAssociationInfo(fai.ProgID);
      if (!pai.Exists)
      {
        pai.Create
        (
          //Description of program/file type
          "Matt's Experimental Outliner",

          new ProgramVerb(
            //Verb name
            "Open",
            //Path and arguments to use
            String.Format("{0} %1", Application.ExecutablePath)
          )
         );

        //optional

        //pai.DefaultIcon = new ProgramIcon(@"C:\SomePath\SomeIcon.ico");
      }


    }




  }
}
