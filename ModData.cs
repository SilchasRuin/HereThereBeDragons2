using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;

namespace HereThereBeDragons;

public class ModData
{
    public static class Traits
    {
        public static readonly Trait Dragonblood = ModManager.RegisterTrait("Dragonblood", new TraitProperties("Dragonblood", true)
            {IsAncestryTrait = true});
        public static readonly Trait DraconicExemplar = ModManager.RegisterTrait("DragonChoice", new TraitProperties("Draconic Exemplar", false));
        public static readonly Trait Bludgeoning = ModManager.RegisterTrait("BludgeoningTraitForFeat", new TraitProperties("Bludgeoning", true));
        public static readonly Trait Unknown = ModManager.RegisterTrait("Unknown", new TraitProperties("Unknown", false));
        public static readonly Trait Line = ModManager.RegisterTrait("Line", new TraitProperties("Line", false));
        public static readonly Trait Resists = ModManager.RegisterTrait("ResistsForFeat", new TraitProperties("Resists", false));
        public static readonly Trait AspectWeapon = ModManager.RegisterTrait("AspectWeapon", new TraitProperties("Aspect Weapon", false));
        public static readonly Trait MagicDragonblood = ModManager.RegisterTrait("MagicDragonBlood", new TraitProperties("Magic Dragonblood", false));
    }

    public static class FeatNames
    {
        public static readonly FeatName DragonBlood = ModManager.RegisterFeatName("DragonbloodHeritage", "Dragonblood");
        public static readonly FeatName Unknown = ModManager.RegisterFeatName("Unknown", "Unknown");
        public static readonly FeatName DeadlyAspect = ModManager.RegisterFeatName("DeadlyAspect", "Deadly Aspect");
        public static readonly FeatName DraconicAspect = ModManager.RegisterFeatName("DraconicAspect", "Draconic Aspect");
        
    }

    internal static class QEffectIds
    {
        internal static QEffectId ScalyHide { get; } = ModManager.RegisterEnumMember<QEffectId>("ScalyHide");
        internal static QEffectId DragonResist { get; } = ModManager.RegisterEnumMember<QEffectId>("DragonResist");
    }
}