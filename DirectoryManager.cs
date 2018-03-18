using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject_ConsoleVersion
{
    class Directory
    {
        int parent = -1;
        List<int> subDirectories = new List<int>();
        List<int> files = new List<int>();
        string name;
        public Directory(string name, int parent = -1)
        {
            this.name = name;
            this.parent = parent;
        }
        public void AddSubDirectory(params int[] i)
        {
            foreach(int dir in i)
                subDirectories.Add(dir);
        }
        public void AddFile(params int[] i)
        {
            foreach(int file in i)
            {
                files.Add(file);
            }
        }
        public List<int> Files
        {
            get { return files; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Parent
        {
            get { return parent; }
        }
        public List<int> SubDir
        {
            get { return subDirectories; }
        }

    }

    class DirectoryManager
    {
        private List<Directory> directories = new List<Directory>();
        private DataManager fileManager = new DataManager();
        public DataManager FileMgr
        {
            get { return fileManager; }
        }
        private string root;
        public DirectoryManager(string rootName)
        {
            root = rootName;
            directories.Add(new Directory(rootName));
        }
        public void AddDirectory(string path, string name)
        {
            int curDir = 0;
            bool isExisted = false;
            foreach (string dir in path.Substring(root.Length).Split(System.IO.Path.DirectorySeparatorChar))
            {
                if (dir == "" || dir == ".")
                    continue;
                if(dir =="..")
                {
                    curDir = directories[curDir].Parent;
                }
                isExisted = false;
                foreach (int subDir in directories[curDir].SubDir)
                {
                    if(directories[subDir].Name==dir)
                    {

                        curDir = subDir;
                        isExisted = true;
                        break;
                    }
                }
                if (!isExisted)
                {
                    directories[curDir].AddSubDirectory(directories.Count);
                    directories.Add(new Directory(dir, curDir));
                    curDir = directories.Count - 1;
                }
            }
            directories[curDir].AddSubDirectory(directories.Count);
            directories.Add(new Directory(name, curDir));
            curDir = directories.Count - 1;
        }
        public void AddFiles(string path, params int[] files)
        {
            int dir = GetDirId(path);
            directories[dir].AddFile(files);
        }
        /**
         * Get directory's id. It will not check if path is within manager's range.
         * Return the directory's id, if not existed, return -1;
         */
        public int GetDirId(string path, int rootId = 0)
        {
            if (rootId ==0 && path.Length>root.Length && path.Substring(0, root.Length) == root)
                path = path.Substring(root.Length);
            string[] dirPaths = path.Split(System.IO.Path.DirectorySeparatorChar);
            int curDir = rootId;
            bool isExisted = false;
            foreach (string dir in dirPaths)
            {
                isExisted = false;
                if (dir == ""|| dir ==".")
                    continue;
                else if (dir == "..")
                    curDir = directories[curDir].Parent;
                foreach (int subDir in directories[curDir].SubDir)
                {
                    if (directories[subDir].Name == dir)
                    {
                        curDir = subDir;
                        isExisted = true;
                        break;
                    }
                }
                if (!isExisted)
                {
                    return -1;
                }
            }
            return curDir;
        }
        public List<int> GetFiles(string path)
        {
            List<int> res = new List<int>();
            int id = GetDirId(path.Substring(root.Length));
            res.AddRange(directories[id].Files);
            return res;
        }
        public List<int> SearchFilesByName(string path, string name)
        {
            List<int> res = new List<int>();

            List<KeyValuePair<int, int>> tempRes = new List<KeyValuePair<int, int>>();
            int dirId = GetDirId(path);
            Stack<int> stack = new Stack<int>();
            stack.Push(dirId);
            while (stack.Count != 0)
            {
                int top = stack.Pop();
                foreach(int subDir in directories[top].SubDir)
                {
                    stack.Push(subDir);
                }
                foreach (int file in directories[top].Files)
                    tempRes.Add(new KeyValuePair<int, int>(file, Utils.LCS(fileManager.GetFile(file).Name, name).Length));
            }
            tempRes.Sort((emp1, emp2) => emp2.Value.CompareTo(emp1.Value));
            foreach(KeyValuePair<int, int> pair in tempRes)
            {
                res.Add(pair.Key);
            }
            return res;
        }

    }
}
