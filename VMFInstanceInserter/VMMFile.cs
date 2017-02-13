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
        public Dictionary<string, string> TopLevelVmf;
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
        public void VMMCollapse(string output)
        {
            Console.WriteLine("collapsing VMM");
            // find the topLevel vmf
            List<Dictionary<string, string>> topLevels = vmfs.FindAll(dic => dic.Keys.Contains("TopLevel"));
            if (topLevels.Count == 0){
                TopLevelVmf = vmfs.First();
            } else {
                TopLevelVmf = topLevels.Aggregate((curMin, x) => (curMin == null || int.Parse(x["TopLevel"]) < int.Parse(curMin["topLevel"]) ? x : curMin));
            }
            vmfs.Remove(TopLevelVmf);

            //start file
            VMFFile topmap = new VMFFile(vmfdir + "/" + TopLevelVmf["File"]);

            VMFVector3Value originVal = new VMFVector3Value { X = 0, Y = 0, Z = 0 };
            VMFVector3Value anglesVal = new VMFVector3Value { Pitch = 0, Roll = 0, Yaw = 0 };
            VMFNumberValue fixup_styleVal = new VMFNumberValue { Value = 0 };

            foreach (Dictionary<string, string> vmf in vmfs)
            {
                Console.WriteLine("Inserting submap of {0}", vmfdir + "/" + vmf["File"]);

                VMFFile submap = new VMFFile(vmfdir + "/" + vmf["File"]);
                
                foreach (VMFStructure worldStruct in submap.World)
                {
                    if (worldStruct.Type == VMFStructureType.Group || worldStruct.Type == VMFStructureType.Solid)
                    {
                        VMFStructure clone = worldStruct.Clone(topmap.LastID, topmap.LastNodeID);
                        clone.Transform(originVal, anglesVal);
                        topmap.World.Structures.Add(clone);
                    }
                }

                foreach (VMFStructure rootStruct in submap.Root)
                {
                    if (rootStruct.Type == VMFStructureType.Entity)
                    {
                        VMFStructure clone = rootStruct.Clone(topmap.LastID, topmap.LastNodeID);
                        clone.Transform(originVal, anglesVal);
                        topmap.Root.Structures.Add(clone);
                    }
                }
                topmap.updateIds();
            }
            topmap.ResolveInstances();
            topmap.Save(vmfdir + ".vmf");
        }
    }
}
