// SelfTookMemoryThought_UnwillingToMutate.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/13/2022 9:34 AM
// last updated 03/13/2022  9:34 AM

using System;
using JetBrains.Annotations;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.PreceptComps
{
    public class SelfTookMemoryThought_AppliedMutagen : PreceptComp_SelfTookMemoryThought
    {
        public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
        {
            if (ev.def != eventDef || !canApplySelfTookThoughts) return;

            try
            {
                var arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
                var victim = ev.GetArg<Pawn>(HistoryEventArgsNames.Victim);


                if (arg.needs != null
                 && arg.needs.mood != null
                 && (!onlyForNonSlaves || !arg.IsSlave)
                 && (thought.minExpectation == null
                  || ExpectationsUtility.CurrentExpectationFor(arg).order >= thought.minExpectation.order))
                {
                    var sIdx = (int) GetVictimState(victim);
                    if (sIdx >= thought.stages.Count) return; //don't apply missing stages 
                    Thought_Memory thought_Memory = ThoughtMaker.MakeThought(thought, precept);

                    thought_Memory.SetForcedStage(sIdx);
                    arg.needs.mood.thoughts.memories.TryGainMemory(thought_Memory);
                }
            }
            catch (Exception e)
            {
                Log.Error($"caught exception while giving applied mutagen thought in precept\n{e}");
            }
        }


        private State GetVictimState([NotNull] Pawn victim)
        {
            if (!victim.IsWillingToMutate()) return State.Willing;
            if (victim.guilt?.IsGuilty == true) return State.UnwillingGuilty;
            return State.Unwilling;
        }

        private enum State
        {
            Unwilling, //first stage is for unwilling 
            UnwillingGuilty, //second stage is for willing 
            Willing //last stage is willing 
        }
    }
}