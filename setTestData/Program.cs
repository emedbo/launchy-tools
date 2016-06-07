using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace setTestData
{
    class Program
    {
        [DllImport("user32.dll")]
        internal static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        internal static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        internal static extern bool SetClipboardData(uint uFormat, IntPtr data);

        const string _current = "CURRENT";

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Feil argumenter");
                var count = 0;
                foreach (var s in args)
                {
                    Console.WriteLine(count + ": " + s);
                }
                return;
            }

            var directory = @"c:\worklog\misc";
            var filename = "testdata.txt";
            var fullpath = Path.Combine(directory, filename);
            var mode = args[0];
            if (mode == "set")
            {
                var key = args.Length == 3 ? args[1] : _current;
                var value = args.Length == 3 ? args[2] : args[1];
                SetTestData(key, value, fullpath);
            }
            else if (mode == "get")
            {
                var arg = args.Length == 2 ? args[1] : null;
                GetTestData(arg, fullpath);
            }
        }

        private static void SetTestData(string key, string value, string directory)
        {
            var content = File.ReadAllLines(directory);
            var dict = ToDictionary(content);
            
            dict[key] = value;
            dict[_current] = value;
            WriteDict(dict, directory);
            SetClipboardData(dict[key]);
        }

        private static void WriteDict(Dictionary<string, string> dict, string path)
        {
            var content = ToStringArray(dict);
            File.WriteAllLines(path, content);
        }

        private static string[] ToStringArray(Dictionary<string, string> dict)
        {
            return dict.Select(x => x.Key + "," + x.Value).ToArray();
        }

        private static Dictionary<string, string> ToDictionary(string[] content)
        {
            var dict = new Dictionary<string, string>();
            content.Select(x => x.Split(',')).ToList().ForEach(x => dict.Add(x[0], x[1]));
            return dict;
        }

        private static void GetTestData(string key, string directory)
        {
            var content = File.ReadAllLines(directory);
            var dict = ToDictionary(content);

            key = key ?? _current;

            if (dict.ContainsKey(key))
            {
                var data = dict[key];
                SetClipboardData(data);
                dict[_current] = data;
                WriteDict(dict, directory);
            }
        }

        private static void SetClipboardData(string content)
        {
            OpenClipboard(IntPtr.Zero);

            var ptr = Marshal.StringToHGlobalUni(content);
            SetClipboardData(13, ptr);
            CloseClipboard();
            Marshal.FreeHGlobal(ptr);
        }
    }
}
