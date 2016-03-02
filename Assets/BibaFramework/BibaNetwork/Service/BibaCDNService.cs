using System;
using System.IO;
using UnityEngine;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using BibaFramework.BibaGame;
using BibaFramework.BibaNetwork;
using LitJson;

namespace BibaFramework.BibaNetwork
{
    public class BibaCDNService : ICDNService, IContentUpdated
    {
        [Inject]
        public IDataService DataService { get; set; }
       
        [Inject]
        public ContentUpdatedFromCDNSignal ContentUpdatedFromCDNSignal { get; set; }

        private AWSCredentials _credentials;
        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    _credentials = new CognitoAWSCredentials(BibaContentConstants.AWS_IDENTITY_POOL_ID, RegionEndpoint.USEast1);
                }
                return _credentials;
            }
        }
        
        private IAmazonS3 _s3Client;
        private IAmazonS3 Client
        {
            get
            {
                if (_s3Client == null)
                {
                    _s3Client = new AmazonS3Client(Credentials, RegionEndpoint.USEast1);
                }
                return _s3Client;
            }
        }
       
        private string S3BucketName { get { return BibaContentConstants.CI_GAME_ID; } }

        private BibaManifest _localManifest;

        #region - IContentUpdated
        public void ReloadContent ()
        {
            _localManifest =  DataService.ReadFromDisk<BibaManifest>(ContentFilePath);
        }

        public string ContentFilePath 
        {
            get 
            {
                return ShouldLoadFromResources ?  BibaContentConstants.GetResourceContentFilePath (BibaContentConstants.MANIFEST_FILENAME) : 
                        BibaContentConstants.GetPersistedPath(BibaContentConstants.MANIFEST_FILENAME);
            }
        }

        public bool ShouldLoadFromResources 
        {
            get 
            {
                var persistedManifest = DataService.ReadFromDisk<BibaManifest>(BibaContentConstants.GetPersistedPath(BibaContentConstants.MANIFEST_FILENAME));
                var resourceManifest = DataService.ReadFromDisk<BibaManifest>(BibaContentConstants.GetResourceContentFilePath(BibaContentConstants.MANIFEST_FILENAME));
                return (persistedManifest == null || persistedManifest.Version <= resourceManifest.Version);
            }
        }
        #endregion

        #region - ICDNService
        public void UpdateFromCDN()
        {
            ReloadContent();
            RetrieveAndWriteData(BibaContentConstants.GetRelativePath(BibaContentConstants.MANIFEST_FILENAME), BibaContentConstants.GetPersistedPath(BibaContentConstants.MANIFEST_FILENAME), ManifestRetrieved);
        }
        #endregion

        void ManifestRetrieved(string remoteManifestString)
        {
            if (string.IsNullOrEmpty(remoteManifestString))
            {
                return;
            }

            var remoteManifest = DataService.ReadFromDisk<BibaManifest>(BibaContentConstants.GetPersistedPath(BibaContentConstants.MANIFEST_FILENAME));
            if (remoteManifest != null && remoteManifest.Version > _localManifest.Version)
            {
                foreach (var remoteLine in remoteManifest.Lines)
                {
                    var localLine = _localManifest.Lines.Find(line => line.FileName == remoteLine.FileName);
                    if ((localLine == null || localLine.Version < remoteLine.Version) && !remoteLine.OptionalDownload)
                    {
                        RetrieveAndWriteData(BibaContentConstants.GetRelativePath(remoteLine.FileName), BibaContentConstants.GetPersistedPath(remoteLine.FileName));
                    }
                }
                _localManifest = remoteManifest;
            }
        }

        void RetrieveAndWriteData(string objectFileName, string savePath, Action<string> callBack = null)
        {
            Debug.Log(string.Format("fetching {0} from bucket {1}", objectFileName, S3BucketName));

            Client.GetObjectAsync(S3BucketName, objectFileName, (responseObj) => {
                var response = responseObj.Response;
                if (response.ResponseStream != null)
                {
                    using (Stream s = response.ResponseStream)
                    {
                        if(!Directory.Exists(Path.GetDirectoryName(savePath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                        }

                        using (FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] data = new byte[32768];
                            int bytesRead = 0;
                            do
                            {
                                bytesRead = s.Read(data, 0, data.Length);
                                fs.Write(data, 0, bytesRead);
                            } while (bytesRead > 0);
                            fs.Flush();

                            ContentUpdatedFromCDNSignal.Dispatch(objectFileName);
                        }
                    }
                }

                if (callBack != null)
                {
                    callBack(savePath);
                }
            });
        }
    }
}