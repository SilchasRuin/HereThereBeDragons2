using Dawnsbury.Core.CharacterBuilder.Feats;
using Dawnsbury.Core.Mechanics.Enumerations;
using Dawnsbury.Modding;
using Dawnsbury.Mods.LoresAndWeaknesses;
using static HereThereBeDragons.ModData;

namespace HereThereBeDragons;

public class LoresRequired
{
    public static Lore Dragon { get; set; } = null!;
    public static void LoadLore()
    {
        Dragon = Lores.RegisterNewLore("Dragon Lore",
            "You have studied the mighty dragons, learning of their habitats, their behaviors, their abilities and, perhaps, the best ways to defeat them.", (_, target) => target.HasTrait(Trait.Dragon), true);
        Feat dragonLore = new TrueFeat(ModManager.RegisterFeatName("HTD_DragonLore", "Dragon Lore"), 1,
            "You've set your mind on learning more about your ancestor and their kin, and perhaps you were even raised by a dragon parent. You've come to understand how dragons can invoke fear but also how they've contributed to society as a whole.",
            "You gain the trained proficiency rank in Diplomacy and Intimidation. If you would automatically become trained in one of those skills (from your background or class, for example), you instead become trained in a skill of your choice." +
            "\n\nYou also gain the Additional Lore general feat for Dragon Lore. If you were already trained in Dragon Lore, you also become trained in a lore skill of your choice.",
            [MTraits.Dragonblood])
            .WithOnSheet(values =>
            {
                values.TrainInThisOrSubstitute(Skill.Diplomacy);
                values.TrainInThisOrSubstitute(Skill.Intimidation);
                if (values.GetProficiency(Dragon.Trait) >= Proficiency.Trained)
                    values.TrainInThisOrSubstitute(Dragon, true);
                Lores.GrantAdditionalLore(values, Dragon);
            });
        ModManager.AddFeat(dragonLore);
    }
}