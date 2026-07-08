using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MineshaftRescaler.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    internal class RoundManagerPatch
    {
        [HarmonyPatch(nameof(RoundManager.SpawnScrapInLevel))]
        [HarmonyTranspiler]
        internal static IEnumerable<CodeInstruction> ChangeMinshaftLootBonus(IEnumerable<CodeInstruction> instructions)
        {
            CodeMatcher matcher = new(instructions);

            CodeMatch[] targetPattern = 
            {
                new CodeMatch(OpCodes.Ldloc_1),
                new CodeMatch(OpCodes.Ldc_I4_6),
                new CodeMatch(OpCodes.Add)
            };
            matcher.MatchForward(false, targetPattern)
                .ThrowIfNotMatch("Couldn't find targetPattern for patching mineshaft bonus!")
                .Advance(1)
                .SetAndAdvance(OpCodes.Ldc_I4_S, (sbyte)MineshaftRescaler.lootAmountBonus.Value);
            return matcher.InstructionEnumeration();
        }
    }
}
