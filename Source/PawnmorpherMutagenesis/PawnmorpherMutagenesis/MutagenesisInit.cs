// MutagenesisInit.cs created by Iron Wolf for PawnmorpherMutagenesis on 07/29/2021 3:14 PM
// last updated 07/29/2021  3:14 PM

using HarmonyLib;
using Verse;

namespace PawnmorpherMutagenesis
{
    [StaticConstructorOnStartup]
    internal static class MutagenesisInit
    {
        static MutagenesisInit()
        {
            var har = new Harmony(PawnmorpherMutagenesisMod.HARMONY_ID);
            har.PatchAll();

        }        
    }
}