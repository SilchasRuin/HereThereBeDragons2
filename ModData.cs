using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Display.Illustrations;
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
        public static readonly FeatName ScalyHide = ModManager.RegisterFeatName("ScalyHide", "Scaly Hide");
        public static readonly FeatName DragonDomain = ModManager.RegisterFeatName("DragonDomain", "Dragon");
        public static readonly FeatName ProtectionDomain = ModManager.RegisterFeatName("ProtectionDomain", "Protection");
    }

    internal static class QEffectIds
    {
        internal static QEffectId ScalyHide { get; } = ModManager.RegisterEnumMember<QEffectId>("ScalyHide");
        internal static QEffectId DraconicBarrage { get; } = ModManager.RegisterEnumMember<QEffectId>("DraconicBarrage");
    }

    internal static class Illustrations
    {
        internal static Illustration DraconicBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrage.png");
        internal static Illustration ForceBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageForce.png");
        internal static Illustration MentalBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageMental.png");
        internal static Illustration ElectricityBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageElectricity.png");
        
    }
}