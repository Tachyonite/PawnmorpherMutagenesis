// OutcomeEffectWorker_MutagenicConversion.cs created by Iron Wolf for PawnmorpherMutagenesis on 03/18/2022 7:20 AM
// last updated 03/18/2022  7:20 AM

using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Pawnmorph;
using Pawnmorph.Chambers;
using Pawnmorph.TfSys;
using PawnmorpherMutagenesis.Aspects;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Rituals
{
    public class OutcomeEffectWorker_MutagenicConversion : RitualOutcomeEffectWorker_FromQuality
    {
        private const string MORALIST_ID = "moralist";
        private const string CONVERTEE_ID = "convertee";
        private const string CONVERTEE_FORMAT = "CONVERTEE"; //format id for the convertee pawn 
        private const string RITUAL_FORMAT = "RITUAL"; //format id for the name of the ritual 
        private const string MORPH_FORMAT = "MORPH"; //format id for the morph they turned into 
        private const string ANIMAL_FORMAT = "ANIMAL";  //format id for the animal they turned into 
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
        {
            float quality = GetQuality(jobRitual, progress);
            OutcomeChance outcome = GetOutcome(quality, jobRitual);
            LookTargets letterLookTargets = jobRitual.selectedTarget;
            string extraLetterText = null;
            
            if (jobRitual.Ritual != null)
            {
                ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out extraLetterText, ref letterLookTargets);
            }
            Pawn moralist = jobRitual.PawnWithRole(MORALIST_ID);
            Pawn convertee = jobRitual.PawnWithRole(CONVERTEE_ID);
            float ideoCertaintyOffset = outcome.ideoCertaintyOffset;

            TaggedString text = outcome.description.Formatted(jobRitual.Ritual.Label.Named(RITUAL_FORMAT), convertee.Named(CONVERTEE_FORMAT)).CapitalizeFirst();
            
            if (outcome.Positive)
            {
                if (outcome.BestPositiveOutcome(jobRitual))
                {
                    //MorphTf and conversion 
                    var morphDef = GetRandomMorph(outcome, jobRitual);
                    MorphAndConvertPawn(convertee, morphDef, jobRitual);
                    text = text.Formatted(morphDef.Named(MORPH_FORMAT));
                    convertee.ideo.OffsetCertainty(ideoCertaintyOffset);
                }
                else
                {
                    //former human and aspect 
                    PawnKindDef getSpecies = SelectTransformationSpecies(outcome, jobRitual);
                    convertee = TransformAndAddAspect(moralist, convertee, getSpecies, jobRitual);
                    text = text.Formatted(getSpecies.Named(ANIMAL_FORMAT)); 

                }

            }

            
         
            foreach (Pawn key in totalPresence.Keys)
            {
                if (key != moralist && key != convertee && outcome.memory != null)
                {
                    Thought_AttendedRitual newThought = (Thought_AttendedRitual)MakeMemory(key, jobRitual, outcome.memory);
                    key.needs.mood.thoughts.memories.TryGainMemory(newThought);
                }
            }
            
            string text2 =   def.OutcomeMoodBreakdown(outcome);


            if (!text2.NullOrEmpty())
            {
                text += "\n\n" + text2;
            }
            if (extraLetterText != null)
            {
                text += "\n\n" + extraLetterText;
            }
            text += "\n\n" + OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
            ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out var extraOutcomeDesc);
            if (extraOutcomeDesc != null)
            {
                text += "\n\n" + extraOutcomeDesc;
            }

            TaggedString letterLabel = "OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"), jobRitual.Ritual.Label.Named("RITUALLABEL"));
            Find.LetterStack.ReceiveLetter(letterLabel, text, outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative, letterLookTargets);
        }

        private Pawn TransformAndAddAspect(Pawn moralist, Pawn convertee, PawnKindDef getSpecies, LordJob_Ritual jobRitual)
        {
            var tfRequest = new TransformationRequest(getSpecies, convertee)
            {
                addMutationToOriginal = true,
                factionResponsible = moralist?.Faction,
                manhunterSettingsOverride = ManhunterTfSettings.Never
            };

            //should use a special mutagen just for this, using default for now 
            TransformedPawn returnedPawn = MutagenDefOf.defaultMutagen.MutagenCached.Transform(tfRequest);
            if (returnedPawn != null)
            {
                var wComp = Find.World.GetComponent<PawnmorphGameComp>();
                wComp.AddTransformedPawn(returnedPawn);

            }else
            {
                Log.Error($"unable to transform pawn {convertee.Name} during conversion ritual! ");
            }

            var tfPawn =  returnedPawn?.TransformedPawns.First();

            if (tfPawn != null)
            {
                var aTracker = tfPawn.GetAspectTracker();
                if (aTracker != null)
                {
                    aTracker.Add(PMMDefOf.AspectDefOf.PMM_ConversionAspect);
                    var aspect = (Conversion) aTracker.GetAspect(PMMDefOf.AspectDefOf.PMM_ConversionAspect);
                    aspect.Init(jobRitual.Ritual); 
                }
            }

            return tfPawn; 
        }

        private void MorphAndConvertPawn([NotNull] Pawn convertee, [NotNull] MorphDef morphDef, [NotNull] LordJob_Ritual lordRitual)
        {
            MutationUtilities.AddAllMorphMutations(convertee, morphDef, MutationUtilities.AncillaryMutationEffects.None);
            convertee.ideo.SetIdeo(lordRitual.Ritual.ideo);

        }

        private MorphDef GetRandomMorph(OutcomeChance outcome, LordJob_Ritual lordRitual)
        {
            MorphDef retVal = lordRitual.Ritual?.ideo?.VeneratedAnimals
                                           ?.Select(a => MorphDef.AllDefs.FirstOrDefault(m => m.race == a))
                                            .Where(a => a != null)
                                            .RandomElementWithFallback();
            if (retVal == null)
            {
                retVal = MorphDef.AllDefs.Where(m => !m.Restricted).RandomElement(); //pure random good for now, should make a more systematic way of getting this 
            }

            return retVal; 

        }

        private PawnKindDef SelectTransformationSpecies(OutcomeChance outcome, LordJob_Ritual lordRitual)
        {
            var veneratedAnimals = lordRitual?.Ritual?.ideo?.VeneratedAnimals ?? Enumerable.Empty<ThingDef>();

            var raceDef = veneratedAnimals.RandomElementWithFallback();
            PawnKindDef retVal; 
            if (raceDef == null)
            {
                retVal = DefDatabase<PawnKindDef>.AllDefs.Where(pk => pk.RaceProps.Animal).RandomElement(); //full random is fine for now, but there should probably be a more systematic approach to this  
            }
            else
            {
                retVal = DefDatabase<PawnKindDef>.AllDefs.Where(pk => pk.race == raceDef).RandomElement(); 
            }

            return retVal; 

        }
    }
}