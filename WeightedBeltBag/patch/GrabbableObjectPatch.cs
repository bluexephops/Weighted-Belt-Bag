using HarmonyLib;
using UnityEngine;

namespace bluexephops.patch;
[HarmonyPatch(typeof(GrabbableObject))]

public class GrabbableObjectPatch
{
    [HarmonyPatch(nameof(GrabbableObject.DiscardItem))]
    [HarmonyPrefix]
    private static bool onDisardItem(ref GrabbableObject __instance)
    {
        //check for picking up beltbag so the weight of the items in the bag can be removed upon drop
        BeltBagItem beltBag = __instance as BeltBagItem;
        if (beltBag != null)
        {
            if (beltBag.objectsInBag.Count > 0)
            {
                for (int i = 0; i < beltBag.objectsInBag.Count; i++)
                {
                    //subtracting one from the weight of the items due to how they are stored
                    if(beltBag.playerHeldBy.carryWeight - ((beltBag.objectsInBag[i].itemProperties.weight - 1) * Plugin.BoundConfig.configPercent.Value) >= 0)
                        beltBag.playerHeldBy.carryWeight -= ((beltBag.objectsInBag[i].itemProperties.weight - 1) * Plugin.BoundConfig.configPercent.Value);
                    //if you reached the clamp, trying to drop more weight than you have causes you to go negative. This prevents that
                    else
                    {
                        __instance.playerHeldBy.carryWeight = 1f;
                    }
                }
            }
            
        }
        return true;
    }

    [HarmonyPatch(nameof(GrabbableObject.GrabItemOnClient))]
    [HarmonyPrefix]
    private static bool onAddItem(ref GrabbableObject __instance)
    {
        //check for picking up beltbag so the weight of the items in the bag can be added upon pickup
        BeltBagItem beltBag = __instance as BeltBagItem;
        if (beltBag != null)
        {
            if (beltBag.objectsInBag.Count > 0)
            {
                for (int i = 0; i < beltBag.objectsInBag.Count; i++)
                {
                    //subtracting one from the weight of the items due to how they are stored, clamping to prevent weight going over the limit
                    beltBag.playerHeldBy.carryWeight = Mathf.Clamp(beltBag.playerHeldBy.carryWeight + ((beltBag.objectsInBag[i].itemProperties.weight - 1) * Plugin.BoundConfig.configPercent.Value), 1f, 10f);
                }
            }
        }
        return true;
    }
}
