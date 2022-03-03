// Worker_IsMorph.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/03/2022 7:37 AM
// last updated 03/03/2022  7:37 AM

using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Thoughts.Precept
{

  
    /// <summary>
    /// thought worker for situational thoughts for humans, required as to still exclude former humans 
    /// </summary>
    /// <seealso cref="RimWorld.ThoughtWorker" />
    public class Worker_IsMorph : ThoughtWorker_Precept
    {
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            if (p?.IsFormerHuman() != false) return false;

            return p.IsMorph();
        }

        

    }
}