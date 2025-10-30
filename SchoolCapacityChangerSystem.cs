// Systems/SchoolCapacityChangerSystem.cs
// Applies SCC settings to all school entities.
// Captures original (vanilla) values once per entity and re-applies from baseline.

namespace SchoolCapacityChanger
{
    using System.Collections.Generic;
    using Colossal.Serialization.Entities;
    using Game;
    using Game.Prefabs;
    using Unity.Collections;
    using Unity.Entities;

    public sealed partial class SchoolCapacityChangerSystem : GameSystemBase
    {
        private readonly Dictionary<Entity, BaselineData> m_BaselineByEntity =
            new Dictionary<Entity, BaselineData>(128);

        private EntityQuery m_SchoolQuery;
        private bool m_ReapplyRequested;

        protected override void OnCreate()
        {
            base.OnCreate();

            m_SchoolQuery = GetEntityQuery(
                ComponentType.ReadWrite<SchoolData>(),
                ComponentType.ReadWrite<ConsumptionData>());

            RequireForUpdate(m_SchoolQuery);

            Enabled = false;
        }

        protected override void OnGamePreload(Purpose purpose, GameMode mode)
        {
            base.OnGamePreload(purpose, mode);

            if (mode == GameMode.Game)
            {
                m_ReapplyRequested = true;
                Enabled = true;
            }
        }

        protected override void OnUpdate()
        {
            if (!m_ReapplyRequested)
            {
                Enabled = false;
                return;
            }

            var schools = m_SchoolQuery.ToEntityArray(Allocator.Temp);
            if (!schools.IsCreated || schools.Length == 0)
            {
                schools.Dispose();
                m_ReapplyRequested = false;
                Enabled = false;
                return;
            }

            var setting = Mod.Setting;
            if (setting == null)
            {
                schools.Dispose();
                m_ReapplyRequested = false;
                Enabled = false;
                return;
            }

            for (int i = 0; i < schools.Length; i++)
            {
                var entity = schools[i];

                var schoolData = EntityManager.GetComponentData<SchoolData>(entity);
                var consumptionData = EntityManager.GetComponentData<ConsumptionData>(entity);

                if (!m_BaselineByEntity.TryGetValue(entity, out var baseline))
                {
                    baseline = new BaselineData
                    {
                        StudentCapacity = schoolData.m_StudentCapacity,
                        Upkeep = consumptionData.m_Upkeep,
                        EducationLevel = schoolData.m_EducationLevel
                    };
                    m_BaselineByEntity.Add(entity, baseline);
                }

                var scalar = GetScalar(setting, baseline.EducationLevel);
                schoolData.m_StudentCapacity = (int)(baseline.StudentCapacity * scalar);
                EntityManager.SetComponentData(entity, schoolData);

                if (setting.ScaleUpkeepWithCapacity)
                {
                    consumptionData.m_Upkeep = (int)(baseline.Upkeep * scalar);
                }
                else
                {
                    consumptionData.m_Upkeep = baseline.Upkeep;
                }

                EntityManager.SetComponentData(entity, consumptionData);
            }

            schools.Dispose();

            m_ReapplyRequested = false;
            Enabled = false;
        }

        // === called from Setting.Apply() ===
        public void RequestReapplyFromSettings()
        {
            m_ReapplyRequested = true;
            Enabled = true;
#if DEBUG
            Mod.Log.Info("[SCC] Settings changed → reapply requested.");
#endif
        }

        private static double GetScalar(Setting setting, byte level)
        {
            switch ((SchoolLevel)level)
            {
                case SchoolLevel.Elementary:
                    return setting.ElementarySlider / 100.0;
                case SchoolLevel.HighSchool:
                    return setting.HighSchoolSlider / 100.0;
                case SchoolLevel.College:
                    return setting.CollegeSlider / 100.0;
                case SchoolLevel.University:
                    return setting.UniversitySlider / 100.0;
                default:
                    return 1.0;
            }
        }

        private struct BaselineData
        {
            public int StudentCapacity;
            public int Upkeep;
            public byte EducationLevel;
        }
    }
}
