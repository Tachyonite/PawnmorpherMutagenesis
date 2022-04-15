using HarmonyLib;
using JetBrains.Annotations;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using PawnmorpherMutagenesis.DefExtensions;


namespace PawnmorpherMutagenesis.HPatches
{
    [HarmonyPatch(typeof(Bill)), HarmonyPatch(nameof(Bill.PawnAllowedToStartAnew))]
    static class RoleLockedRecipePatches
    {

        [HarmonyPostfix]
        static void CheckRoleRestrictionAbility(ref bool __result, Bill __instance, Pawn p)
        {
            if (__result == false)
            {  // nothing to do here
                return;
            }
            RoleRestrictions extension = __instance.recipe.GetModExtension<RoleRestrictions>();
            if (extension == null)
            {
                extension = __instance.recipe.ProducedThingDef?.GetModExtension<RoleRestrictions>();
                if (extension == null)
                {
                    return;
                }
            }
            Precept_Role role = p.Ideo.GetRole(p);
            if (role == null)
            {
                JobFailReason.Is($"Missing required role: {extension.role.label.CapitalizeFirst()}",__instance.Label);
                __result = false;
                return;
            }
            bool roleAllowed = role.def == extension.role;  // I'd make the extension have a list of roles that are allowed
            __result = roleAllowed;
        }
    }
}
