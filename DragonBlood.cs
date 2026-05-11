using System.Reflection;
using System.Runtime.CompilerServices;
using Dawnsbury.Audio;
using Dawnsbury.Auxiliary;
using Dawnsbury.Core;
using Dawnsbury.Core.Animations;
using Dawnsbury.Core.CharacterBuilder;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using Dawnsbury.Core.CharacterBuilder.Selections.Options;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Coroutines.Options;
using Dawnsbury.Core.Coroutines.Options.Reactive;
using Dawnsbury.Core.Coroutines.Requests;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Creatures.Parts;
using Dawnsbury.Core.Intelligence;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Damage;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core.Roller;
using Dawnsbury.Core.Tiles;
using Dawnsbury.Display;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Display.Text;
using Dawnsbury.Modding;
using SpiritDamage;
using static HereThereBeDragons.ModData;

namespace HereThereBeDragons;

public static class DragonBlood
{
    public static IEnumerable<Feat> CreateDragonbloodFeats()
    {
        #region Heritage
        Feat dragonBloodHeritage = new Feat(MFeatNames.DragonBlood,
                "You're descended in some way from a dragon. Your physical features might show this outwardly, with a pair of draconic horns, patches of scaly skin, or even a tail, or you might develop an internal reserve of draconic power. ",
                "You gain the dragonblood trait, in addition to the traits from your ancestry. " +
                "When you roll a success on a saving throw against a fear effect, you get a critical success instead. " +
                "You can choose from dragonblood feats and feats from your ancestry whenever you gain an ancestry feat.",
                [MTraits.Dragonblood, Trait.VersatileHeritage], null)
            .WithPermanentQEffect(
                "When you roll a success on a saving throw against a fear effect, you get a critical success instead.",
                qf =>
                {
                    qf.AdjustSavingThrowCheckResult =
                        (_, _, action, initialResult) =>
                        {
                            if (action.HasTrait(Trait.Fear) && initialResult == CheckResult.Success)
                            {
                                return CheckResult.CriticalSuccess;
                            }
                            return initialResult;
                        };
                }
            )
            .WithOnSheet(sheet =>
                {
                    sheet.Ancestries.Add(MTraits.Dragonblood);
                    sheet.AddSelectionOption(new SingleFeatSelectionOption("DraconicExemplar", "Draconic Exemplar", -1,
                        feat => feat.HasTrait(MTraits.DraconicExemplar)));
                }
            )
            .WithOnCreature(creature => creature.Traits.Add(MTraits.Dragonblood));
        yield return dragonBloodHeritage;
        #endregion
        #region DraconicExemplar
        Feat draconicExemplarAdamantine = new(ModManager.RegisterFeatName("AdamantineExemplar", "Adamantine"),
            "The powerful adamantine dragons are one of several dragons known as skymetal dragons. Adamantine dragons are typically steadfast and loyal. Once they commit to a certain purpose, changing their minds is nigh impossible.",
            "The power of the adamantine dragon causes you to deal bludgeoning damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
            [MTraits.DraconicExemplar, Trait.Primal, MTraits.Bludgeoning, Trait.Reflex, MTraits.Burrow], null);
        yield return draconicExemplarAdamantine;
        Feat draconicExemplarConspirator = new(ModManager.RegisterFeatName("ConspiratorExemplar", "Conspirator"),
            "Hidden among the shadows and upper echelons of society are the conspirator dragons. These dragons are schemers, always looking to manipulate and control others, either for personal gain or simply for the thrill of watching their machinations play out.",
            "The power of the conspirator dragon causes you to deal poison damage in a cone if you choose a breath weapon and is connected to the Occult tradition.",
            [MTraits.DraconicExemplar, Trait.Occult, Trait.Poison, Trait.Fortitude], null);
        yield return draconicExemplarConspirator;
        Feat draconicExemplarDiabolic = new(ModManager.RegisterFeatName("DiabolicExemplar", "Diabolic"),
            "Some scholars argue diabolic dragons are just extensions of Hell, living creatures that break off from the plane to enact its will. Whether this is true or whether diabolical dragons are simply the reborn souls of dragons sent to Hell, the fact remains that these dragons are powerful, cunning, and tyrannical.",
            "The power of the diabolic dragon causes you to deal fire damage in a cone if you choose a breath weapon and is connected to the Divine tradition.",
            [MTraits.DraconicExemplar, Trait.Divine, Trait.Fire, Trait.Reflex], null);
        yield return draconicExemplarDiabolic;
        Feat draconicExemplarFortune = new(ModManager.RegisterFeatName("FortuneExemplar", "Fortune"),
            "Fortune dragons have the innate ability to draw upon the raw magical energies that surround them. Fortune dragons are seekers of novel experiences. This desire for originality leads fortune dragons to approach visitors of other ancestries with curiosity, though this interest often proves short lived",
            "The power of the fortune dragon causes you to deal force damage in a cone if you choose a breath weapon and is connected to the Arcane tradition.",
            [MTraits.DraconicExemplar, Trait.Arcane, Trait.Force, Trait.Reflex], null);
        yield return draconicExemplarFortune;
        Feat draconicExemplarHorned = new(ModManager.RegisterFeatName("HornedExemplar", "Horned"),
            "The magic that flows through primal dragons can manifest more animalistic or bestial features in a given type of dragon. Notably among these are the massive paired horns of the horned dragon. Horned dragons are generally contemplative and have a fixation on knowledge and self-discipline, traits belied by their bestial appearance.",
            "The power of the horned dragon causes you to deal poison damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
            [MTraits.DraconicExemplar, Trait.Primal, Trait.Poison, Trait.Fortitude, MTraits.Swim], null);
        yield return draconicExemplarHorned;
        Feat draconicExemplarMirage = new(ModManager.RegisterFeatName("MirageExemplar", "Mirage"),
            "Mirage dragons are masters of illusion magic and use their powers to deceive others and further their own agendas. Mirage dragons are vain and egotistical figures. They ultimately care more about themselves than others.",
            "The power of the mirage dragon causes you to deal mental damage in a cone if you choose a breath weapon and is connected to the Arcane tradition.",
            [MTraits.DraconicExemplar, Trait.Arcane, Trait.Mental, Trait.Will], null);
        yield return draconicExemplarMirage;
        Feat draconicExemplarOmen = new(ModManager.RegisterFeatName("OmenExemplar", "Omen"),
            "Omen dragons are bound to see the future—nebulous though it might be—at all times. Visions of the future hound them like a quiet song that never stops playing in their minds. Omen dragons have a natural compulsion to share the futures they see, but they have no compunctions about what the visions show and share their knowledge with the wicked as readily as the virtuous.",
            "The power of the omen dragon causes you to deal mental damage in a cone if you choose a breath weapon and is connected to the Occult tradition.",
            [MTraits.DraconicExemplar, Trait.Occult, Trait.Mental, Trait.Will], null);
        yield return draconicExemplarOmen;
        Feat draconicExemplarHeaven = new(ModManager.RegisterFeatName("HeavenlyExemplar", "Heavenly"),
            "Heavenly dragons are protectors of the innocent and enemies of the wicked. Wise and with vast knowledge, they offer their advice to the worthy who come to them in their homes among the mountain peaks.",
            "The power of the heavenly dragon causes you to deal electricity damage in a line if you choose a breath weapon and is connected to the Divine tradition.",
            [MTraits.DraconicExemplar, Trait.Divine, Trait.Electricity, MTraits.Line, Trait.Reflex, Trait.Homebrew],
            null);
        yield return draconicExemplarHeaven;
        Feat draconicExemplarBlizzard = new(ModManager.RegisterFeatName("BlizzardExemplar", "Blizzard"),
            "On the peaks of icy mountains and in the eternal cold of the poles, where spring never comes, blizzard dragons dwell in the snow and frost. Blizzard dragons are seldom interested in the goings on of non-dragons, who they consider to be lesser creatures.",
            "The power of the blizzard dragon causes you to deal cold damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
            [MTraits.DraconicExemplar, Trait.Primal, Trait.Cold, Trait.Fortitude, Trait.Homebrew, MTraits.Swim], null);
        yield return draconicExemplarBlizzard;
        Feat draconicExemplarDeep = new(ModManager.RegisterFeatName("DeepExemplar", "Deep"),
            "Within the bowels of the earth, deep dragons await with endless patience. The longest lived of all wyrms, deep dragons wait and watch, laying century long plans from within lairs deep underground.",
            "The power of the deep dragon causes you to deal acid damage in a line if you choose a breath weapon and is connected to the Arcane tradition.",
            [MTraits.DraconicExemplar, Trait.Arcane, Trait.Acid, Trait.Reflex, MTraits.Line, Trait.Homebrew, MTraits.Burrow], null);
        yield return draconicExemplarDeep;
        Feat draconicExemplarEmpyreal = new(ModManager.RegisterFeatName("EmpyrealExemplar", "Empyreal"),
            "Empyreal dragons have a direct connection to Heaven. Using the blessings of Heaven, empyreal dragons protect others and intercede against wickedness. Empyreal dragons are wise, considerate, and compassionate. When speaking with others, empyreal dragons are patient and understanding.",
            "The power of the empyreal dragon causes you to deal spirit damage in a cone if you choose a breath weapon and is connected to the Divine tradition.",
            [MTraits.DraconicExemplar, Trait.Divine, SpiritTrait.Spirit, Trait.Reflex], null);
        yield return draconicExemplarEmpyreal;
        Feat draconicExemplarUnknown = new(MFeatNames.Unknown,
            "Your draconic exemplar's nature hasn't revealed itself yet.",
            "You do not have to choose an exemplar at level 1, however some class features require you to choose an exemplar, if you wish to take one of those features, retrain to a different exemplar.",
            [MTraits.DraconicExemplar, MTraits.Unknown], null);
        yield return draconicExemplarUnknown;
        #endregion
        #region Level 1
        TrueFeat breathOfTheDragon = new(
            ModManager.RegisterFeatName("BreathOfTheDragon", "Breath of the Dragon"),
            1, "You can unleash a powerful breath weapon like your draconic exemplar.",
            "Tapping into the physiology of your draconic ancestor, you can exhale a torrent of energy in a 15-foot cone or a 30-foot line, dealing 1d4 damage. Each creature in the area must attempt a basic saving throw against the higher of your class DC or spell DC. You can't use this ability again for 1d4 rounds.\n\nAt 3rd level and every 2 levels thereafter, the damage increases by 1d4. The shape of the breath, the damage type, and the saving throw match those of your draconic exemplar. This ability has the trait associated with the type of damage it deals.",
            [MTraits.Dragonblood]);
        CreateBreathLogic(breathOfTheDragon);
        yield return breathOfTheDragon;
        TrueFeat draconicResistance = new(
            ModManager.RegisterFeatName("DraconicResistance", "Draconic Resistance"),
            1, "Draconic magic safeguards you from harm.",
            "You gain resistance equal to half your level (minimum 1) to the damage type associated with your draconic exemplar. Double this resistance against damage of that type dealt to you by dragons. If your draconic exemplar is associated with bludgeoning, piercing, or slashing damage, instead of gaining resistance to that type you can choose acid, cold, fire, electricity, or sonic.",
            [MTraits.Dragonblood]);
        CreateDraconicResistanceLogic(draconicResistance);
        yield return draconicResistance;
        TrueFeat draconicResistanceB = new(
            ModManager.RegisterFeatName("DraconicResistanceB", "Draconic Resistance - Adamantine"),
            1, "Draconic magic safeguards you from harm.",
            "You gain resistance equal to half your level (minimum 1) to the damage type associated with your draconic exemplar. Double this resistance against damage of that type dealt to you by dragons. If your draconic exemplar is associated with bludgeoning, piercing, or slashing damage, instead of gaining resistance to that type you can choose acid, cold, fire, electricity, or sonic.",
            [MTraits.Dragonblood]);
        CreateDraconicResistChoice(draconicResistanceB);
        yield return draconicResistanceB;
        //damagekind choice feats
        Feat acidResist = new(ModManager.RegisterFeatName("Acid", "Acid"), null, "You resist acid.",
            [Trait.Acid, MTraits.Resists], null);
        CreateDraconicResistB(acidResist);
        yield return acidResist;
        Feat coldResist = new(ModManager.RegisterFeatName("Cold", "Cold"), null, "You resist cold.",
            [Trait.Cold, MTraits.Resists], null);
        CreateDraconicResistB(coldResist);
        yield return coldResist;
        Feat fireResist = new(ModManager.RegisterFeatName("Fire", "Fire"), null, "You resist fire.",
            [Trait.Fire, MTraits.Resists], null);
        CreateDraconicResistB(fireResist);
        yield return fireResist;
        Feat electricityResist = new(ModManager.RegisterFeatName("Electricity", "Electricity"), null,
            "You resist electricity.", [Trait.Electricity, MTraits.Resists], null);
        CreateDraconicResistB(electricityResist);
        yield return electricityResist;
        Feat sonicResist = new(ModManager.RegisterFeatName("Sonic", "Sonic"), null, "You resist sonic.",
            [Trait.Sonic, MTraits.Resists], null);
        CreateDraconicResistB(sonicResist);
        yield return sonicResist;
        // level 1 ancestry feats continued
        TrueFeat arcaneDragonBlood = new(
            ModManager.RegisterFeatName("ArcaneDragonblood", "Arcane Dragonblood"),
            1,
            "You descend from a dragon that wields mastery of their magical abilities, such as a fortune dragon or mirage dragon. As such, you can instinctively grasp the intricacies of magic.",
            "You gain the trained proficiency rank in Arcana. If you would automatically become trained in Arcana (from your background or class, for example), you instead become trained in a skill of your choice. You can cast shield as an arcane innate spell at will",
            [MTraits.Dragonblood, Trait.Arcane, MTraits.MagicDragonblood, MTraits.Lineage]);
        CreateMagicalDragonbloodLogic(arcaneDragonBlood);
        yield return arcaneDragonBlood;
        TrueFeat divineDragonBlood = new(
            ModManager.RegisterFeatName("DivineDragonblood", "Divine Dragonblood"),
            1,
            "You can trace your lineage to a dragon with almost deific powers, such as a diabolic dragon or heaven dragon.",
            "You gain the trained proficiency rank in Religion. If you would automatically become trained in Religion (from your background or class, for example), you instead become trained in a skill of your choice. You can cast guidance as a divine innate spell at will",
            [MTraits.Dragonblood, Trait.Divine, MTraits.MagicDragonblood, MTraits.Lineage]);
        CreateMagicalDragonbloodLogic(divineDragonBlood);
        yield return divineDragonBlood;
        TrueFeat occultDragonBlood = new(
            ModManager.RegisterFeatName("OccultDragonblood", "Occult Dragonblood"),
            1,
            "Your blood contains a tiny fragment of unusual or inexplicable power from a mysterious dragon, such as a conspirator dragon or omen dragon.",
            "You gain the trained proficiency rank in Occultism. If you would automatically become trained in Occultism (from your background or class, for example), you instead become trained in a skill of your choice. You can cast open door as an occult innate spell at will",
            [MTraits.Dragonblood, Trait.Occult, MTraits.MagicDragonblood, MTraits.Lineage]);
        CreateMagicalDragonbloodLogic(occultDragonBlood);
        yield return occultDragonBlood;
        TrueFeat primalDragonBlood = new(
            ModManager.RegisterFeatName("PrimalDragonblood", "Primal Dragonblood"),
            1,
            "A dragon with a deep connection to the natural world, such as an adamantine dragon or a horned dragon, resides somewhere on your family tree.",
            "You gain the trained proficiency rank in Nature. If you would automatically become trained in Nature (from your background or class, for example), you instead become trained in a skill of your choice. You can cast tanglefoot as a primal innate spell at will",
            [MTraits.Dragonblood, Trait.Primal, MTraits.MagicDragonblood, MTraits.Lineage]);
        CreateMagicalDragonbloodLogic(primalDragonBlood);
        yield return primalDragonBlood;
        TrueFeat scalyHide = new(MFeatNames.ScalyHide,
            1,
            "You were born with a layer of scales across your entire body that resemble those of your draconic progenitor.",
            "When you’re unarmored, the scales give you a +1 item bonus to AC with a Dexterity cap of +3. The item bonus to AC increases to +2 at 5th level. The item bonus to AC from these scales is cumulative with armor potency runes on your explorer's clothing, or the mystic armor spell." +
            "\n\n{b}Special{/b} You cannot take this feat and Draconic Aspect. You should take this feat at level 1 only.",
            [MTraits.Dragonblood]);
        CreateScalyHideLogic(scalyHide);
        yield return scalyHide;
        TrueFeat draconicAspect = new(MFeatNames.DraconicAspect,
            1,
            "You have an obvious draconic feature, such as sharp claws, a snout full of sharp teeth, or strong reptilian tail, that you can use offensively.",
            "You gain your choice of one of the following unarmed attacks. The attack is in the brawling group and has the listed damage die and traits.\n\n    {b}• Claw{/b} 1d4 slashing (agile, finesse, unarmed)\n    {b}• Jaws{/b} 1d6 piercing (forceful, unarmed)\n    {b}• Tail{/b} 1d6 bludgeoning (sweep, trip, unarmed)" +
            "\n\n{b}Special{/b} You cannot take this feat and Scaly Hide. You should take this feat at level 1 only.",
            [MTraits.Dragonblood], AspectFeats(DraconicAspectFeats()));
        draconicAspect.WithPrerequisite(sheet => !sheet.HasFeat(MFeatNames.ScalyHide),
            "You cannot take this feat and Scaly Hide.");
        yield return draconicAspect;

        #endregion
        #region Level 5
        TrueFeat deadlyAspect = new(MFeatNames.DeadlyAspect,
            5, "You have honed the unarmed attack your draconic heritage has granted you to a lethal degree.",
            "The unarmed attack you gained from Draconic Aspect gains the deadly d8 trait.",
            [MTraits.Dragonblood]);
        deadlyAspect.WithPrerequisite(MFeatNames.DraconicAspect, "Draconic Aspect");
        yield return deadlyAspect;
        TrueFeat traditionalResists = new(
            ModManager.RegisterFeatName("TraditionalResists", "Traditional Resistances"),
            5, "Due to your blood, you have some resistance to certain types of magic.",
            "You gain a +1 status bonus to AC and saves against spells and other magical effects from the same tradition as your lineage. This bonus increases to +2 against sleep and paralysis effects.",
            [MTraits.Dragonblood]);
        CreateTraditionalResistancesLogic(traditionalResists);
        yield return traditionalResists;
        TrueFeat dragonsFlight = new(ModManager.RegisterFeatName("DragonsFlight", "Dragon's Flight"),
            5,
            "You have grown a small pair of draconic wings or have honed your use of the wings you've had since birth.",
            "You Fly. If you don't normally have a fly Speed, you gain a fly Speed of 20 feet for this movement. You must end your movement on solid ground.",
            [MTraits.Dragonblood]);
        CreateDragonsFlightLogic(dragonsFlight);
        yield return dragonsFlight;
        TrueFeat draconicScent = new(ModManager.RegisterFeatName("DraconicScent", "Draconic Scent"),
            5, "Your sense of smell has heightened to be as keen as that of a dragon.",
            "Creatures within 30 feet cannot be undetected by you.",
            [MTraits.Dragonblood]);
        CreateDraconicScentLogic(draconicScent);
        yield return draconicScent;
        Feat bloodAndSpirit = new TrueFeat(ModManager.RegisterFeatName("BloodAndSpirit", "Blood and Spirit"), 5, 
            "Your connection to the divine has sanctified your blood, but the power only manifests when you are shedding it.",
            "Choose holy or unholy. While you are taking persistent bleed damage, you can Interact to coat a piercing or slashing weapon you’re wielding with your blood. Until the end of your turn, your Strikes with that weapon deal an additional 1d6 spirit damage with the chosen trait to creatures with the opposing trait. Alternatively, you can deal 1d6 slashing damage to yourself (which can’t be resisted in any way) as part of that Interact action if you aren’t taking persistent bleed damage.",
            [MTraits.Dragonblood]).WithPrerequisite(divineDragonBlood.FeatName, "Divine Dragonblood");
        CreateBloodAndSpiritLogic(bloodAndSpirit);
        yield return bloodAndSpirit;

        #endregion
        #region Level 9
        Feat formidableBreath = new TrueFeat(MFeatNames.FormidableBreath, 9,
            "Thanks to rigorous breathing exercises and a diet similar to that of your lineage, your magical breath is more powerful.",
            "The area of your Breath of the Dragon increases to 30 feet for a cone or 60 feet for a line, and the damage dice are d6s instead of d4s.",
            [MTraits.Dragonblood]).WithPrerequisite(breathOfTheDragon.FeatName, "Breath of the Dragon");
        yield return formidableBreath;
        Feat trueDragonsFlight = new TrueFeat(MFeatNames.TrueDragonsFlight,
            9, "Your wings have grown more powerful, capable of keeping you aloft longer.",
            "You have flying at all times. (You ignore difficult and hazardous terrain and can move over water, lava and chasms.)",
            [MTraits.Dragonblood]).WithPrerequisite(dragonsFlight.FeatName, "Dragon's Flight");
        CreateTrueFlightLogic(trueDragonsFlight);
        yield return trueDragonsFlight;
        Feat wingBuffet = new TrueFeat(ModManager.RegisterFeatName("WingBuffet", "Wing Buffet"), 9,
                "You have a pair of draconic wings strong enough to batter your foes away and shove them away.",
                "Choose up to two creatures adjacent to you. Attempt an Athletics check and compare it to the Fortitude DC of each target. This counts as two attacks for your multiple attack penalty, but the penalty doesn't increase until after both attacks."
                + S.FourDegreesOfSuccess(
                    "The target takes bludgeoning damage equal to double your level and is pushed up to 10 feet away from you.",
                    "The target takes bludgeoning damage equal to your level and is pushed up to 5 feet away from you.",
                    "The target takes bludgeoning damage equal to half your level.",
                    "You fall prone at the end of this activity."),
                [MTraits.Dragonblood, Trait.Attack]).WithActionCost(2)
            .WithPrerequisite(values => values.GetProficiency(Trait.Athletics) >= Proficiency.Expert,
                "You must be an expert in Athletics.");
        CreateWingBuffetLogic(wingBuffet);
        yield return wingBuffet;
        Feat shelteringWings = new TrueFeat(ModManager.RegisterFeatName("ShelteringWings", "Sheltering Wings"), 9, 
                "You raise your fledgling wings to protect you from attack.",
            "You gain a +1 circumstance bonus to AC and to saving throws against spells that target you until the start of your next turn. If you have True Dragon's Flight, this bonus increases to +2 as your large wings provide greater protection.",
            [MTraits.Dragonblood])
            .WithPrerequisite(dragonsFlight.FeatName, "Dragon's Flight");
        CreateShelteringWingsLogic(shelteringWings);
        yield return shelteringWings;
        #endregion
        #region Level 13
        Feat majesticPresence = new TrueFeat(ModManager.RegisterFeatName("MajesticPresence", "Majestic Presence"),
            13, "By taking an impressive stance, the full force of your personality cows lesser beings.",
            "Each enemy in a 20- foot emanation must attempt a Will save against the higher of your class DC or spell DC. Regardless of the result of the saving throw, the creature is temporarily immune to your Majestic Presence for the rest of the encounter." +
            S.FourDegreesOfSuccess("The creature is unaffected.", "The creature is frightened 1.",
                "The creature is frightened 2.", "The creature is frightened 4."),
            [MTraits.Dragonblood, Trait.Emotion, Trait.Fear, Trait.Mental, Trait.Visual]).WithActionCost(1);
        CreateMajesticPresenceLogic(majesticPresence);
        yield return majesticPresence;
        Feat draconicVeil = new TrueFeat(ModManager.RegisterFeatName("DraconicVeil", "Draconic Veil"), 13,
            "Like some dragons, your forebear had the supernatural ability to change their shape to walk among humanoids.",
            "You have learned how to mimic this ability. You can cast enlarge as a 2nd-rank innate spell once per day, except that you may cast it as a free action at the start of an encounter and may only target yourself. The spell's tradition matches your draconic exemplar's.",
            [MTraits.Dragonblood]);
        CreateDraconicVeilLogic(draconicVeil);
        yield return draconicVeil;
        Feat debilitatingBreath = new TrueFeat(MFeatNames.DebilitatingBreath, 13, "Your breath causes lasting hindrance to those it harms.",
            "Creatures who fail their saves against your Breath of the Dragon gain one of the following conditions, based on the damage type of your breath: clumsy 1 (electricity, cold, fire, or sonic damage), enfeebled 1 (acid, physical, or poison damage), or stupefied 1 (force, mental, spirit, vitality, or void damage). The condition lasts until the end of your next turn.", [MTraits.Dragonblood])
            .WithPrerequisite(breathOfTheDragon.FeatName, "Breath of the Dragon");
        yield return debilitatingBreath;
        #endregion
    }
    //Logics
    #region Level 1
    private static void CreateBreathLogic(TrueFeat breathWeapon)
    {
        breathWeapon.WithActionCost(2)
            .WithPrerequisite(
                sheet => !sheet.HasFeat(MFeatNames.Unknown) &&
                         sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.DraconicExemplar)),
                "You must select a draconic exemplar.")
            .WithOnCreature((sheet, creature) =>
            {
                Feat? feat = sheet.AllFeats.FirstOrDefault(ft =>
                    ft.HasTrait(MTraits.DraconicExemplar) && ft.FeatName != MFeatNames.Unknown);
                if (feat == null)
                    return;
                int num = 10;
                if (creature.PersistentCharacterSheet?.Class != null)
                {
                    List<int> nums = [];
                    Trait classTrait = creature.PersistentCharacterSheet.Class.ClassTrait;
                    int num2 = 10 + creature.Abilities.Get(creature.Abilities.KeyAbility) +
                               sheet.Proficiencies.Get(classTrait).ToNumber(creature.ProficiencyLevel);
                    nums.Add(num2);
                    if (creature.Spellcasting?.Sources != null)
                    {
                        foreach (SpellcastingSource source in creature.Spellcasting.Sources)
                        {
                            nums.Add(10 + source.SpellcastingAbilityModifier + sheet.Proficiencies
                                .Get(source.SpellcastingTradition).ToNumber(creature.ProficiencyLevel));
                        }
                    }

                    num = nums.Max();
                }

                creature.AddQEffect(new QEffect("Breath of the Dragon {icon:TwoActions}",
                    $"DC {num} basic {WhichSave(feat).HumanizeTitleCase2()} save to deal {(creature.Level + 1) / 2}{(creature.HasFeat(MFeatNames.FormidableBreath) ? "d6" : "d4")} {DetermineDamageKind(feat).HumanizeTitleCase2().ToLower()} damage in a {(!creature.HasFeat(MFeatNames.FormidableBreath) ? IsCone(feat) ? "15-foot cone" : "30-foot line" : IsCone(feat) ? "30-foot cone" : "60-foot line")}.")
                {
                    ProvideMainAction = (Func<QEffect, Possibility>)(qfSelf =>
                    {
                        Creature owner = qfSelf.Owner;
                        int dc = owner.ClassOrSpellDC();
                        if (!owner.HasFeat(MFeatNames.FormidableBreath))
                        {
                            return new ActionPossibility(new CombatAction(owner, IllustrationName.BreathWeapon,
                                        "Breath of the Dragon",
                                        [DetermineTrait(feat), Trait.Magical, Trait.Basic],
                                        $"{{b}}Area{{/b}} {(IsCone(feat) ? "15-foot cone" : "30-foot line")}\n{{b}}Saving throw{{/b}} basic {WhichSave(feat).HumanizeTitleCase2()}\n\nDeal {S.HeightenedVariable((owner.Level + 1) / 2, 1)}d4 {DetermineDamageKind(feat).HumanizeTitleCase2().ToLower()} damage (basic DC {dc.ToString()} {WhichSave(feat).HumanizeTitleCase2().ToLower()} save mitigates).\n\nThen you can't use Breath of the Dragon again for 1d4 rounds.",
                                        IsCone(feat) ? Target.Cone(3) : Target.Line(6)).WithActionCost(2)
                                    .WithProjectileCone(IllustrationName.BreathWeapon, 15,
                                        IsCone(feat) ? ProjectileKind.Cone : ProjectileKind.Ray)
                                    .WithSoundEffect(SfxName.FireRay)
                                    .WithSavingThrow(new SavingThrow(WhichSave(feat), dc))
                                    .WithEffectOnEachTarget(async (spell, caster, target, result) =>
                                        {
                                            await CommonSpellEffects.DealBasicDamage(spell,
                                                caster,
                                                target, result, (caster.Level + 1) / 2 + "d4",
                                                DetermineDamageKind(feat));
                                            if (caster.HasFeat(MFeatNames.DebilitatingBreath) && result <= CheckResult.Failure)
                                            {
                                                if (DetermineDamageKind(feat) is DamageKind.Fire
                                                    or DamageKind.Electricity or DamageKind.Cold or DamageKind.Sonic)
                                                    target.AddQEffect(QEffect.Clumsy(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                                else if (DetermineDamageKind(feat) is DamageKind.Acid
                                                         or DamageKind.Poison or DamageKind.Slashing
                                                         or DamageKind.Piercing or DamageKind.Bludgeoning)
                                                    target.AddQEffect(QEffect.Enfeebled(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                                else if (DetermineDamageKind(feat) is DamageKind.Force
                                                         or DamageKind.Mental or DamageKind.Positive
                                                         or DamageKind.Negative || DetermineDamageKind(feat) == DamageSpirit.Spirit)
                                                    target.AddQEffect(QEffect.Stupefied(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                            }
                                        }
                                        )
                                    .WithEffectOnChosenTargets((_, caster, _) =>
                                        Task.FromResult(
                                            caster.AddQEffect(QEffect.CannotUseForXRound("Breath of the Dragon", caster,
                                                R.Next(2, 5))))))
                                .WithPossibilityGroup("Natural weapon");
                        }

                        return new ActionPossibility(new CombatAction(owner, IllustrationName.BreathWeapon,
                                    "Breath of the Dragon",
                                    [DetermineTrait(feat), Trait.Magical, Trait.Basic],
                                    $"{{b}}Area{{/b}} {(IsCone(feat) ? "30-foot cone" : "60-foot line")}\n{{b}}Saving throw{{/b}} basic {WhichSave(feat).HumanizeTitleCase2()}\n\nDeal {S.HeightenedVariable((owner.Level + 1) / 2, 1)}d6 {DetermineDamageKind(feat).HumanizeTitleCase2().ToLower()} damage (basic DC {dc.ToString()} {WhichSave(feat).HumanizeTitleCase2().ToLower()} save mitigates).\n\nThen you can't use Breath of the Dragon again for 1d4 rounds.",
                                    IsCone(feat) ? Target.Cone(6) : Target.Line(12)).WithActionCost(2)
                                .WithProjectileCone(IllustrationName.BreathWeapon, 15,
                                    IsCone(feat) ? ProjectileKind.Cone : ProjectileKind.Ray)
                                .WithSoundEffect(SfxName.FireRay).WithSavingThrow(new SavingThrow(WhichSave(feat), dc))
                                .WithEffectOnEachTarget(async (spell, caster, target, result) =>
                                    {
                                        await CommonSpellEffects.DealBasicDamage(spell,
                                            caster,
                                            target, result, (caster.Level + 1) / 2 + "d6", DetermineDamageKind(feat));
                                        if (caster.HasFeat(MFeatNames.DebilitatingBreath) && result <= CheckResult.Failure)
                                        {
                                            if (DetermineDamageKind(feat) is DamageKind.Fire
                                                or DamageKind.Electricity or DamageKind.Cold or DamageKind.Sonic)
                                                target.AddQEffect(QEffect.Clumsy(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                            else if (DetermineDamageKind(feat) is DamageKind.Acid
                                                     or DamageKind.Poison or DamageKind.Slashing
                                                     or DamageKind.Piercing or DamageKind.Bludgeoning)
                                                target.AddQEffect(QEffect.Enfeebled(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                            else if (DetermineDamageKind(feat) is DamageKind.Force
                                                         or DamageKind.Mental or DamageKind.Positive
                                                         or DamageKind.Negative || DetermineDamageKind(feat) == DamageSpirit.Spirit)
                                                target.AddQEffect(QEffect.Stupefied(1).WithExpirationAtEndOfSourcesNextTurn(caster, true));
                                        }
                                    }
                                    )
                                .WithEffectOnChosenTargets((_, caster, _) =>
                                    Task.FromResult(
                                        caster.AddQEffect(QEffect.CannotUseForXRound("Breath of the Dragon", caster,
                                            R.Next(2, 5))))))
                            .WithPossibilityGroup("Natural weapon");
                    })
                });
            });
    }

    //Feat logic
    private static void CreateDraconicResistanceLogic(TrueFeat draconicResistance)
    {
        draconicResistance
            .WithPrerequisite(
                (sheet) => !sheet.HasFeat(MFeatNames.Unknown)
                           && !sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.Bludgeoning))
                           && sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.DraconicExemplar)),
                "You must select a draconic exemplar other than adamantine.")
            .WithOnCreature((sheet, self) =>
            {
                Feat? feat = sheet.AllFeats.FirstOrDefault(ft =>
                    ft.HasTrait(MTraits.DraconicExemplar) && ft.FeatName != MFeatNames.Unknown);
                if (feat != null && !feat.HasTrait(MTraits.Bludgeoning))
                {
                    self.AddQEffect(new QEffect()
                    {
                        StateCheck = innerSelf =>
                            innerSelf.Owner.WeaknessAndResistance.Resistances.Add(new SpecialResistance(
                                $"{DetermineDamageKind(feat).HumanizeLowerCase2()}" + " dragon", (action, kind) =>
                                    action != null
                                    && action.Owner.HasTrait(Trait.Dragon)
                                    && kind == DetermineDamageKind(feat),
                                (self.Level / 2 > 1 ? self.Level / 2 : 1) * 2, null
                            ))
                    });
                    self.AddQEffect(QEffect.DamageResistance(DetermineDamageKind(feat),
                        self.Level / 2 > 1 ? self.Level / 2 : 1));
                }
            });
    }

    private static void CreateDraconicResistChoice(TrueFeat draconicResistance)
    {
        draconicResistance
            .WithPrerequisite(
                (sheet) => !sheet.HasFeat(MFeatNames.Unknown)
                           && sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.Bludgeoning))
                           && sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.DraconicExemplar)),
                "You must select the adamantine draconic exemplar.")
            .WithOnSheet(AlternateResistSelection);
    }

    private static void CreateDraconicResistB(Feat resists)
    {
        resists.WithOnCreature(self =>
        {
            self.AddQEffect(new QEffect()
            {
                StateCheck = innerSelf =>
                    innerSelf.Owner.WeaknessAndResistance.Resistances.Add(new SpecialResistance(
                        $"{DetermineDamageKind(resists).HumanizeLowerCase2()}" + " dragon", (action, kind) =>
                            action != null
                            && action.Owner.HasTrait(Trait.Dragon)
                            && kind == DetermineDamageKind(resists),
                        (self.Level / 2 > 1 ? self.Level / 2 : 1) * 2, null
                    ))
            });
            self.AddQEffect(QEffect.DamageResistance(DetermineDamageKind(resists),
                self.Level / 2 > 1 ? self.Level / 2 : 1));
        });
    }

    private static void CreateMagicalDragonbloodLogic(TrueFeat magicBlood)
    {
        magicBlood.WithPrerequisite(
                sheet => !sheet.HasFeat(MFeatNames.Unknown) 
                         && sheet.AllFeats.Exists(feat => feat.HasTrait(MTraits.DraconicExemplar)
                                                          && sheet.AllFeats.FirstOrDefault(feat1 =>
                                                                    feat1.HasTrait(MTraits.DraconicExemplar))!
                                                                .HasTrait(DetermineTraitMagic(magicBlood))
                           )
                         && sheet.AllFeats.Count(ft => ft.HasTrait(MTraits.Lineage)) <= 1,
                "You must select a draconic exemplar from the same magical tradition.")
            .WithOnSheet(sheet => { sheet.TrainInThisOrSubstitute(DetermineSkill(magicBlood)); })
            .WithOnCreature(self =>
            {
                self.GetOrCreateSpellcastingSource(SpellcastingKind.Innate, MTraits.Dragonblood, Ability.Charisma,
                    DetermineTraitMagic(magicBlood)).WithSpells(
                    [DetermineSpellId(magicBlood)], 1);
            });
    }

    private static void CreateScalyHideLogic(TrueFeat scalyHide)
    {
        scalyHide.WithPrerequisite(sheet => !sheet.HasFeat(MFeatNames.DraconicAspect),
                "You cannot take this feat and Draconic Aspect.")
            .WithPermanentQEffect(null, selfQf =>
            {
                Item createdArmor = new Item(new ModdedIllustration("HTDAssets/Scale.png"), "Scaly Hide",
                        [Trait.Armor, Trait.UnarmoredDefense, Trait.Cloth])
                    .WithArmorProperties(new ArmorProperties(selfQf.Owner.Level < 5 ? 1 : 2, 3, 0, 0, 10));
                ReplicateArmorRunes(selfQf.Owner, createdArmor);
                if (!selfQf.Owner.Armor.WearsArmor)
                {
                    selfQf.Owner.AddQEffect(new QEffect("Scaly Hide",
                        "While you are unarmored, the scales give you an item bonus to ac.",
                        ExpirationCondition.Never, selfQf.Owner, new ModdedIllustration("HTDAssets/Scale.png"))
                    {
                        Id = MQEffectIds.ScalyHide,
                        ProvidesArmor = createdArmor,
                        DoNotShowUpOverhead = true,
                        Dismissable = false
                    });
                }

                if (selfQf.Owner.Armor.WearsArmor && selfQf.Owner.HasEffect(MQEffectIds.ScalyHide))
                {
                    QEffect? qEffect = selfQf.Owner.FindQEffect(MQEffectIds.ScalyHide);
                    qEffect?.ExpiresAt = ExpirationCondition.Immediately;
                }

                selfQf.StateCheckWithVisibleChanges = _ =>
                {
                    if (selfQf.Owner.FindQEffect(QEffectId.MageArmor) == null ||
                        !selfQf.Owner.HasEffect(MQEffectIds.ScalyHide) || createdArmor.Runes.Any(rune =>
                            rune.ItemName is ItemName.ArmorPotencyRunestone or ItemName.ArmorPotencyRunestone2
                                or ItemName.ArmorPotencyRunestone3)) return Task.CompletedTask;
                    Item rune1 = Items.GetItemTemplate(ItemName.ArmorPotencyRunestone);
                    rune1.RuneProperties?.ApplyRuneOntoItem(rune1, createdArmor);
                    return Task.CompletedTask;
                };
            });
    }
    #endregion
    #region Level 5
    private static void CreateTraditionalResistancesLogic(TrueFeat traditionalResistances)
    {
        traditionalResistances.WithPrerequisite(
                values => values.AllFeats.Exists(ft => ft.HasTrait(MTraits.MagicDragonblood)),
                "You must have Arcane Dragonblood, Divine Dragonblood, Occult Dragonblood, or Primal Dragonblood.")
            .WithOnCreature((sheet, self) =>
            {
                self.AddQEffect(new QEffect("Traditional Resistances",
                    "You gain a +1 status bonus to AC and saves against spells and other magical effects from the same tradition as your lineage. This bonus increases to +2 against sleep and paralysis effects.")
                {
                    BonusToDefenses = (_, action, _) =>
                    {
                        Feat? feat = sheet.AllFeats.FirstOrDefault(ft => ft.HasTrait(MTraits.MagicDragonblood));
                        if (action != null
                            && feat != null
                            && action.HasTrait(DetermineTraitMagic(feat))
                            && (action.CountsAsMagical || action.SpellInformation != null))
                        {
                            return new Bonus(action.HasTrait(Trait.Sleep) || action.SpellId is SpellId.Paralyze ? 2 : 1,
                                BonusType.Status, "Traditional Resistances");
                        }

                        if (action != null
                            && action.Name.Contains("Despair: Paralysis")
                            && feat != null
                            && feat.HasTrait(Trait.Divine))
                            return new Bonus(2, BonusType.Status, "Traditional Resistances");
                        return null;
                    }
                });
            });
    }

    private static void CreateDragonsFlightLogic(TrueFeat dragonsFlight)
    {
        dragonsFlight.WithActionCost(1)
            .WithOnCreature(self =>
                {
                    self.AddQEffect(new QEffect("Dragon's Flight {icon:Action}",
                            "You can fly up to 20 feet, you must end this movement on solid ground.")
                        {
                            ProvideMainAction = _ =>
                            {
                                ActionPossibility dragonFly = new(new CombatAction(self,
                                        IllustrationName.Fly, "Dragon's Flight", [Trait.Move, Trait.Basic],
                                        "You can fly up to 20 feet, you must end this movement on solid ground.",
                                        Target.Self()
                                    ).WithActionCost(1)
                                    .WithEffectOnChosenTargets(async (action, innerSelf, _) =>
                                        {
                                            QEffect littleFly = QEffect.Flying()
                                                .WithExpirationNever();
                                            littleFly.BonusToAllSpeeds = _ =>
                                                new Bonus(4, BonusType.Untyped, "Dragon's Flight");
                                            innerSelf.AddQEffect(littleFly);
                                            // Get a floodfill for movement using striding, after making the user flying
                                            List<Option> tileOptions =
                                            [
                                                new CancelOption(true)
                                            ];
                                            CombatAction? moveAction = Possibilities.Create(self)
                                                .Filter(ap =>
                                                {
                                                    if (ap.CombatAction.ActionId != ActionId.Stride)
                                                        return false;
                                                    ap.CombatAction.ActionCost = 0;
                                                    ap.RecalculateUsability();
                                                    return true;
                                                }).CreateActions(true).FirstOrDefault(pw =>
                                                    pw.Action.ActionId == ActionId.Stride) as CombatAction;
                                            IList<Tile> floodFill = Pathfinding.Floodfill(innerSelf, innerSelf.Battle,
                                                    new PathfindingDescription()
                                                    {
                                                        Squares = 4,
                                                        Style = { MaximumSquares = 4 }
                                                    })
                                                .Where(tile =>
                                                    tile.LooksFreeTo(innerSelf)
                                                    && tile.Kind != TileKind.Chasm
                                                    && tile.Kind != TileKind.Water
                                                    && tile.Kind != TileKind.Lava)
                                                .ToList();
                                            floodFill.ForEach(tile =>
                                            {
                                                if (moveAction == null ||
                                                    !(bool)moveAction.Target.CanBeginToUse(innerSelf)) return;
                                                tileOptions.Add(moveAction.CreateUseOptionOn(tile)
                                                    .WithIllustration(moveAction.Illustration));
                                            });
                                            // Pick a tile to fly to
                                            Option chosenTile = (await innerSelf.Battle.SendRequest(
                                                new AdvancedRequest(innerSelf,
                                                    "Choose where to Fly to or right-click to cancel. You must end your movement on solid ground.",
                                                    tileOptions)
                                                {
                                                    IsMainTurn = false,
                                                    IsStandardMovementRequest = true,
                                                    TopBarIcon = IllustrationName.Fly,
                                                    TopBarText =
                                                        "Choose where to Fly to or right-click to cancel. You must end your movement on solid ground.",
                                                })).ChosenOption;
                                            switch (chosenTile)
                                            {
                                                case CancelOption:
                                                    action.RevertRequested = true;
                                                    innerSelf.RemoveAllQEffects(qf => qf == littleFly);
                                                    break;
                                                case TileOption tOpt:
                                                    // Perform fly
                                                    await tOpt.Action();
                                                    innerSelf.RemoveAllQEffects(qf => qf == littleFly);
                                                    break;
                                            }
                                        }
                                    ));
                                return self.HasFeat(MFeatNames.TrueDragonsFlight) ? null : dragonFly;
                            }
                        }
                    );
                }
            );
    }

    private static void CreateDraconicScentLogic(TrueFeat draconicScent)
    {
        draconicScent.WithPermanentQEffect(qf =>
            {
                qf.StateCheck = qfThis =>
                {
                    qfThis.Owner.Battle.AllCreatures.Where(cr => cr.DistanceTo(qfThis.Owner) <= 6)
                        .ForEach(cr => cr.DetectionStatus.Undetected = false);
                };
            }
        );
    }

    private static void CreateBloodAndSpiritLogic(Feat bloodSpirit)
    {
        bloodSpirit.WithPermanentQEffect("You can bleed yourself to deal additional spirit damage to holy and unholy foes.", qf =>
        {
            qf.ProvideMainAction = effect =>
            {
                CombatAction drawBloodHoly = new CombatAction(effect.Owner,IllustrationName.Good, "Holy",
                        [Trait.Basic, Trait.Manipulate],
                        "If you are not taking persistent bleed damage, take 1d6 slashing damage (this damage cannot be resisted in any way). Until the end of your turn, your Strikes with a weapon that deals piercing or slashing damage deal an additional 1d6 spirit damage with the holy trait to creatures with the unholy trait.",
                        Target.Self().WithAdditionalRestriction(self => self.HeldItems.Any(w => w.WeaponProperties is not null && w.DetermineDamageKinds().Any(kind => kind is DamageKind.Slashing or DamageKind.Piercing)) ? null : "Must be wielding a piercing or slashing weapon."))
                    .WithActionCost(1)
                    .WithSoundEffect(SfxName.SwordStrike)
                    .WithEffectOnChosenTargets(async (action, self, _) =>
                    {
                        if (!self.QEffects.Any(qff =>qff.Key != null && qff.Key.ContainsIgnoreCase("Persistent") && qff.GetPersistentDamageKind() == DamageKind.Bleed))
                        {
                            QEffect ignoreResists = new()
                            {
                                IgnoreAmountOfResistanceAgainstYourActions = (_, combatAction, _, _, _) => combatAction == action ? int.MaxValue : 0
                            };
                            self.AddQEffect(ignoreResists);
                            await CommonSpellEffects.DealDirectDamage(action,
                                DiceFormula.FromText("1d6", "Blood and Spirit"), self, CheckResult.Success,
                                DamageKind.Slashing);
                            ignoreResists.ExpiresAt = ExpirationCondition.Immediately;
                        }
                        List<Item> weapons = self.HeldItems.Where(wpn => wpn.DetermineDamageKinds().Any(kind => kind is DamageKind.Slashing or DamageKind.Piercing)).ToList();
                        Item weapon = weapons[0];
                        QEffect blood = new("Blood and Spirit",$"Your Strikes with {weapon.Name} deal an additional 1d6 spirit damage with the holy trait to creatures with the unholy trait.", ExpirationCondition.ExpiresAtEndOfYourTurn, self,
                            action.Illustration)
                        {
                            AddExtraKindedDamageOnStrike = (strike, target) =>
                            {
                                if (strike.Item == null || strike.Item != weapon ||
                                    !target.HasTrait(UnholyTrait.Unholy))
                                    return null;
                                strike.Traits.Add(HolyTrait.Holy);
                                return new KindedDamage(DiceFormula.FromText("1d6", "Blood and Spirit"),
                                    DamageSpirit.Spirit);
                            },
                            Key = nameof(blood)
                        };
                        self.AddQEffect(blood);
                    });
                CombatAction drawBloodUnholy = new CombatAction(effect.Owner,IllustrationName.Evil, "Unholy",
                        [Trait.Basic, Trait.Manipulate],
                        "If you are not taking persistent bleed damage, take 1d6 slashing damage (this damage cannot be resisted in any way). Until the end of your turn, your Strikes with a weapon that deals piercing or slashing damage deal an additional 1d6 spirit damage with the unholy trait to creatures with the holy trait.",
                        Target.Self().WithAdditionalRestriction(self => self.HeldItems.Any(w => w.WeaponProperties is not null && w.DetermineDamageKinds().Any(kind => kind is DamageKind.Slashing or DamageKind.Piercing)) ? null : "Must be wielding a piercing or slashing weapon."))
                    .WithActionCost(1)
                    .WithSoundEffect(SfxName.SwordStrike)
                    .WithEffectOnChosenTargets(async (action, self, _) =>
                    {
                        if (!self.QEffects.Any(qff =>qff.Key != null && qff.Key.ContainsIgnoreCase("Persistent") && qff.GetPersistentDamageKind() == DamageKind.Bleed))
                        {
                            QEffect ignoreResists = new()
                            {
                                IgnoreAmountOfResistanceAgainstYourActions = (_, combatAction, _, _, _) => combatAction == action ? int.MaxValue : 0
                            };
                            self.AddQEffect(ignoreResists);
                            await CommonSpellEffects.DealDirectDamage(action,
                                DiceFormula.FromText("1d6", "Blood and Spirit"), self, CheckResult.Success,
                                DamageKind.Slashing);
                            ignoreResists.ExpiresAt = ExpirationCondition.Immediately;
                        }
                        List<Item> weapons = self.HeldItems.Where(wpn => wpn.DetermineDamageKinds().Any(kind => kind is DamageKind.Slashing or DamageKind.Piercing)).ToList();
                        Item weapon = weapons[0];
                        QEffect blood = new("Blood and Spirit", $"Your Strikes with {weapon.Name} deal an additional 1d6 spirit damage with the unholy trait to creatures with the holy trait.", ExpirationCondition.ExpiresAtEndOfYourTurn, self,
                            action.Illustration)
                        {
                            AddExtraKindedDamageOnStrike = (strike, target) =>
                            {
                                if (strike.Item == null || strike.Item != weapon ||
                                    !target.HasTrait(HolyTrait.Holy))
                                    return null;
                                strike.Traits.Add(UnholyTrait.Unholy);
                                return new KindedDamage(DiceFormula.FromText("1d6", "Blood and Spirit"),
                                    DamageSpirit.Spirit);
                            },
                            Key = nameof(blood)
                        };
                        self.AddQEffect(blood);
                    });
                SubmenuPossibility subMenu =
                    new(MIllustrations.CreateIllustration("BloodSpirit"), "Blood and Spirit")
                    {
                        Subsections = [new PossibilitySection("Blood and Spirit") { Possibilities = [new ActionPossibility(drawBloodHoly), new ActionPossibility(drawBloodUnholy)] }]
                    };
                return subMenu;
            };

        });
    }

    #endregion
    #region Level 9

    private static void CreateTrueFlightLogic(Feat trueFlight)
    {
        trueFlight.WithPermanentQEffect("You have flying at all times.", qf =>
        {
            
            Creature self = qf.Owner;
            qf.StateCheck = _ => self.AddQEffect(QEffect.Flying().WithExpirationEphemeral().WithIllustration(IllustrationName.Fly));
            // QEffect flight = new()
            // {
            //     StateCheckWithVisibleChanges = async _ =>
            //     {
            //         int speed = SetSpeed(self.Speed);
            //         self.AddQEffect(QEffect.Flying().WithExpirationEphemeral());
            //         self.AddQEffect(new QEffect(ExpirationCondition.Ephemeral)
            //         {
            //             BonusToAllSpeeds = _ => new Bonus(4 - speed, BonusType.Untyped, "True Dragon's Flight")
            //         });
            //     },
            //     Illustration = IllustrationName.Fly,
            //     Description = "You have a fly speed of 20 feet.",
            //     Id = MQEffectIds.Flight,
            //     Name = "Flying",
            // };
            // qf.ProvideContextualAction = _ =>
            // {
            //     if (self.HasEffect(MQEffectIds.Flight))
            //         return new ActionPossibility(new CombatAction(self, IllustrationName.Fly, "Land",
            //                 [Trait.Basic, Trait.DoesNotBreakStealth],
            //                 "You cease flying and return to land, regaining your land speed.",
            //                 Target.Self().WithAdditionalRestriction(cr =>
            //                     cr.Occupies.IsSolidGround ? null : "You must land on solid ground."))
            //             .WithActionCost(0)
            //             .WithEffectOnSelf(cr => cr.RemoveAllQEffects(effect => effect.Id == MQEffectIds.Flight)));
            //     return new ActionPossibility(new CombatAction(self, IllustrationName.Fly, "Begin Flight",
            //             [Trait.Basic, Trait.DoesNotBreakStealth],
            //             "You take flight, you begin flying and your speed becomes 20 foot.", Target.Self())
            //         .WithActionCost(0).WithEffectOnSelf(cr => cr.AddQEffect(flight)));
            // };
        });
    }

    private static void CreateWingBuffetLogic(Feat wingBuffet)
    {
        wingBuffet.WithPermanentQEffect(null, qf =>
        {
            Creature self = qf.Owner;
            qf.ProvideActionIntoPossibilitySection = (_, section) =>
            {
                if (section.PossibilitySectionId == PossibilitySectionId.AttackManeuvers)
                {
                    CombatAction wing = new(self, IllustrationName.FiendishWings, "Wing Buffet",
                        [
                            Trait.Attack, Trait.Basic, Trait.AttackDoesNotTargetAC,
                            Trait.AttackDoesNotIncreaseMultipleAttackPenalty
                        ],
                        "Choose up to two creatures adjacent to you. Attempt an Athletics check and compare it to the Fortitude DC of each target. This counts as two attacks for your multiple attack penalty, but the penalty doesn't increase until after both attacks."
                        + S.FourDegreesOfSuccess(
                            "The target takes bludgeoning damage equal to double your level and is pushed up to 10 feet away from you.",
                            "The target takes bludgeoning damage equal to your level and is pushed up to 5 feet away from you.",
                            "The target takes bludgeoning damage equal to half your level.",
                            "You fall prone at the end of this activity."),
                        Target.MultipleCreatureTargets(Target.AdjacentCreature(), Target.AdjacentCreature())
                            .WithMinimumTargets(1).WithMustBeDistinct());
                    wing.WithActionCost(2).WithActionId(ActionIds.WingBuffet);
                    wing.WithEffectOnChosenTargets(async (action, _, targets) =>
                    {
                        int roll = R.NextD20();
                        foreach (Creature target in targets.ChosenCreatures)
                        {
                            CheckBreakdown breakdown = CombatActionExecution.BreakdownAttack(
                                new CombatAction(self, null!, "Wings",
                                        [Trait.Basic, Trait.Attack, Trait.AttackDoesNotIncreaseMultipleAttackPenalty],
                                        "", Target.Self())
                                    .WithActionId(ActionIds.WingBuffet)
                                    .WithActiveRollSpecification(new ActiveRollSpecification(
                                        TaggedChecks.SkillCheck(Skill.Athletics),
                                        TaggedChecks.DefenseDC(Defense.Fortitude))), target);
                            CheckBreakdownResult breakdownResult = new(breakdown, roll);
                            string str1 = breakdown.DescribeWithFinalRollTotal(breakdownResult);
                            string str2 = "";
                            switch (breakdownResult.CheckResult)
                            {
                                case CheckResult.CriticalSuccess:
                                    await CommonSpellEffects.DealDirectDamage(action,
                                        DiceFormula.FromText((self.Level * 2).ToString()), target,
                                        CheckResult.CriticalSuccess, DamageKind.Bludgeoning);
                                    await self.PushCreature(target, 2);
                                    str2 = "{b}{Green}Critical Success{/}{/b} vs " + target.Name;
                                    break;
                                case CheckResult.Success:
                                    await CommonSpellEffects.DealDirectDamage(action,
                                        DiceFormula.FromText(self.Level.ToString()), target, CheckResult.Success,
                                        DamageKind.Bludgeoning);
                                    await self.PushCreature(target, 1);
                                    str2 = "{Green}Success{/} vs " + target.Name;
                                    break;
                                case CheckResult.Failure:
                                    await CommonSpellEffects.DealDirectDamage(action,
                                        DiceFormula.FromText(self.Level.ToString()), target, CheckResult.Success,
                                        DamageKind.Bludgeoning);
                                    str2 = "{Red}Failure{/} vs " + target.Name;
                                    break;
                                case CheckResult.CriticalFailure:
                                    self.AddQEffect(new QEffect(ExpirationCondition.EphemeralAtEndOfImmediateAction)
                                        { WhenExpires = _ => self.AddQEffect(QEffect.Prone()) });
                                    str2 = "{b}{Red}Critical Failure{/}{/b} vs " + target.Name;
                                    break;
                            }

                            var lime = Microsoft.Xna.Framework.Color.Lime;
                            var red = Microsoft.Xna.Framework.Color.Red;
                            DefaultInterpolatedStringHandler interpolatedStringHandler = new(10, 3);
                            interpolatedStringHandler.AppendLiteral(" (");
                            ref DefaultInterpolatedStringHandler local =
                                ref interpolatedStringHandler;
                            var d20Roll = breakdownResult.D20Roll;
                            string str4 = d20Roll + breakdown.TotalCheckBonus.WithPlus();
                            local.AppendFormatted(str4);
                            interpolatedStringHandler.AppendLiteral("=");
                            interpolatedStringHandler.AppendFormatted(breakdownResult.D20Roll +
                                                                      breakdown.TotalCheckBonus);
                            interpolatedStringHandler.AppendLiteral(" vs. ");
                            interpolatedStringHandler.AppendFormatted(breakdown.TotalDC);
                            interpolatedStringHandler.AppendLiteral(").");
                            string stringAndClear = interpolatedStringHandler.ToStringAndClear();
                            string log = $"{str2}{stringAndClear}";
                            string logDetails = str1;
                            target.Overhead(breakdownResult.CheckResult.HumanizeTitleCase2(),
                                breakdownResult.CheckResult >= CheckResult.Success ? lime : red, log, "Wing Buffet",
                                logDetails);
                        }

                        ++self.Actions.AttackedThisManyTimesThisTurn;
                        ++self.Actions.AttackedThisManyTimesThisTurn;
                    });
                    return new ActionPossibility(wing);
                }

                return null;
            };
        });
    }

    private static void CreateShelteringWingsLogic(Feat shelteringWings)
    {
        shelteringWings.WithPermanentQEffect("You gain a +1 circumstance bonus to AC and to saving throws against spells that target you until the start of your next turn. If you have True Dragon's Flight, this bonus increases to +2.", qf =>
        {
            qf.ProvideMainAction = effect =>
            {
                CombatAction shelter = new CombatAction(effect.Owner, IllustrationName.AngelicWings,
                        "Sheltering Wings",
                        [Trait.Basic, MTraits.Dragonblood], $"You gain a +{(effect.Owner.HasFeat(MFeatNames.TrueDragonsFlight) ? 2 : 1)} circumstance bonus to AC and to saving throws against spells that target you until the start of your next turn.",
                        Target.Self())
                    .WithActionCost(1)
                    .WithEffectOnChosenTargets(async (spell, caster, _) =>
                    {
                        QEffect shelter = new("Sheltering Wings", spell.Description,
                            ExpirationCondition.ExpiresAtStartOfYourTurn, caster, spell.Illustration)
                        {
                            BonusToDefenses = (qEffect, action, defense) => action != null && action.Targets(qEffect.Owner) &&
                                                                            defense.IsSavingThrow() ||
                                                                             defense == Defense.AC ? new Bonus(qEffect.Owner.HasFeat(MFeatNames.TrueDragonsFlight) ? 2 : 1, BonusType.Circumstance, "Sheltering Wings", true) : null

                        };
                        caster.AddQEffect(shelter);
                    });
                return new ActionPossibility(shelter).WithPossibilityGroup("Abilities");
            };
            qf.WithName("Sheltering Wings {icon:Action}");
            qf.Description = $"You gain a +{(qf.Owner.HasFeat(MFeatNames.TrueDragonsFlight) ? 2 : 1)} circumstance bonus to AC and to saving throws against spells that target you until the start of your next turn.";
        });
    }

    #endregion
    #region Level 13
    private static void CreateMajesticPresenceLogic(Feat presence)
    {
        presence.WithPermanentQEffect("Frighten all enemies in a 20- foot emanation dependent on a Will save.",
            qf =>
            {
                qf.ProvideMainAction = effect =>
                {
                    Creature self = effect.Owner;
                    CombatAction majesty = new CombatAction(self, IllustrationName.Fear, "Majestic Presence",
                            [MTraits.Dragonblood, Trait.Emotion, Trait.Fear, Trait.Mental, Trait.Visual],
                            "Each enemy in a 20- foot emanation must attempt a Will save against the higher of your class DC or spell DC. Regardless of the result of the saving throw, the creature is temporarily immune to your Majestic Presence for the rest of the encounter." +
                            S.FourDegreesOfSuccess("The creature is unaffected.", "The creature is frightened 1.",
                                "The creature is frightened 2.", "The creature is frightened 4."),
                            Target.Emanation(4).WithIncludeOnlyIf((at, creature) =>
                                (creature.FindQEffect(MQEffectIds.Majestic) is not { } majestic ||
                                 majestic.Source != self) && !creature.FriendOf(at.OwnerAction.Owner))
                                .WithAdditionalRequirementOnCaster(caster => caster.Battle.AllCreatures.Any(cr => cr.DistanceTo(caster) <= 4 && cr.EnemyOf(caster) && cr.FindQEffect(MQEffectIds.Majestic)?.Source != caster) ? Usability.Usable : Usability.NotUsable("No creature who can be affected by Majestic Presence.")))
                        .WithActionCost(1)
                        .WithSavingThrow(new SavingThrow(Defense.Will, self.ClassOrSpellDC()))
                        .WithEffectOnEachTarget(async (_, caster, target, result) =>
                        {
                            switch (result)
                            {
                                case CheckResult.CriticalSuccess:
                                    break;
                                case CheckResult.CriticalFailure:
                                    target.AddQEffect(QEffect.Frightened(4));
                                    break;
                                case CheckResult.Failure:
                                    target.AddQEffect(QEffect.Frightened(2));
                                    break;
                                case CheckResult.Success:
                                    target.AddQEffect(QEffect.Frightened(1));
                                    break;
                            }
                            target.AddQEffect(new QEffect { Id = MQEffectIds.Majestic, Source = caster});
                        });
                    return new ActionPossibility(majesty).WithPossibilityGroup("Abilities");
                };
            });
    }

    private static void CreateDraconicVeilLogic(Feat veil)
    {
        veil.WithOnCreature((values, self) =>
        {
            self.GetOrCreateSpellcastingSource(SpellcastingKind.Innate, MTraits.Dragonblood, Ability.Charisma,
                DetermineTraitMagic(values.AllFeats.FirstOrDefault(ft => ft.HasTrait(MTraits.DraconicExemplar)))).WithSpells(
                [SpellId.Enlarge], 2);
        })
        .WithPermanentQEffect(null, qf =>
        {
            qf.StartOfCombatReaction = effect =>
            {
                Creature self = effect.Owner;
                if (self.Spellcasting?.Sources.FirstOrDefault(source => source.ClassOfOrigin == MTraits.Dragonblood)?.Spells.FirstOrDefault(sp => sp.SpellId == SpellId.Enlarge) is not {} enlarge
                    || !self.Spellcasting.CanCastReactiveSpell(enlarge.WithActionCost(0)))
                {
                    return null;
                }
                enlarge.WithActionCost(0);
                ReactionOption reaction = ReactionOption.CreateFromSpellAsAReaction(enlarge, "Do you want to cast {i}enlarge{/i} as a free action?",
                    async () =>
                    {
                        self.Spellcasting.UseUpSpellcastingResources(enlarge);
                        await enlarge.EffectOnOneTarget?.Invoke(enlarge, self, self, CheckResult.Success)!;
                    }).WithIsFreeAction();
                reaction.Caption = "Draconic Veil";
                return reaction;
            };
            qf.ModifyActionPossibility = (_, spell) =>
            {
                if (spell.SpellId != SpellId.Enlarge || spell.SpellcastingSource?.ClassOfOrigin != MTraits.Dragonblood)
                    return;
                spell.Target = Target.Self();
            };
            qf.Innate = false;
        });
        
    }
    #endregion
    //Aspect Feats
    #region Aspect Feats

    private static IEnumerable<Feat> DraconicAspectFeats()
    {
        Feat draconicClaws = new(ModManager.RegisterFeatName("Dragonblood.AspectClaws", "Draconic Aspect Claws"),
            "You have sharp claws.",
            "The claws are in the brawling group and deal 1d4 slashing damage and are agile, finesse and unarmed. You need a free hand to use claws.",
            [MTraits.AspectWeapon], null);
        draconicClaws.WithOnCreature(self =>
        {
            Item claws = new Item(MIllustrations.DragonClaws, "claws",
                    Trait.Agile, Trait.Finesse, Trait.Unarmed, Trait.Brawling)
                .WithWeaponProperties(new WeaponProperties("1d4", DamageKind.Slashing));
            if (self.HasFeat(MFeatNames.DeadlyAspect))
                claws.Traits.Add(Trait.DeadlyD8);
            if (self.HasFreeHand)
                self.WithAdditionalUnarmedStrike(claws);
        });
        yield return draconicClaws;
        Feat draconicJaw = new(ModManager.RegisterFeatName("Dragonblood.AspectJaw", "Draconic Aspect Jaw"),
            "You have sharp teeth in a powerful jaw.",
            "Your jaw is in the brawling group and deals 1d6 piercing damage and is forceful and unarmed.",
            [MTraits.AspectWeapon], null);
        draconicJaw.WithOnCreature(self =>
        {
            Item jaw = CommonItems.CreateNaturalWeapon(IllustrationName.Jaws, "jaw", "1d6", DamageKind.Piercing,
                Trait.Forceful, Trait.Unarmed, Trait.Brawling);
            if (self.HasFeat(MFeatNames.DeadlyAspect))
                jaw.Traits.Add(Trait.DeadlyD8);
            self.WithAdditionalUnarmedStrike(jaw);
        });
        yield return draconicJaw;
        Feat draconicTail = new(ModManager.RegisterFeatName("Dragonblood.AspectTail", "Draconic Aspect Tail"),
            "You have a strong reptilian tail.",
            "Your tail is in the brawling group and deals 1d6 bludgeoning damage and is forceful and unarmed.",
            [MTraits.AspectWeapon], null);
        draconicTail.WithOnCreature(self =>
        {
            Item tail = new Item(MIllustrations.DragonTail, "tail",
                    Trait.Sweep, Trait.Trip, Trait.Unarmed, Trait.Brawling)
                .WithWeaponProperties(new WeaponProperties("1d6", DamageKind.Bludgeoning));
            if (self.HasFeat(MFeatNames.DeadlyAspect))
                tail.Traits.Add(Trait.DeadlyD8);
            self.WithAdditionalUnarmedStrike(tail);
        });
        yield return draconicTail;
    }

    private static List<Feat> AspectFeats(IEnumerable<Feat> feats)
    {
        List<Feat> aspectFeats = [];
        foreach (Feat feat in feats)
        {
            aspectFeats.Add(feat);
        }

        return aspectFeats;
    }

    #endregion

    //Utility functions
    private static DamageKind DetermineDamageKind(Feat feat)
    {
        if (feat.HasTrait(Trait.Fire))
            return DamageKind.Fire;
        if (feat.HasTrait(Trait.Cold))
            return DamageKind.Cold;
        if (feat.HasTrait(Trait.Electricity))
            return DamageKind.Electricity;
        if (feat.HasTrait(Trait.Force))
            return DamageKind.Force;
        if (feat.HasTrait(Trait.Mental))
            return DamageKind.Mental;
        if (feat.HasTrait(MTraits.Bludgeoning))
            return DamageKind.Bludgeoning;
        if (feat.HasTrait(Trait.Acid))
            return DamageKind.Acid;
        if (feat.HasTrait(SpiritTrait.Spirit))
            return DamageSpirit.Spirit;
        return feat.HasTrait(Trait.Poison) ? DamageKind.Poison : DamageKind.Untyped;
    }

    private static bool IsCone(Feat feat)
    {
        return !feat.HasTrait(MTraits.Line);
    }

    private static Defense WhichSave(Feat feat)
    {
        if (feat.HasTrait(Trait.Reflex))
            return Defense.Reflex;
        if (feat.HasTrait(Trait.Fortitude))
            return Defense.Fortitude;
        return feat.HasTrait(Trait.Will) ? Defense.Will : Defense.Reflex;
    }

    private static Trait DetermineTrait(Feat feat)
    {
        if (feat.HasTrait(Trait.Fire))
            return Trait.Fire;
        if (feat.HasTrait(Trait.Cold))
            return Trait.Cold;
        if (feat.HasTrait(Trait.Electricity))
            return Trait.Electricity;
        if (feat.HasTrait(Trait.Force))
            return Trait.Force;
        if (feat.HasTrait(Trait.Mental))
            return Trait.Mental;
        if (feat.HasTrait(MTraits.Bludgeoning))
            return MTraits.Bludgeoning;
        if (feat.HasTrait(Trait.Acid))
            return Trait.Acid;
        if (feat.HasTrait(SpiritTrait.Spirit))
            return SpiritTrait.Spirit;
        return feat.HasTrait(Trait.Poison) ? Trait.Poison : Trait.None;
    }

    private static Skill DetermineSkill(Feat feat)
    {
        if (feat.HasTrait(Trait.Arcane))
            return Skill.Arcana;
        if (feat.HasTrait(Trait.Primal))
            return Skill.Nature;
        return feat.HasTrait(Trait.Divine) ? Skill.Religion : Skill.Occultism;
    }

    private static Trait DetermineTraitMagic(TrueFeat feat)
    {
        if (feat.HasTrait(Trait.Arcane))
            return Trait.Arcane;
        if (feat.HasTrait(Trait.Primal))
            return Trait.Primal;
        return feat.HasTrait(Trait.Divine) ? Trait.Divine : Trait.Occult;
    }

    private static Trait DetermineTraitMagic(Feat? feat)
    {
        if (feat == null || feat.HasTrait(Trait.Arcane))
            return Trait.Arcane;
        if (feat.HasTrait(Trait.Primal))
            return Trait.Primal;
        return feat.HasTrait(Trait.Divine) ? Trait.Divine : Trait.Occult;
    }

    private static void ReplicateArmorRunes(Creature self, Item createdArmor)
    {
        Item? baseArmor = self.BaseArmor;
        if (baseArmor != null)
        {
            foreach (Item rune in baseArmor.Runes)
            {
                if (rune.RuneProperties != null &&
                    rune.RuneProperties.CanBeAppliedTo?.Invoke(rune, createdArmor) == null)
                    rune.RuneProperties.ApplyRuneOntoItem(rune, createdArmor);
            }
        }
    }

    private static void AlternateResistSelection(CalculatedCharacterSheetValues sheet)
    {
        sheet.AddSelectionOption(new SingleFeatSelectionOption("ResistanceChoice", "Choose a resistance",
            -1, feat1 => feat1.HasTrait(MTraits.Resists)));
    }

    private static SpellId DetermineSpellId(Feat feat)
    {
        if (feat.HasTrait(Trait.Arcane))
            return SpellId.Shield;
        if (feat.HasTrait(Trait.Divine))
            return SpellId.Guidance;
        return feat.HasTrait(Trait.Occult) ? SpellId.OpenDoor : SpellId.Tanglefoot;
    }

    private static int SetSpeed(int speed)
    {
        var ints = new List<int>();
        if (ints == null) throw new ArgumentNullException(nameof(ints));
        ints.Add(speed);
        return ints.FirstOrDefault();
    }

    public static QEffect WithIllustration(this QEffect effect, Illustration illustration)
    {
        effect.Illustration = illustration;
        return effect;
    }
}