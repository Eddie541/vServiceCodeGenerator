using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UtilitiesLib;

namespace VSCDBusinessLib {
    public abstract class ManagerIOBase {
        public const string Abstract = "abstract";
        public const string Protected = "protected";
        public const string Partial = "partial";
        public const string Public = "public";
        public const string Static = "static";
        public const string Private = "private";
        public const string _Instance = "_instance";
        public const string Instance = "Instance";
        public const string Override = "override";

        public const string Method = "Method";
        public const string Parameter = "Parameter";
        public const string DataManager = "DataManager";

        protected string _validationErrors;

        protected bool InitializeBuilder(string xmlDataFile, string schemaDirectory, ref XMLBuilder builder) {
            bool initialized = false;
            StringBuilder sb = new StringBuilder();
            XDocument doc = XMLBuilder.XMLReaderDocumentValidate(schemaDirectory, BusinessConstants.JmlfdcNamespace, xmlDataFile, sb);
            if (doc != null) {
                builder = new XMLBuilder(doc);
                initialized = true;
            }

            _validationErrors = sb.ToString();
            if (_validationErrors.IsNullOrEmpty() == false) {
                throw new ApplicationException(string.Format("The XML file {0} is not valid...{1}", xmlDataFile, _validationErrors));
            }

            return initialized;
        }

    }
}
