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
        private void GenereteVirtualFiles(int NonUniqefilesCount, int UniqueFilesCount, ref List<Program.FileKeys> fileList)
        {
            //Add virtual files to List usage Program.FileKeys(FileBytes)

            //Add unique file sizes to List
            Random rnd = new Random(UniqueFilesCount);
            for (int i = 0; i < UniqueFilesCount; i++)
            {
                fileList.Add(new Program.FileKeys(rnd.Next()));
            }


            //add non-unique files to List
            for (int i = 0; i < NonUniqefilesCount; i++)
            {
                fileList.Add(new Program.FileKeys(0));


            }


        }



        private List<Program.FileKeys> GetDirectoryFilesAsFileKeys(string directory)
        {

            DirectoryInfo di = new DirectoryInfo(directory);
            FileInfo[] fileList = di.GetFiles();
            List<Program.FileKeys> fl = new List<Program.FileKeys>();

            foreach (FileInfo filesInfo in fileList)
            {
                fl.Add(new Program.FileKeys(filesInfo, filesInfo.Length) { });
            }

            return fl;
        }

        [TestMethod()]
        public void DeleteUniqueItemsTestRealFiles()
        {
            List<Program.FileKeys> lisfilesFileKeyses = new List<Program.FileKeys>();
            lisfilesFileKeyses = GetDirectoryFilesAsFileKeys(@"d:\code\FindDulicateFiles\InpudFolder\");

            Assert.AreEqual(12, lisfilesFileKeyses.Count, "Files count should be 12");

            List<Program.FileKeys> UniqeItemsRemovedList = new List<Program.FileKeys>();

            UniqeItemsRemovedList = Program.DeleteUniqueItems(lisfilesFileKeyses);
            Assert.AreEqual(8, UniqeItemsRemovedList.Count, "After Removing unique files; in List should be 8 items");


        }

        [TestMethod()]
        public void DeleteUniqueItemsTestVirtualFiles()
        {
            int GenerateNonUniqueFilesCount = 1000;
            int GenerateUniqueFilesCount = 1000;
            List<Program.FileKeys> lisfilesFileKeyses = new List<Program.FileKeys>();
            GenereteVirtualFiles(GenerateNonUniqueFilesCount, GenerateUniqueFilesCount, ref lisfilesFileKeyses);


            List<Program.FileKeys> UniqeItemsRemovedList = new List<Program.FileKeys>();

            UniqeItemsRemovedList = Program.DeleteUniqueItems(lisfilesFileKeyses);
            Assert.AreEqual(GenerateNonUniqueFilesCount, UniqeItemsRemovedList.Count, String.Format("After Removing unique files; in List should be {0} items", GenerateNonUniqueFilesCount));


        }

        [TestMethod()]
        public void CompareTwoFilesTest()
        {
            FileInfo file1 = new FileInfo(@"c:\file1.txt");
            FileInfo file2 = new FileInfo(@"c:\file2.txt");
            using (FileStream fs = file1.Create())
            using (FileStream fs2 = file2.Create())
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("EqualTextinFiles");

                //Add some information to the file.
                fs.Write(info, 0, info.Length);
                fs2.Write(info, 0, info.Length);
            }
            
            Assert.IsTrue(Program.CompareTwoFiles(file1, file2));
            file1.Delete();
            file2.Delete();
        }
    }
}