using Newtonsoft.Json;
using SWP.Classes.Repository;
using System;
using System.Windows.Media.Imaging;

namespace SWP.Classes.Game
{
    [Serializable]
    public class UnitItem
    {
        public UnitItem(int id = -1)
        {
            ID = id;
        }

        public int ID { get; private set; }
        [JsonIgnore] public string Name => UnitsRepo.GetUnitName(ID);
        [JsonIgnore] public BitmapImage Image => ImagesRepo.GetImage(ID);


        public void SetID(int unitID)
        {
            ID = unitID;
        }
    }
}
