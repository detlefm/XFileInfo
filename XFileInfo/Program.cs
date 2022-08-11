using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFileInfo {
    internal class Program {


        static Dictionary<int, string> mediaInfo =
                    new Dictionary<int, string>() {
                        { 314,"FrameHeight"}, { 315,"FrameRate"}, 
                        { 316,"FrameWidth"},  { 27,"VideoLength"}
                    };


        public static Dictionary<string, string> GetMediaDetails(string path) {
            Dictionary<string, string> resultDict = new Dictionary<string, string>();
            try {
                var folder = System.IO.Path.GetDirectoryName(path);
                var fname = System.IO.Path.GetFileName(path);
                Shell32.Shell shell = new Shell32.Shell();

                Shell32.Folder shellFolder = shell.NameSpace(folder);
                Shell32.FolderItem folderitem = shellFolder.ParseName(fname);
                foreach (var kvp in mediaInfo) {
                    var erg = shellFolder.GetDetailsOf(folderitem, kvp.Key);
                    resultDict.Add(kvp.Value, erg.ToString());
                }
            } catch (Exception ex) {
                resultDict.Add("Error", ex.Message);
            }
            return resultDict;
        }

        [STAThread]
        static void Main(string[] args) {
            if (args.Length == 0) {
                Console.WriteLine("usage: XFileInfo.exe FILENAME");
                return;
            }
            var pathname = args[0];
            var dict = GetMediaDetails(pathname);
            dict.Add("Filename", pathname);
            Console.WriteLine("{");
            foreach (var kvp in dict) {
                Console.WriteLine("    \"{0}\" : \"{1}\"", kvp.Key, kvp.Value.Trim('\u200e'));

            }
            Console.WriteLine("}");
        }
    }
}
