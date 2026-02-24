using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MineshaftRescaler.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        [HarmonyPatch("SpawnScrapInLevel")]
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> ChangeMinshaftLootBonus(IEnumerable<CodeInstruction> instructions)
        {
            int index = -1;
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 10; i < codes.Count; i++)
            {
                if (codes[i - 2].opcode == OpCodes.Ldloc_1 && codes[i - 1].opcode == OpCodes.Ldc_I4_6 && codes[i].opcode == OpCodes.Add)
                {
                    index = i - 1;
                    break;
                }
            }
            if (index > -1)
            {
                codes[index].opcode = OpCodes.Ldc_I4_S;
                codes[index].operand = (sbyte)MineshaftRescaler.lootAmountBonus;
                MineshaftRescaler.Logger.LogDebug("Successfully injected new loot bonus count.");
            }
            else
            {
                MineshaftRescaler.Logger.LogError("Failed to inject new mineshaft loot bonus amount. ldcloc.1 -> ldc.i4.6 -> add not found in RoundManager.SpawnScrapInLevel.");
            }
            return codes;
        }
    }
}
