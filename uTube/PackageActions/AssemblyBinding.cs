using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.XPath;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using Umbraco.Core.Logging;
using umbraco.interfaces;

namespace uTube.PackageActions
{

    /// <summary>
    /// KUDOS to Jeavon - this is from the bundling Umbraco package 'Optimus'
    /// </summary>
    public class AddAssemblyBinding : IPackageAction
    {
        public string Alias()
        {
            return "uTube.AddAssemblyBinding";
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {

            // Set result default to false
            bool result = false;

            // Set insert node default true
            bool insertNode = true;

            // Set modified document default to false
            bool modified = false;

            // Get attribute values of xmlData
            string name, publicKeyToken, oldVersion, newVersion;
            if (!this.GetAttribute(xmlData, "name", out name) || !this.GetAttribute(xmlData, "publicKeyToken", out publicKeyToken) || !this.GetAttribute(xmlData, "oldVersion", out oldVersion) || !this.GetAttribute(xmlData, "newVersion", out newVersion))
            {
                return result;
            }

            string filename = HttpContext.Current.Server.MapPath("/web.config");
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(filename);
            }
            catch (FileNotFoundException)
            {
            }

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(document.NameTable);
            nsmgr.AddNamespace("bindings", "urn:schemas-microsoft-com:asm.v1");

            XPathNavigator nav = document.CreateNavigator().SelectSingleNode("//bindings:assemblyBinding", nsmgr);
            if (nav == null)
            {
                throw new Exception("Invalid Configuration File");
            }

            // Look for existing nodes with same path like the new node
            if (nav.HasChildren)
            {
                // Look for existing nodeType nodes
                var node =
                    nav.SelectSingleNode(string.Format("./bindings:dependentAssembly/bindings:assemblyIdentity[@publicKeyToken = '{0}' and @name='{1}']", publicKeyToken, name), nsmgr);

                // If path already exists 
                if (node != null)
                {
                    if (node.MoveToNext())
                    {
                        if (node.MoveToAttribute("oldVersion", string.Empty))
                        {
                            node.SetValue(oldVersion);
                        }

                        if (node.MoveToParent())
                        {
                            if (node.MoveToAttribute("newVersion", string.Empty))
                            {
                                node.SetValue(newVersion);
                            }
                        }

                        // Cancel insert node operation
                        insertNode = false;

                        // Lets update versions instead
                        modified = true;
                    }
                    else
                    {
                        //Log error message
                        string message = "Error at AddAssemblyBinding package action: "
                             + "Updating \"" + name + "\" assembly binding failed.";
                        LogHelper.Warn(typeof(AddAssemblyBinding), message);
                    }
                }
            }

            // Check for insert flag
            if (insertNode)
            {
                var newNodeContent =
                    string.Format(
                        "<dependentAssembly><assemblyIdentity name=\"{0}\" publicKeyToken=\"{1}\" culture=\"neutral\" />"
                        + "<bindingRedirect oldVersion=\"{2}\" newVersion=\"{3}\" /></dependentAssembly>",
                        name,
                        publicKeyToken,
                        oldVersion,
                        newVersion);

                nav.AppendChild(newNodeContent);

                modified = true;

            }

            if (modified)
            {
                try
                {
                    document.Save(filename);

                    // No errors so the result is true
                    result = true;
                }
                catch (Exception e)
                {
                    // Log error message
                    string message = "Error at execute AddAssemblyBinding package action: " + e.Message;
                    LogHelper.Error(typeof(AddAssemblyBinding), message, e);
                }
            }
            return result;

        }

        public XmlNode SampleXml()
        {
            string str =
                "<Action runat=\"install\" undo=\"false\" alias=\"uTube.AddAssemblyBinding\" name=\"newone\" publicKeyToken=\"973c344c93aac60d\" oldVersion=\"1\" newVersion=\"2\" />";
            return helper.parseStringToXmlNode(str);
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return false;
        }

        /// <summary>
        /// Get a named attribute from xmlData root node
        /// </summary>
        /// <param name="xmlData">The data that must be appended to the web.config file</param>
        /// <param name="attribute">The name of the attribute</param>
        /// <param name="value">returns the attribute value from xmlData</param>
        /// <returns>True, when attribute value available</returns>
        private bool GetAttribute(XmlNode xmlData, string attribute, out string value)
        {
            //Set result default to false
            bool result = false;

            //Out params must be assigned
            value = String.Empty;

            //Search xml attribute
            XmlAttribute xmlAttribute = xmlData.Attributes[attribute];

            //When xml attribute exists
            if (xmlAttribute != null)
            {
                //Get xml attribute value
                value = xmlAttribute.Value;

                //Set result successful to true
                result = true;
            }
            else
            {
                //Log error message
                string message = "Error at AddAssemblyBinding package action: "
                     + "Attribute \"" + attribute + "\" not found.";
                LogHelper.Warn(typeof(AddAssemblyBinding), message);
            }
            return result;
        }

    }
}