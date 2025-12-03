using DunGen;
using HarmonyLib;

namespace MineshaftRescaler.Patches
{
    [HarmonyPatch(typeof(Dungeon))]
    public class PreGenerateDungeonPatch
    {
        [HarmonyPatch("PreGenerateDungeon")]
        [HarmonyPostfix]
        private static void PreGenerateDungeonPostfix(Dungeon __instance)
        {
            if (__instance.DungeonFlow.name == "Level3Flow")
            {
                MineshaftRescaler.Logger.LogDebug("Level3Flow detected, applying dungeon generation changes.");
                __instance.DungeonFlow.Length.Min = MineshaftRescaler.minLength;
                __instance.DungeonFlow.Length.Max = MineshaftRescaler.maxLength;
                float totalLength = MineshaftRescaler.mainLength + MineshaftRescaler.cavesLength + MineshaftRescaler.fireLength;
                float relativeMainLength = MineshaftRescaler.mainLength / totalLength;
                float relativeCavesLength = MineshaftRescaler.cavesLength / totalLength;
                float relativeFireLength = MineshaftRescaler.fireLength / totalLength;
                __instance.DungeonFlow.Lines[0].Length = relativeMainLength;
                __instance.DungeonFlow.Lines[1].Position = relativeMainLength;
                __instance.DungeonFlow.Lines[1].Length = relativeCavesLength;
                __instance.DungeonFlow.Lines[2].Position = relativeMainLength + relativeCavesLength;
                __instance.DungeonFlow.Lines[2].Length = relativeFireLength;
            }
        }
    }
}
