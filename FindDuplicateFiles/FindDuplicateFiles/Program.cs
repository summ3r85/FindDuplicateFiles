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

            public FileKeys(){
            }
            public FileKeys(double length)
            {
                FileObj = new FileInfo("null_file");
                Length = length;
            }
            public FileKeys(FileInfo fileinfo, double length)
            {
                FileObj = fileinfo;
                Length = length;
            }

            public double Length { get; set; }
            public FileInfo FileObj { get ;
                    set; }

            public override string ToString()
            {
                return "filename: " + FileObj.Name + "   Size: " + FileObj.Length;
            }
            
            
        }

        const int BYTES_TO_READ = sizeof(Int64);
        public static bool CompareTwoFiles(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            int iterations = (int)Math.Ceiling((double)first.Length / BYTES_TO_READ);
            try
            {
                using (FileStream fs1 = first.OpenRead())
                using (FileStream fs2 = second.OpenRead())
                {
                    byte[] one = new byte[BYTES_TO_READ];
                    byte[] two = new byte[BYTES_TO_READ];

                    for (int i = 0; i < iterations; i++)
                    {
                        fs1.Read(one, 0, BYTES_TO_READ);
                        fs2.Read(two, 0, BYTES_TO_READ);

                        if (BitConverter.ToInt64(one, 0) != BitConverter.ToInt64(two, 0))
                            //if (!Array.Equals(one, two))
                            return false;
                    }
                }
                return true;
            }
            catch {
                return false;
            }


            
        }

        public static List<FileKeys> DeleteUniqueItems(List<FileKeys> fileLists)
        {
            List<FileKeys> tempFileLists = new List<FileKeys>();
            List<FileKeys> GroupedListBySize = fileLists.OrderBy(o => o.Length).ToList();
            bool isDuplicated = false;
            while (GroupedListBySize.Count > 1)
            {
                int i = 0;
                int j = 1;
                if (GroupedListBySize[i].Length == GroupedListBySize[j].Length)
                    {
                        //add duplicated to new List
                        tempFileLists.Add(GroupedListBySize[j]);

                        //remove from old
                        GroupedListBySize.RemoveAt(j);
                        isDuplicated = true;
                    }
                    else
                    {
                        if (isDuplicated) tempFileLists.Add(GroupedListBySize[i]);
                        GroupedListBySize.RemoveAt(i);
                        isDuplicated = false;
                    }
                 if (isDuplicated && GroupedListBySize.Count == 1) tempFileLists.Add(fileLists[i]);
                 
            }
          return tempFileLists;
        }

        static int Main(string[] args)
        {

         
            if (args.Length==0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Directory is not entered.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(@"FindDuplicateFiles.exe DirectoryPath. Example: FindDuplicateFiles.exe C:\Dir");
                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadLine();
                return 0;

            }
            string directoryArg = args[0];
            
            try
            {
                DirectoryInfo di = new DirectoryInfo(directoryArg);
                List<FileKeys> fl = new List<FileKeys>();

                //FileInfo[] fileList = di.GetFiles("*", SearchOption.AllDirectories);
                //Array2List; we can use just FileInfo instead FileKeys, just for training purpose.
                //foreach (FileInfo filesInfo in fileList)
                //{
                //    fl.Add(new FileKeys(filesInfo, filesInfo.Length));

                //}

                List<FileInfo> fileList2 = new List<FileInfo>();
                fileList2 = GetFiles(di, ref fileList2);
                foreach (var filesInfo in fileList2)
                {
                    fl.Add(new FileKeys(filesInfo, filesInfo.Length));
                }

               
                List<FileKeys> flsorted = DeleteUniqueItems(fl);

              
                while (flsorted.Count > 0)
                {
                    int i = 0;
                    int jj = 0;
                    for (int j = 1; j < flsorted.Count; j++)
                    {
                        
                        if (flsorted[i].Length == flsorted[j].Length)
                        {
                            if (CompareTwoFiles(flsorted[i].FileObj, flsorted[j].FileObj))
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
                
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:{0}", e);
                return 1;
            }
            

            
        }
        
        static List<FileInfo> GetFiles(DirectoryInfo dir, ref List<FileInfo> FileList )

        {
            
            try
            {
                FileInfo[] FileListArray = dir.GetFiles();
                foreach (FileInfo filesInfo in FileListArray)
                {
                    try
                    {
                        
                        FileList.Add(filesInfo);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
            }

            try
            {
                foreach (var subdir in dir.GetDirectories())
                {
                    GetFiles(subdir, ref FileList);
                }
            }
            catch
            {

            }


            return FileList;  
        }
    }
}
