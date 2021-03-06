using System;
using System.IO;
using BibaFramework.BibaNetwork;
using UnityEngine;

namespace BibaFramework.BibaGame
{
    public class JSONDataService : IDataService
    {
        [Inject]
		public BibaAccount BibaAccount { get; set; }

		[Inject]
		public BibaDevice BibaDevice { get; set; }

		public void Save()
		{
			WriteAccountModel ();
			WriteDeviceModel ();
		}

		void WriteAccountModel ()
        {
			WriteToDisk(BibaAccount, Path.Combine(Application.persistentDataPath, BibaGameConstants.ACCOUNT_MODEL_DATA_PATH));
        }

		public BibaAccount LoadAccountModel ()
        {
			BibaAccount = ReadFromDisk<BibaAccount>(Path.Combine(Application.persistentDataPath, BibaGameConstants.ACCOUNT_MODEL_DATA_PATH));
			return BibaAccount;
        }

		void WriteDeviceModel ()
		{
			WriteToDisk(BibaDevice, Path.Combine(Application.persistentDataPath, BibaGameConstants.DEVICE_MODEL_DATA_PATH));
		}

		public BibaDevice LoadDeviceModel ()
		{
			BibaDevice = ReadFromDisk<BibaDevice>(Path.Combine(Application.persistentDataPath, BibaGameConstants.DEVICE_MODEL_DATA_PATH));
			return BibaDevice;
		}

        public void WriteToDisk<T>(T objectToWrite, string path)
        {
            Debug.Log(string.Format("Writing: {0} with Hashcode: {1} to Path:{2}", objectToWrite.GetType().Name, objectToWrite.GetHashCode(), path));

            if(!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }

			var jsonStr = JsonUtility.ToJson(objectToWrite);
            File.WriteAllText(path, jsonStr);
        }

        public T ReadFromDisk<T>(string path)
        {
            Debug.Log(string.Format("Reading: {0}", path));

            //Check if file is accessible in the fileSystem
            if (File.Exists(path))
            {
				return JsonUtility.FromJson<T>(File.ReadAllText(path));
            }

            //Check in the Resources folder if not found in the file system
            var textAsset = Resources.Load<TextAsset>(BibaContentConstants.GetRelativePath(Path.GetFileNameWithoutExtension(path)));
            if (textAsset != null && !string.IsNullOrEmpty(textAsset.text))
            {
				return JsonUtility.FromJson<T>(textAsset.text);
            }

            return Activator.CreateInstance<T>(); 
        }
    }
}

