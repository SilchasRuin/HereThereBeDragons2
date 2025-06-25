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
        foreach (Feat feat in DragonBlood.CreateDragonbloodFeats())
            ModManager.AddFeat(feat);
        foreach (Feat feat in DragonDeityDomain.CreateDomainFeats())
            ModManager.AddFeat(feat);
    }
}