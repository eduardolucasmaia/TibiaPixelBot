namespace TibiaPixelBot
{
    public class FunctionRulePorcent
    {

        public int Id;

        public bool Active;

        public int PorcentMinHealth;

        public int PorcentMaxHealth;

        public int PorcentMinMana;

        public int PorcentMaxMana;

        public bool Hotkey;

        public string Spell;

        public string Hotkey01;

        public string Hotkey02;

        public bool AusentHealth;

        public bool AusentMana;

        public int TimerInterval;

        public int Prioridade;

        public bool EnabledHealth;

        public bool EnabledMana;

        public FunctionRulePorcent(int pId, bool pActive, int pPorcentMinHealth, int pPorcentMaxHealth, int pPorcentMinMana, int pPorcentMaxMana, bool pHotkey, string pSpell, string pHotkey01, string pHotkey02, bool pAusentHealth, bool pAusentMana, int pTimerInterval, int pPrioridade, bool pEnabledHealth, bool pEnabledMana)
        {
            this.Id = pId;
            this.Active = pActive;
            this.PorcentMinHealth = pPorcentMinHealth;
            this.PorcentMaxHealth = pPorcentMaxHealth;
            this.PorcentMinMana = pPorcentMinMana;
            this.PorcentMaxMana = pPorcentMaxMana;
            this.Hotkey = pHotkey;
            this.Spell = pSpell;
            this.Hotkey01 = pHotkey01;
            this.Hotkey02 = pHotkey02;
            this.AusentHealth = pAusentHealth;
            this.AusentMana = pAusentMana;
            this.TimerInterval = pTimerInterval;
            this.Prioridade = pPrioridade;
            this.EnabledHealth = pEnabledHealth;
            this.EnabledMana = pEnabledMana;
        }
    }
}

