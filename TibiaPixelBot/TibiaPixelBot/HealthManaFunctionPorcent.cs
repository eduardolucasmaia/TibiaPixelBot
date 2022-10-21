namespace TibiaPixelBot
{
    public class HealthManaFunctionPorcent
    {
        public bool Enabled { get; set; }

        public bool HotKeyOrSpell { get; set; }

        public string Spell { get; set; }

        public string HotKey01 { get; set; }

        public string HotKey02 { get; set; }

        public bool AbsentOrPresentHealth { get; set; }

        public bool AbsentOrPresentMana { get; set; }

        public int MinHealth { get; set; }

        public int MaxHealth { get; set; }

        public int MinMana { get; set; }

        public int MaxMana { get; set; }

        public int Interval { get; set; }

        public int Priority { get; set; }

        public bool EnabledHealth { get; set; }

        public bool EnabledMana { get; set; }
    }
}
