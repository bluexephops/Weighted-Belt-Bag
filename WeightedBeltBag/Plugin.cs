using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using bluexephops.patch;

namespace bluexephops;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    public static Plugin Instance { get; set; }

    public static ManualLogSource Log => Instance.Logger;

    private readonly Harmony _harmony = new(PluginInfo.PLUGIN_GUID);


    public Plugin()
    {
        Instance = this;
    }
    internal static BeltBagConfig BoundConfig { get; private set; } = null!;
    private void Awake()
    {
        BoundConfig = new BeltBagConfig(base.Config);
        Log.LogInfo($"Applying patches...");
        ApplyPluginPatch();
        Log.LogInfo($"Patches applied");
    }

    /// <summary>
    /// Applies the patch to the game.
    /// </summary>
    private void ApplyPluginPatch()
    {
        _harmony.PatchAll(typeof(BeltBagItemPatch));
        _harmony.PatchAll(typeof(GrabbableObjectPatch));
    }
}
