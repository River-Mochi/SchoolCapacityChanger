// Localization/LocaleEN.cs
// English strings for "[SCC] School Capacity Changer" (Options UI).

namespace SchoolCapacityChanger
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting)
        {
            m_Setting = setting;
        }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(
            IList<IDictionaryEntryError> errors,
            Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                // Mod root in Options
                { m_Setting.GetSettingsLocaleID(), "[SCC] School Capacity Changer" },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.CapacitySection), "Main" },
                { m_Setting.GetOptionTabLocaleID(Setting.ResetSection),    "Reset" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.CapacityGroup),     "Student Capacity" },
                { m_Setting.GetOptionGroupLocaleID(Setting.OtherOptionsGroup), "Additional Options" },

                // Sliders
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ElementarySlider)), "Elementary" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ElementarySlider)),  "Set elementary school capacity relative to vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HighSchoolSlider)), "High School" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HighSchoolSlider)),  "Set high school capacity relative to vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CollegeSlider)), "College" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CollegeSlider)),  "Set college capacity relative to vanilla." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.UniversitySlider)), "University" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.UniversitySlider)),  "Set university capacity relative to vanilla." },

                // Toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)), "Scale upkeep with capacity" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)),  "If enabled, upkeep will also be multiplied by the chosen capacity." },

                // Buttons
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToVanilla)), "Reset to Vanilla" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToVanilla)),  "Reset all capacities back to 100%." },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToModDefault)), "Reset to SCC defaults" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToModDefault)),  "Reset all capacities to the mod's recommended values." },
            };
        }

        public void Unload()
        {
        }
    }
}
