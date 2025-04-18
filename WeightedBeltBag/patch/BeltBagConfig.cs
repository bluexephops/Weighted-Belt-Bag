using BepInEx.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;

namespace bluexephops.patch
{
    internal class BeltBagConfig
    {
        public readonly ConfigEntry<float> configPercent;


        public BeltBagConfig(ConfigFile config)
        {
            config.SaveOnConfigSet = false;
           configPercent = config.Bind(
           "General",
           "Percentage of weight",
           1.0f,
           "Percentage of the item's weight to use for the belt bag"
           );
            ClearOrphanedEntries(config);
            config.Save();
        }

        static void ClearOrphanedEntries(ConfigFile config)
        {
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(config);
            orphanedEntries.Clear();
        }
    }
}
