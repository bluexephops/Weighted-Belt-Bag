using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace YourThunderstoreTeam.patch;

[HarmonyPatch(typeof(BeltBagItem))]

public class BeltBagItemPatch
    {
    [HarmonyPatch(nameof(BeltBagItem.PutObjectInBagLocalClient))]
    [HarmonyPrefix]
    private static bool OnAddToBag(ref BeltBagItem __instance,object[] __args)
    {
        
        GrabbableObject grabbableObject = (GrabbableObject)__args[0];
        HUDManager.Instance.AddTextToChatOnServer("Adding to bag Weight:" + grabbableObject.itemProperties.weight);
        __instance.itemProperties.weight += grabbableObject.itemProperties.weight - 1;
        
        return true;
    }
}

