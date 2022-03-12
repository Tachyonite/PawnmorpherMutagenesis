// Worker_HasMutagenicWeapon.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/11/2022 10:53 AM
// last updated 03/11/2022  10:53 AM

using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Thoughts.Precept
{
    /// <summary>
    ///     thought worker for a precept related thought when a pawn has a mutagenic weapon equipped
    /// </summary>
    /// <seealso cref="RimWorld.ThoughtWorker_Precept" />
    public class Worker_HasMutagenicWeapon : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            ThingWithComps equipment = p?.equipment?.Primary;
            return equipment?.IsMutagenicWeapon() == true;
        }
    }
}