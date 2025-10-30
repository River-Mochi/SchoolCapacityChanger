// Localization/LocaleZH_CN.cs
// Simplified Chinese strings for "[SCC] School Capacity Changer" (Options UI).

namespace SchoolCapacityChanger
{
    using System.Collections.Generic;
    using Colossal;

    public sealed class LocaleZH_CN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleZH_CN(Setting setting)
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
                { m_Setting.GetSettingsLocaleID(), "[SCC] 学校容量调整" },

                // Tabs
                { m_Setting.GetOptionTabLocaleID(Setting.CapacitySection), "容量" },
                { m_Setting.GetOptionTabLocaleID(Setting.ResetSection),    "重置" },

                // Groups
                { m_Setting.GetOptionGroupLocaleID(Setting.CapacityGroup),     "学生容量" },
                { m_Setting.GetOptionGroupLocaleID(Setting.OtherOptionsGroup), "其他选项" },

                // Sliders
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ElementarySlider)), "小学" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ElementarySlider)),  "按原版的百分比调整小学容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.HighSchoolSlider)), "中学" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.HighSchoolSlider)),  "按原版的百分比调整中学容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.CollegeSlider)), "学院" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.CollegeSlider)),  "按原版的百分比调整学院容量。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.UniversitySlider)), "大学" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.UniversitySlider)),  "按原版的百分比调整大学容量。" },

                // Toggle
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)), "维护费用随容量变化" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ScaleUpkeepWithCapacity)),  "开启后，维护费用也会用同样的倍率重新计算。" },

                // Buttons
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToVanilla)), "重置为 100%" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToVanilla)),  "把所有容量恢复到原版 100%。" },

                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.ResetToModDefault)), "重置为 SCC 默认值" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.ResetToModDefault)),  "把所有容量恢复为本模组推荐的数值。" },
            };
        }

        public void Unload()
        {
        }
    }
}
