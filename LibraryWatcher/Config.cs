using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace LibraryWatcher {
    class Config {

        /// <summary>
        /// Checks if the setting with the given key exists in the configuration file
        /// </summary>
        /// <param name="key">Setting Key</param>
        /// <returns>true if key is set, false otherwise</returns>
        public static bool isSettingSet(string key) {
            foreach (SettingsProperty curProperty in Properties.Settings.Default.Properties) {
                if (curProperty.Name.Equals(key)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the setting value for key is valid (not empty)
        /// </summary>
        /// <param name="key">Setting Key</param>
        /// <returns>true if setting is set, and valid. false otherwise</returns>
        public static bool isValidSettingValue(string key) {
            if (isSettingSet(key)) {
                return !((string) Properties.Settings.Default[key]).Equals("");
            }
            return false;
        }

    }
}
