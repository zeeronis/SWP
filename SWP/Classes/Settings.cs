using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Classes
{
    [Serializable]
    public class Settings
    {
        public float TeamUnitSlotHeight { get; set; } = 50;
        public float TeamUnitSlotWidth { get; set; } = 50;

        public float FilterUnitSlotHeight { get; set; } = 50;
        public float FilterUnitSlotWidth { get; set; } = 50;
    }
}
