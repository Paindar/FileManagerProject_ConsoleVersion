using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject_ConsoleVersion
{
    class FileIndex
    {
        String path;
        String MD5;
        long size;
        DateTime time;
        HashSet<string> tags = new HashSet<string>();
        //TODO may need List<tag> to generate invert table
        public FileIndex(string s)
        {
            string[] args = s.Split('\b');
            if (args.Length < 4)
                throw new Exception("Incorrect parameter.");
            path = args[0];
            MD5 = args[1];
            size = Convert.ToInt64(args[2]);
            time = Convert.ToDateTime(args[3]);
            for (int i = 4; i < args.Length; i++)
            {
                if(args[i]!="")
                    tags.Add(args[i]);
            }
        }
        public FileIndex(ref System.IO.FileInfo info)
        {
            this.path = info.FullName;
            this.MD5 = Utils.GetMD5(path);
            this.size = info.Length;
            this.time = info.LastWriteTime;
        }
        public void AddTags(params string[] tags)
        {
            foreach (string tag in tags)
                this.tags.Add(tag);
        }
        public string ConvertToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(path);
            builder.Append('\b');
            builder.Append(MD5);
            builder.Append('\b');
            builder.Append(size);
            builder.Append('\b');
            builder.Append(time.ToString("o"));
            builder.Append('\b');
            foreach (string tag in tags)
            {
                builder.Append(tag);
                builder.Append('\b');
            }
            return builder.ToString();
        }
        public string GetFullPath() => path;
        public long GetSize() => size;
        public DateTime GetTime() => this.time;
        public HashSet<string> Tags { get { return tags; } }
    }
}
