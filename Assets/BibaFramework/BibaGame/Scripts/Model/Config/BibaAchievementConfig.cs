using System;
using System.Collections.Generic;
using UnityEngine;

namespace BibaFramework.BibaGame
{
    public class BibaAchievementConfig : ScriptableObject
    {   
        public BibaEquipmentType EquipmentType;
        public int TimePlayed;
        public string DescriptionPrefix { get { return _prefixDict [EquipmentType]; } }
        public string DescriptionSuffix;
        public string Description { get { return DescriptionPrefix + " " + DescriptionSuffix; } }

        public virtual string Id {
            get {
                return GenerateId(EquipmentType, TimePlayed);
            }
        }

        public static string GenerateId(BibaEquipmentType equipmentType, int timePlayed)
        {
            return string.Format(BibaGameConstants.BASIC_ACHIEVEMENT_ID_FORMATTED, equipmentType.ToString(), timePlayed.ToString());
        }

        private static readonly Dictionary<BibaEquipmentType, string> _prefixDict = new Dictionary<BibaEquipmentType, string>()
        {
            {  BibaEquipmentType.bridge, BibaGameConstants.ACHIEVEMENT_PREFIX_BRIDGE },
            {  BibaEquipmentType.climber, BibaGameConstants.ACHIEVEMENT_PREFIX_CLIMBER },
            {  BibaEquipmentType.overhang, BibaGameConstants.ACHIEVEMENT_PREFIX_OVERHANG },
            {  BibaEquipmentType.slide, BibaGameConstants.ACHIEVEMENT_PREFIX_SLIDE},
            {  BibaEquipmentType.swing, BibaGameConstants.ACHIEVEMENT_PREFIX_SWING},
            {  BibaEquipmentType.tube, BibaGameConstants.ACHIEVEMENT_PREFIX_TUBE },
        };
    }
}