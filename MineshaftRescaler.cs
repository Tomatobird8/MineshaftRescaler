using BepInEx;
using BepInEx.Logging;
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
        public static int minLength;
        public static int maxLength;
        public static float mainLength;
        public static float cavesLength;
        public static float fireLength;

        public static bool changeMineshaftLootBonus;
        public static int lootAmountBonus;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            applyMineshaftSizeChanges = Config.Bind<bool>("General", "ApplyMineshaftSizeChanges", true, "Applies the mineshaft size changes listed below.").Value;
            minLength = Config.Bind<int>("General", "MinLength", 14, "Minimum generation length").Value;
            maxLength = Config.Bind<int>("General", "MaxLength", 18, "Maximum generation length").Value;
            mainLength = Config.Bind<float>("General", "MainLength", 0.25f, "Relative length of the Main side section.").Value;
            cavesLength = Config.Bind<float>("General", "CavesLength", 0.6f, "Relative length of the caves section.").Value;
            fireLength = Config.Bind<float>("General", "FireLength", 0.15f, "Relative length of the Fire side section.").Value;

            changeMineshaftLootBonus = Config.Bind<bool>("General", "ChangeMineshaftLootBonus", false, "Change the bonus amount of loot to spawn in the mineshaft interior.").Value;
            lootAmountBonus = Config.Bind<int>("General", "LootAmountBonus", 6, "Amount of extra loot to spawn in mineshaft.").Value;


            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            if (applyMineshaftSizeChanges)
            {
                Logger.LogDebug($"Patching... {nameof(PreGenerateDungeonPatch)}");
                Harmony.PatchAll(typeof(PreGenerateDungeonPatch));
            }
            if (changeMineshaftLootBonus)
            {
                Logger.LogDebug($"Patching... {nameof(RoundManager)}");
                Harmony.PatchAll(typeof(RoundManagerPatch));
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
