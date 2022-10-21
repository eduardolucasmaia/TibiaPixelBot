using System.Collections.Generic;

namespace TibiaPixelBot
{
    public class ObjectSerialize
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public List<AutoAction> AutoActionList { get; set; }
        public List<HealthManaFunctionPoint> HealthManaFunctionPointList { get; set; }
        public List<HealthManaFunctionPorcent> HealthManaFunctionPorcentList { get; set; }

        public bool ActiveAutoScan { get; set; }

        public int FunctionPorcentPosXMinHealth { get; set; }
        public int FunctionPorcentPosYMinHealth { get; set; }

        public int FunctionPorcentPosXMaxHealth { get; set; }
        public int FunctionPorcentPosYMaxHealth { get; set; }

        public int FunctionPorcentPosXMinMana { get; set; }
        public int FunctionPorcentPosYMinMana { get; set; }

        public int FunctionPorcentPosXMaxMana { get; set; }
        public int FunctionPorcentPosYMaxMana { get; set; }

        public int AutoActionPosX { get; set; }
        public int AutoActionPosY { get; set; }

        public bool ActiveAutoLoot { get; set; }
        public int AutoClickPlayerPosX { get; set; }
        public int AutoClickPlayerPosY { get; set; }
        public int AutoClickLootPosX { get; set; }
        public int AutoClickLootPosY { get; set; }
        public decimal AutoLootIntervalo { get; set; }

        //public KeyboardHook.VKeys VKeysLoot01 { get; set; }
        //public KeyboardHook.VKeys VKeysLoot02 { get; set; }
        public int VKeysLoot01 { get; set; }
        public int VKeysLoot02 { get; set; }

    }
}
