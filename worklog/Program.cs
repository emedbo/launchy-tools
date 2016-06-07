using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace worklog
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = @"c:\worklog\jobblogg";
            var today = DateTime.Today;

            if (args.Any())
            {
                int numDays = 0;
                if (int.TryParse(args[0], out numDays))
                {
                    today = today.AddDays(numDays);
                }

                DateTime parsed;
                if (DateTime.TryParseExact(args[0], new[] { "ddMMyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsed))
                {
                    today = parsed;
                }
            }

            var thisMonth = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames[today.Month - 1];
            var filenameToday = string.Format("{0:yy-MM-dd}.log", today);

            var dirPath = Path.Combine(directory, thisMonth);
            var fullPath = Path.Combine(directory, thisMonth, filenameToday);
            var lineNums = 0;
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
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
