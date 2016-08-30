using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace FindDuplicateFiles
{
    public class Program
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

        public static List<FileKeys> DeleteUniqueItems(List<FileKeys> fileLists)
        {
            List<FileKeys> tempFileLists = new List<FileKeys>();
            List<FileKeys> GroupedListBySize = fileLists.OrderBy(o => o.FileObj.Length).ToList();
            while (GroupedListBySize.Count > 1)
            {
                int i = 0;
                bool isDuplicated = false;
                for (int j = i + 1; j < GroupedListBySize.Count; j++)
                {
                    if (GroupedListBySize[i].FileObj.Length == GroupedListBySize[j].FileObj.Length)
                    {
                        //add duplicated to new List
                        tempFileLists.Add(GroupedListBySize[j]);

                        //remove from old
                        GroupedListBySize.RemoveAt(j);
                        j--;
                        isDuplicated = true;
                    }
                    else
                    {
                        if (isDuplicated) tempFileLists.Add(GroupedListBySize[i]);
                        GroupedListBySize.RemoveAt(i);
                        break;
                    }
                }
                if (isDuplicated && GroupedListBySize.Count==1) tempFileLists.Add(fileLists[i]);


            }

            return tempFileLists;
        }

        static void Main(string[] args)
        {

         
            if (args.Length==0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Directory is not entered.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"FindDuplicateFiles.exe DirectoryPath. Example: FindDuplicateFiles.exe C:\Dir");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                return;

            }
            string directoryArg = args[0];
            Console.WriteLine("Scanning Folder {0}", directoryArg);
            try
            {
                DirectoryInfo di = new DirectoryInfo(directoryArg);
                FileInfo[] fileList = di.GetFiles("*", SearchOption.AllDirectories);
                List<FileKeys> fl = new List<FileKeys>();

                //Array2List; we can use just FileInfo instead FileKeys, just for training purpose.
                foreach (FileInfo filesInfo in fileList)
                {
                    fl.Add(new FileKeys() { FileObj = filesInfo });
                  
                }
                List<FileKeys> flsorted = DeleteUniqueItems(fl);

              
                while (flsorted.Count > 0)
                {
                    int i = 0;
                    int jj = 0;
                    for (int j = 1; j < flsorted.Count; j++)
                    {
                        if (flsorted[i].FileObj.Length == flsorted[j].FileObj.Length)
                        {
                            if (CompareTwoFiles(flsorted[i], flsorted[j]))
                            {
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
                    if (jj == flsorted.Count - 1)
                    {
                        flsorted.RemoveAt(0);
                    }

                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:{0}", e);
                return;
            }
            

            
        }
    }
}
