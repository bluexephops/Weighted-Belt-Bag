using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

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
                    beltBag.playerHeldBy.carryWeight -= beltBag.objectsInBag[i].itemProperties.weight - 1;
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
                    //subtracting one from the weight of the items due to how they are stored
                    beltBag.playerHeldBy.carryWeight += beltBag.objectsInBag[i].itemProperties.weight - 1;
                }
            }

        }
        return true;
    }
}
