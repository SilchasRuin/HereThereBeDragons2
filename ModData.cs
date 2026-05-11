using System.Drawing;
using Dawnsbury.Core;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Modding;
using Color = Microsoft.Xna.Framework.Color;

namespace HereThereBeDragons;

public class ModData
{
    public static class MTraits
    {
        public static readonly Trait Dragonblood = ModManager.RegisterTrait("Dragonblood", new TraitProperties("Dragonblood", true) {IsAncestryTrait = true});
        public static readonly Trait DraconicExemplar = ModManager.RegisterTrait("DragonChoice", new TraitProperties("Draconic Exemplar", false));
        public static readonly Trait Bludgeoning = ModManager.RegisterTrait("BludgeoningTraitForFeat", new TraitProperties("Bludgeoning", true));
        public static readonly Trait Unknown = ModManager.RegisterTrait("Unknown", new TraitProperties("Unknown", false));
        public static readonly Trait Line = ModManager.RegisterTrait("Line", new TraitProperties("Line", false));
        public static readonly Trait Resists = ModManager.RegisterTrait("ResistsForFeat", new TraitProperties("Resists", false));
        public static readonly Trait AspectWeapon = ModManager.RegisterTrait("AspectWeapon", new TraitProperties("Aspect Weapon", false));
        public static readonly Trait MagicDragonblood = ModManager.RegisterTrait("MagicDragonBlood", new TraitProperties("Magic Dragonblood", false));
        public static readonly Trait Lineage = ModManager.RegisterTrait("Lineage", new TraitProperties("Lineage", true, "A feat with this trait indicates a character's descendance from a particular type of creature. You can have only one lineage feat.", true, Color.BurlyWood, false, true));
        public static readonly Trait Burrow = ModManager.RegisterTrait("HTDBurrow", new TraitProperties("Burrow", false));
        public static readonly Trait Swim = ModManager.RegisterTrait("HTDSwim", new TraitProperties("Swim", false));
    }

    public static class MFeatNames
    {
        public static readonly FeatName DragonBlood = ModManager.RegisterFeatName("DragonbloodHeritage", "Dragonblood");
        public static readonly FeatName Unknown = ModManager.RegisterFeatName("Unknown", "Unknown");
        public static readonly FeatName DeadlyAspect = ModManager.RegisterFeatName("DeadlyAspect", "Deadly Aspect");
        public static readonly FeatName DraconicAspect = ModManager.RegisterFeatName("DraconicAspect", "Draconic Aspect");
        public static readonly FeatName ScalyHide = ModManager.RegisterFeatName("ScalyHide", "Scaly Hide");
        public static readonly FeatName DragonDomain = ModManager.RegisterFeatName("Dragon’}", "Dragon");
        public static readonly FeatName ProtectionDomain = ModManager.RegisterFeatName("Protection", "Protection");
        public static readonly FeatName FormidableBreath =  ModManager.RegisterFeatName("FormidableBreath", "Formidable Breath");
        public static readonly FeatName DebilitatingBreath = ModManager.RegisterFeatName("DebilitatingBreath", "Debilitating Breath");
        public static readonly FeatName TrueDragonsFlight = ModManager.RegisterFeatName("TrueDragonsFlight", "True Dragon's Flight");
    }

    internal static class MQEffectIds
    {
        internal static QEffectId ScalyHide { get; } = ModManager.RegisterEnumMember<QEffectId>("ScalyHide");
        internal static QEffectId DraconicBarrage { get; } = ModManager.RegisterEnumMember<QEffectId>("DraconicBarrage");
        internal static QEffectId Flight { get; } = ModManager.RegisterEnumMember<QEffectId>("HTD_Flight");
        internal static QEffectId Majestic {  get; } = ModManager.RegisterEnumMember<QEffectId>("HTD_Majestic");
    }

    internal static class MIllustrations
    {
        internal static Illustration DraconicBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrage.png");
        internal static Illustration ForceBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageForce.png");
        internal static Illustration MentalBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageMental.png");
        internal static Illustration ElectricityBarrageIllustration { get; } = new ModdedIllustration("HTDAssets/DraconicBarrageElectricity.png");
        internal static Illustration DragonTail { get; } = new ModdedIllustration("HTDAssets/Tail.png");
        internal static Illustration DragonClaws { get; } = new ModdedIllustration("HTDAssets/Claws.png");
        internal static Illustration CreateIllustration(string name)
        {
            return new ModdedIllustration("HTDAssets/" + name + ".png");
        }
    }

    internal static class ActionIds
    {
        internal static ActionId WingBuffet = ModManager.RegisterEnumMember<ActionId>("WingBuffet");
    }
}