using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Classes.Repository
{
    public static class FilesRepo
    {
        public const string teamsFileName = "Teams.json";
        public const string unitsDataFileName = "UnitsNames.json";
        public readonly static string dataFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Data\";
        public readonly static string saveFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\Data\";

        private readonly static JsonSerializer serializer = new JsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore
        };


        private static void CheckDirectory(string path)
        {
            if (!Directory.Exists(Directory.GetParent(path).FullName))
            {
                Directory.CreateDirectory(Directory.GetDirectoryRoot(path));
            }
        }

        public static void SaveObjToJson<T>(string fullPath, T data)
        {
            CheckDirectory(fullPath);

            using (StreamWriter sw = new StreamWriter(fullPath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, data);
                }
            }
        }

        public static T LoadJsonToObj<T>(string fullPath, Func<T> defaultObj)
        {
            try
            {
                CheckDirectory(fullPath);

                string json = File.ReadAllText(fullPath);
                if (json != string.Empty)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
                else
                {
                    return defaultObj();
                }
            }
            catch (Exception)
            {
                return defaultObj();
            }
        }
    }
}
