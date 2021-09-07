using BepInEx;
using UnityEngine;
using RoR2;
using R2API.Utils;
using System;
using R2API;
using BepInEx.Configuration;
using System.Collections.Generic;

namespace TitanicPlainsBisons
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Moffein.TitanicPlainsBisons", "Titanic Plains Bisons", "1.1.1")]
    [R2API.Utils.R2APISubmoduleDependency(nameof(DirectorAPI))]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync,VersionStrictness.DifferentModVersionsAreOk)]
    public class TitanicPlainsBisons : BaseUnityPlugin
    {
        public void Awake()
        {
            bool loopOnly = base.Config.Bind<bool>(new ConfigDefinition("Settings", "Loop Only"), true, new ConfigDescription("Bisons only start spawning on Titanic Plains after looping.")).Value;
            DirectorCard bisonDC = new DirectorCard
            {
                spawnCard = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscBison"),
                selectionWeight = 1,
                allowAmbushSpawn = true,
                preventOverhead = false,
                minimumStageCompletions =  loopOnly ? 5 : 0,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard
            };
            DirectorAPI.DirectorCardHolder bisonCard = new DirectorAPI.DirectorCardHolder
            {
                Card = bisonDC,
                MonsterCategory = DirectorAPI.MonsterCategory.Minibosses,
                InteractableCategory = DirectorAPI.InteractableCategory.None
            };

            DirectorAPI.MonsterActions += delegate (List<DirectorAPI.DirectorCardHolder> list, DirectorAPI.StageInfo stage)
            {
                switch (stage.stage)
                {
                    case DirectorAPI.Stage.TitanicPlains:
                        if (!list.Contains(bisonCard))
                        {
                            list.Add(bisonCard);
                        }
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
