// AutoLocker.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/26/2021 6:01 AM
// last updated 07/26/2021  6:01 AM

using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.ThingComps
{
    public class AutoLocker : ThingComp
    {
        public override void Notify_Equipped(Pawn pawn)
        {
            base.Notify_Equipped(pawn);
            if (!(parent is Apparel apParent)) return; 
            var ap = pawn.apparel;
            ap?.Lock(apParent); 

        }
    }
}