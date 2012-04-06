

using System.IO;
using System.Net;

namespace MCForge.Utilities {

    //TODO: add xml comments

    public class FileUtils {

        public const string PropertiesPath = "properties/";
        public const string LevelsPath = "levels/";
        public const string DllsPath = "dlls/";
        public const string ExtrasPath = "extras/";
        public const string TextPath = "text/";


        /// <summary>
        /// Downloads  a file from the specifed website
        /// </summary>
        /// <param name="url">File address</param>
        /// <param name="saveLocation">Location to save the file</param>
        public static void CreateFileFromWeb(string url, string saveLocation) {
            using (var client = new WebClient())
                client.DownloadFile(url, saveLocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="saveLocation"></param>
        public static void CreateFileFromBytes(byte[] bytes, string saveLocation) {
            using (var stuff = File.Create(saveLocation))
                stuff.Write(bytes, 0, bytes.Length);
        }


        /// <summary>
        /// Creates all of the core directories and files
        /// </summary>
        public static void Init() {

        }
    }
}