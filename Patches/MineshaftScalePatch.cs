using DunGen;
using DunGen.Graph;
using HarmonyLib;

namespace MineshaftRescaler.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class MineshaftScalePatch
    {
        [HarmonyPatch(nameof(RoundManager.Start))]
        [HarmonyPostfix]
        private static void Start_Postfix(RoundManager __instance)
        {
            for (int i = 0;i < __instance.dungeonFlowTypes.Length; i++)
            {
                if (__instance.dungeonFlowTypes[i].dungeonFlow.name == "Level3Flow")
                {
                    MineshaftRescaler.Logger.LogDebug("Level3Flow found! Applying dungeon generation changes.");
                    DungeonFlow mineshaftFlow = __instance.dungeonFlowTypes[i].dungeonFlow;
                    mineshaftFlow.Length.Min = MineshaftRescaler.minLength.Value;
                    mineshaftFlow.Length.Max = MineshaftRescaler.maxLength.Value;
                    mineshaftFlow.BranchCount.Min = MineshaftRescaler.minBranches.Value;
                    mineshaftFlow.BranchCount.Max = MineshaftRescaler.maxBranches.Value;
                    float totalLength = MineshaftRescaler.mainLength.Value + MineshaftRescaler.cavesLength.Value + MineshaftRescaler.fireLength.Value;
                    float relativeMainLength = MineshaftRescaler.mainLength.Value / totalLength;
                    float relativeCavesLength = MineshaftRescaler.cavesLength.Value / totalLength;
                    float relativeFireLength = MineshaftRescaler.fireLength.Value / totalLength;
                    mineshaftFlow.Lines[0].Length = relativeMainLength;
                    mineshaftFlow.Lines[1].Position = relativeMainLength;
                    mineshaftFlow.Lines[1].Length = relativeCavesLength;
                    mineshaftFlow.Lines[2].Position = relativeMainLength + relativeCavesLength;
                    mineshaftFlow.Lines[2].Length = relativeFireLength;
                    break;
                }
            }
        }
    }
}
