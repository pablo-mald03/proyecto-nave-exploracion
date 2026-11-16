using System.Collections.Generic;

namespace NaveExploracion.Data
{
    public struct MissionEntry
    {
        public string MissionId;
        public string Title;
        public string BriefingText;
        public string TargetSceneName;

        public MissionEntry(string missionId, string title, string briefingText, string targetSceneName)
        {
            MissionId = missionId;
            Title = title;
            BriefingText = briefingText;
            TargetSceneName = targetSceneName;
        }
    }

    public static class MissionDatabase
    {
        public const string MISSION_01 = "MISSION_01";
        public const string MISSION_02 = "MISSION_02";
        public const string MISSION_03 = "MISSION_03";

        public static readonly Dictionary<string, MissionEntry> Entries = new Dictionary<string, MissionEntry>
        {
            {
                MISSION_01,
                new MissionEntry(
                    MISSION_01,
                    "MISSION 01: COLLECT ICE",
                    "Choose where to search for lunar ice: in areas that receive daily sunlight, or in the dark depths at the south pole.",
                    "cave_scene_final"
                )
            },
            {
                MISSION_02,
                new MissionEntry(
                    MISSION_02,
                    "MISIÓN 02: RECOLECCIÓN DE REGOLITO",
                    "Despliega el protocolo de extracción en el cráter norte para obtener muestras minerales de escudo térmico.",
                    "Mission_02_Scene"
                )
            },
            {
                MISSION_03,
                new MissionEntry(
                    MISSION_03,
                    "MISIÓN 03: COLLECT REGOLYT",
                    "Sincroniza la antena de alta ganancia con la órbita terrestre para transmitir los datos científicos de Shackleton Camp.",
                    "Mission_03_Scene"
                )
            }
        };

        public static MissionEntry GetMission(string missionId)
        {
            if (Entries.TryGetValue(missionId, out MissionEntry entry))
            {
                return entry;
            }

            return new MissionEntry(
                missionId,
                "NOT REGISTERED MISSION",
                $"Mission with identifier: {missionId}. Was not found",
                string.Empty
            );
        }
    }
}