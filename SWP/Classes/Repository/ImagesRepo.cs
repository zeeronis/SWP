using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;

namespace SWP.Classes.Repository
{
    public static class ImagesRepo
    {
        private static BitmapImage defaultSlot = new BitmapImage(new Uri(@"pack://application:,,,/Resources/sw_slot1.png"));
        private static Dictionary<int, BitmapImage> imagesDictionary = new Dictionary<int, BitmapImage>();


        public static BitmapImage GetImage(int unitID)
        {
            if(unitID == -1)
            {
                return defaultSlot;
            }

            if (!imagesDictionary.ContainsKey(unitID))
            {
                var image = new BitmapImage(new Uri(
                    Directory.GetCurrentDirectory()
                    + string.Format(@"\Images\{0}.png", unitID)));

                if (image == null)
                {
                    return defaultSlot;
                }

                imagesDictionary.Add(unitID, image);
            }

            return imagesDictionary[unitID];
        }
    }
}
