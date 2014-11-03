using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace UtilitiesLib
{
    public class XMLBuilder {

        private XDocument _document;

        private XElement ParentElement { get; set; }

        private XElement CurrentParent { get; set; }

        public XDocument CurrentDocument {
            get { return _document; }
            private set { _document = value; }
        }

        private string _fileLocation = "";
        public string FileLocation {
            get { return _fileLocation; }
            set { _fileLocation = value; }
        }

        public XNamespace CurrentNamespace {
            get;
            set;
        }

        public static StringBuilder SchemaValidationMessage { get; private set; }

        public XMLBuilder() {
            _document = new XDocument();
        }

        public XMLBuilder(XmlReader reader) {
            _document = new XDocument(reader);
        }

        public XMLBuilder(XDocument document) {
            _document = document;
        }

        public XMLBuilder(string location, bool load) {
            this._fileLocation = location;
            if (load) {
                _document = XDocument.Load(location);
            } else {
                _document = new XDocument(new XDeclaration("1.0", null, "no"));
            }
        }


        public void SetParentElement(string name) {
            XName elementName = SetNamespace(name);
            if (ParentElement == null) {
                ParentElement = new XElement(elementName);
                CurrentDocument.Add(ParentElement);
                CurrentParent = ParentElement;
            } else {
                CurrentParent = ParentElement;
            }
        }

        private void SetParentElement(XName elementName) {
            if (ParentElement == null) {
                ParentElement = new XElement(elementName);
                CurrentDocument.Add(ParentElement);
                CurrentParent = ParentElement;
            } else {
                CurrentParent = ParentElement;
            }
        }

        private XName SetNamespace(string name) {
            XName xName = null;
            if (CurrentNamespace != null) {
                xName = CurrentNamespace + name;
            } else {
                xName = name;
            }
            return xName;
        }


        public void SetNextParent(string name) {
            XName parentName = SetNamespace(name);
            XElement nextParent = null;
            if (CurrentParent != null) {
                if (CurrentParent.Name != parentName) {
                    nextParent = new XElement(parentName);
                    CurrentParent.Add(nextParent);
                    CurrentParent = nextParent;
                }
            } else {
                if (ParentElement != null) {
                    CurrentParent = new XElement(parentName);
                    ParentElement.Add(CurrentParent);
                } else {
                    SetParentElement(parentName);
                }
            }

        }

        public bool ResetParent(string name) {
            XName parentName = SetNamespace(name);
            XElement parent = _document.Descendants(parentName).Last();
            if (parent != null) {
                CurrentParent = parent;
                return true;
            } else {
                return false;
            }
        }

        public void AddChildToCurrentParent(string name, object value) {
            XName elementName = SetNamespace(name);
            XElement childElement = new XElement(elementName, value);
            if (CurrentParent != null) {
                CurrentParent.Add(childElement);
            }
        }

        public void AddChildToCurrentParent(string name, object value, string attribute, object attributeValue) {
            XName elementName = SetNamespace(name);
            XElement childElement = new XElement(elementName, value);
            XAttribute at = new XAttribute(attribute, attributeValue);
            childElement.Add(at);
            if (CurrentParent != null) {
                CurrentParent.Add(childElement);
            }
        }

        public void AddChildElements(string childName, Dictionary<string, object> childElementValues) {
            XName elementName = SetNamespace(childName);
            if (CurrentParent == null) {
                return;
            }
            XElement childElement = new XElement(elementName);
            foreach (KeyValuePair<string, object> kvp in childElementValues) {
                XElement elem = new XElement(kvp.Key, kvp.Value);
                childElement.Add(elem);
            }
            CurrentParent.Add(childElement);
        }

        public void AddAttribute(string name, XName attrName, object value) {
            XName parentName = SetNamespace(name);
            XElement parentElement = null;
            if (CurrentParent != null && CurrentParent.Name.ToString().Equals(parentName)) {
                parentElement = CurrentParent;
            }
            if (parentElement == null) {
                parentElement = _document.Element(parentName);
                if (parentElement == null && CurrentParent != null) {
                    parentElement = CurrentParent.Element(parentName);
                }
            }
            if (parentElement != null) {
                parentElement.SetAttributeValue(attrName, value);
            }
        }

        public void AddAttributes(string name, params object[] attributeValues) {
            XName parentName = SetNamespace(name);
            if (attributeValues != null) {
                if (attributeValues.Count() % 2 == 0) {
                    int i = 0;
                    XElement parentElement = null;
                    if (CurrentParent != null && CurrentParent.Name.ToString().Equals(parentName)) {
                        parentElement = CurrentParent;
                    }
                    if (parentElement == null) {
                        parentElement = _document.Element(parentName);
                        if (parentElement == null && CurrentParent != null) {
                            parentElement = CurrentParent.Element(parentName);
                        }
                    }
                    if (parentElement != null) {
                        string at = "";
                        foreach (object t in attributeValues) {
                            if (i % 2 == 0) {
                                at = attributeValues[i].ToString();
                            } else {
                                object value = attributeValues[i];
                                XAttribute attribute = new XAttribute(at, value);
                                parentElement.Add(attribute);
                            }
                            i++;
                        }
                    }
                }
            }
        }

        public void AddProcessingInstruction(string target, string instruction) {
            this.CurrentDocument.Add(new XProcessingInstruction(target, instruction));
        }

        public void SetParentNamespace(string namespaceName) {
            XNamespace xmlns = XNamespace.Get(namespaceName);
            this.CurrentNamespace = xmlns;
        }

        public void WriteToFile() {
            using (var writer = new StreamWriter(_fileLocation, false, new UTF8Encoding(false))) {
                this.CurrentDocument.Save(writer);
            }
        }

        public XElement GetElement(string childName) {
            return CurrentDocument.Element(childName);
        }

        public XElement GetChildElement(string parentName, string childName) {
            XElement childElement = null;
            XElement parentElement = CurrentDocument.Element(parentName);
            if (parentElement != null) {
                childElement = parentElement.Element(childName);
            }
            return childElement;
        }

        public object GetAttributeValue(string childName, string attribute) {
            return CurrentDocument.Element(childName).Attribute(attribute).Value;
        }

        public XElement GetFirstDecendant(string childName) {
            return (CurrentDocument.Descendants(childName).First());
        }

        public IEnumerable<XElement> GetFirstDecendants(string childName) {
            return (CurrentDocument.Descendants(childName).First()).Descendants();
        }

        public object GetValueAt(string childName, string elementName) {
            return (CurrentDocument.Descendants(childName).First()).Descendants(elementName).First().Value;
        }


        public static XDocument XMLReaderDocumentValidate(string schemaDirectoryPath, string targetNamespace, string fileLocation, StringBuilder sb, string xsdName = "") {
            var xmlText = File.ReadAllText(fileLocation);
            XmlReader reader;
            XDocument xDocument = null;

            ValidationEventHandler validationEventHandler = new ValidationEventHandler(ValidationHandler);
            XmlReaderSettings settings = new XmlReaderSettings();
            DirectoryInfo di = new DirectoryInfo(schemaDirectoryPath);
            string filter = xsdName.IsNullOrEmpty() ? "*.xsd" : xsdName;
            foreach (FileInfo fo in di.GetFiles(filter)) {
                if (settings.Schemas.Contains(fo.FullName) == false) {
                    settings.Schemas.Add(targetNamespace, fo.FullName);
                }
            }
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += validationEventHandler;
            reader = XmlReader.Create(new StringReader(xmlText), settings);
            if (IsValidDocument(reader, ref xDocument, sb)) {
                return xDocument;
            } else {
                return null;
            }
        }


        public static XName GetElementXName(string elementName, string namespaceName) {
            return XName.Get(elementName, namespaceName);
        }

        public static int ConvertToInt(XElement element) {
            int value = 0;
            string sValue = (element == null || element.Value == null) ? "0" : element.Value.ToString();
            Int32.TryParse(sValue, out value);
            return value;
        }

        public static decimal ConvertToDecimal(XElement element) {
            decimal value = 0;
            string sValue = (element == null || element.Value == null) ? "0" : element.Value.ToString();
            Decimal.TryParse(sValue, out value);
            return value;
        }

        public static DateTime ConvertToDateTime(XElement element) {
            DateTime value = DateTime.MinValue;
            if (element != null && element.Value != null) {
                value = (DateTime)element;
            }
            return value;
        }

        public static bool ConvertToBool(XElement element) {
            bool value = false;
            if (element != null) {
                value = element.Value.ToString().Equals("Y", StringComparison.InvariantCultureIgnoreCase);
            }
            return value;
        }


        private static bool IsValidDocument(XmlReader reader, ref XDocument document, StringBuilder sb) {
            bool isValid = true;
            SchemaValidationMessage = new StringBuilder();
            try {
                document = XDocument.Load(reader);
            } catch (Exception e) {
                isValid = false;
                sb.Append(e.Message);
            }
            isValid = SchemaValidationMessage.Length == 0;
            if (isValid == false) {
                sb.Append(SchemaValidationMessage);
            }
            return isValid;
        }

        private static void ValidationHandler(object sender, ValidationEventArgs vea) {
            if (SchemaValidationMessage.Length == 0) {
                SchemaValidationMessage.AppendFormat("XML file not valid:  {0} ...", vea.Message);
            }
        }		 	
	


    }
}
