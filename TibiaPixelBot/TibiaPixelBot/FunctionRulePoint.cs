namespace TibiaPixelBot
{
    public class FunctionRulePoint
    {
        public int Id;

        public bool Active;

        public int PosX;

        public int PoxY;

        public bool Hotkey;

        public string Spell;

        public string Hotkey01;

        public string Hotkey02;

        public bool Ausent;

        public bool Health;

        public int TimerInterval;

        public int Prioridade;

        public FunctionRulePoint(int pId, bool pActive, int pPosX, int pPoxY, bool pHotkey, string pSpell, string pHotkey01, string pHotkey02, bool pAusent, bool pHealth, int pTimerInterval, int pPrioridade)
        {
            this.Active = pActive;
            this.PosX = pPosX;
            this.PoxY = pPoxY;
            this.Hotkey = pHotkey;
            this.Spell = pSpell;
            this.Hotkey01 = pHotkey01;
            this.Hotkey02 = pHotkey02;
            this.Ausent = pAusent;
            this.Health = pHealth;
            this.TimerInterval = pTimerInterval;
            this.Prioridade = pPrioridade;
            this.Id = pId;
        }
    }
}
