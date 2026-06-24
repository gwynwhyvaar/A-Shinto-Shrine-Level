using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Constants;
using Structs;
using UnityEngine;
using Object = System.Object;

public class GameStateInformation
{
    /// <summary>
    /// tag to determine if an NPV has been knocked out ..
    /// </summary>
    public bool HasKnockeOutdNpc { get; set; }

    /// <summary>
    /// field to check or set the disguise status of the player ...
    /// </summary>
    public bool IsInDisguise { get; set; }

    /// <summary>
    /// if the game is over ..
    /// </summary>
    public bool IsGameOver { get; set; }

    /// <summary>
    /// if the level has started ... 
    /// </summary>
    public bool IsLevelStarted { get; set; }

    /// <summary>
    /// list of missions ...
    /// </summary>
    private List<Mission> Missions = new();

    private GameStateInformation()
    {
        // seed data
        SetupMissions();
    }

    private static GameStateInformation _instance;

    private static Object _lock = new();

    public void SetupMissions()
    {
        // set up the missions

        #region Mission #1

        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION1, 
            MissionText = "Attend the play and join in the dance festival",
            Tag = MissionConstants.MISSION1_TAG,
            GameObjectName = "KaguraDenGameObjectIteraction",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION1, 
            MissionText = "Write and leave a prayer",
            Tag = MissionConstants.MISSION1_TAG,
            GameObjectName = "EmaGameItem",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION1, 
            MissionText = "Offer prayers and pay your respects at the shrine",
            Tag = MissionConstants.MISSION1_TAG,
            GameObjectName = "HaidenShrine",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION1, 
            MissionText = "Pray and perform water ablutions at the shrine",
            Tag = MissionConstants.MISSION1_TAG, 
            GameObjectName = "AbolutionShrine_01",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION1, 
            MissionText = "Receive the Abbot's blessing",
            Tag = MissionConstants.MAIN_MISSION_TAG,
            GameObjectName = "Abbot_01",
            MissionId = Guid.NewGuid().ToString("N")
        });

        #endregion

        #region Mission #2

        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION2,
            MissionText = "Subdue a temple monk and don a disguise",
            Tag = MissionConstants.MISSION2_TAG,
            GameObjectName = "HaidenMonk_02",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION2,
            MissionText = "Listen in on the temple monks' discussion",
            Tag = MissionConstants.MISSION2_TAG,
            GameObjectName = "EavesDropSpot",
            MissionId = Guid.NewGuid().ToString("N")
        });
        Missions.Add(new Mission()
        {
            MissionName = MissionConstants.MISSION2,
            MissionText = "Obtain evidence of Oda Nobunaga's corruption",
            Tag = MissionConstants.MAIN_MISSION2_TAG,
            GameObjectName = "FinalEvidence",
            MissionId = Guid.NewGuid().ToString("N")
        });

        #endregion
    }

    public static GameStateInformation Instance
    {
        get
        {
            if (_instance == null)
                lock (_lock)
                {
                    if (_instance == null) _instance = new GameStateInformation();
                }

            return _instance;
        }
    }

    public void RemoveMissions(string gameObjectName, string missionTag)
    {
        Debug.Log($"Removing mission {missionTag} for {gameObjectName}");
        var mission = Missions.Find(x => x.GameObjectName == gameObjectName && x.Tag == missionTag);
        // remove from list ..
        Missions.Remove(mission);
        Debug.Log($"Removed mission {mission.MissionId} for {gameObjectName}");
    }

    public string GetMissionText(string missionTag)
    {
        var missionTextBuilder = new StringBuilder();
        var missionNumber = 1;
        foreach (var mission in Missions)
            if (mission.Tag == missionTag)
            {
                missionTextBuilder.AppendLine($"{missionNumber}. {mission.MissionText}");
                missionNumber++;
            }

        return missionTextBuilder.ToString();
    }

    public string GetMissionTitle(string missionTag)
    {
        return Missions.FirstOrDefault(x => x.Tag == missionTag).MissionName;
    }

    public bool MissionExists(string gameObjectName)
    {
        foreach (var mission in Missions)
        {
            // Debug.Log($"in MissionExists() - Checking {gameObjectName}");
            // Debug.Log($"in MissionExists() - Comparing: {gameObjectName} - {mission.GameObjectName}");
            if (mission.GameObjectName.Equals(gameObjectName))
            {
                // Debug.Log($"in MissionExists() - Comparing Game object: {gameObjectName} - with Mission: {mission.GameObjectName} returned true.");
                return true;
            }
        }
        // Debug.Log($"in MissionExists() - couldnt find any missions for : {gameObjectName} - in {PrintAllMissions()}");
        return false;
        // return Missions.Any(x => x.GameObjectName == gameObjectName);
    }

    /// <summary>
    /// tag to check if the quest -missions are completed ..
    /// </summary>
    /// <param name="missionTag"></param>
    /// <returns></returns>
    public bool IsMissionComplete(string missionTag)
    {
        // Debug.Log($"Missions count {Missions.Count(x => x.Tag == missionTag)} for {missionTag}");
        return Missions != null && Missions.Count(x => x.Tag == missionTag) < 1 && IsLevelStarted;
    }

    /// <summary>
    /// tag to determine if the main mission's are completed ..
    /// </summary>
    /// <param name="missionTag"></param>
    /// <returns></returns>
    public bool IsMainMissionComplete(string missionTag)
    {
        return IsMissionComplete(missionTag);
    }

    /// <summary>
    /// get a count of missions based on a mission tag ..
    /// </summary>
    /// <param name="missionTag"></param>
    /// <returns></returns>
    public int GetMissionCount(string missionTag)
    {
        // Debug.unityLogger.Log($"{nameof(GetMissionCount)} - Mission count {missionTag} for {Missions.Count(x => x.Tag == missionTag)}");
        return Missions.Count(x => x.Tag == missionTag);
    }

    public string PrintAllMissions()
    {
        StringBuilder missionsBuilder = new StringBuilder();
        foreach (var x in Missions)
        {
           // yield return $"{x.GameObjectName}: {x.MissionText}";
            missionsBuilder.AppendLine($"{x.GameObjectName}: {x.Tag}, {x.MissionText}, {x.MissionId}");
        }
        return missionsBuilder.ToString();
    }
}