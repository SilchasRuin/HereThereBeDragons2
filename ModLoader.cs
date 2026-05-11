using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
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
        DragonDeityDomain.CreateDomainFeats();
        if (ModManager.TryParse("LoresAndWeaknesses.Lore", out Trait _)) LoresRequired.LoadLore();
    }
}