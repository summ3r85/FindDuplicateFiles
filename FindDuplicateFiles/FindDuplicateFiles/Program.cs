using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FindDuplicateFiles
{
    class Program
    {   
        public class FileKeys
        {

            public FileInfo FileObj { get; set; }

            public override string ToString()
            {
                return "filename: " + FileObj.Name + "   Size: " + FileObj.Length;
            }

            
        }
        static bool CompareTwoFiles(FileKeys first, FileKeys second)
        {
            if (first.FileObj.Length != second.FileObj.Length)
                return false;

            using (FileStream fs1 = first.FileObj.OpenRead())
            using (FileStream fs2 = second.FileObj.OpenRead())
            {
                for (int i = 0; i < first.FileObj.Length; i++)
                {
                    if (fs1.ReadByte() != fs2.ReadByte())
                        return false;
                }
            }

            return true;
        }

        static List<FileKeys> DeleteUniqueItems(List<FileKeys> fileLists)
        {
            List<FileKeys> tempFileLists = new List<FileKeys>();
           while (fileLists.Count > 0)
            {
                int i = 0;
                bool isDuplicated = false;
                for (int j = i + 1; j < fileLists.Count; j++)
                {
                    if (fileLists[i].FileObj.Length == fileLists[j].FileObj.Length)
                    {
                        //add duplicated to new List
                        tempFileLists.Add(fileLists[j]);

                        //remove from old
                        fileLists.RemoveAt(j);
                        j--;
                        isDuplicated = true;
                    }
                }
                if (isDuplicated) tempFileLists.Add(fileLists[i]);
                fileLists.RemoveAt(i);

            }

            return tempFileLists;
        }

        static void Main(string[] args)
        {

            string directoryArg = args[0];
            if (directoryArg.Length<0)
            {
                Console.WriteLine(@"Directory is not entered. Usage: FindDuplicateFiles.exe C:\Dir");
                Console.ReadLine();
                
            }
            Console.WriteLine("Scanning Folder {0}", directoryArg);
            DirectoryInfo di = new DirectoryInfo(directoryArg);

            FileInfo[] fileList = di.GetFiles();
            List<FileKeys> fl = new List<FileKeys>();
            
            foreach (FileInfo filesInfo in fileList)
            {
                fl.Add(new FileKeys() {FileObj = filesInfo} );
            }

            List<FileKeys> flsorted = DeleteUniqueItems(fl);

            //Sort by Size;
            flsorted.Sort((keys, fileKeys) =>(int)keys.FileObj.Length);

            while (flsorted.Count>0)
            {
                int i = 0;
                int jj = 0;
                for (int j = 1; j <flsorted.Count; j++)
                {
                    if (flsorted[i].FileObj.Length == flsorted[j].FileObj.Length)
                    {
                        if (CompareTwoFiles(flsorted[i], flsorted[j])) { 
                            Console.WriteLine("{0}\r\n{1}\r\n", flsorted[i], flsorted[j]);
                            }
                    }
                    else
                    {
                        flsorted.RemoveAt(i);
                        break;
                    }
                    jj = j;
                }
                if (jj==flsorted.Count-1)
                {
                    flsorted.RemoveAt(0);
                }

            }
            Console.ReadLine();
        }
    }
}
