// UnwillingTo_Mutate.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 9:12 AM
// last updated 03/13/2022  9:12 AM

using System;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.PreceptComps
{
    /// <summary>
    ///     precept comp for determining if a pawn is unwilling to mutate another pawn
    /// </summary>
    /// <seealso cref="RimWorld.PreceptComp_UnwillingToDo" />
    public class UnwillingTo_Mutate : PreceptComp_UnwillingToDo
    {
        /// <summary>
        ///     if this action is allowed to be done to guilty parties or not
        /// </summary>
        public bool allowGuilty;

        public bool checkWilling = true;

        public override bool MemberWillingToDo(HistoryEvent ev)
        {
            try
            {
                if (ev.def != PMHistoryEventDefOf.ApplyMutagenicsOn) return true;

                Pawn doer = ev.GetDoer();
                var victim = ev.GetArg<Pawn>(HistoryEventArgsNames.Victim);


                if (allowGuilty && victim?.guilt?.IsGuilty == true) return true;
                return checkWilling && victim.IsWillingToMutate();
            }
            catch (ArgumentException aEv)
            {
                Log.Error($"caught exception in {nameof(UnwillingTo_Guilty)}\n{aEv}");
                return false;
            }
        }
    }
}