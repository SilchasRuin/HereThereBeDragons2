using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
using Dawnsbury.Core.CharacterBuilder.Selections.Options;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;

namespace HereThereBeDragons;

public class ModLoader
{
   [DawnsburyDaysModMainMethod]
    public static void LoadMod()
    {
        foreach (Feat ancestryFeat in AllFeats.All.Where(item => item is AncestrySelectionFeat))
        {
            ancestryFeat.Subfeats.Add(AddDragonBloodHeritage());
        }

        foreach (Feat feat in DragonBlood.CreateDragonbloodFeats())
        {
            ModManager.AddFeat(feat);
        }
    }

    private static Feat AddDragonBloodHeritage()
    {
        Feat dragonBloodHeritage = new HeritageSelectionFeat(ModData.FeatNames.DragonBlood,
                "You're descended in some way from a dragon. Your physical features might show this outwardly, with a pair of draconic horns, patches of scaly skin, or even a tail, or you might develop an internal reserve of draconic power. ",
                "You gain the dragonblood trait, in addition to the traits from your ancestry. " +
                "When you roll a success on a saving throw against a fear effect, you get a critical success instead. " +
                "You can choose from dragonblood feats and feats from your ancestry whenever you gain an ancestry feat.")
            .WithPermanentQEffect(
                "When you roll a success on a saving throw against a fear effect, you get a critical success instead.",
                qf =>
                {
                    qf.AdjustSavingThrowCheckResult = (Func<QEffect, Defense, CombatAction, CheckResult, CheckResult>)
                        ((_, _, action, initialResult) =>
                        {
                            if (action.HasTrait(Trait.Fear) && initialResult == CheckResult.Success)
                            {
                                return CheckResult.CriticalSuccess;
                            }
                            return initialResult;
                        });
                }
            )
            .WithOnSheet(sheet =>
                {
                    sheet.Ancestries.Add(ModData.Traits.Dragonblood);
                    sheet.AddSelectionOption(new SingleFeatSelectionOption("DraconicExemplar", "Draconic Exemplar", -1, feat => feat.HasTrait(ModData.Traits.DraconicExemplar)));
                }
            );
        return dragonBloodHeritage;
    }
}