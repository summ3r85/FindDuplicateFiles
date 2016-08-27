using Microsoft.VisualStudio.TestTools.UnitTesting;
using FindDuplicateFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindDuplicateFiles.Tests
{
    [TestClass()]
    public class ProgramTests
    {

        public List<Program.FileKeys> GetDirectoryFilesAsFileKeys(string directory)
        {

            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] fileList = di.GetFiles();
            List<Program.FileKeys> fl = new List<Program.FileKeys>();

            foreach (FileInfo filesInfo in fileList)
            {
                fl.Add(new Program.FileKeys() { FileObj = filesInfo });
            }

            return fl;
        }

        [TestMethod()]
        public void DeleteUniqueItemsTest()
        {
            List<Program.FileKeys> lisfilesFileKeyses = new List<Program.FileKeys>();
            lisfilesFileKeyses = GetDirectoryFilesAsFileKeys(@"d:\code\FindDulicateFiles\InpudFolder\");
            
            Assert.AreEqual(lisfilesFileKeyses.Count,12,"Files Count should be 12");

            List < Program.FileKeys > UniqeItemsRemovedList = new List<Program.FileKeys>();
            UniqeItemsRemovedList = Program.DeleteUniqueItems(lisfilesFileKeyses);
            Assert.AreEqual(UniqeItemsRemovedList.Count, 8, "After Removing unique files size Count should be 12");


        }
    }
}