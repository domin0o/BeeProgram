using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeProgram
{
    static internal class HoneyVault
    {
        public const float NECTAR_CONVERSION_RATIO = .19f;
        public const float LOW_LEVEL_WARNING = 10f;
        private static float honey = 25f;
        private static float nectar = 100f;

        public static void ConvertNectarToHoney(float amount)
        {
            float nectarToConvert = amount;

            if(nectarToConvert > nectar)
            {
                nectarToConvert = nectar;
            }
            nectar -= nectarToConvert;
            honey += nectarToConvert * NECTAR_CONVERSION_RATIO;
        }

        public static bool ConsumeHoney(float amount)
        {
            if(amount <= honey)
            {
                honey -=amount;
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CollectNectar(float amount)
        {
            if(amount > 0f)
            {
                nectar += amount;
            }
        }

        public static string StatusReport
        {
            get
            {
                string status = $"Jednostki miodu: {honey:0.0}\n" + $"Jednostki nektaru: {nectar:0.0}\n";
                string warnings = "";
                if (honey < LOW_LEVEL_WARNING)
                {
                    return warnings = "MAŁO MIODU - DODAJ PRODUCENTÓW MIODU";
                }
                if(nectar < LOW_LEVEL_WARNING)
                {
                    return warnings = "MAŁO NEKTARU - DODAJ PRODUCENTÓW NEKTARU";
                }
                return status + warnings;
            }
        }
    }
}
