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
using Dawnsbury.Core.Coroutines.Requests;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Creatures.Parts;
using Dawnsbury.Core.Intelligence;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core.Tiles;
using Dawnsbury.Display;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Display.Text;
using Dawnsbury.Modding;

namespace HereThereBeDragons;

public class DragonBlood
{
    public static IEnumerable<Feat> CreateDragonbloodFeats()
    {
        //DraconicExemplar feats
            Feat draconicExemplarAdamantine = new Feat(ModManager.RegisterFeatName("AdamantineExemplar", "Adamantine"),
                "The powerful adamantine dragons are one of several dragons known as skymetal dragons. Adamantine dragons are typically steadfast and loyal. Once they commit to a certain purpose, changing their minds is nigh impossible.", "The power of the adamantine dragon causes you to deal bludgeoning damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Primal, ModData.Traits.Bludgeoning, Trait.Reflex], null);
            yield return draconicExemplarAdamantine;
            Feat draconicExemplarConspirator = new Feat(ModManager.RegisterFeatName("ConspiratorExemplar", "Conspirator"),
                "Hidden among the shadows and upper echelons of society are the conspirator dragons. These dragons are schemers, always looking to manipulate and control others, either for personal gain or simply for the thrill of watching their machinations play out.", "The power of the conspirator dragon causes you to deal poison damage in a cone if you choose a breath weapon and is connected to the Occult tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Occult, Trait.Poison, Trait.Fortitude], null);
            yield return draconicExemplarConspirator;
            Feat draconicExemplarDiabolic = new Feat(ModManager.RegisterFeatName("DiabolicExemplar", "Diabolic"),
                "Some scholars argue diabolic dragons are just extensions of Hell, living creatures that break off from the plane to enact its will. Whether this is true or whether diabolical dragons are simply the reborn souls of dragons sent to Hell, the fact remains that these dragons are powerful, cunning, and tyrannical.", "The power of the diabolic dragon causes you to deal fire damage in a cone if you choose a breath weapon and is connected to the Divine tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Divine, Trait.Fire, Trait.Reflex], null);
            yield return draconicExemplarDiabolic;
            Feat draconicExemplarFortune = new Feat(ModManager.RegisterFeatName("FortuneExemplar", "Fortune"),
                "Fortune dragons have the innate ability to draw upon the raw magical energies that surround them. Fortune dragons are seekers of novel experiences. This desire for originality leads fortune dragons to approach visitors of other ancestries with curiosity, though this interest often proves short lived", "The power of the fortune dragon causes you to deal force damage in a cone if you choose a breath weapon and is connected to the Arcane tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Arcane, Trait.Force, Trait.Reflex], null);
            yield return draconicExemplarFortune;
            Feat draconicExemplarHorned = new Feat(ModManager.RegisterFeatName("HornedExemplar", "Horned"),
                "The magic that flows through primal dragons can manifest more animalistic or bestial features in a given type of dragon. Notably among these are the massive paired horns of the horned dragon. Horned dragons are generally contemplative and have a fixation on knowledge and self-discipline, traits belied by their bestial appearance.", "The power of the horned dragon causes you to deal poison damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Primal, Trait.Poison, Trait.Fortitude], null);
            yield return draconicExemplarHorned;
            Feat draconicExemplarMirage = new Feat(ModManager.RegisterFeatName("MirageExemplar", "Mirage"),
                "Mirage dragons are masters of illusion magic and use their powers to deceive others and further their own agendas. Mirage dragons are vain and egotistical figures. They ultimately care more about themselves than others.", "The power of the mirage dragon causes you to deal mental damage in a cone if you choose a breath weapon and is connected to the Arcane tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Arcane, Trait.Mental, Trait.Will], null);
            yield return draconicExemplarMirage;
            Feat draconicExemplarOmen = new Feat(ModManager.RegisterFeatName("OmenExemplar", "Omen"),
                "Omen dragons are bound to see the future—nebulous though it might be—at all times. Visions of the future hound them like a quiet song that never stops playing in their minds. Omen dragons have a natural compulsion to share the futures they see, but they have no compunctions about what the visions show and share their knowledge with the wicked as readily as the virtuous.", "The power of the omen dragon causes you to deal mental damage in a cone if you choose a breath weapon and is connected to the Occult tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Occult, Trait.Mental, Trait.Will], null);
            yield return draconicExemplarOmen;
            Feat draconicExemplarHeaven = new Feat(ModManager.RegisterFeatName("HeavenlyExemplar", "Heavenly"),
                "Heavenly dragons are protectors of the innocent and enemies of the wicked. Wise and with vast knowledge, they offer their advice to the worthy who come to them in their homes among the mountain peaks.", "The power of the heavenly dragon causes you to deal electricity damage in a line if you choose a breath weapon and is connected to the Divine tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Divine, Trait.Electricity, ModData.Traits.Line, Trait.Reflex, Trait.Homebrew], null);
            yield return draconicExemplarHeaven;
            Feat draconicExemplarBlizzard = new Feat(ModManager.RegisterFeatName("BlizzardExemplar", "Blizzard"),
                "On the peaks of icy mountains and in the eternal cold of the poles, where spring never comes, blizzard dragons dwell in the snow and frost. Blizzard dragons are seldom interested in the goings on of non-dragons, who they consider to be lesser creatures.", "The power of the blizzard dragon causes you to deal cold damage in a cone if you choose a breath weapon and is connected to the Primal tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Primal, Trait.Cold, Trait.Fortitude, Trait.Homebrew], null);
            yield return draconicExemplarBlizzard;
            Feat draconicExemplarDeep = new Feat(ModManager.RegisterFeatName("DeepExemplar", "Deep"),
                " Within the bowels of the earth, deep dragons await with endless patience. The longest lived of all wyrms, deep dragons wait and watch, laying century long plans from within lairs deep underground.", "The power of the deep dragon causes you to deal acid damage in a line if you choose a breath weapon and is connected to the Arcane tradition.",
                [ModData.Traits.DraconicExemplar, Trait.Arcane, Trait.Acid, Trait.Reflex, ModData.Traits.Line, Trait.Homebrew], null);
            yield return draconicExemplarDeep;
            Feat draconicExemplarUnknown = new Feat(ModData.FeatNames.Unknown,
                "Your draconic exemplar's nature hasn't revealed itself yet.", "You do not have to choose an exemplar at level 1, however some class features require you to choose an exemplar, if you wish to take one of those features, retrain to a different exemplar.",
                [ModData.Traits.DraconicExemplar, ModData.Traits.Unknown], null);
            yield return draconicExemplarUnknown;
            // level 1 ancestry feats
            TrueFeat breathOfTheDragon = new TrueFeat(ModManager.RegisterFeatName("BreathOfTheDragon", "Breath of the Dragon"),
                1,"You can unleash a powerful breath weapon like your draconic exemplar.", "Tapping into the physiology of your draconic ancestor, you can exhale a torrent of energy in a 15-foot cone or a 30-foot line, dealing 1d4 damage. Each creature in the area must attempt a basic saving throw against the higher of your class DC or spell DC. You can't use this ability again for 1d4 rounds.\n\nAt 3rd level and every 2 levels thereafter, the damage increases by 1d4. The shape of the breath, the damage type, and the saving throw match those of your draconic exemplar. This ability has the trait associated with the type of damage it deals.",
                [ModData.Traits.Dragonblood]);
            CreateBreathLogic(breathOfTheDragon);
            yield return breathOfTheDragon;
            TrueFeat draconicResistance = new TrueFeat(ModManager.RegisterFeatName("DraconicResistance", "Draconic Resistance"),
                1,"Draconic magic safeguards you from harm.", "You gain resistance equal to half your level (minimum 1) to the damage type associated with your draconic exemplar. Double this resistance against damage of that type dealt to you by dragons. If your draconic exemplar is associated with bludgeoning, piercing, or slashing damage, instead of gaining resistance to that type you can choose acid, cold, fire, electricity, or sonic.",
                [ModData.Traits.Dragonblood]);
            CreateDraconicResistanceLogic(draconicResistance);
            yield return draconicResistance;
            TrueFeat draconicResistanceB = new TrueFeat(ModManager.RegisterFeatName("DraconicResistanceB", "Draconic Resistance - Adamantine"),
                1,"Draconic magic safeguards you from harm.", "You gain resistance equal to half your level (minimum 1) to the damage type associated with your draconic exemplar. Double this resistance against damage of that type dealt to you by dragons. If your draconic exemplar is associated with bludgeoning, piercing, or slashing damage, instead of gaining resistance to that type you can choose acid, cold, fire, electricity, or sonic.",
                [ModData.Traits.Dragonblood]);
            CreateDraconicResistChoice(draconicResistanceB);
            yield return draconicResistanceB;
            //damagekind choice feats
            Feat acidResist = new Feat(ModManager.RegisterFeatName("Acid", "Acid"), null, "You resist acid.",
                [Trait.Acid, ModData.Traits.Resists], null);
                CreateDraconicResistB(acidResist);
            yield return acidResist;
            Feat coldResist = new Feat(ModManager.RegisterFeatName("Cold", "Cold"), null, "You resist cold.",
                [Trait.Cold, ModData.Traits.Resists], null);
                CreateDraconicResistB(coldResist);
            yield return coldResist;
            Feat fireResist = new Feat(ModManager.RegisterFeatName("Fire", "Fire"), null, "You resist fire.",
                [Trait.Fire, ModData.Traits.Resists], null);
                CreateDraconicResistB(fireResist);
            yield return fireResist;
            Feat electricityResist = new Feat(ModManager.RegisterFeatName("Electricity", "Electricity"), null,
                "You resist electricity.", [Trait.Electricity, ModData.Traits.Resists], null);
                CreateDraconicResistB(electricityResist);
            yield return electricityResist;
            Feat sonicResist = new Feat(ModManager.RegisterFeatName("Sonic", "Sonic"), null, "You resist sonic.",
                [Trait.Sonic, ModData.Traits.Resists], null);
                CreateDraconicResistB(sonicResist);
            yield return sonicResist;
            // level 1 ancestry feats continued
            TrueFeat arcaneDragonBlood = new TrueFeat(ModManager.RegisterFeatName("ArcaneDragonblood", "Arcane Dragonblood"),
                1,"You descend from a dragon that wields mastery of their magical abilities, such as a fortune dragon or mirage dragon. As such, you can instinctively grasp the intricacies of magic.", 
                "You gain the trained proficiency rank in Arcana. If you would automatically become trained in Arcana (from your background or class, for example), you instead become trained in a skill of your choice. You can cast shield as an arcane innate spell at will",
                [ModData.Traits.Dragonblood, Trait.Arcane, ModData.Traits.MagicDragonblood]);
            CreateMagicalDragonbloodLogic(arcaneDragonBlood);
            yield return arcaneDragonBlood;
            TrueFeat divineDragonBlood = new TrueFeat(ModManager.RegisterFeatName("DivineDragonblood", "Divine Dragonblood"),
                1,"You can trace your lineage to a dragon with almost deific powers, such as a diabolic dragon or heaven dragon.", 
                "You gain the trained proficiency rank in Religion. If you would automatically become trained in Religion (from your background or class, for example), you instead become trained in a skill of your choice. You can cast guidance as a divine innate spell at will",
                [ModData.Traits.Dragonblood, Trait.Divine, ModData.Traits.MagicDragonblood]);
            CreateMagicalDragonbloodLogic(divineDragonBlood);
            yield return divineDragonBlood;
            TrueFeat occultDragonBlood = new TrueFeat(ModManager.RegisterFeatName("OccultDragonblood", "Occult Dragonblood"),
                1,"Your blood contains a tiny fragment of unusual or inexplicable power from a mysterious dragon, such as a conspirator dragon or omen dragon.", 
                "You gain the trained proficiency rank in Occultism. If you would automatically become trained in Occultism (from your background or class, for example), you instead become trained in a skill of your choice. You can cast open door as an occult innate spell at will",
                [ModData.Traits.Dragonblood, Trait.Occult, ModData.Traits.MagicDragonblood]);
            CreateMagicalDragonbloodLogic(occultDragonBlood);
            yield return occultDragonBlood;
            TrueFeat primalDragonBlood = new TrueFeat(ModManager.RegisterFeatName("PrimalDragonblood", "Primal Dragonblood"),
                1,"A dragon with a deep connection to the natural world, such as an adamantine dragon or a horned dragon, resides somewhere on your family tree.", 
                "You gain the trained proficiency rank in Nature. If you would automatically become trained in Nature (from your background or class, for example), you instead become trained in a skill of your choice. You can cast tanglefoot as a primal innate spell at will",
                [ModData.Traits.Dragonblood, Trait.Primal, ModData.Traits.MagicDragonblood]);
            CreateMagicalDragonbloodLogic(primalDragonBlood);
            yield return primalDragonBlood;
            TrueFeat scalyHide = new TrueFeat(ModData.FeatNames.ScalyHide,
                1,"You were born with a layer of scales across your entire body that resemble those of your draconic progenitor.", "When you’re unarmored, the scales give you a +1 item bonus to AC with a Dexterity cap of +3. The item bonus to AC increases to +2 at 5th level. The item bonus to AC from these scales is cumulative with armor potency runes on your explorer's clothing, or the mystic armor spell." +
                "\n\n{b}Special{/b} You cannot take this feat and Draconic Aspect. You should take this feat at level 1 only.",
                [ModData.Traits.Dragonblood]);
            CreateScalyHideLogic(scalyHide);
            yield return scalyHide;
            TrueFeat draconicAspect = new TrueFeat(ModData.FeatNames.DraconicAspect,
                1,"You have an obvious draconic feature, such as sharp claws, a snout full of sharp teeth, or strong reptilian tail, that you can use offensively.", 
                "You gain your choice of one of the following unarmed attacks. The attack is in the brawling group and has the listed damage die and traits.\n\n    {b}•Claw{/b} 1d4 slashing (agile, finesse, unarmed)\n    {b}•Jaws{/b} 1d6 piercing (forceful, unarmed)\n    {b}•Tail{/b} 1d6 bludgeoning (sweep, trip, unarmed)" +
                "\n\n{b}Special{/b} You cannot take this feat and Scaly Hide. You should take this feat at level 1 only.",
                [ModData.Traits.Dragonblood], AspectFeats(DraconicAspectFeats()));
            draconicAspect.WithPrerequisite(sheet => !sheet.HasFeat(ModData.FeatNames.ScalyHide), "You cannot take this feat and Scaly Hide.");
            yield return draconicAspect;
            //Level 5 feats
            TrueFeat deadlyAspect = new TrueFeat(ModData.FeatNames.DeadlyAspect,
                5,"You have honed the unarmed attack your draconic heritage has granted you to a lethal degree.", 
                "The unarmed attack you gained from Draconic Aspect gains the deadly d8 trait.",
                [ModData.Traits.Dragonblood]);
            deadlyAspect.WithPrerequisite(ModData.FeatNames.DraconicAspect, "Draconic Aspect");
            yield return deadlyAspect;
            TrueFeat traditionalResists = new TrueFeat(ModManager.RegisterFeatName("TraditionalResists", "Traditional Resistances"),
                5,"Due to your blood, you have some resistance to certain types of magic.", 
                "You gain a +1 status bonus to AC and saves against spells and other magical effects from the same tradition as your lineage. This bonus increases to +2 against sleep and paralysis effects.",
                [ModData.Traits.Dragonblood]);
            CreateTraditionalResistancesLogic(traditionalResists);
            yield return traditionalResists;
            TrueFeat dragonsFlight = new TrueFeat(ModManager.RegisterFeatName("DragonsFlight", "Dragon's Flight"),
                5,"You have grown a small pair of draconic wings or have honed your use of the wings you've had since birth.", 
                "You Fly. If you don't normally have a fly Speed, you gain a fly Speed of 20 feet for this movement. You must end your movement on solid ground.",
                [ModData.Traits.Dragonblood]);
            CreateDragonsFlightLogic(dragonsFlight);
            yield return dragonsFlight;
            TrueFeat draconicScent = new TrueFeat(ModManager.RegisterFeatName("DraconicScent", "Draconic Scent"),
                5,"Your sense of smell has heightened to be as keen as that of a dragon.", 
                "Creatures within 30 feet cannot be undetected by you.",
                [ModData.Traits.Dragonblood]);
            CreateDraconicScentLogic(draconicScent);
            yield return draconicScent;
            
    }
    private static void CreateBreathLogic(TrueFeat breathWeapon)
    {
        breathWeapon.WithActionCost(2)
            .WithPrerequisite((sheet) => (!sheet.HasFeat(ModData.FeatNames.Unknown) && sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.DraconicExemplar)))), "You must select a draconic exemplar.")
            .WithOnCreature((sheet, creature) =>
    {
      Feat? feat = sheet.AllFeats.FirstOrDefault(ft => ft.HasTrait(ModData.Traits.DraconicExemplar) && ft.FeatName != ModData.FeatNames.Unknown);
      if (feat == null)
        return;
      creature.AddQEffect(new QEffect("Breath of the Dragon", "You have a breath weapon.")
      {
        ProvideMainAction = (Func<QEffect, Possibility>) (qfSelf =>
        {
          Creature owner = qfSelf.Owner;
          int dc = owner.ClassOrSpellDC();
          return new ActionPossibility(new CombatAction(owner, IllustrationName.BreathWeapon, "Breath weapon", [Trait.Basic, DetermineTrait(feat)], 
              $"{{b}}Area{{/b}} {(IsCone(feat) ? "15-foot cone" : "30-foot line")}\n{{b}}Saving throw{{/b}} basic Reflex\n\nDeal {S.HeightenedVariable((owner.Level + 1) / 2, 1)}d4 {DetermineDamageKind(feat).HumanizeTitleCase2().ToLower()} damage (basic DC {dc.ToString()} {WhichSave(feat).HumanizeTitleCase2().ToLower()} save mitigates).\n\nThen you can't use Breath weapon again for 1d4 rounds.", 
              IsCone(feat) ? Target.Cone(3) : Target.Line(6)).WithActionCost(2).WithProjectileCone(IllustrationName.BreathWeapon, 15, ProjectileKind.Cone).WithSoundEffect(SfxName.FireRay).WithSavingThrow(new SavingThrow(WhichSave(feat), dc)).WithEffectOnEachTarget( 
              (async (spell, caster, target, result) => await CommonSpellEffects.DealBasicDamage(spell, caster, target, result, ((caster.Level + 1) / 2).ToString() + "d4", DetermineDamageKind(feat)))).WithEffectOnChosenTargets((async (_, caster, _) => caster.AddQEffect(QEffect.CannotUseForXRound("Breath Weapon", caster, R.Next(2, 5))))))
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
                (sheet) => (!sheet.HasFeat(ModData.FeatNames.Unknown)
                            && !sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.Bludgeoning)))
                            && sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.DraconicExemplar)))),
                "You must select a draconic exemplar other than adamantine.")
            .WithOnCreature((sheet, self) =>
            {
                Feat? feat = sheet.AllFeats.FirstOrDefault(ft =>
                    ft.HasTrait(ModData.Traits.DraconicExemplar) && ft.FeatName != ModData.FeatNames.Unknown);
                if (feat != null && !feat.HasTrait(ModData.Traits.Bludgeoning))
                {
                    self.AddQEffect(new QEffect()
                    {
                        StateCheck = innerSelf =>
                            innerSelf.Owner.WeaknessAndResistance.Resistances.Add(new SpecialResistance($"{DetermineDamageKind(feat).HumanizeTitleCase2()}"+" Dragon", (action, kind) => action != null
                                    && action.Owner.HasTrait(Trait.Dragon)
                                    && kind == DetermineDamageKind(feat),
                                (self.Level / 2 > 1 ? self.Level / 2 : 1)*2,null
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
                (sheet) => (!sheet.HasFeat(ModData.FeatNames.Unknown) 
                            && sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.Bludgeoning))) 
                            && sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.DraconicExemplar)))),
                "You must select the adamantine draconic exemplar.")
            .WithOnSheet(sheet => {AlternateResistSelection(sheet);});
    }
    private static void CreateDraconicResistB(Feat resists)
    {
        resists.WithOnCreature(self =>
        {
            self.AddQEffect(new QEffect()
            {
                StateCheck = innerSelf =>
                    innerSelf.Owner.WeaknessAndResistance.Resistances.Add(new SpecialResistance($"{DetermineDamageKind(resists).HumanizeTitleCase2()}"+" Dragon", (action, kind) => action != null
                            && action.Owner.HasTrait(Trait.Dragon)
                            && kind == DetermineDamageKind(resists),
                        (self.Level / 2 > 1 ? self.Level / 2 : 1)*2,null
                    ))
            });
            self.AddQEffect(QEffect.DamageResistance(DetermineDamageKind(resists),
                self.Level / 2 > 1 ? self.Level / 2 : 1));
        });
    }
    private static void CreateMagicalDragonbloodLogic(TrueFeat magicBlood)
    {
        magicBlood.WithPrerequisite(
                (sheet) => (!sheet.HasFeat(ModData.FeatNames.Unknown) 
                            && sheet.AllFeats.Exists(feat => (feat.HasTrait(ModData.Traits.DraconicExemplar))
                            && sheet.AllFeats.FirstOrDefault(feat => (feat.HasTrait(ModData.Traits.DraconicExemplar))).HasTrait(DetermineTrait(magicBlood))
                            )),
                "You must select a draconic exemplar from the same magical tradition.")
            .WithOnSheet(sheet =>
            {
                sheet.TrainInThisOrSubstitute(DetermineSkill(magicBlood));
            })
            .WithOnCreature(self =>
            {
                self.GetOrCreateSpellcastingSource(SpellcastingKind.Innate, ModData.Traits.Dragonblood, Ability.Charisma, DetermineTrait(magicBlood)).WithSpells(
                        [DetermineSpellId(magicBlood)], 1);
            });
    }
    private static void CreateScalyHideLogic(TrueFeat scalyHide)
    {
        scalyHide.WithPrerequisite(sheet => !sheet.HasFeat(ModData.FeatNames.DraconicAspect), "You cannot take this feat and Draconic Aspect.")
            .WithPermanentQEffect(null,selfQf =>
        {
            Item createdArmor = new Item(new ModdedIllustration("HTDAssets/Scale.png"), "Scaly Hide",
                    [Trait.Armor, Trait.UnarmoredDefense, Trait.Cloth])
                .WithArmorProperties(new ArmorProperties(selfQf.Owner.Level < 5 ? 1 : 2, 3, 0, 0, 10));
            ReplicateArmorRunes(selfQf.Owner, createdArmor);
            if (selfQf.Owner.Armor.WearsArmor != true)
            {
                selfQf.Owner.AddQEffect(new QEffect("Scaly Hide",
                    "While you are unarmored, the scales give you an item bonus to ac.",
                    ExpirationCondition.Never, selfQf.Owner, new ModdedIllustration("HTDAssets/Scale.png"))
                {
                    Id = ModData.QEffectIds.ScalyHide,
                    ProvidesArmor = createdArmor,
                    DoNotShowUpOverhead = true,
                    Dismissable = false
                });
            }
            if (selfQf.Owner.Armor.WearsArmor && selfQf.Owner.HasEffect(ModData.QEffectIds.ScalyHide))
            {
                QEffect? qEffect = selfQf.Owner.FindQEffect(ModData.QEffectIds.ScalyHide);
                if (qEffect != null) qEffect.ExpiresAt = ExpirationCondition.Immediately;
            }
            selfQf.StateCheckWithVisibleChanges = async qf =>
            {
                if (selfQf.Owner.FindQEffect(QEffectId.MageArmor) != null &&
                    selfQf.Owner.HasEffect(ModData.QEffectIds.ScalyHide))
                {
                    if (!createdArmor.Runes.Any(rune =>
                            rune.ItemName is ItemName.ArmorPotencyRunestone or ItemName.ArmorPotencyRunestone2
                                or ItemName.ArmorPotencyRunestone3))
                    {
                        Item rune1 = Items.GetItemTemplate(ItemName.ArmorPotencyRunestone);
                        rune1.RuneProperties?.ModifyItem(createdArmor);
                    }
                }
            };
        });
    }
    private static void CreateTraditionalResistancesLogic(TrueFeat traditionalResistances)
    {
        traditionalResistances.WithPrerequisite(
            values => values.AllFeats.Exists(ft => ft.HasTrait(ModData.Traits.MagicDragonblood)),
            "You must have Arcane Dragonblood, Divine Dragonblood, Occult Dragonblood, or Primal Dragonblood.")
            .WithOnCreature((sheet, self) =>
            {
                self.AddQEffect(new QEffect()
                    {
                    BonusToDefenses = (_, action, defense) =>
                    {
                        Feat? feat = sheet.AllFeats.FirstOrDefault(ft => ft.HasTrait(ModData.Traits.MagicDragonblood));
                        if (defense is Defense.AC or Defense.Fortitude or Defense.Will or Defense.Reflex 
                            && action != null 
                            && feat != null
                            && action.HasTrait(DetermineTrait(feat))
                            && action.CountsAsMagical)
                        {
                            int amount = 1;
                            if (action.HasTrait(Trait.Sleep) || action.Description.Contains("Paralyze") || action.Description.Contains("Paralysis"))
                                amount += 1;
                            return new Bonus(amount, BonusType.Status, "Traditional Resistances");
                        }
                        return null;
                    },
                    });
            });
    }
    private static void CreateDragonsFlightLogic(TrueFeat dragonsFlight)
    {
        dragonsFlight.WithActionCost(1)
            .WithOnCreature(self =>
                {
                    self.AddQEffect(new QEffect("Dragon's Flight",
                        "You can fly up to 20 feet, you must end this movement on solid ground.")
                        {
                            ProvideMainAction = qf =>
                            {
                                ActionPossibility dragonFly = new ActionPossibility(new CombatAction(self,
                                        IllustrationName.Fly, "Dragon's Flight", [Trait.Move],
                                        "You can fly up to 20 feet, you must end this movement on solid ground.",
                                        Target.Self()
                                    ).WithActionCost(1)
                                    .WithEffectOnChosenTargets(async (action, innerSelf, _) =>
                                        {
                                            QEffect littleFly = QEffect.Flying()
                                                .WithExpirationNever();
                                            littleFly.BonusToAllSpeeds = qfThis =>
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
                                                    break;
                                                case TileOption tOpt:
                                                    // Perform fly
                                                    await tOpt.Action();
                                                    innerSelf.RemoveAllQEffects(qf => qf == littleFly);
                                                    break;
                                            }
                                        }
                                    ));
                                return dragonFly;
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
    private static IEnumerable<Feat> DraconicAspectFeats()
    {
        Feat draconicClaws = new(ModManager.RegisterFeatName("Dragonblood.AspectClaws", "Draconic Aspect Claws"),
            "You have sharp claws.",
            "The claws are in the brawling group and deal 1d4 slashing damage and are agile, finesse and unarmed. You need a free hand to use claws.",
            [ModData.Traits.AspectWeapon], null);
        draconicClaws.WithOnCreature(self =>
        {
            Item claws = CommonItems.CreateNaturalWeapon(IllustrationName.DragonClaws, "claws",
                "1d4", DamageKind.Slashing, Trait.Agile, Trait.Finesse, Trait.Unarmed);
            if (self.HasFeat(ModData.FeatNames.DeadlyAspect))
                claws.Traits.Add(Trait.DeadlyD8);
            if (self.HasFreeHand)
                self.WithAdditionalUnarmedStrike(claws);
        });
        yield return draconicClaws;
        Feat draconicJaw = new(ModManager.RegisterFeatName("Dragonblood.AspectJaw", "Draconic Aspect Jaw"),
            "You have sharp teeth in a powerful jaw.",
            "Your jaw is in the brawling group and deals 1d6 piercing damage and is forceful and unarmed.",
            [ModData.Traits.AspectWeapon], null);
        draconicJaw.WithOnCreature(self =>
        {
            Item jaw = CommonItems.CreateNaturalWeapon(IllustrationName.Jaws, "jaw", "1d6", DamageKind.Piercing,
                Trait.Forceful, Trait.Unarmed);
            if (self.HasFeat(ModData.FeatNames.DeadlyAspect))
                jaw.Traits.Add(Trait.DeadlyD8);
            self.WithAdditionalUnarmedStrike(jaw);
        });
        yield return draconicJaw;
        Feat draconicTail = new(ModManager.RegisterFeatName("Dragonblood.AspectTail", "Draconic Aspect Tail"),
            "You have a strong reptilian tail.",
            "Your tail is in the brawling group and deals 1d6 bludgeoning damage and is forceful and unarmed.",
            [ModData.Traits.AspectWeapon], null);
        draconicTail.WithOnCreature(self =>
        {
            Item tail = CommonItems.CreateNaturalWeapon(IllustrationName.SeaSerpentTail256, "tail", "1d6", DamageKind.Bludgeoning,
                Trait.Sweep, Trait.Trip, Trait.Unarmed);
            if (self.HasFeat(ModData.FeatNames.DeadlyAspect))
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
        if (feat.HasTrait(ModData.Traits.Bludgeoning))
            return DamageKind.Bludgeoning;
        if (feat.HasTrait(Trait.Acid))
            return DamageKind.Acid;
        return feat.HasTrait(Trait.Poison) ? DamageKind.Poison : DamageKind.Untyped;
    }
    private static bool IsCone(Feat feat)
    {
        return !feat.HasTrait(ModData.Traits.Line);
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
        if (feat.HasTrait(ModData.Traits.Bludgeoning))
            return ModData.Traits.Bludgeoning;
        if (feat.HasTrait(Trait.Acid))
            return Trait.Acid;
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
    private static Trait DetermineTrait(TrueFeat feat)
    {
        if (feat.HasTrait(Trait.Arcane))
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
                if (rune.RuneProperties != null && (rune.RuneProperties.CanBeAppliedTo == null || rune.RuneProperties.CanBeAppliedTo(rune, createdArmor) == null))
                    rune.RuneProperties.ModifyItem(createdArmor);
            }
        }
    }
    private static void AlternateResistSelection(CalculatedCharacterSheetValues sheet)
    { 
        sheet.AddSelectionOption(new SingleFeatSelectionOption("ResistanceChoice", "Choose a resistance",
                -1, feat1 => feat1.HasTrait(ModData.Traits.Resists)));
    }
    private static SpellId DetermineSpellId(Feat feat)
    {
        if (feat.HasTrait(Trait.Arcane))
            return SpellId.Shield;
        if (feat.HasTrait(Trait.Divine))
            return SpellId.Guidance;
        return feat.HasTrait(Trait.Occult) ? SpellId.OpenDoor : SpellId.Tanglefoot;
    }
}