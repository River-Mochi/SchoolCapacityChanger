// Settings/Setting.cs
// Options for "[SCC] School Capacity Changer".
// Uses original CO-style names so LocaleEN/LocaleZH_CN map 1:1.

namespace SchoolCapacityChanger
{
    using Colossal.IO.AssetDatabase;
    using Game.Modding;
    using Game.Settings;
    using Game.UI;
    using Unity.Entities;

    [FileLocation("ModsSettings/SchoolCapacity/SchoolCapacity")]
    [SettingsUIGroupOrder(CapacityGroup, OtherOptionsGroup)]
    [SettingsUIShowGroupName(CapacityGroup)]
    public sealed class Setting : ModSetting
    {
        // Tabs / sections
        public const string CapacitySection = "Modify School Capacity";
        public const string ResetSection = "Reset";

        // Groups
        public const string CapacityGroup = "Student Capacity";
        public const string OtherOptionsGroup = "Additional Options";

        private bool m_ScaleUpkeepWithCapacity = true;

        public Setting(IMod mod)
            : base(mod)
        {
            // fresh save → seed defaults
            if (ElementarySlider == 0)
            {
                SetDefaults();
            }
        }

        public override void Apply()
        {
            base.Apply();

            var world = World.DefaultGameObjectInjectionWorld;
            if (world == null)
            {
                return;
            }

            var system = world.GetExistingSystemManaged<SchoolCapacityChangerSystem>();
            if (system != null)
            {
                // this is from your working System version
                system.RequestReapplyFromSettings();
            }
        }

        // ===== Capacity sliders =====

        [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(CapacitySection, CapacityGroup)]
        public int ElementarySlider
        {
            get; set;
        }

        [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(CapacitySection, CapacityGroup)]
        public int HighSchoolSlider
        {
            get; set;
        }

        [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(CapacitySection, CapacityGroup)]
        public int CollegeSlider
        {
            get; set;
        }

        [SettingsUISlider(min = 10, max = 500, step = 5, scalarMultiplier = 1, unit = Unit.kPercentage)]
        [SettingsUISection(CapacitySection, CapacityGroup)]
        public int UniversitySlider
        {
            get; set;
        }

        // this was hidden in the original, but you can unhide later if you want
        [SettingsUISection(CapacitySection, CapacityGroup)]
        [SettingsUIHidden]
        public bool ScaleUpkeepWithCapacity
        {
            get => m_ScaleUpkeepWithCapacity;
            set
            {
                m_ScaleUpkeepWithCapacity = value;
                // same trick the original used
                Extraneous = !value;
            }
        }

        // ===== Reset buttons (on the Reset tab/section) =====

        [SettingsUISection(CapacitySection, ResetSection)]
        [SettingsUIButton]
        public bool ResetToVanilla
        {
            set
            {
                SetToVanilla();
                Apply();
            }
        }

        [SettingsUISection(CapacitySection, ResetSection)]
        [SettingsUIButton]
        public bool ResetToModDefault
        {
            set
            {
                SetDefaults();
                Apply();
            }
        }

        [SettingsUIHidden]
        public bool Extraneous
        {
            get; set;
        }

        public override void SetDefaults()
        {
            // SCC recommended defaults
            ElementarySlider = 200;
            HighSchoolSlider = 120;    // you had 120 in the newer version
            CollegeSlider = 125;
            UniversitySlider = 125;
            ScaleUpkeepWithCapacity = true;
            Extraneous = false;
        }

        public void SetToVanilla()
        {
            ElementarySlider = 100;
            HighSchoolSlider = 100;
            CollegeSlider = 100;
            UniversitySlider = 100;
            ScaleUpkeepWithCapacity = true;
        }
    }
}
