// UnwillingToMutate.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 8:29 AM
// last updated 03/13/2022  8:29 AM

using System;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.PreceptComps
{
    /// <summary>
    ///     precept comp to restrict events on guilty pawns
    /// </summary>
    /// <seealso cref="RimWorld.PreceptComp_UnwillingToDo" />
    public class UnwillingTo_Guilty : PreceptComp_UnwillingToDo
    {
        /// <summary>
        ///     if this action is allowed to be done to guilty parties or not
        /// </summary>
        public bool allowGuilty;

        public override bool MemberWillingToDo(HistoryEvent ev)
        {
            try
            {
                if (ev.def != eventDef) return true;
                Pawn doer = ev.GetDoer();
                var victim = ev.GetArg<Pawn>(HistoryEventArgsNames.Victim);


                if (allowGuilty && victim?.guilt?.IsGuilty == true) return true;
                return base.MemberWillingToDo(ev);
            }
            catch (ArgumentException aEv)
            {
                Log.Error($"caught exception in {nameof(UnwillingTo_Guilty)}\n{aEv}");
                return false;
            }
        }
    }
}