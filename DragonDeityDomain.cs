using System.Diagnostics;
using Dawnsbury.Audio;
using Dawnsbury.Core;
using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.CharacterBuilder.FeatsDb;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Common;
using Dawnsbury.Core.CharacterBuilder.FeatsDb.Spellbook;
using Dawnsbury.Core.CharacterBuilder.Spellcasting;
using Dawnsbury.Core.CombatActions;
using Dawnsbury.Core.Coroutines.Options;
using Dawnsbury.Core.Creatures;
using Dawnsbury.Core.Mechanics;
using Dawnsbury.Core.Mechanics.Core;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Core.Mechanics.Targeting;
using Dawnsbury.Core.Mechanics.Treasure;
using Dawnsbury.Core.Possibilities;
using Dawnsbury.Core.Roller;
using Dawnsbury.Display;
using Dawnsbury.Display.Illustrations;
using Dawnsbury.Display.Text;
using Dawnsbury.Modding;
using Microsoft.Xna.Framework;

namespace HereThereBeDragons;

public class DragonDeityDomain
{
    private static readonly SpellId DraconicBarrage = ModManager.RegisterNewSpell(
            "DraconicBarrage",
            1,
            ((spellId, caster, spellLevel, inCombat, spellInformation) =>
            {
                return Spells.CreateModern(
                        ModData.Illustrations.DraconicBarrageIllustration,
                        "Draconic Barrage",
                        [Trait.Uncommon, Trait.Cleric, Trait.Focus],
                        "You shape energy into a small group of tiny dragons (or other serpentine creatures) that flit around you.",
                        "Choose fire, force, mental, or electricity damage when you Cast the Spell. For the duration of the spell, your Strikes with weapons or unarmed attacks deal 1 additional damage of the chosen type, as the dragons add their energy to your attacks." +
                        "\n\nYou can Sustain the spell to change the damage type. In addition, you can Sustain the spell to have the dragons fly off to bombard a creature within 60 feet. That creature takes 2d4 damage of the chosen type (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends." +
                        "\n\n{b}Heightened (+1){/b} The additional amount of damage from the dragons increases by 1 and the damage dealt by the dragons' bombardment increases by 2d4.",
                        Target.Self(),
                        spellLevel,
                        null)
                    .WithActionCost(2)
                    .WithSoundEffect(SfxName.MagicWeapon)
                    .WithVariants([
                        new SpellVariant("FIRE", "Fire Barrage", ModData.Illustrations.DraconicBarrageIllustration),
                        new SpellVariant("FORCE", "Force Barrage", ModData.Illustrations.ForceBarrageIllustration),
                        new SpellVariant("MENTAL", "Mental Barrage", ModData.Illustrations.MentalBarrageIllustration),
                        new SpellVariant("ELECTRICITY", "Electricity Barrage", ModData.Illustrations.ElectricityBarrageIllustration)
                    ]).WithCreateVariantDescription((_, v) =>
                    {
                        string str4 = v?.Id switch
                        {
                            "FIRE" => "fire",
                            "FORCE" => "force",
                            "MENTAL" => "mental",
                            "ELECTRICITY" => "electricity",
                            _ => "unknown"
                        };
                        string str5 = str4;
                        return
                            $"Your Strikes with weapons or unarmed attacks deal 1 additional {str5} damage, as the dragons add their energy to your attacks. You can Sustain the spell to change the damage type. In addition, you can Sustain the spell to have the dragons fly off to bombard a creature within 60 feet. That creature takes 2d4 {str5} damage (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends.\n\n" +
                            "{b}Heightened (+1){/b} The additional amount of damage from the dragons increases by 1 and the damage dealt by the dragons' bombardment increases by 2d4.";
                    })
                    .WithEffectOnSelf(async (spell, self) =>
                    {
                        Debug.Assert(spell?.ChosenVariant != null, "spell?.ChosenVariant != null");
                        string id = spell.ChosenVariant.Id;
                        string str6 = id switch
                        {
                            "FIRE" => "fire",
                            "FORCE" => "force",
                            "MENTAL" => "mental",
                            "ELECTRICITY" => "electricity",
                            _ => "unknown"
                        };
                        switch (id)
                        {
                            case "FIRE":
                                self.AddQEffect(CreateBarrageLogic(spell, DamageKind.Fire));
                                break;
                            case "FORCE":
                                self.AddQEffect(CreateBarrageLogic(spell, DamageKind.Force));
                                break;
                            case "MENTAL":
                                self.AddQEffect(CreateBarrageLogic(spell, DamageKind.Mental));
                                break;
                            case "ELECTRICITY":
                                self.AddQEffect(CreateBarrageLogic(spell, DamageKind.Electricity));
                                break;
                        }
                    });
            })
        );

    private static readonly SpellId DragonRoar = ModManager.RegisterNewSpell(
            "RoarOfTheDragon",
            4,
            (spellId, caster, spellLevel, inCombat, spellInformation) =>
            {
                return Spells.CreateModern(
                        new ModdedIllustration("HTDAssets/Roar.png"),
                        "Roar of the Dragon",
                        [
                            Trait.Uncommon, Trait.Focus, Trait.Cleric, Trait.Emotion, Trait.Mental, Trait.Auditory,
                            Trait.Fear, Trait.NoHeightening
                        ],
                        "You channel the might of dragons into your voice, letting out a roar that engenders respect in dragonkind but that instills fear in most other creatures.",
                        "All enemies within the area other than dragons, and creatures with deep ties to dragonkind (such as a barbarian with the draconic instinct, a sorcerer with the draconic bloodline, or a member of a culture that reveres dragons), must attempt a Will save." +
                        $"\n{S.FourDegreesOfSuccess("The target is unaffected.", "The target is frightened 1.", "The target is frightened 2.", "The target is frightened 3 and fleeing for 1 round.")}",
                        Target.SelfExcludingEmanation(6).WithIncludeOnlyIf((area, cr) => cr.EnemyOf(area.OwnerAction.Owner) && !cr.HasTrait(Trait.Dragon) && !cr.HasFeat(FeatName.DraconicBloodline) && !cr.HasFeat(FeatName.DragonInstinct)),
                        spellLevel,
                        SpellSavingThrow.Standard(Defense.Will)
                    )
                    .WithActionCost(2)
                    .WithSoundEffect(SfxName.BeastRoar)
                    .WithEffectOnEachTarget( async (spell, self, target, checkResult) =>
                    {
                        int num;
                        switch (checkResult)
                        {
                            case CheckResult.CriticalFailure:
                                num = 3;
                                break;
                            case CheckResult.Failure:
                                num = 2;
                                break;
                            case CheckResult.Success:
                                num = 1;
                                break;
                            case CheckResult.CriticalSuccess:
                                return;
                            default:
                                num = 0;
                                break;
                        }
                        target.AddQEffect(QEffect.Frightened(num));
                        if (checkResult != CheckResult.CriticalFailure)
                            return;
                        target.AddQEffect(QEffect.Fleeing(self).WithExpirationAtStartOfSourcesTurn(self, 1));
                        });
            }
        );
    private static readonly SpellId ProtectorsSphere = ModManager.RegisterNewSpell(
        "ProtectorsSphere",
        4,
        (spellId, caster, spellLevel, inCombat, spellInformation) =>
        {
            return Spells.CreateModern(
                    IllustrationName.CircleOfProtection,
                    "Protector's Sphere",
                    [Trait.Uncommon, Trait.Focus, Trait.Aura],
                    "A protective aura emanates out from you, safeguarding you and your allies.",
                    "You gain resistance 3 to all damage. Your allies also gain this resistance while in the aura." +
                    "\n\n{b}Heightened (+1){/b} The resistance increases by 1.",
                    Target.Self(),
                    spellLevel,
                    null)
                .WithActionCost(2)
                .WithEffectOnSelf(self =>
                    {
                        self.AnimationData.AddAuraAnimation(IllustrationName.BlessCircle, 3f, Color.DarkViolet);
                        self.AddQEffect(new QEffect().AddGrantingOfTechnical(nearby => 
                            nearby.DistanceTo(self) <= 3 && !nearby.HasTrait(Trait.Object) && nearby.FriendOf(self),
                            qfProtect =>
                            {
                                qfProtect.Illustration = IllustrationName.CircleOfProtection;
                                qfProtect.Name = "Protector's Sphere";
                                qfProtect.Description = $"You gain resistance {(spellLevel-1).ToString()} to all damage.";
                                qfProtect.Owner.AddQEffect(QEffect.DamageResistanceAllExcept(spellLevel - 1, []).WithExpirationEphemeral());
                            }
                            
                                ));
                    }
                );
        }
    );
    
    public static IEnumerable<Feat> CreateDomainFeats()
    {
        Feat dragonDomain = ClericClassFeatures.CreateDomain(ModData.FeatNames.DragonDomain, "You draw on the power of dragons, linnorms, and other powerful reptilian creatures.", DraconicBarrage, DragonRoar);
        ClericClassFeatures.AllDomainFeats.Add(dragonDomain);
        yield return dragonDomain;
        Feat clericDomain1 = CreateAdvancedDomainFeat(Trait.Cleric, dragonDomain);
        AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(clericDomain1);
        yield return clericDomain1;
        Feat championDomain1 = CreateAdvancedDomainFeat(Trait.Champion, dragonDomain);
        AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(championDomain1);
        yield return championDomain1;
        Feat oracleDomain1 = CreateAdvancedDomainFeat(Trait.Oracle, dragonDomain);
        AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(oracleDomain1);
        yield return oracleDomain1;
        if (ModManager.TryParse("ProtectorsSacrifice", out SpellId protectorsSacrifice))
        {
            Feat protectionDomain = ClericClassFeatures.CreateDomain(ModData.FeatNames.ProtectionDomain, "You ward yourself and others.", protectorsSacrifice, ProtectorsSphere);
            ClericClassFeatures.AllDomainFeats.Add(protectionDomain);
            yield return protectionDomain;
            Feat clericDomain = CreateAdvancedDomainFeat(Trait.Cleric, protectionDomain);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(clericDomain);
            yield return clericDomain;
            Feat championDomain = CreateAdvancedDomainFeat(Trait.Champion, protectionDomain);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(championDomain);
            yield return championDomain;
            Feat oracleDomain = CreateAdvancedDomainFeat(Trait.Oracle, protectionDomain);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.AdvancedDeitysDomain).Subfeats.Add(oracleDomain);
            yield return oracleDomain;
            Feat tianDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Tian"),
                "Tian, the Highest Dragon, is a divine celestial being revered as a protector of the heavens and just rulership. Dwelling in the highest reaches of the celestial realm and often associated with the stars and the turning of the heavens, Tian represents both spiritual elevation and unyielding duty. Tian stands as a guardian of order and the eternal structure of the cosmos.\n\nOften invoked by emperors, astrologers, and heavenly dragons, Tian’s presence is felt in the arc of stars and the path of the sun. He is the ever-watchful sentinel who wards off demons from ascending to the celestial realm and ensures that mortal rulers do not defy the heavenly mandate.",
                "{b}•Edicts{/b} Uphold cosmic and social order, act as a guardian to those under your protection, honor celestial phenomena, combat demonic or otherworldly threats that endanger the world.\n{b}•Anathema{/b} Disobey or defy rightful and just authority without cause, engage in behavior that undermines order, desecrate celestial sites or mock the heavens, aid fiends or aberrations in entering the material or celestial plane.",
                [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainAir, FeatName.DomainSun, ModData.FeatNames.ProtectionDomain, ModData.FeatNames.DragonDomain], ItemName.Longspear, [SpellId.TrueStrike, SpellId.FalseLife ,SpellId.DeflectCriticalHit, SpellId.Fly], Skill.Religion);
            tianDeity.Traits.Add(Trait.Homebrew);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.Cleric).Subfeats.Add(tianDeity);
            yield return tianDeity;
            Feat apsuDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Apsu"),
                "Texts more ancient than recorded time kept in the halls of draconic sages assert that Apsu and his mate Sarshallatu were the first dragons. The two spawned seven children and as a family, they shaped the mortal world of Golarion. However, one of these children broke Apsu’s heart during that time. Instead of wanting to create, his child, named Dahak, sought only to destroy. As much as it pained Apsu to fight his child, he could not let this world be destroyed senselessly, but when Apsu had gained the upper hand, his partner stopped him. Dahak then fled to Hell to nurse his wounds." +
                "\n\nTo this day, Apsu continues to battle against the forces that seek to destroy this world, forever reminded of his failure to stop his child and the pain of Sarshallatu’s betrayal. Though he strives for peace, he knows that one day, he will have to confront Dahak once again and that all dragonkind will be involved in the inevitable conflict. Such a war would sweep across the Points of Light like a massive wildfire. Until that time, Apsu entreats mortal artists and leaders to make the world the best place it can possibly be. Somewhat ironically, when Apsu contemplates these acts of beauty and harmony, he grows ever more despondent about his fated battle against his son. ",
                "{b}•Edicts{/b} Seek and destroy evil, travel the world, help others fend for themselves\n{b}•Anathema{/b} Fail to pursue a foe who has betrayed your mercy, attack a creature without certainty of wrongdoing",
                [NineCornerAlignment.NeutralGood, NineCornerAlignment.ChaoticGood, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainFamily, FeatName.DomainTravel, ModData.FeatNames.ProtectionDomain, ModData.FeatNames.DragonDomain], ItemName.Staff, [SpellId.TrueStrike ,SpellId.Haste, SpellId.ReboundingBarrier], Skill.Diplomacy);
            apsuDeity.Traits.Add(Trait.Homebrew);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.Cleric).Subfeats.Add(apsuDeity);
            yield return apsuDeity;
        }
        else
        {
            Feat tianDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Tian"),
                "Tian, the Highest Dragon, is a divine celestial being revered as a protector of the heavens and just rulership. Dwelling in the highest reaches of the celestial realm and often associated with the stars and the turning of the heavens, Tian represents both spiritual elevation and unyielding duty. Tian stands as a guardian of order and the eternal structure of the cosmos.\n\nOften invoked by emperors, astrologers, and heavenly dragons, Tian’s presence is felt in the arc of stars and the path of the sun. He is the ever-watchful sentinel who wards off demons from ascending to the celestial realm and ensures that mortal rulers do not defy the heavenly mandate.",
                "{b}•Edicts{/b} Uphold cosmic and social order, act as a guardian to those under your protection, honor celestial phenomena, combat demonic or otherworldly threats that endanger the world.\n{b}•Anathema{/b} Disobey or defy rightful and just authority without cause, engage in behavior that undermines order, desecrate celestial sites or mock the heavens, aid fiends or aberrations in entering the material or celestial plane.",
                [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainAir, FeatName.DomainSun, FeatName.DomainFamily, ModData.FeatNames.DragonDomain], ItemName.Longspear, [SpellId.TrueStrike, SpellId.FalseLife ,SpellId.DeflectCriticalHit, SpellId.Fly], Skill.Religion);
            tianDeity.Traits.Add(Trait.Homebrew);
            AllFeats.All.Find(ft => ft.FeatName == FeatName.Cleric).Subfeats.Add(tianDeity);
            yield return tianDeity;
        }
        Feat shenLongDeity = new DeitySelectionFeat(
            ModManager.RegisterFeatName("Deity: Shen-Long"),
            "Shen-Long is the celestial dragon of storm and sky, the divine force responsible for summoning the clouds and bringing the life-giving rains that sustain the land. Revered as both a temperamental storm bringer and a generous provider, Shen-Long is deeply respected across agrarian societies, especially in eastern lands. In some imperial traditions, he is considered the dragon most closely tied to the Mandate of Heaven, embodying nature’s approval or discontent with rulers.\n\nAs a deity, Shen-Long values balance above all—between wet and dry, storm and calm, power and humility. His followers often serve as shamans, storm callers, or advisors to rulers, interpreting his will through the wind and sky.",
            "{b}•Edicts{/b} Protect and respect the natural balance of weather and seasons, offer prayers and rituals to ensure seasonal rains and calm storms, intervene when rulers act unjustly, especially if their misdeeds disturb the harmony of the land.\n{b}•Anathema{/b} Cause or support environmental destruction that disrupts rain patterns, use magic to unnaturally alter weather for personal gain or revenge, mock or exploit dragons or dragon-spirits, ignore omens in the wind, clouds, or storms",
            [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.ChaoticNeutral, NineCornerAlignment.LawfulGood],
            [FeatName.HealingFont, FeatName.HarmfulFont], [FeatName.DomainAir, FeatName.DomainLightning, FeatName.DomainWater, ModData.FeatNames.DragonDomain], ItemName.Whip, [SpellId.PushingGust, SpellId.ObscuringMist ,SpellId.LightningBolt, SpellId.Fly], Skill.Nature);
        shenLongDeity.Traits.Add(Trait.Homebrew);
        AllFeats.All.Find(ft => ft.FeatName == FeatName.Cleric).Subfeats.Add(shenLongDeity);
        yield return shenLongDeity;
    }

    static QEffect CreateBarrageLogic(CombatAction spell, DamageKind damageType)
    {
        DamageKind damageKind = damageType;
        Debug.Assert(spell.ChosenVariant != null, "spell.ChosenVariant != null");
        QEffect barrageLogic = new()
        {
            Id = ModData.QEffectIds.DraconicBarrage,
            ProvideMainAction = qfThis =>
            {
                Creature self = qfThis.Owner;
                qfThis.Illustration = damageKind switch
                {
                    DamageKind.Fire => ModData.Illustrations.DraconicBarrageIllustration,
                    DamageKind.Mental => ModData.Illustrations.MentalBarrageIllustration,
                    DamageKind.Electricity => ModData.Illustrations.ElectricityBarrageIllustration,
                    DamageKind.Force => ModData.Illustrations.ForceBarrageIllustration,
                    _ => qfThis.Illustration
                };
                qfThis.Name = $"{damageKind.HumanizeTitleCase2()}" + " Barrage";
                qfThis.Description = $"You deal additional {damageKind.HumanizeLowerCase2()} damage, you can sustain this spell to change the type or you can sustain and end this spell to deal damage.";
                CombatAction change = CombatAction.CreateSimple(self, "Change Barrage Damage Type", Trait.Concentrate);
                change.Illustration = IllustrationName.BlueD20;
                change.Description = "You can sustain this spell to change the damage type draconic barrage deals.";
                List<DamageKind> damages = [DamageKind.Fire,DamageKind.Mental, DamageKind.Electricity, DamageKind.Force];
                if (damages == null) throw new ArgumentNullException(nameof(damages));
                damages.Remove(damageKind);
                List<string> damagesStr = [];
                if (damagesStr == null) throw new ArgumentNullException(nameof(damagesStr));
                damagesStr.AddRange(damages.Select(damageKind2 => damageKind2.ToString()));
                damagesStr.Add("cancel");
                change.WithEffectOnSelf(async (action, innerSelf) =>
                {
                    ChoiceButtonOption chosenOption = await innerSelf.AskForChoiceAmongButtons(
                        IllustrationName.QuestionMark,
                        "Choose Barrage Damage Type",
                        damagesStr.ToArray());
                    if (damagesStr[chosenOption.Index] != "cancel")
                    {
                        damageKind = damages[chosenOption.Index];
                        qfThis.Name = $"{damages[chosenOption.Index].HumanizeTitleCase2()}" + " Barrage";
                        qfThis.Description = $"You deal additional {damages[chosenOption.Index].HumanizeLowerCase2()} damage, you can sustain this spell to change the type or you can sustain and end this spell to deal damage.";
                        qfThis.Illustration = damages[chosenOption.Index] switch
                        {
                            DamageKind.Fire => ModData.Illustrations.DraconicBarrageIllustration,
                            DamageKind.Mental => ModData.Illustrations.MentalBarrageIllustration,
                            DamageKind.Electricity => ModData.Illustrations.ElectricityBarrageIllustration,
                            DamageKind.Force => ModData.Illustrations.ForceBarrageIllustration,
                            _ => qfThis.Illustration
                        };
                    }
                    else action.RevertRequested = true;
                });
            return new ActionPossibility(change);
            },
            AddExtraStrikeDamage = (strike, innerSelf) =>
            {
                if (!strike.HasTrait(Trait.Unarmed) && !strike.HasTrait(Trait.Weapon))
                    return null;
                return new ValueTuple<DiceFormula, DamageKind>(
                    DiceFormula.FromText(spell.SpellLevel.ToString(), "Draconic Barrage"), damageKind);
            },
            ProvideContextualAction = qfInner =>
            {
                Creature caster = qfInner.Owner;
                string damage = damageKind.HumanizeLowerCase2();
                CombatAction barrage = CombatAction.CreateSimple(caster, "Unleash Draconic Barrage", Trait.Concentrate);
                barrage.Description = "{b}Range{/b} 60 feet\n\nA target within 60 feet takes " +
                                      (spell.SpellLevel * 2).ToString() + "d4 " + damage +
                                      " damage (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends.";
                barrage.SpellcastingSource = spell.SpellcastingSource;
                barrage.WithActionCost(1);
                barrage.Illustration = spell.Illustration;
                barrage.Target = Target.Ranged(12);
                barrage.WithSpellSavingThrow(Defense.Reflex);
                barrage.WithEffectOnEachTarget(async (spell2, user, target, check) =>
                {
                    await CommonSpellEffects.DealBasicDamage(spell2, user, target, check,
                        (spell.SpellLevel * 2).ToString() + "d4", damageKind);
                    user.RemoveAllQEffects(qfThis =>
                        qfThis == qfThis.Owner.FindQEffect(ModData.QEffectIds.DraconicBarrage));
                });
                return new ActionPossibility(barrage);
            }
        };
        return barrageLogic;
    }

    private static Feat CreateAdvancedDomainFeat(Trait forClass, Feat domainFeat) 
    {
        var name = domainFeat.Name;
        SpellId advancedSpell = (SpellId)domainFeat.Tag!;
        Spell spell = AllSpells.CreateModernSpellTemplate(advancedSpell, forClass);
        Feat advancedDomain = new TrueFeat(ModManager.RegisterFeatName("AdvancedDomain:" + forClass.HumanizeTitleCase2() + ":" + name, name + ": " + spell.Name), 8, "Your studies or prayers have unlocked deeper secrets of the " + name.ToLower() + " domain.",
                $"You learn the {forClass.HumanizeTitleCase2().ToLower()} focus spell " + AllSpells.CreateSpellLink(advancedSpell, forClass) + ", and you gain 1 focus point, up to a maximum 3.", [forClass], null)
            .WithIllustration(spell.Illustration)
            .WithRulesBlockForSpell(advancedSpell, forClass)
            .WithPrerequisite(values => values.HasFeat(domainFeat.FeatName), "You must have the " + name + " domain.")
            .WithOnSheet(sheet =>
            {
                switch (sheet.Sheet.Class?.ClassTrait)
                {
                    case Trait.Cleric:
                        sheet.AddFocusSpellAndFocusPoint(Trait.Cleric, Ability.Wisdom, advancedSpell);
                        break;
                    case Trait.Champion:
                        sheet.AddFocusSpellAndFocusPoint(Trait.Champion, Ability.Charisma, advancedSpell);
                        break;
                    case Trait.Oracle:
                        sheet.AddFocusSpellAndFocusPoint(Trait.Oracle, Ability.Charisma, advancedSpell);
                        break;
                }
            });
        return advancedDomain;
    }
}