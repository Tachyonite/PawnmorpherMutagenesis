using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using RimWorld;
using Verse;
using PawnmorpherMutagenesis.DefExtensions;


namespace PawnmorpherMutagenesis.HPatches
{
    static class RoleLockedRecipePatches
    {
        [HarmonyPatch(typeof(Bill)), HarmonyPatch(nameof(Bill.PawnAllowedToStartAnew)), HarmonyPostfix]
        static bool CheckRoleRestrictionAbility(ref bool __result, Bill __instance, Pawn p)
        {
            if (__instance.recipe.HasModExtension<RoleRestrictions>())
            {
                if (p.Ideo.GetRole(p) == null || !p.Ideo.GetRole(p).def.defName.Equals(__instance.recipe.GetModExtension<RoleRestrictions>().role.defName))
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}
