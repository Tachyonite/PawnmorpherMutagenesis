// CollarHediff.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/25/2021 9:45 PM
// last updated 07/25/2021  9:45 PM

using Pawnmorph;
using Pawnmorph.ThingComps;
using Pawnmorph.Utilities;
using PawnmorpherMutagenesis.ThingComps;
using RimWorld;
using UnityEngine;
using Verse;

namespace PawnmorpherMutagenesis.Hediffs
{
    public class CollarHediff : HediffWithComps
    {
        private SapienceLevelPickerComp _comp;


        private bool _init = false;

        private int _lastIdx;

        public override int CurStageIndex
        {
            get
            {
                int idx = Mathf.Min(def.stages.Count - 1, (int) (PickerComp?.CurLevel ?? SapienceLevel.Feral));

                if (_lastIdx != idx)
                {
                    var spComp = pawn.needs?.TryGetNeed<Need_Control>();
                    if (spComp != null) spComp.NotifyMaxLevelDirty();
                }

                _lastIdx = idx;
                return idx;
            }
        }

        public override void PostTick()
        {
            base.PostTick();
            if (!_init)
            {
                _init = true;
                SapienceTracker sTracker = pawn.GetSapienceTracker();
                if (sTracker == null || sTracker.CurrentState != null) return;
                sTracker.EnterState(SapienceStateDefOf.Animalistic, 1);
                var sNeed = pawn.needs?.TryGetNeed<Need_Control>();
                sNeed?.NotifyMaxLevelDirty();
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _lastIdx, "lastIndex", -1);
        }

        private SapienceLevelPickerComp PickerComp
        {
            get
            {
                if (_comp == null) GetComp();
                if (_comp == null) Log.Warning($"unable to find sapience picker comp on {pawn.Label}");
                return _comp;
            }
        }

        private void GetComp()
        {
            Pawn_ApparelTracker app = pawn.apparel;
            if (app == null) return;
            foreach (Apparel apparel in app.WornApparel.MakeSafe())
            {
                _comp = apparel.TryGetComp<SapienceLevelPickerComp>();
                if (_comp != null) return;
            }

            foreach (Apparel apparel in app.LockedApparel.MakeSafe())
            {
                _comp = apparel.TryGetComp<SapienceLevelPickerComp>();
                if (_comp != null) return;
            }
        }
    }
}