using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VMFInstanceInserter
{
    public class VMMFile
    {

        public List<Dictionary<string, string>> vmfs;
        public string vmfdir { get; private set; }
        public VMMFile(String path, String rootDir = null)
        {
            vmfdir = path.Substring(0, path.Length - 4);
            vmfs = new List<Dictionary<string, string>>();

            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                StreamReader reader = new StreamReader(stream);
                String line;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine().Trim();
                    if (line == "VMF")
                    {
                        Dictionary<string, string> vmf = new Dictionary<string, string>();
                        while (!reader.EndOfStream && (line = reader.ReadLine().Trim()) != "}")
                        {
                            if (line == "{" || line.Length == 0)
                                continue;
                            line = line.Replace("\"", " ").Replace("\t", " ").Replace("\n", " ").Trim();
                            System.Diagnostics.Debug.WriteLine(line);
                            vmf.Add(line.Split(new char[] { ' ' }, 2)[0],
                                    line.Split(new char[] { ' ' }, 2)[1].TrimStart());
                        }
                        vmfs.Add(vmf);
                    }
                }
            }
        }
    }
}
