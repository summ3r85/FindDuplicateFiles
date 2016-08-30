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
            
            Assert.AreEqual(12, lisfilesFileKeyses.Count,"Files count should be 12");

            List < Program.FileKeys > UniqeItemsRemovedList = new List<Program.FileKeys>();

            UniqeItemsRemovedList = Program.DeleteUniqueItems(lisfilesFileKeyses);
            Assert.AreEqual(8, UniqeItemsRemovedList.Count, "After Removing unique files; in List should be be 8 items");


        }
    }
}