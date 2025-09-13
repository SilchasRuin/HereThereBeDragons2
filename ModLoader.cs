using Dawnsbury.Core.CharacterBuilder.Feats;
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