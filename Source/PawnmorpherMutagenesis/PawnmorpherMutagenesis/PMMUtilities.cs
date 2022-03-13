// PMMUtilities.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/29/2021 3:05 PM
// last updated 07/29/2021  3:05 PM

using System;
using JetBrains.Annotations;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis
{
    /// <summary>
    ///     various utility functions
    /// </summary>
    public static class PMMUtilities
    {
        public static bool IsWillingToMutate([NotNull] this Pawn pawn)
        {
            if (pawn == null) throw new ArgumentNullException(nameof(pawn));

            if (pawn.story?.traits?.HasTrait(PMTraitDefOf.MutationAffinity) == true) return true;

            //now check ideo if applicable 
            Ideo ideo = pawn.ideo?.Ideo;
            if (ideo != null) return ideo.HasPrecept(PMMDefOf.PreceptDefOf.PM_MutationsLoved);

            return false;
        }
    }
}