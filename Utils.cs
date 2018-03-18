using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerProject_ConsoleVersion
{
    class Utils
    {

        public static string GetMD5(string s)
        {
            try
            {
                FileStream file = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retval = md5.ComputeHash(file);
                file.Close();

                StringBuilder sc = new StringBuilder();
                for (int i = 0; i < retval.Length; i++)
                {
                    sc.Append(retval[i].ToString("x2"));
                }
                return sc.ToString();
                //Console.WriteLine("文件MD5：{0}", sc);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return "";
        }
        public static string GetSHA512(string s)
        {
            try
            {
                FileStream file = new FileStream(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                SHA512 sha512 = new SHA512CryptoServiceProvider();
                byte[] crypto = sha512.ComputeHash(file);
                file.Close();
                return Convert.ToBase64String(crypto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "";
        }
        /// <summary>
        /// Get 2 strings longest common substring.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string LCS(string a, string b)
        {
            int[,] vs = new int[a.Length + 1, b.Length + 1];
            StringBuilder builder = new StringBuilder();
            int i = 0, j = 0;
            for ( i = 1; i <= a.Length; i++)
            {
                for ( j = 1; j <= b.Length; j++)
                {
                    if (a[i - 1] == b[j - 1])
                    {
                        vs[i, j] = vs[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        vs[i, j] = Math.Max(vs[i - 1, j], vs[i, j - 1]);
                    }
                }
            }
            i = a.Length;
            j = b.Length;
            while (i > 0 && j > 0)
            {
                if (vs[i, j - 1] == vs[i, j])
                {
                    j--;
                }
                
                else if (vs[i - 1, j] == vs[i, j])
                {
                    i--;
                }
                else if (vs[i - 1, j - 1] + 1 == vs[i, j])
                {
                    builder.Insert(0, a[i - 1]);
                    i--; j--;
                }
            }
            return builder.ToString();
        }
    }
}
