using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace MineshaftRescaler
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class MineshaftRescaler : BaseUnityPlugin
    {
        public static MineshaftRescaler Instance { get; private set; } = null!;
        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        public static int minLength;
        public static int maxLength;
        public static float mainLength;
        public static float cavesLength;
        public static float fireLength;

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            minLength = Config.Bind<int>("General", "MinLength", 14, "Minimum generation length").Value;
            maxLength = Config.Bind<int>("General", "MaxLength", 18, "Maximum generation length").Value;
            mainLength = Config.Bind<float>("General", "MainLength", 0.25f, "Relative length of the Main side section.").Value;
            cavesLength = Config.Bind<float>("General", "CavesLength", 0.6f, "Relative length of the caves section.").Value;
            fireLength = Config.Bind<float>("General", "FireLength", 0.15f, "Relative length of the Fire side section.").Value;

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

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
