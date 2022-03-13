// SelfTookMemoryThought_IgnoreGuilty.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 8:45 AM
// last updated 03/13/2022  8:45 AM

using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.PreceptComps
{
    /// <summary>
    /// applies a self though on the giver if the victim pawn is not guilty 
    /// </summary>
    /// <seealso cref="RimWorld.PreceptComp_SelfTookMemoryThought" />
    public class SelfTookMemoryThought_IgnoreGuilty : PreceptComp_SelfTookMemoryThought
    {

        public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
        {
            var victim = ev.GetArg<Pawn>(HistoryEventArgsNames.Victim);
            if (victim == null)
            {
                Log.Error($"unable to find victim on history event {ev.def.defName}!");
                return; 
            }

            if(victim.guilt?.IsGuilty != true) base.Notify_MemberTookAction(ev, precept, canApplySelfTookThoughts);
        }
    }
}