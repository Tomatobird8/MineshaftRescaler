using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using MineshaftRescaler.Patches;

namespace MineshaftRescaler
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class MineshaftRescaler : BaseUnityPlugin
    {
        public static MineshaftRescaler Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static bool applyMineshaftSizeChanges;
        public static ConfigEntry<int> minLength = null!;
        public static ConfigEntry<int> maxLength = null!;
        public static ConfigEntry<int> minBranches = null!;
        public static ConfigEntry<int> maxBranches = null!;
        public static ConfigEntry<float> mainLength = null!;
        public static ConfigEntry<float> cavesLength = null!;
        public static ConfigEntry<float> fireLength = null!;

        public static bool changeMineshaftLootBonus;
        public static ConfigEntry<int> lootAmountBonus = null!;

        internal static bool isDawnLibLoaded;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            applyMineshaftSizeChanges = Config.Bind<bool>("General", "ApplyMineshaftSizeChanges", true, "Applies the mineshaft size changes listed below.").Value;
            minLength = Config.Bind<int>("General", "MinLength", 14, "Minimum generation length");
            maxLength = Config.Bind<int>("General", "MaxLength", 18, "Maximum generation length");
            minBranches = Config.Bind<int>("General", "MinBranches", 14, "Minimum amount of branches to generate");
            maxBranches = Config.Bind<int>("General", "MaxBranches", 18, "Maximum amount of branches to generate");
            mainLength = Config.Bind<float>("General", "MainLength", 0.25f, "Relative length of the Main side section.");
            cavesLength = Config.Bind<float>("General", "CavesLength", 0.6f, "Relative length of the caves section.");
            fireLength = Config.Bind<float>("General", "FireLength", 0.15f, "Relative length of the Fire side section.");

            changeMineshaftLootBonus = Config.Bind<bool>("General", "ChangeMineshaftLootBonus", false, "Change the bonus amount of loot to spawn in the mineshaft interior.").Value;
            lootAmountBonus = Config.Bind<int>("General", "LootAmountBonus", 6, "Amount of extra loot to spawn in mineshaft.");

            isDawnLibLoaded = Chainloader.PluginInfos.ContainsKey("com.github.teamxiaolan.dawnlib");
            Logger.LogInfo($"Dawnlib installed: {isDawnLibLoaded}");

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            if (applyMineshaftSizeChanges)
            {
                Logger.LogDebug($"Patching... {nameof(MineshaftScalePatch)}");
                Harmony.PatchAll(typeof(MineshaftScalePatch));
            }
            if (changeMineshaftLootBonus)
            {
                if (isDawnLibLoaded)
                {
                    Logger.LogDebug($"Patching... {nameof(RoundManagerDawnLibSupportPatch)}");
                    Harmony.PatchAll(typeof(RoundManagerDawnLibSupportPatch));
                }
                else
                {
                    Logger.LogDebug($"Patching... {nameof(RoundManager)}");
                    Harmony.PatchAll(typeof(RoundManagerPatch));
                }
            }

            Logger.LogDebug("Finished patching!");
        }

        internal static void Unpatch()
        {
            Logger.LogDebug("Unpatching...");

            Harmony?.UnpatchSelf();

            Logger.LogDebug("Finished unpatching!");
        }
    }
}
