// EventTracker.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/06/2022 10:28 AM
// last updated 03/06/2022  10:28 AM

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using UnityEngine;
using Verse;

namespace PawnmorpherMutagenesis.HEvents
{
    /// <summary>
    ///     class for tracking the statistics of a specific history event
    /// </summary>
    public class EventTracker : IExposable
    {
        [NotNull] private int[] _tickArray;

        private int _curCount;

        private float? _meanVal;
        [NotNull] private  HistoryEventDef _eventDef;

        /// <summary>
        ///     Gets the event definition this instance is listening for.
        /// </summary>
        /// <value>
        ///     The event definition.
        /// </value>
        [NotNull]
        public HistoryEventDef EventDef => _eventDef;


        /// <summary>
        ///     get the last recorded event in ticks. returns null if there are no samples
        /// </summary>
        [CanBeNull]
        public int? LastValue
        {
            get
            {
                if (_curCount == 0) return null;
                return _tickArray[_curCount - 1];
            }
        }

        /// <summary>
        ///     get the mean time between events in ticks. returns null if the mean can't be defined due to lack of samples
        /// </summary>
        [CanBeNull]
        public float? MeanDeltaT
        {
            get
            {
                if (_curCount <= 1) return null;

                if (_meanVal == null)
                {
                    var dCounter = 0;
                    for (var i = 1; i < _curCount; i++) dCounter += _tickArray[i] - _tickArray[i - 1];

                    _meanVal = dCounter / (float) (_curCount - 1);
                }

                return _meanVal.Value;
            }
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="EventTracker" /> class.
        /// </summary>
        /// <param name="eventDef">The event definition.</param>
        /// <param name="sampleCount">The sample count.</param>
        /// <exception cref="ArgumentOutOfRangeException">sampleCount - must be greater then 0</exception>
        /// <exception cref="ArgumentNullException">eventDef</exception>
        public EventTracker([NotNull] HistoryEventDef eventDef, int sampleCount)
        {
            if (sampleCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(sampleCount), sampleCount, "must be greater then 0");

            _eventDef = eventDef ?? throw new ArgumentNullException(nameof(eventDef));
            _tickArray = new int[sampleCount];
        }

        public EventTracker() {  }

        /// <summary>
        ///     listen to the history event that occured at the current tick
        /// </summary>
        /// <param name="hEvent"></param>
        /// <param name="currentTick"></param>
        public void Listen(in HistoryEvent hEvent, int currentTick)
        {
            if (hEvent.def != EventDef) return;

            if (_curCount == 0)
            {
                _tickArray[0] = currentTick;
                _curCount++;
            }
            else if (_curCount == _tickArray.Length)
            {
                PushBack();
                _tickArray[_curCount - 1] = currentTick;
            }
            else
            {
                _tickArray[_curCount - 1] = currentTick;
                _curCount++;
            }

            _meanVal = null;
        }

        /// <summary>
        ///     exposes data to be saved
        /// </summary>
        public void ExposeData()
        {
            List<int> tmpList = _tickArray?.ToList() ?? new List<int>();

            Scribe_Collections.Look(ref tmpList, "samples");
            tmpList = tmpList ?? new List<int>(); 
            if (Scribe.mode == LoadSaveMode.LoadingVars)
                for (var i = 0; i < Mathf.Min(tmpList.Count, _tickArray.Length); i++)
                    _tickArray[i] = tmpList[i];

            Scribe_Defs.Look(ref _eventDef, "eventDef");

        }


        private void PushBack()
        {
            for (var i = 1; i < _tickArray.Length; i++) _tickArray[i - 1] = _tickArray[i];
        }
    }
}