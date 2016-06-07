using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"c:\worklog\tasks";
            var taskNumber= 0;

            if (args.Any())
            {
                int parsed = 0;
                if (int.TryParse(args[0], out parsed))
                {
                    taskNumber = parsed;
                }
            }
            else
            {
                return;
            }
            if (taskNumber == 0)
            {
                return;
            }

            var filenameTask = string.Format("{0}.log", taskNumber);

            var fullPath = Path.Combine(directory, filenameTask);
            var lineNums = 0;
            if (!File.Exists(fullPath))
            {
                File.Create(fullPath);
            }
            else
            {
                var content = File.ReadLines(fullPath).ToList();
                if (!string.IsNullOrEmpty(content.LastOrDefault()))
                {
                    File.AppendAllLines(fullPath, new[] { string.Empty, string.Empty });
                }
                lineNums = content.Count() + 2;
            }

            Process.Start(@"C:\Program Files\Sublime Text 3\subl.exe", fullPath + ":" + lineNums + ":0");
        }
    }
}
