using System.Diagnostics;
using Dawnsbury.Audio;
using Dawnsbury.Core;
using Dawnsbury.Core.Animations.AuraAnimations;
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
using SpiritDamage;

namespace HereThereBeDragons;

public static class DragonDeityDomain
{
    internal static readonly SpellId DraconicBarrage = ModManager.RegisterNewSpell(
            "DraconicBarrage",
            0,
            (_, _, spellLevel, inCombat, _) =>
            {
                return Spells.CreateModern(
                        ModData.MIllustrations.DraconicBarrageIllustration,
                        "Draconic Barrage",
                        [Trait.Uncommon, Trait.Cleric, Trait.Focus],
                        "You shape energy into a small group of tiny dragons (or other serpentine creatures) that flit around you.",
                        $"Choose fire, force, mental, or spirit damage when you Cast the Spell. For the duration of the spell, your Strikes with weapons or unarmed attacks deal {S.HeightenedVariable(spellLevel, 1)} additional damage of the chosen type, as the dragons add their energy to your attacks." +
                        $"\n\nYou can Sustain the spell to change the damage type. In addition, you can Sustain the spell to have the dragons fly off to bombard a creature within 60 feet. That creature takes {S.HeightenedVariable(2*spellLevel, 2)}d4 damage of the chosen type (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends.",
                        Target.Self(),
                        spellLevel,
                        null)
                    .WithActionCost(2)
                    .WithSoundEffect(SfxName.MagicWeapon)
                    .WithHeighteningNumerical(spellLevel, 1, inCombat, 1, "The additional amount of damage from the dragons increases by 1 and the damage dealt by the dragons' bombardment increases by 2d4." )
                    .WithVariants([
                        new SpellVariant("FIRE", "Fire Barrage", ModData.MIllustrations.DraconicBarrageIllustration),
                        new SpellVariant("FORCE", "Force Barrage", ModData.MIllustrations.ForceBarrageIllustration),
                        new SpellVariant("MENTAL", "Mental Barrage", ModData.MIllustrations.MentalBarrageIllustration),
                        new SpellVariant("SPIRIT", "Spirit Barrage", ModData.MIllustrations.ElectricityBarrageIllustration)
                    ]).WithCreateVariantDescription((_, v) =>
                    {
                        string str4 = v?.Id switch
                        {
                            "FIRE" => "fire",
                            "FORCE" => "force",
                            "MENTAL" => "mental",
                            "SPIRIT" => "spirit",
                            _ => "unknown"
                        };
                        return
                            $"Your Strikes with weapons or unarmed attacks deal 1 additional {str4} damage, as the dragons add their energy to your attacks. You can Sustain the spell to change the damage type. In addition, you can Sustain the spell to have the dragons fly off to bombard a creature within 60 feet. That creature takes 2d4 {str4} damage (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends.\n\n" +
                            "{b}Heightened (+1){/b} The additional amount of damage from the dragons increases by 1 and the damage dealt by the dragons' bombardment increases by 2d4.";
                    })
                    .WithEffectOnSelf((spell, self) =>
                    {
                        Debug.Assert(spell.ChosenVariant != null);
                        string id = spell.ChosenVariant.Id;
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
                            case "SPIRIT":
                                self.AddQEffect(CreateBarrageLogic(spell, DamageSpirit.Spirit));
                                break;
                        }
                        return Task.CompletedTask;
                    });
            }
        );

    public static readonly SpellId DragonRoar = ModManager.RegisterNewSpell(
            "RoarOfTheDragon",
            0,
            (_, _, spellLevel, _, _) =>
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
                    .WithEffectOnEachTarget((_, self, target, checkResult) =>
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
                                return Task.CompletedTask;
                            default:
                                num = 0;
                                break;
                        }
                        target.AddQEffect(QEffect.Frightened(num));
                        if (checkResult != CheckResult.CriticalFailure)
                            return Task.CompletedTask;
                        target.AddQEffect(QEffect.Fleeing(self).WithExpirationAtStartOfSourcesTurn(self, 1));
                        return Task.CompletedTask;
                    });
            }
        );

    public static readonly SpellId ProtectorsSphere = ModManager.RegisterNewSpell(
        "ProtectorsSphere",
        0,
        (_, _, spellLevel, inCombat, _) =>
        {
            return Spells.CreateModern(
                    IllustrationName.CircleOfProtection,
                    "Protector's Sphere",
                    [Trait.Uncommon, Trait.Focus, Trait.Aura],
                    "A protective aura emanates out from you, safeguarding you and your allies.",
                    $"You gain resistance {S.HeightenedVariable(spellLevel-1, 3)} to all damage. Your allies also gain this resistance while in the aura.",
                    Target.Self(),
                    spellLevel,
                    null)
                .WithActionCost(2)
                .WithHeighteningNumerical(spellLevel, 4, inCombat, 1, "The resistance increases by 1.")
                .WithEffectOnSelf(self =>
                    {
                        self.AddQEffect(new QEffect {
                            SpawnsAura = _ => new MagicCircleAuraAnimation(IllustrationName.AngelicHaloCircleWhite, Color.DarkViolet, 3f) }
                            .AddGrantingOfTechnical(nearby => 
                            nearby.DistanceTo(self) <= 3 && !nearby.HasTrait(Trait.Object) && nearby.FriendOf(self),
                            qfProtect =>
                            {
                                qfProtect.Illustration = IllustrationName.CircleOfProtection;
                                qfProtect.Name = "Protector's Sphere";
                                qfProtect.Description = $"You gain resistance {(spellLevel-1).ToString()} to all damage.";
                                qfProtect.Owner.AddQEffect(QEffect.DamageResistanceAllExcept(spellLevel - 1).WithExpirationEphemeral());
                            }));
                    }
                );
        }
    );
    
    public static void CreateDomainFeats()
    {
        Feat dragonDomain = ClericClassFeatures.CreateDomain(ModData.MFeatNames.DragonDomain, "You draw on the power of dragons, linnorms, and other powerful reptilian creatures.", DraconicBarrage, DragonRoar);
        ClericClassFeatures.AllDomainFeats.Add(dragonDomain);
        ModManager.AddFeat(dragonDomain);
        Feat clericDomain1 = CreateAdvancedDomainFeat(Trait.Cleric, dragonDomain);
        ModManager.AddFeat(clericDomain1);
        AllFeats.GetFeatByFeatName(FeatName.AdvancedDomain).Subfeats?.Add(clericDomain1);
        Feat championDomain1 = CreateAdvancedDomainFeat(Trait.Champion, dragonDomain);
        ModManager.AddFeat(championDomain1);
        AllFeats.GetFeatByFeatName(FeatName.AdvancedDeitysDomain).Subfeats?.Add(championDomain1);
        Feat oracleDomain1 = CreateAdvancedDomainFeat(Trait.Oracle, dragonDomain);
        ModManager.AddFeat(oracleDomain1);
        AllFeats.GetFeatByFeatName(FeatName.DomainFluency).Subfeats?.Add(oracleDomain1);
        if (ModManager.TryParse("ProtectorsSacrifice", out SpellId protectorsSacrifice))
        {
            Feat protectionDomain = ClericClassFeatures.CreateDomain(ModData.MFeatNames.ProtectionDomain, "You ward yourself and others.", protectorsSacrifice, ProtectorsSphere);
            ClericClassFeatures.AllDomainFeats.Add(protectionDomain);
            ModManager.AddFeat(protectionDomain);
            Feat clericDomain = CreateAdvancedDomainFeat(Trait.Cleric, protectionDomain);
            ModManager.AddFeat(clericDomain);
            AllFeats.GetFeatByFeatName(FeatName.AdvancedDomain).Subfeats?.Add(clericDomain);
            Feat championDomain = CreateAdvancedDomainFeat(Trait.Champion, protectionDomain);
            ModManager.AddFeat(championDomain);
            AllFeats.GetFeatByFeatName(FeatName.AdvancedDeitysDomain).Subfeats?.Add(championDomain);
            Feat oracleDomain = CreateAdvancedDomainFeat(Trait.Oracle, protectionDomain);
            ModManager.AddFeat(oracleDomain);
            AllFeats.GetFeatByFeatName(FeatName.DomainFluency).Subfeats?.Add(oracleDomain);
            Feat tianDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Tian"),
                "Tian, the Highest Dragon, is a divine celestial being revered as a protector of the heavens and just rulership. Dwelling in the highest reaches of the celestial realm and often associated with the stars and the turning of the heavens, Tian represents both spiritual elevation and unyielding duty. Tian stands as a guardian of order and the eternal structure of the cosmos.\n\nOften invoked by emperors, astrologers, and heavenly dragons, Tian’s presence is felt in the arc of stars and the path of the sun. He is the ever-watchful sentinel who wards off demons from ascending to the celestial realm and ensures that mortal rulers do not defy the heavenly mandate.",
                "{b}•Edicts{/b} Uphold cosmic and social order, act as a guardian to those under your protection, honor celestial phenomena, combat demonic or otherworldly threats that endanger the world.\n{b}•Anathema{/b} Disobey or defy rightful and just authority without cause, engage in behavior that undermines order, desecrate celestial sites or mock the heavens, aid fiends or aberrations in entering the material or celestial plane.",
                [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainAir, FeatName.DomainSun, ModData.MFeatNames.ProtectionDomain, ModData.MFeatNames.DragonDomain], ItemName.Longspear, [SpellId.TrueStrike, SpellId.FalseLife ,SpellId.DeflectCriticalHit, SpellId.Stoneskin], Skill.Society);
            tianDeity.Traits.Add(Trait.Homebrew);
            AllFeats.GetFeatByFeatName(FeatName.Cleric).Subfeats?.Add(tianDeity);
            ModManager.AddFeat(tianDeity);
            Feat dyeusDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Dyeus"),
                "Ancient texts, older than memory and guarded by draconic sages, tell of Dyeus and his mate Teymatha—the first dragons. Together, they brought forth five children and shaped the mortal realm known as the Points of Light. But one child, Canak, turned from creation and sought only destruction.\n\nHeartbroken, Dyeus stood against his own son. Though it pained him to fight his blood, he would not let the world be undone. As he gained the upper hand, Teymatha intervened, allowing Canak to escape into the depths of Hell, wounded and vengeful.\n\nSince then, Dyeus has devoted himself to shielding other families from such tragedy. He watches where strife brews, guiding mortals toward reconciliation through subtle signs and quiet grace. Though he cannot undo his own loss, he finds hope in every bond preserved. Nonetheless, he prepares for the day when he must face Canak again, in a war that could consume all dragonkind.",
                "{b}•Edicts{/b} Seek and destroy evil, travel the world, protect families\n{b}•Anathema{/b} Fail to pursue a foe who has betrayed your mercy, attack a creature without certainty of wrongdoing",
                [NineCornerAlignment.NeutralGood, NineCornerAlignment.ChaoticGood, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainFamily, FeatName.DomainTravel, ModData.MFeatNames.ProtectionDomain, ModData.MFeatNames.DragonDomain], ItemName.Staff, [SpellId.TrueStrike ,SpellId.Haste, SpellId.ReboundingBarrier], Skill.Diplomacy);
            dyeusDeity.Traits.Add(Trait.Homebrew);
            AllFeats.GetFeatByFeatName(FeatName.Cleric).Subfeats?.Add(dyeusDeity);
            ModManager.AddFeat(dyeusDeity);
        }
        else
        {
            Feat tianDeity = new DeitySelectionFeat(
                ModManager.RegisterFeatName("Deity: Tian"),
                "Tian, the Highest Dragon, is a divine celestial being revered as a protector of the heavens and just rulership. Dwelling in the highest reaches of the celestial realm and often associated with the stars and the turning of the heavens, Tian represents both spiritual elevation and unyielding duty. Tian stands as a guardian of order and the eternal structure of the cosmos.\n\nOften invoked by emperors, astrologers, and heavenly dragons, Tian’s presence is felt in the arc of stars and the path of the sun. He is the ever-watchful sentinel who wards off demons from ascending to the celestial realm and ensures that mortal rulers do not defy the heavenly mandate.",
                "{b}•Edicts{/b} Uphold cosmic and social order, act as a guardian to those under your protection, honor celestial phenomena, combat demonic or otherworldly threats that endanger the world.\n{b}•Anathema{/b} Disobey or defy rightful and just authority without cause, engage in behavior that undermines order, desecrate celestial sites or mock the heavens, aid fiends or aberrations in entering the material or celestial plane.",
                [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.LawfulGood],
                [FeatName.HealingFont], [FeatName.DomainAir, FeatName.DomainSun, FeatName.DomainFamily, ModData.MFeatNames.DragonDomain], ItemName.Longspear, [SpellId.TrueStrike, SpellId.FalseLife ,SpellId.DeflectCriticalHit, SpellId.Stoneskin], Skill.Society);
            tianDeity.Traits.Add(Trait.Homebrew);
            AllFeats.GetFeatByFeatName(FeatName.Cleric).Subfeats?.Add(tianDeity);
            ModManager.AddFeat(tianDeity);
        }
        Feat shenLongDeity = new DeitySelectionFeat(
            ModManager.RegisterFeatName("Deity: Shen-Long"),
            "Shen-Long is the celestial dragon of storm and sky, the divine force responsible for summoning the clouds and bringing the life-giving rains that sustain the land. Revered as both a temperamental storm bringer and a generous provider, Shen-Long is deeply respected across agrarian societies, especially in eastern lands. In some imperial traditions, he is considered the dragon most closely tied to the Mandate of Heaven, embodying nature’s approval or discontent with rulers.\n\nAs a deity, Shen-Long values balance above all—between wet and dry, storm and calm, power and humility. His followers often serve as shamans, storm callers, or advisors to rulers, interpreting his will through the wind and sky.",
            "{b}•Edicts{/b} Protect and respect the natural balance of weather and seasons, offer prayers and rituals to ensure seasonal rains and calm storms, intervene when rulers act unjustly, especially if their misdeeds disturb the harmony of the land.\n{b}•Anathema{/b} Cause or support environmental destruction that disrupts rain patterns, use magic to unnaturally alter weather for personal gain or revenge, mock or exploit dragons or dragon-spirits, ignore omens in the wind, clouds, or storms",
            [NineCornerAlignment.TrueNeutral, NineCornerAlignment.NeutralGood, NineCornerAlignment.LawfulNeutral, NineCornerAlignment.ChaoticNeutral, NineCornerAlignment.LawfulGood],
            [FeatName.HealingFont, FeatName.HarmfulFont], [FeatName.DomainAir, FeatName.DomainLightning, FeatName.DomainWater, ModData.MFeatNames.DragonDomain], ItemName.Whip, [SpellId.PushingGust, SpellId.ObscuringMist ,SpellId.LightningBolt, SpellId.DrawTheLightning], Skill.Nature);
        shenLongDeity.Traits.Add(Trait.Homebrew);
        AllFeats.GetFeatByFeatName(FeatName.Cleric).Subfeats?.Add(shenLongDeity);
        ModManager.AddFeat(shenLongDeity);
    }

    private static QEffect CreateBarrageLogic(CombatAction spell, DamageKind damageType)
    {
        DamageKind damageKind = damageType;
        Debug.Assert(spell.ChosenVariant != null);
        QEffect barrageLogic = new()
        {
            Id = ModData.MQEffectIds.DraconicBarrage,
            ProvideMainAction = qfThis =>
            {
                Creature self = qfThis.Owner;
                qfThis.Illustration = damageKind switch
                {
                    DamageKind.Fire => ModData.MIllustrations.DraconicBarrageIllustration,
                    DamageKind.Mental => ModData.MIllustrations.MentalBarrageIllustration,
                    DamageKind.Force => ModData.MIllustrations.ForceBarrageIllustration,
                    _ => qfThis.Illustration
                };
                if (damageKind == DamageSpirit.Spirit)
                    qfThis.Illustration = ModData.MIllustrations.ElectricityBarrageIllustration;
                qfThis.Name = $"{damageKind.HumanizeTitleCase2()}" + " Barrage";
                qfThis.Description = $"You deal additional {damageKind.HumanizeLowerCase2()} damage, you can sustain this spell to change the type or you can sustain and end this spell to deal damage.";
                CombatAction change = CombatAction.CreateSimple(self, "Change Barrage Damage Type", Trait.Concentrate, Trait.SustainASpell);
                change.Illustration = IllustrationName.BlueD20;
                change.Description = "You can sustain this spell to change the damage type draconic barrage deals.";
                List<DamageKind> damages = [DamageKind.Fire,DamageKind.Mental, DamageSpirit.Spirit, DamageKind.Force];
                damages.Remove(damageKind);
                List<string> damagesStr = [];
                damagesStr.AddRange(damages.Select(damageKind2 => damageKind2.ToStringOrTechnical()));
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
                            DamageKind.Fire => ModData.MIllustrations.DraconicBarrageIllustration,
                            DamageKind.Mental => ModData.MIllustrations.MentalBarrageIllustration,
                            DamageKind.Force => ModData.MIllustrations.ForceBarrageIllustration,
                            _ => qfThis.Illustration
                        };
                        if (damages[chosenOption.Index] == DamageSpirit.Spirit)
                            qfThis.Illustration = ModData.MIllustrations.ElectricityBarrageIllustration;
                    }
                    else action.RevertRequested = true;
                });
            return new ActionPossibility(change);
            },
            AddExtraStrikeDamage = (strike, _) =>
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
                CombatAction barrage = CombatAction.CreateSimple(caster, "Unleash Draconic Barrage", Trait.Concentrate, Trait.SustainASpell);
                barrage.Description = "{b}Range{/b} 60 feet\n\nA target within 60 feet takes " +
                                      spell.SpellLevel * 2 + "d4 " + damage +
                                      " damage (basic Reflex save). Once the dragons have been used in this way, they wink out of existence and the spell ends.";
                barrage.SpellcastingSource = spell.SpellcastingSource;
                barrage.WithActionCost(1);
                barrage.Illustration = spell.Illustration;
                barrage.Target = Target.Ranged(12);
                barrage.WithSpellSavingThrow(Defense.Reflex);
                barrage.WithEffectOnEachTarget(async (spell2, user, target, check) =>
                {
                    await CommonSpellEffects.DealBasicDamage(spell2, user, target, check,
                        spell.SpellLevel * 2 + "d4", damageKind);
                    user.RemoveAllQEffects(qfThis =>
                        qfThis == qfThis.Owner.FindQEffect(ModData.MQEffectIds.DraconicBarrage));
                });
                return new ActionPossibility(barrage);
            }
        };
        return barrageLogic;
    }

    private static Feat CreateAdvancedDomainFeat(Trait forClass, Feat domainFeat) 
    {
        string name = domainFeat.Name;
        SpellId advancedSpell = (SpellId)domainFeat.Tag!;
        Spell spell = AllSpells.CreateModernSpellTemplate(advancedSpell, forClass);
        Feat advancedDomain = new Feat(ModManager.RegisterFeatName("AdvancedDomain:" + forClass.HumanizeTitleCase2() + ":" + name, name + ": " + spell.Name), "Your studies or prayers have unlocked deeper secrets of the " + name.ToLower() + " domain.",
                $"You learn the {forClass.HumanizeTitleCase2().ToLower()} focus spell " + AllSpells.CreateSpellLink(advancedSpell, forClass) + ", and you gain 1 focus point, up to a maximum 3.", [], null)
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
                    case Trait.Oracle:
                        sheet.AddFocusSpellAndFocusPoint(Trait.Oracle, Ability.Charisma, advancedSpell);
                        break;
                    case Trait.Champion:
                        sheet.AddFocusSpellAndFocusPoint(Trait.Champion, Ability.Charisma, advancedSpell);
                        break;
                }
            });
        return advancedDomain;
    }
}