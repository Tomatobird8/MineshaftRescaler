using HarmonyLib;
using Dawn;

namespace MineshaftRescaler.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    public class RoundManagerDawnLibSupportPatch
    {
        [HarmonyPatch(nameof(RoundManager.Start))]
        [HarmonyPostfix]
        public static void Start_Postfix()
        {
            bool Found = false;
            for (int i = 0; i < RoundManager.Instance.dungeonFlowTypes.Length; i++)
            {
                if (RoundManager.Instance.dungeonFlowTypes[i].dungeonFlow.name == "Level3Flow")
                {
                    DawnDungeonInfo info = RoundManager.Instance.dungeonFlowTypes[i].dungeonFlow.GetDawnInfo();
                    info.ExtraScrapGeneration = MineshaftRescaler.lootAmountBonus.Value;
                    Found = true;
                    MineshaftRescaler.Logger.LogInfo($"Level3Flow found! Bonus of {MineshaftRescaler.lootAmountBonus.Value} has been applied.");
                    break;
                }
            };
            if (!Found) MineshaftRescaler.Logger.LogWarning("Level3Flow was not found! Scrap amount bonus won't be applied.");
        }
    }
}
