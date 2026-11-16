using UnityEngine;

namespace NaveExploracion.Data
{
    public static class MissionSaveManager
    {
        private const string KEY_HAS_SAVE = "SAVE_HAS_DATA";
        private const string KEY_POS_X = "SAVE_POS_X";
        private const string KEY_POS_Y = "SAVE_POS_Y";
        private const string KEY_POS_Z = "SAVE_POS_Z";
        private const string KEY_ROT_Y = "SAVE_ROT_Y";
        private const string KEY_MISSIONS_COUNT = "SAVE_MISSIONS_COMPLETED";

        private const string LAST_MISSION_SCENE_KEY = "LastMissionScene";

        public static bool HasSavedGame() => PlayerPrefs.GetInt(KEY_HAS_SAVE, 0) == 1;

        //Checkpoint saver method
        public static void SaveCheckpoint(Transform playerTransform)
        {
            if (playerTransform == null) return;

            Vector3 pos = playerTransform.position;
            PlayerPrefs.SetFloat(KEY_POS_X, pos.x);
            PlayerPrefs.SetFloat(KEY_POS_Y, pos.y);
            PlayerPrefs.SetFloat(KEY_POS_Z, pos.z);
            PlayerPrefs.SetFloat(KEY_ROT_Y, playerTransform.eulerAngles.y);
            PlayerPrefs.SetInt(KEY_HAS_SAVE, 1);
            PlayerPrefs.Save();
        }

        //Spawn point loader method
        public static void ApplySavedSpawn(Transform playerTransform, Transform defaultSpawnPoint = null)
        {
            if (playerTransform == null) return;

            CharacterController cc = playerTransform.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            if (HasSavedGame())
            {
                Vector3 savedPos = new Vector3(
                    PlayerPrefs.GetFloat(KEY_POS_X),
                    PlayerPrefs.GetFloat(KEY_POS_Y),
                    PlayerPrefs.GetFloat(KEY_POS_Z)
                );
                playerTransform.position = savedPos;
                playerTransform.rotation = Quaternion.Euler(0f, PlayerPrefs.GetFloat(KEY_ROT_Y), 0f);
            }
            else if (defaultSpawnPoint != null)
            {
                playerTransform.position = defaultSpawnPoint.position;
                playerTransform.rotation = defaultSpawnPoint.rotation;
            }

            if (cc != null) cc.enabled = true;
        }

        // hook for your teammate's missions
        public static void MarkMissionCompleted(string missionId)
        {
            string missionKey = $"MISSION_DONE_{missionId}";
            if (PlayerPrefs.GetInt(missionKey, 0) == 0)
            {
                PlayerPrefs.SetInt(missionKey, 1);
                int currentTotal = GetCompletedMissionsCount();
                PlayerPrefs.SetInt(KEY_MISSIONS_COUNT, currentTotal + 1);
                PlayerPrefs.Save();
            }
        }

        public static bool IsMissionCompleted(string missionId)
        {
            return PlayerPrefs.GetInt($"MISSION_DONE_{missionId}", 0) == 1;
        }

        public static int GetCompletedMissionsCount()
        {
            return PlayerPrefs.GetInt(KEY_MISSIONS_COUNT, 0);
        }

        public static void ResetSaveData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public static void SetLastMissionScene(string sceneName)
        {
            PlayerPrefs.SetString(LAST_MISSION_SCENE_KEY, sceneName);
            PlayerPrefs.Save();
        }

        public static string GetLastMissionScene()
        {
            return PlayerPrefs.GetString(LAST_MISSION_SCENE_KEY, "Base_Scene");
        }
    }
}