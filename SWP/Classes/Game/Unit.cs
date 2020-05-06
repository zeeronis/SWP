namespace SWP.Classes.Game
{
    public class Unit
    {
        public Unit()
        {
        }

        public Unit(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        public int ID { get; private set; }
        public string Name { get; private set; }


        public void SetInfo(Unit unit)
        {
            ID = unit.ID;
            Name = unit.Name;
        }
    }
}
