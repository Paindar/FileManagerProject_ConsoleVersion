using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace FileManagerProject_ConsoleVersion
{
    class DataManager
    {
        
        List<FileIndex> fileInfos = new List<FileIndex>();
        Hashtable table = new Hashtable();

        /**
         * Add a file info into manager. 
         * If flag is true, it will be add/replace whatever its path is existed in table, otherwise a exception will be threw.
         */
        public int AddFileIndex(FileInfo info, bool flag)
        {
            FileIndex index = new FileIndex(ref info);
            if(!table.ContainsKey(info.FullName))
            {
                int i = fileInfos.Count;
                fileInfos.Add(index);
                table.Add(info.FullName, i);
                return i;
            }
            else
            {
                if (flag)
                {
                    int i = (int)table[info.FullName];
                    fileInfos[i] = index;
                    return i;
                }
            }
            throw new Exception("Duplication file path!");
        }
        public FileInfo GetFile(int i)
        {
            return new FileInfo(fileInfos[i].GetFullPath());
        }
        public int GetFileId(string path)
        {
            if(table.ContainsKey(path))
                return (int)table[path];
            return -1;
        }
        public bool IsFileChanged(FileInfo info)
        {
            int id = GetFileId(info.FullName);
            return id == -1 || info.Length != fileInfos[id].GetSize() ||  fileInfos[id].GetTime().Equals(info.LastWriteTime)==false;
        }
        public bool IsPathExist(string path)
        {
            return table.ContainsKey(path);
        }

        public  void ReadFrom(string filename)
        {
            fileInfos.Clear();
            table.Clear();
            if (!File.Exists(filename))
                return;
            Console.WriteLine("Start reading.");
            using (System.IO.StreamReader file =
                new StreamReader(filename))
            {
                string s;
                while ((s = file.ReadLine()) != null)
                {
                    FileIndex index = new FileIndex(s);
                    int i = fileInfos.Count;
                    fileInfos.Add(index);
                    table.Add(index.GetFullPath(), i);
                }
                
            }
            Console.WriteLine($"Loading done , fileInfos={fileInfos.Count}");
        }

        public void WriteTo(string filename)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filename))
            {
                foreach(FileIndex index in fileInfos)
                {
                    file.WriteLine(index.ConvertToString());
                }
            }
        }

    }
}
