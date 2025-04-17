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
        //Add object weight to the belt bag's weight, subtracting 1 due to weight on objects being stored with with 1+object weight
        __instance.itemProperties.weight += grabbableObject.itemProperties.weight - 1;
        //Reset carry weight to 0 and readd the weights of the items due to weight not updating dynamically
        __instance.playerHeldBy.carryWeight = 0;
        for(int i = 0; i < 4; ++i)
        {
            if(__instance.playerHeldBy.ItemSlots[i])
                __instance.playerHeldBy.carryWeight += __instance.playerHeldBy.ItemSlots[i].itemProperties.weight;
        }
        
        
        return true;
    }
}

