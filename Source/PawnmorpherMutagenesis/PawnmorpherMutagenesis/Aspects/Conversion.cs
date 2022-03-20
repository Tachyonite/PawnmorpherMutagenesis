// Conversion.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/19/2022 8:58 AM
// last updated 03/19/2022  8:58 AM

using System;
using JetBrains.Annotations;
using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Aspects
{
    /// <summary>
    ///     aspect applied onto a pawn that has undergone a mutation conversion ritual
    /// </summary>
    /// <seealso cref="Pawnmorph.Aspect" />
    public class Conversion : Aspect
    {
        private const int TICK_PERIOD = 330;
        private Precept_Ritual _tfRitual;
        private bool? _isCorrectIdeo;
    

        public void Init([NotNull] Precept_Ritual tfRitual)
        {
            _tfRitual = tfRitual ?? throw new ArgumentNullException(nameof(tfRitual));
        }

        /// <summary> Called every tick. </summary>
        public override void PostTick()
        {
            base.PostTick();
            Pawn_IdeoTracker ideo = Pawn.ideo;
            if (Pawn.IsHashIntervalTick(TICK_PERIOD) && !IsCorrectIdeo && ideo != null)
            {
                ideo.OffsetCertainty(-0.05f);
                if (ideo.Certainty <= 0)
                {
                    ideo.IdeoConversionAttempt(0.3f, _tfRitual.ideo);
                    IsCorrectIdeo = ideo.Ideo == _tfRitual.ideo;
                }
            }
        }

        private bool IsCorrectIdeo
        {
            get
            {
                if (_isCorrectIdeo == null) _isCorrectIdeo = Pawn.ideo.Ideo == _tfRitual?.ideo;

                return _isCorrectIdeo.Value;
            }
            set => _isCorrectIdeo = value;
        }


        /// <summary> Called during IExposable's ExposeData to serialize data. </summary>
        protected override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref _tfRitual, "tfRitual");
        }
    }
}