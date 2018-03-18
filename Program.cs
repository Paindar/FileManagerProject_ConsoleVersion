using System;
using System.Collections.Generic;
using System.IO;

namespace FileManagerProject_ConsoleVersion
{
    class Program
    {
        const String rootPath = "E:\\GDriver";
        static int directoryDepth = 0;
        static int fileCount = 0;
        static DirectoryManager dirManager = new DirectoryManager(rootPath);

        static void ReadFile(DirectoryInfo path)
        {
            int curDir = dirManager.GetDirId(path.FullName);
            foreach (DirectoryInfo NextFolder in path.GetDirectories())
            {
                directoryDepth++;
                dirManager.AddDirectory(path.FullName, NextFolder.Name);
                Console.WriteLine($"{ directoryDepth}.{NextFolder.Name}");
                ReadFile(NextFolder);
            }
            List<int> files = new List<int>();
            foreach (FileInfo NextFile in path.GetFiles())
            {
                String fileAbsolutePath = NextFile.FullName;
                if (dirManager.FileMgr.IsFileChanged(NextFile))
                {
                    //Console.WriteLine($"Duplicate at {NextFile.FullName}, id is {manager.GetFileId(fileAbsolutePath)}, time is {NextFile.LastWriteTime}");
                    int fileId = dirManager.FileMgr.AddFileIndex(NextFile, false);
                }
                files.Add(dirManager.FileMgr.GetFileId(fileAbsolutePath));
                fileCount++;
            }
            dirManager.AddFiles(path.FullName, files.ToArray());
            directoryDepth--;
        }

        static void Main(string[] args)
        {
            DateTime beforDT = System.DateTime.Now;
            Console.WriteLine("Hello world!");
            dirManager.FileMgr.ReadFrom("file.dat");
            DirectoryInfo TheFolder = new DirectoryInfo(rootPath);
            ReadFile(TheFolder);
            Console.WriteLine($"File count = {fileCount}");

            DateTime afterDT = System.DateTime.Now;
            TimeSpan ts = afterDT.Subtract(beforDT);
            dirManager.FileMgr.WriteTo("file.dat");
            Console.WriteLine("DateTime总共花费{0}ms.", ts.TotalMilliseconds);
            string s;
            while((s=Console.ReadLine())!=null)
            {
                string[] subArgs = s.Split(' ');
                //Console.WriteLine(Utils.LCS(subArgs[0], subArgs[1]));
                if(subArgs[0]=="find")
                {
                    if(subArgs[1]=="name")
                    {
                        int maxResult = (subArgs.Length == 5 ? Convert.ToInt32(subArgs[4]) : 10);
                        foreach(int i in dirManager.SearchFilesByName(subArgs[2], subArgs[3]))
                        {
                            if (maxResult < 0)
                                break;
                            Console.WriteLine("\t"+dirManager.FileMgr.GetFile(i).FullName);
                            maxResult--;
                        }
                    }
                }
            }

        }
        
    }
}
