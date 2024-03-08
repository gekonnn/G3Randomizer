using G3Archive;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace G3Randomizer
{
    class Program
    {
        static DirectoryInfo AskDirectory()
        {
            string DataPath = "";
            
            while (!Directory.Exists(DataPath) && !Directory.Exists(Path.Combine(DataPath, "Data")))
            {
                Console.WriteLine("Please specify the Gothic 3 folder directory (eg. C:\\Program Files (x86)\\Steam\\steamapps\\common\\Gothic 3): ");
                DataPath = Console.ReadLine()!;
            }

            return new DirectoryInfo(DataPath);
        }
        static void Main(string[] args)
        {
            string regexPattern = "\\.p\\d\\d";

            DirectoryInfo DataPath = AskDirectory();
            FileInfo extractPath = new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Extracted"));
            FileInfo[] Files = DataPath.GetFiles();

            FileInfo IgnoredExtsFile = new FileInfo("ignored_extensions.txt");
            FileInfo TargetArchivesFile = new FileInfo("target_archives.txt");
            Console.WriteLine(IgnoredExtsFile.FullName);

            List<string> IgnoredExts = new List<string>(File.ReadAllLines(IgnoredExtsFile.FullName));
            List<string> TargetPaks = new List<string>(File.ReadAllLines(TargetArchivesFile.FullName));
            
            Random rnd = new Random();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            foreach (FileInfo file in Files)
            {
                if (!TargetPaks.Contains(Path.GetFileNameWithoutExtension(file.FullName))) 
                {
                    Console.WriteLine("Skipping " + file.Name);
                    continue; 
                }
                // Might cause a little trouble with .pXX files, as for each file with the same name the arrays will be scrambled once again
                if (Regex.Match(file.Name, regexPattern, RegexOptions.IgnoreCase).Success || file.Extension == ".pak")
                {
                    Console.WriteLine("Extracting " + file.Name);

                    FileInfo DestFolder = new FileInfo(Path.Combine(extractPath.FullName, Path.GetFileNameWithoutExtension(file.FullName)));
                    G3Pak_Archive Archive = new G3Pak_Archive();
                    Archive.ReadArchive(file);
                    Archive.Extract(DestFolder.FullName, IgnoredExts);

                    string[] entries = Directory.GetFiles(DestFolder.FullName, "*", SearchOption.AllDirectories);
                    Dictionary<string, List<FileInfo>> entryTable = new Dictionary<string, List<FileInfo>>();
                    foreach(string entry in entries)
                    {
                        FileInfo _File = new FileInfo(entry);
                        if (!entryTable.ContainsKey(_File.Extension))
                        {
                            entryTable.Add(_File.Extension, new List<FileInfo>());
                        }
                        entryTable[_File.Extension].Add(_File);
                    }

                    foreach (KeyValuePair<string, List<FileInfo>> pair in entryTable)
                    {
                        while(pair.Value.Count - 2 > 0)
                        {
                            FileInfo File1 = pair.Value[rnd.Next(pair.Value.Count)];
                            pair.Value.Remove(File1);
                            FileInfo File2 = pair.Value[rnd.Next(pair.Value.Count)];
                            pair.Value.Remove(File2);

                            Console.WriteLine(string.Format("Replacing {0} with {1}...", File1.Name, File2.Name));

                            File.Move(File1.FullName, File1.DirectoryName + "\\TEMP1");
                            File.Move(File2.FullName, File2.DirectoryName + "\\TEMP2");
                            File.Move(File1.DirectoryName + "\\TEMP1", File2.FullName);
                            File.Move(File2.DirectoryName + "\\TEMP2", File1.FullName);
                        }
                    }
                }
            }
            
            sw.Stop();
            Console.WriteLine(string.Format("Done! ({0}s)", sw.Elapsed));
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}