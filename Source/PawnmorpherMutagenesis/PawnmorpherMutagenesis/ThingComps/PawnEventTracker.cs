// PawnEventTracker.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/06/2022 7:01 PM
// last updated 03/06/2022  7:01 PM

using System.Collections.Generic;
using JetBrains.Annotations;
using PawnmorpherMutagenesis.HEvents;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.ThingComps
{


    public class PawnEventTrackerProps : CompProperties
    {
        public const int DEFAULT_TRACKER_SAMPLE = 7; 

        public PawnEventTrackerProps()
        {
            compClass = typeof(PawnEventTracker);
        }

        public int sampleCount = DEFAULT_TRACKER_SAMPLE; 

    }

    /// <summary>
    /// thing comp that tracks different events that happen to the attached pawn 
    /// </summary>
    /// <seealso cref="Verse.ThingComp" />
    public class PawnEventTracker : ThingComp
    {
        private int SampleCount => (props as PawnEventTrackerProps)?.sampleCount ?? PawnEventTrackerProps.DEFAULT_TRACKER_SAMPLE;

        [NotNull]
        private Dictionary<HistoryEventDef, EventTracker> _trackers = new Dictionary<HistoryEventDef, EventTracker>();

        [CanBeNull] public EventTracker this[HistoryEventDef hDef] => _trackers.TryGetValue(hDef);

        /// <summary>
        /// Gets the time (in ticks) the specified event happened to this pawn.
        /// </summary>
        /// <param name="hDef">The h definition.</param>
        /// <returns>the last time the event happened, null if the event hasn't happened before</returns>
        public int? GetLastTickForEventType(HistoryEventDef hDef)
        {
            return this[hDef]?.LastValue; 
        }


        /// <summary>
        /// Listens to specified event.
        /// </summary>
        /// <param name="hEvent">The h event.</param>
        /// <param name="createTrackerIfNoneExists">if set to <c>true</c> [create tracker if none exists].</param>
        public void ListenToEvent(HistoryEvent hEvent, bool createTrackerIfNoneExists = true)
        {
            if (!_trackers.TryGetValue(hEvent.def, out EventTracker tracker))
            {
                if (!createTrackerIfNoneExists) return;

                tracker = new EventTracker(hEvent.def, SampleCount);
                _trackers[hEvent.def] = tracker;
            }

            tracker.Listen(hEvent, Find.TickManager.TicksAbs);
        }




    }
}