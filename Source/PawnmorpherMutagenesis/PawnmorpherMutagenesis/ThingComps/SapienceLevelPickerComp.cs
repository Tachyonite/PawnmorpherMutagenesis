// SapiencePickerComp.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/25/2021 8:54 PM
// last updated 07/25/2021  8:54 PM

using System.Collections.Generic;
using Pawnmorph;
using Verse;

namespace PawnmorpherMutagenesis.ThingComps
{
    public class SapienceLevelPickerComp : ThingComp
    {
        private Command_Action _cachedGizmo;

        private SapienceLevel _curLevel;

        private Gizmo[] _cachedArray;
        public SapienceLevel CurLevel => _curLevel;


        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            return CachedArray;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _curLevel, nameof(CurLevel));
        }

        private Command_Action CachedGizmo
        {
            get
            {
                if (_cachedGizmo == null)
                    _cachedGizmo = new Command_Action
                    {
                        action = CycleActiveMode, defaultLabel = _curLevel.ToString().Translate(),
                        defaultDesc = _curLevel.ToString().Translate() //TODO hookup gizmo graphics once yap makes them 
                    };

                return _cachedGizmo;
            }
        }

        private Gizmo[] CachedArray
        {
            get
            {
                if (_cachedArray == null) _cachedArray = new[] {CachedGizmo};

                return _cachedArray;
            }
        }

        private void CycleActiveMode()
        {
            if (_curLevel == SapienceLevel.Feral)
                _curLevel = SapienceLevel.Sapient;
            else
                _curLevel = (SapienceLevel) ((int) _curLevel + 1);

            _cachedGizmo.defaultLabel = _curLevel.ToString().Translate();
        }
    }

    public class SapienceLevelPickerCompProps : CompProperties
    {
        public SapienceLevelPickerCompProps()
        {
            compClass = typeof(SapienceLevelPickerComp);
        }
    }
}