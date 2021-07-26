// CollarHediff.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/25/2021 9:45 PM
// last updated 07/25/2021  9:45 PM

using System.Linq;
using Pawnmorph;
using Pawnmorph.Utilities;
using PawnmorpherMutagenesis.ThingComps;
using UnityEngine;
using Verse;

namespace PawnmorpherMutagenesis.Hediffs
{
    public class CollarHediff : HediffWithComps
    {
        private SapienceLevelPickerComp _comp;


        private SapienceLevelPickerComp PickerComp
        {
            get
            {
                if (_comp == null)
                    _comp = pawn.apparel?.WornApparel.MakeSafe()
                                .Select(a => a.TryGetComp<SapienceLevelPickerComp>())
                                .FirstOrDefault();

                return _comp;
            }
        }

        public override int CurStageIndex => Mathf.Min(def.stages.Count - 1, (int) (PickerComp?.CurLevel ?? SapienceLevel.Feral));
    }
}