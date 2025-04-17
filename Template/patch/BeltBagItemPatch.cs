using HarmonyLib;

namespace YourThunderstoreTeam.patch;

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
            //Add object weight to the player's weight, subtracting 1 due to weight on objects being stored with with 1+object weight
            __instance.playerHeldBy.carryWeight += grabbableObject.itemProperties.weight - 1;
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
            __instance.playerHeldBy.carryWeight -= grabbableObject.itemProperties.weight - 1;
        }
        return true;
    }

}

