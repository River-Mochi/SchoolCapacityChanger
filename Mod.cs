// Mod.cs
// Entry for "[SCC] School Capacity Changer".
// Supports multiple locales; only en-US and zh-HANS enabled now.

namespace SchoolCapacityChanger
{
    using System.Collections.Generic;
    using Colossal;
    using Colossal.IO.AssetDatabase;
    using Colossal.Logging;
    using Game;
    using Game.Modding;
    using Game.SceneFlow;

    public sealed class Mod : IMod
    {
        public const string ModName = "[SCC] School Capacity Changer";
        public const string VersionShort = "1.0.0";

        public static readonly ILog Log =
            LogManager.GetLogger("SchoolCapacityChanger").SetShowsErrorsInUI(false);

        public static Setting? Setting
        {
            get; private set;
        }

        // prevent double locale install
        private static readonly HashSet<string> s_InstalledLocales = new();

        public void OnLoad(UpdateSystem updateSystem)
        {
            Log.Info($"{ModName} v{VersionShort} OnLoad");

            // settings first
            var setting = new Setting(this);
            Setting = setting;

            // locales BEFORE register options
            AddLocale("en-US", new LocaleEN(setting));
            AddLocale("zh-HANS", new LocaleZH_CN(setting));

            // future locales (leave commented for now):
            // AddLocale("fr-FR", new LocaleFR(setting));
            // AddLocale("de-DE", new LocaleDE(setting));
            // AddLocale("es-ES", new LocaleES(setting));
            // AddLocale("it-IT", new LocaleIT(setting));
            // AddLocale("ja-JP", new LocaleJA(setting));
            // AddLocale("ko-KR", new LocaleKO(setting));
            // AddLocale("vi-VN", new LocaleVI(setting));
            // AddLocale("pl-PL", new LocalePL(setting));
            // AddLocale("pt-BR", new LocalePT_BR(setting));
            // AddLocale("zh-HANT", new LocaleZH_HANT(setting));

            // load saved settings
            AssetDatabase.global.LoadSettings("SchoolCapacityChanger", setting, new Setting(this));

            // now show in Options UI
            setting.RegisterInOptionsUI();

            // run our system early in prefab phases so baseline is ready
            updateSystem.UpdateBefore<SchoolCapacityChangerSystem>(SystemUpdatePhase.PrefabUpdate);
            updateSystem.UpdateBefore<SchoolCapacityChangerSystem>(SystemUpdatePhase.PrefabReferences);

            // re-register locales on locale change
            var lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.onActiveDictionaryChanged -= OnLocaleChanged;
                lm.onActiveDictionaryChanged += OnLocaleChanged;
            }
        }

        public void OnDispose()
        {
            var lm = GameManager.instance?.localizationManager;
            if (lm != null)
            {
                lm.onActiveDictionaryChanged -= OnLocaleChanged;
            }

            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }

            Log.Info("OnDispose");
        }

        private void OnLocaleChanged()
        {
            var lm = GameManager.instance?.localizationManager;
            var active = lm?.activeLocaleId;
            if (string.IsNullOrEmpty(active))
            {
                return;
            }

            // make sure active language has our keys
            EnsureLocaleInstalled(active!);

            // keep options refreshed
            Setting?.RegisterInOptionsUI();
#if DEBUG
            Log.Info($"[SCC] Locale changed → {active}");
#endif
        }

        private static void AddLocale(string localeId, IDictionarySource source)
        {
            var lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                Log.Warn("No LocalizationManager; cannot add locale source.");
                return;
            }

            if (!s_InstalledLocales.Add(localeId))
            {
                return;
            }

            lm.AddSource(localeId, source);
        }

        private static void EnsureLocaleInstalled(string localeId)
        {
            // If user switches to zh-HANS in game, we want our zh-HANS source available.
            // Here we only know about the two installed above.
            var lm = GameManager.instance?.localizationManager;
            if (lm == null)
            {
                return;
            }

            if (!s_InstalledLocales.Contains(localeId))
            {
                if (localeId == "zh-HANS" && Setting != null)
                {
                    lm.AddSource("zh-HANS", new LocaleZH_CN(Setting));
                    s_InstalledLocales.Add("zh-HANS");
                }
            }
        }
    }
}
