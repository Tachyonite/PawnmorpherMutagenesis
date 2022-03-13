// PMMDefOf.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 8:53 AM
// last updated 03/13/2022  8:53 AM

using JetBrains.Annotations;
using RimWorld;

namespace PawnmorpherMutagenesis
{
    public static class PMMDefOf
    {
        [DefOf]
        public static class PreceptDefOf
        {
            static PreceptDefOf()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(PreceptDefOf));

                
            }

            public static PreceptDef PMM_MorphingLiked;
            public static PreceptDef PMM_MorphingLoved;
            public static PreceptDef PMM_MorphingRequired;
            public static PreceptDef PM_MutationsLoved; 
        }
    }
}