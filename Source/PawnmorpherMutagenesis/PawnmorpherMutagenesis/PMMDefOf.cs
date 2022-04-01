// PMMDefOf.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 8:53 AM
// last updated 03/13/2022  8:53 AM

using JetBrains.Annotations;
using Pawnmorph;
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

        [DefOf]
        public static class AspectDefOf
        {
            static AspectDefOf()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(AspectDefOf));
            }

            public static AspectDef PMM_ConversionAspect;
        }

        [DefOf]
        public static class InternalDefOf
        {
            static InternalDefOf()
            {
                DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
            }
            public static PreceptDef PMM_IdeoRole_Mutachemist;
        }
    }

}