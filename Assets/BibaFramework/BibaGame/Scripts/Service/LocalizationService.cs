﻿using System;
using System.Collections.Generic;
using UnityEngine;
using BibaFramework.BibaNetwork;

namespace BibaFramework.BibaGame
{
    public class LocalizationService : BaseSettingsService<Dictionary<string, Dictionary<SystemLanguage, string>>>
    {
        public override string SettingsFileName {
            get {
                return BibaContentConstants.LOCALIZATION_SETTINGS_FILE;
            }
        }

        #region - IContentUpdated
        public override void ReloadContent()
        {
            _settings = new Dictionary<string, Dictionary<SystemLanguage, string>>();
            
            var localizationSettings = DataService.ReadFromDisk<BibaLocalizationSettings>(ContentFilePath);
            foreach (var localization in localizationSettings.Localizations)
            {
                if (!_settings.ContainsKey(localization.Key))
                {
                    _settings.Add(localization.Key, new Dictionary<SystemLanguage, string>());
                }
                
                var localizationKeyDictionary = _settings [localization.Key];
                if (!localizationKeyDictionary.ContainsKey(localization.Language))
                {
                    localizationKeyDictionary.Add(localization.Language, localization.Text);
                } 
                else
                {
                    localizationKeyDictionary [localization.Language] = localization.Text;
                }
            }
        }
        #endregion

        public string GetText(string key)
        {
            if (Settings.ContainsKey(key))
            {
                var keyDict = Settings [key];
                return keyDict.ContainsKey(Application.systemLanguage) ? keyDict [Application.systemLanguage] : keyDict [SystemLanguage.English];
            } 
            else
            {
                return key;
            }
        }
    }
}