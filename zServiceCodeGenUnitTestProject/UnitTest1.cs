using System;
using System.Text;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UtilitiesLib;
using ValiantServiceGeneratorViewModelLib;
using VSCDBusinessLib;

namespace zServiceCodeGenUnitTestProject {
    [TestClass]
    public class UnitTest1 {

        ServiceDataFileManager sdfm;
        DataManagerFileManager dmfm;

        [TestMethod]
        public void TestMethod1() {
            MainViewModel mvm = new MainViewModel() {
                SourceDirectory = @"C:\Projects\DcamDataService\DcamLocalDbDataLib",
                DestinationDirectory = @"C:\testGen\DcamDataService.ServiceInterface",
                ControllerDirectory = @"C:\Projects\DcamDataService\DcamDataService\Controllers",
                InterfaceDefinitions = @"C:\testGen\DataFiles\ServiceDataSchema.xml",
                SchemaDirectory = @"C:\testGen\Schemas",
                DataManagerDefinitions = @"C:\testGen\DataFiles\DataManagerLibrary.xml",
                DataManagerNamespace = "DcamBusiness",
                ManagerDirectory = @"C:\testGen\DataManagerTest"

            };

            sdfm = new ServiceDataFileManager(mvm.SchemaDirectory, mvm.InterfaceDefinitions, mvm.SourceDirectory,
                mvm.DestinationDirectory);
            sdfm.GenerateServiceData();

            dmfm = new DataManagerFileManager(mvm.SchemaDirectory, mvm.DataManagerDefinitions, mvm.DataManagerNamespace, mvm.ManagerDirectory);
            dmfm.GenerateDataManagerCode();


        }




    }
}
