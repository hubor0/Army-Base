using System.Threading;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Towers;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity.Towers.Weapons;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Utils;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNewtonsoft.Json.Utilities;
using MelonLoader;
using armybase;
using UnityEngine;
using UnityEngine.Playables;
using System.Linq;
using System.Collections.Generic;



[assembly: MelonInfo(typeof(armybase.armybase), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace armybase
{
public class armybase : BloonsTD6Mod
    {
        public class ArmyBase : ModTower
        {
            public override TowerSet TowerSet => TowerSet.Military;
            public override string BaseTower => TowerType.DartMonkey;
            public override int Cost => 1025;
            public override string DisplayName => "Army Base";
            public override string Name => "ArmyBase";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 5;
            public override string Description => "Deploys a Monkey Soldier from the end of the track to pop Bloons.";
            public override string Icon => "000_army_base";
            public override string Portrait => "000_army_base";

            public override ParagonMode ParagonMode => ParagonMode.Base000;

            // Credit GuiGuiLavise: Monkey Barack
            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.RemoveBehaviors<NecromancerZoneModel>();
                towerModel.RemoveBehaviors<AttackModel>();
                towerModel.ApplyDisplay<hangar>();
                towerModel.radius = Game.instance.model.GetTower(TowerType.HeliPilot).radius;
                var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
                var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
                summon.weapons[0].projectile.name = "AttackModel_Summon3_";
                summon.weapons[0].emission = new NecromancerEmissionModel("BaseDeploy_", 1, 1, 1, 1, 1, 1, 0, null, null, null, 1, 1, 1, 1, 2);
                summon.weapons[0].rate = 22f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection = false;
                summon.name = "AttackModel_Summon_";
                summon.weapons[0].projectile.GetDamageModel().damage = 1;
                summon.weapons[0].projectile.ApplyDisplay<merc1>();
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
                summon.weapons[0].projectile.pierce = 13;
                summon.range = 100000;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 0;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespan = 999999f;
                summon.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                agemodel.lifespanFrames = 0;
                agemodel.lifespan = 999999f;
                agemodel.rounds = 9999;
                towerModel.range = 20;
                summon.weapons[0].projectile.AddBehavior(agemodel);
                towerModel.AddBehavior(summon);
            }

        }
        public class u100 : ModUpgrade<ArmyBase>
        {
            public override int Path => TOP;
            public override int Tier => 1;
            public override int Cost => 290;
            public override string Portrait => "100copy";
            public override string DisplayName => "Quick Feet";
            public override string Description => "Soldiers now move twice as fast";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar100>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed *= 1.2f;
            }
        }
        public class u200 : ModUpgrade<ArmyBase>
        {

            public override int Path => TOP;
            public override int Tier => 2;
            public override int Cost => 360;
            public override string Portrait => "200copy";

            public override string DisplayName => "Faster Deploy";
            public override string Description => "Deploys soldiers 20% more frequently";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar200>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].rate -= 1.1f;

            }
        }
        public class u300 : ModUpgrade<ArmyBase>
        {

            public override int Path => TOP;
            public override int Tier => 3;
            public override int Cost => 720;
            public override string Portrait => "300copy";

            public override string DisplayName => "Monkey Charge";
            public override string Description => "Monkey Soldiers move even faster and deploy more frequently.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar300>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<merccharge>();
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed *= 1.3f;
                summon.weapons[0].rate -= 8.5f;

            }
        }
        public class u400 : ModUpgrade<ArmyBase>
        {

            public override int Path => TOP;
            public override int Tier => 4;
            public override int Cost => 3275;
            public override string Portrait => "400copy";

            public override string DisplayName => "Dirtbike";
            public override string Description => "Deploys extra fast Dirtbike Monkeys which deal extra damage to Ceramic Bloons.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar400>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed *= 2f;
                summon.weapons[0].rate -= 10f;
                summon.weapons[0].projectile.ApplyDisplay<dirtbike>();
                summon.weapons[0].projectile.pierce += 10;
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Fortified", "Fortified", 1, 10, false, false));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 1, 10, false, false));
            }
        }
        public class u500 : ModUpgrade<ArmyBase>
        {

            public override int Path => TOP;
            public override int Tier => 5;
            public override int Cost => 19200;
            public override string Portrait => "500copy";

            public override string DisplayName => "Road Hog";
            public override string Description => "Deploys Super fast Motorcycles which deal even more Damage to MOAB class Bloons";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar500>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed *= 2f;
                summon.weapons[0].projectile.ApplyDisplay<roadhog>();
                summon.weapons[0].rate -= 1.5f;
                summon.weapons[0].projectile.pierce += 5;
                summon.weapons[0].projectile.GetDamageModel().damage += 15;
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Fortified", "Fortified", 1, 30, false, false));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 1, 30, false, false));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Moab", "Moab", 1, 25, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Bfb", "Bfb", 1, 25, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Ddt", "Ddt", 1, 30, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Zomg", "Zomg", 1, 18, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Bad", "Bad", 1, 20, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1, 15, false, true));
            }
        }
        public class u010 : ModUpgrade<ArmyBase>
        {

            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override int Cost => 200;
            public override string Portrait => "010copy";

            public override string DisplayName => "Camo Uncovering";
            public override string Description => "Can reveal Camo Bloons";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar010>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.RemoveFilter<FilterInvisibleModel>();
                summon.weapons[0].projectile.SetHitCamo(true);
                var diab = Game.instance.model.GetTowerFromId("NinjaMonkey-020").GetAttackModel().weapons[0].projectile.GetBehavior<RemoveBloonModifiersModel>().Duplicate();
                diab.cleanseCamo = true;
                summon.weapons[0].projectile.collisionPasses = new int[] { 0, -1 };
                summon.weapons[0].projectile.AddBehavior(diab);
            }
        }
        public class u020 : ModUpgrade<ArmyBase>
        {

            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override int Cost => 1000;
            public override string Portrait => "020copy";

            public override string DisplayName => "Darts";
            public override string Description => "Whenever pops a Bloon shoots a single Weak Dart";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar020>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                var dart = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                dart.projectile = Game.instance.model.GetTowerFromId("MonkeySub").GetAttackModel().weapons[0].projectile.Duplicate();
                dart.projectile.pierce = 2;
                dart.projectile.GetDamageModel().damage = 1;
                summon.weapons[0].projectile.AddBehavior(dart);
            }
        }
        public class u030 : ModUpgrade<ArmyBase>
        {
            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override int Cost => 1280;
            public override string Portrait => "030copy";

            public override string DisplayName => "Grenade Launcher";
            public override string Description => "Now equipped with a grenade launcher shoots grenades whenever pops a Bloon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar030>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<mercdemoman>();
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                summon.weapons[0].projectile.pierce += 5;
                var nade = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                nade.projectile = Game.instance.model.GetTowerFromId("MonkeyAce").GetAttackModel().weapons[0].projectile.Duplicate();
                nade.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-020").GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                nade.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                nade.projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
                nade.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                nade.projectile.ApplyDisplay<nade>();
                nade.projectile.GetDamageModel().damage = 3;
                summon.weapons[0].projectile.AddBehavior(nade);
            }
        }
        public class u040 : ModUpgrade<ArmyBase>
        {
            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override int Cost => 9000;
            public override string Portrait => "040copy";

            public override string DisplayName => "Tank Commander";
            public override string Description => "Shoots high damaging Missiles. Force Deploy: Speed up the Deploy time of the next Tank";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar040>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<tank>();
                summon.weapons[0].projectile.pierce += 50;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                var shell = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                shell.projectile = Game.instance.model.GetTowerFromId("MonkeyAce").GetAttackModel().weapons[0].projectile.Duplicate();
                shell.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-020").GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                shell.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                shell.projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
                shell.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                shell.projectile.GetDamageModel().damage = 15;
                shell.projectile.ApplyDisplay<shell>();
                shell.projectile.scale *= 3;
                shell.projectile.SetHitCamo(true);
                shell.projectile.AddBehavior(new DamageModifierForTagModel("Moab", "Moab", 1, 2, false, true));
                shell.projectile.AddBehavior(new DamageModifierForTagModel("Bfb", "Bfb", 1, 2, false, true));
                shell.projectile.AddBehavior(new DamageModifierForTagModel("Zomg", "Zomg", 1, 2, false, true));
                shell.projectile.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1, 4, false, true));
                summon.weapons[0].projectile.AddBehavior(shell);

                towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MortarMonkey-040").GetAbility().Duplicate());
                towerModel.GetAbility().name = "forcedeploy";
                towerModel.GetAbility().GetBehavior<TurboModel>().Lifespan = 4;
                towerModel.GetAbility().icon = GetSpriteReference(mod, "Ability-Icon");
            }
        }
        public class u050 : ModUpgrade<ArmyBase>
        {
            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override int Cost => 80600;
            public override string Portrait => "050copy";

            public override string DisplayName => "Mecha Commander";
            public override string Description => "Steers a super-powerful Bloon exterminating Mecha that shoots a volley of 6 guided missiles around itself whenever hits a Bloon.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar050>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<mecha>();
                summon.weapons[0].projectile.pierce += 40;
                var missle = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                missle.projectile = Game.instance.model.GetTowerFromId("MonkeyAce").GetAttackModel().weapons[0].projectile.Duplicate();
                missle.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-200").GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate());
                missle.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter").GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                missle.projectile.AddBehavior(Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate());
                missle.projectile.AddBehavior(Game.instance.model.GetTowerFromId("Adora 20").GetAttackModel().weapons[0].projectile.GetBehavior<AdoraTrackTargetModel>().Duplicate());
                missle.projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                missle.emission = new ArcEmissionModel("ArcEmissionModel_", 6, 0, 360, null, false, false);
                missle.projectile.GetDamageModel().damage = 30;
                missle.projectile.SetHitCamo(true);
                missle.projectile.ApplyDisplay<grandmissle>();
                missle.projectile.scale *= 3;
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Moab", "Moab", 1, 20, false, true));
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Bfb", "Bfb", 1, 35, false, true));
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Ddt", "Ddt", 1, 50, false, true));
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Zomg", "Zomg", 1, 134, false, true));
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Bad", "Bad", 1, 230, false, true));
                missle.projectile.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1, 100, false, true));
                summon.weapons[0].projectile.AddBehavior(missle);
                summon.weapons[0].rate += 8f;
            }
        }
        public class u001 : ModUpgrade<ArmyBase>
        {
            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override int Cost => 240;
            public override string Portrait => "001copy";
            public override string DisplayName => "Sturdier Soldiers";
            public override string Description => "Can now withstand additional 4 Bloons";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar001>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.pierce += 4;
            }
        }
        public class u002 : ModUpgrade<ArmyBase>
        {
            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override int Cost => 340;
            public override string Portrait => "002copy";
            public override string DisplayName => "Barbed Wires";
            public override string Description => "Soldiers can now pop +1 Bloon layers";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar002>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.GetDamageModel().damage += 1;
            }
        }
        public class u003 : ModUpgrade<ArmyBase>
        {
            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override int Cost => 800;
            public override string Portrait => "003copy";
            public override string DisplayName => "Ballistic Shield";
            public override string Description => "Withstands even more Bloons. Can pop Lead and Frozen Bloons and Sometimes Knockback.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar003>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<mercshield>();
                summon.weapons[0].projectile.pierce += 25;
                var Knockback = Game.instance.model.GetTowerFromId("NinjaMonkey-010").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate<WindModel>();
                Knockback.chance = 0.5f;
                Knockback.distanceMin = 25;
                Knockback.distanceMax = 50;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                summon.weapons[0].projectile.AddBehavior(Knockback);
            }
        }
        public class u004 : ModUpgrade<ArmyBase>
        {
            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override int Cost => 5000;
            public override string Portrait => "004copy";
            public override string DisplayName => "Patrol Jeep";
            public override string Description => "Sends Patrol Jeeps with extra Resistance and Damage.";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar004>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].projectile.ApplyDisplay<jeep>();
                summon.weapons[0].projectile.pierce += 95;
                summon.weapons[0].projectile.GetDamageModel().damage += 5;
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Moab", "Moab", 1, 159, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Bfb", "Bfb", 1, 289, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Zomg", "Zomg", 1, 349, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1, 360, false, true));
            }
        }
        public class u005 : ModUpgrade<ArmyBase>
        {
            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override int Cost => 49000;
            public override string Portrait => "005copy";
            public override string DisplayName => "Offense Battalion Convoy";
            public override string Description => "6x6 Off-Road Truck capable of running over the biggest of Bloons";

            public override void ApplyUpgrade(TowerModel towerModel)
            {
                towerModel.ApplyDisplay<hangar005>();
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].rate += 10f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speed -= 0.2f;
                summon.weapons[0].projectile.ApplyDisplay<obc>();
                summon.weapons[0].projectile.pierce = 990;
                summon.weapons[0].projectile.GetDamageModel().damage += 50;
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Moab", "Moab", 1, 389, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Bfb", "Bfb", 1, 1089, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Ddt", "Ddt", 1, 589, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Zomg", "Zomg", 1, 2589, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Bad", "Bad", 1, 5589, false, true));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel_Ceramic", "Ceramic", 1, 58, false, false));
                summon.weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("Boss", "Boss", 1, 5700, false, true));


            }
        }
        public class nade : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
            }
        }
        public class grandmissle : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
            }
        }
        public class shell : ModDisplay
        {
            public override string BaseDisplay => Generic2dDisplay;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
            }
        }
        public class hangar : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();
                    
                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar001 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest001"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar002 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest002"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar010 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest010"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar020 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest020"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar100 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "basetest100"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar200 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222";
            public override string PrefabName => "basetest200";

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar300 : ModCustomDisplay
        {
            public override string AssetBundleName => "basetoppath1"; 
            public override string PrefabName => "basetest300"; 

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar400 : ModCustomDisplay
        {
            public override string AssetBundleName => "basetoppath1"; 
            public override string PrefabName => "basetest400"; 

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(76 / 255f, 52 / 255f, 33 / 255f));
                }
            }
        }
        public class hangar500 : ModCustomDisplay
        {
            public override string AssetBundleName => "basetoppath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest500"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(33 / 255f, 33 / 255f, 33 / 255f));
                }
            }
        }
        public class hangar003 : ModCustomDisplay
        {
            public override string AssetBundleName => "basemidpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest003"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar004 : ModCustomDisplay
        {
            public override string AssetBundleName => "basemidpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest004"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar005 : ModCustomDisplay
        {
            public override string AssetBundleName => "basemidpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest005"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar030 : ModCustomDisplay
        {
            public override string AssetBundleName => "basebotpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest030"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar040 : ModCustomDisplay
        {
            public override string AssetBundleName => "basebotpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest040"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class hangar050 : ModCustomDisplay
        {
            public override string AssetBundleName => "basebotpath1"; // loads from "assets.bundle"
            public override string PrefabName => "basetest050"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(33 / 255f, 33 / 255f, 33 / 255f));
                }
            }
        }
        public class mercshield : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "shield"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class merccharge : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "merccharge"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(76 / 255f, 52 / 255f, 33 / 255f));
                }
            }
        }
        public class mercdemoman : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "mercnade"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public class dirtbike : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "dirtbike"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(76 / 255f, 52 / 255f, 33 / 255f));
                }
            }
        }
        public class roadhog : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "roadhog"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(33 / 255f, 33 / 255f, 33 / 255f));
                }
            }
        }
        public class jeep : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "jeep"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 93;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(14 / 255f, 29 / 255f, 16 / 255f));
                }
            }
        }
        public class obc : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "obc"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 89;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(14 / 255f, 29 / 255f, 16 / 255f));
                }
            }
        }
        public class tank : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "tank"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 107;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(17 / 255f, 24 / 255f, 14 / 255f));
                }
            }
        }
        public class mecha : ModCustomDisplay
        {
            public override string AssetBundleName => "mercs"; // loads from "assets.bundle"
            public override string PrefabName => "mecha"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 107;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(33 / 255f, 33 / 255f, 33 / 255f));
                }
            }
        }
        public class merc1 : ModCustomDisplay
        {
            public override string AssetBundleName => "base222"; // loads from "assets.bundle"
            public override string PrefabName => "soldierproj1"; // loads the "MyModel" prefab

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.transform.GetChild(0).transform.localScale *= 86;
                foreach (var meshRenderer in node.GetMeshRenderers())
                {
                    meshRenderer.ApplyOutlineShader();

                    meshRenderer.SetOutlineColor(new Color(0 / 255f, 80 / 255f, 0 / 255f));
                }
            }
        }
        public override void OnApplicationStart()
        {
            ModHelper.Msg<armybase>("Army Base loaded!");
        }
        }
        }
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.SimulationBehaviors.NecroData), nameof(NecroData.RbePool))]
         internal static class Necro_RbePool
        {
            [HarmonyPrefix]
            private static bool Postfix(NecroData __instance, ref int __result)
            {
                var tower = __instance.tower;
                    __result = 9999;
                return false;
            }
        }
