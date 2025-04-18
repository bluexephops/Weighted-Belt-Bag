﻿using HarmonyLib;
using UnityEngine;

namespace bluexephops.patch;

[HarmonyPatch(typeof(BeltBagItem))]

public class BeltBagItemPatch
    {
    [HarmonyPatch(nameof(BeltBagItem.PutObjectInBagLocalClient))]
    [HarmonyPrefix]
    private static bool OnAddToBag(ref BeltBagItem __instance,object[] __args)
    {
        
        GrabbableObject grabbableObject = (GrabbableObject)__args[0];
        if(grabbableObject)
        {
            //Add object weight to the player's weight, subtracting 1 due to weight on objects being stored with with 1+object weight, clamping to make sure it doesn't go over the max
            __instance.playerHeldBy.carryWeight = Mathf.Clamp(__instance.playerHeldBy.carryWeight + grabbableObject.itemProperties.weight - 1, 1f, 10f);
           
        }
        return true;
    }

    [HarmonyPatch(nameof(BeltBagItem.RemoveObjectFromBag))]
    [HarmonyPrefix] 
    private static bool OnRemoveFromBag(ref BeltBagItem __instance,object[] __args)
    {
        //get the object from the belt bag's list using the passed in item id
        int itemId = (int)__args[0];
        GrabbableObject grabbableObject = __instance.objectsInBag[itemId];
        if (grabbableObject)
        {
            //subtract the item's weight to the player's weight, subtracting 1 due to how it is stored
            if (__instance.playerHeldBy.carryWeight - grabbableObject.itemProperties.weight >= 0.0f)
                __instance.playerHeldBy.carryWeight -= grabbableObject.itemProperties.weight - 1.0f;
            //if you reached the clamp, trying to drop more weight than you have causes you to go negative. This prevents that
            else
                { 
                __instance.playerHeldBy.carryWeight = __instance.itemProperties.weight; 
            }
        }
        return true;
    }

}

