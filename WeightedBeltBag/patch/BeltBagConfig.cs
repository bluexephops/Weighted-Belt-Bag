using BepInEx.Configuration;
using CSync.Extensions;
using CSync.Lib;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace bluexephops.patch
{
    internal class BeltBagConfig : SyncedConfig2<BeltBagConfig>
    {
        [SyncedEntryField] public SyncedEntry<float> configPercent;

        public BeltBagConfig(ConfigFile config) : base("bluexephops.Plugin.Guid")
        {

            config.SaveOnConfigSet = false;
            configPercent = config.BindSyncedEntry(
                new ConfigDefinition("General", "Percentage of weight"),
            1.0f,
             new ConfigDescription("Percentage of the item's weight to use for the belt bag (1 is 100%).")
            );
            ClearOrphanedEntries(config);
            config.Save();
            ConfigManager.Register(this);
        }

        static void ClearOrphanedEntries(ConfigFile config)
        {
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(config);
            orphanedEntries.Clear();
        }
    }
}
