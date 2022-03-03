// Worker_IsMorph.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/03/2022 7:37 AM
// last updated 03/03/2022  7:37 AM

using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Thoughts
{

    /// <summary>
    /// thought worker for situational thoughts for morphs
    /// </summary>
    /// <seealso cref="RimWorld.ThoughtWorker" />
    public class Worker_IsMorph : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p?.IsFormerHuman() != false) return false;

            return p.IsMorph(); 
        }
    }


    /// <summary>
    /// thought worker for situational thoughts for humans, required as to still exclude former humans 
    /// </summary>
    /// <seealso cref="RimWorld.ThoughtWorker" />
    public class Worker_IsHuman : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (p?.IsFormerHuman() != false) return false;

            return !p.IsMorph();
        }

    }
}