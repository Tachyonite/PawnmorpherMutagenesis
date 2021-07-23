using Pawnmorph;
using RimWorld;
using Verse;

namespace PawnmorpherMutagenesis.Thoughts.Precept
{
    /// <summary>
    ///     Situation thought for Mutagenic Passport (social).
    /// </summary>
    /// <seealso cref="RimWorld.Thought_Situational" />
	public class ThoughtWorker_Precept_IsNotMorph_Social : ThoughtWorker_Precept_Social
    {
        /// <summary>
        ///     Social change if the pawn is a morph, but not the other one (for the percept)
        /// </summary>
        /// <param name="p">The pawn that judge</param>
        /// <param name="otherPawn">The judged pawn</param>
        protected override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
        {
            return MorphUtilities.IsMorph(p) && !MorphUtilities.IsMorph(otherPawn);
        }
    }

    /// <summary>
    ///     Situation thought for Mutagenic Passport.
    /// </summary>
    /// <seealso cref="RimWorld.Thought_Situational" />
    public class ThoughtWorker_Precept_IsNotMorph : ThoughtWorker_Precept
    {
        /// <summary>
        ///     Thought if the pawn is not a morph (for the percept)
        /// </summary>
        /// <param name="p">Pawn</param>
        protected override ThoughtState ShouldHaveThought(Pawn p)
        {
            return !MorphUtilities.IsMorph(p);
        }
    }
}
