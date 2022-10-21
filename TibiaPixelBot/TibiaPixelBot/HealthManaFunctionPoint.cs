namespace TibiaPixelBot
{
    public class HealthManaFunctionPoint
    {
        public bool Enabled { get; set; }

        public int PosX { get; set; }

        public int PosY { get; set; }

        public bool HotKeyOrSpell { get; set; }

        public string Spell { get; set; }

        public string HotKey01 { get; set; }

        public string HotKey02 { get; set; }

        public bool AbsentOrPresent { get; set; }

        public bool HealthOrMana { get; set; }

        public int Interval { get; set; }

        public int Priority { get; set; }
    }
}