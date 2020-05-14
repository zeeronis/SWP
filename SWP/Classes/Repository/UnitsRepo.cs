using Newtonsoft.Json;
using SWP.Classes.Game;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SWP.Classes.Repository
{
    public static class UnitsRepo
    {
        private static BitmapImage defaultSlot;

        private static Unit[] units;
        private static BitmapImage[] unitImages;


        public static void LoadImagesData()
        {
            try
            {
                defaultSlot = new BitmapImage(new Uri(@"pack://application:,,,/Resources/sw_slot1.png"));
            }
            catch (Exception)
            {
                defaultSlot = new BitmapImage();
            }


            var namesList = FilesRepo.LoadJsonToObj(
                FilesRepo.dataFolder + FilesRepo.unitsDataFileName,
                () => { return new List<string>(); });

            units = new Unit[namesList.Count];
            unitImages = new BitmapImage[namesList.Count];
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

        public static BitmapImage GetUnitImage(int unitID)
        {
            if (unitID == -1)
            {
                return defaultSlot;
            }

            if (unitImages[unitID] == null)
            {
                var image = new BitmapImage(new Uri(
                    Directory.GetCurrentDirectory()
                    + string.Format(@"\Images\{0}.png", unitID)));

                if (image == null)
                {
                    return defaultSlot;
                }

                unitImages[unitID] = image;
            }

            return unitImages[unitID];
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
