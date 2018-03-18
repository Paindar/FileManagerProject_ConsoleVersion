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
        Hashtable tagList = new Hashtable();

        /// <summary>
        /// Add a file info into manager. 
        /// If flag is true, it will be add/replace whatever its path is existed in table, otherwise a exception will be threw.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="flag"></param>
        /// <returns>the file's id</returns>
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
        public void AddTagToFile(int fileId, params string[] tags)
        {
            if(fileId>=fileInfos.Count)
            {
                throw new Exception("File doesn't exist.");
            }
            foreach (string tag in tags)
            {
                if (!tagList.ContainsKey(tag))
                {
                    tagList.Add(tag, new List<int>());
                }
                ((List<int>)(tagList[tag])).Add(fileId);
                fileInfos[fileId].AddTags(tag);
            }
            
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
        public List<int> GetFileByTag(string tag) => tagList.ContainsKey(tag) ? (List<int>)tagList[tag] : new List<int>();
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
                    foreach (string tag in index.Tags)
                    {
                        if (!tagList.ContainsKey(tag))
                        {
                            tagList.Add(tag, new List<int>());
                        }
                        ((List<int>)(tagList[tag])).Add(i);
                                fileInfos[i].AddTags(tag);
                    }
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
