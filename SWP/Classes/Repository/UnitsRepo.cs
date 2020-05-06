using Newtonsoft.Json;
using SWP.Classes.Game;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SWP.Classes.Repository
{
    public static class UnitsRepo
    {
        public const string teamsFileName = "Teams.json";
        public const string unitsDataFileName = "UnitsNames.json";
        public readonly static string dataFolder = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
            + @"\Data\";

        private static Unit[] units;
        public static bool NeedSave { get; private set; }


        public static void SetNeedSave(bool isNeedSave = true)
        {
            NeedSave = isNeedSave;
        }

        public static void LoadUnitsData()
        {
            string json = File.ReadAllText(dataFolder + unitsDataFileName);
            var namesList = JsonConvert.DeserializeObject<List<string>>(json);

            units = new Unit[namesList.Count];
            for (int i = 0; i < namesList.Count; i++)
            {
                units[i] = new Unit(i, namesList[i]);
            }
        }

        public static string GetUnitName(int id)
        {
            if (units != null && units.Length - 1 >= id)
            {
                return units[id].Name;
            }

            return string.Empty;
        }

        public static List<UnitItem> GetAllUnits()
        {
            var units = new List<UnitItem>();
            foreach (var item in UnitsRepo.units)
            {
                units.Add(new UnitItem(item.ID));
            }

            return units;
        }
    }
}
