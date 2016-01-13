﻿using System;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using UnityEditor;
using UnityEngine;

namespace BibaFramework.BibaMenuEditor
{
    public class GoogleSpreadsheetImporter
	{
        // This is the Redirect URI for installed applications.
        // If you are building a web application, you have to set your
        // Redirect URI at https://code.google.com/apis/console.
        private const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        private const string OAUTH_CLIENT_ID = "781246264254-rjj0c39gekl8rp08vqirlakpct731vhg.apps.googleusercontent.com";
        private const string OAUTH_CLIENT_SECRET = "9gnHCPjWS_U6VWdx2ZuTxX-F";

        private const string OAUTH_PARAMETERS_APPLICATION_NAME = "BibaVentures";
        private const string OAUTH_PARAMETERS_SCOPE = "https://www.googleapis.com/auth/drive https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        private const string OAUTH_PARAMETERS_REFRESH_TOKEN = "1/f7h3h6ZA9GN5M8dNp_IMjIdKS6WU5hLuL8osYbfWpAA";
        private const string OAUTH_PARAMETERS_ACCESS_TOKEN = "ya29.ZgKA7DmPkrL0RPGI7u2QCFFIqhk3gq5dMa_A6UShXvkBC1xFPQD4Wlels6Y_ugu8wKLM";
        private const string OAUTH_PARAMETERS_ACCESS_TYPE = "offline";
        private const string OAUTH_PARAMETERS_ACCESS_CODE = "4/z6cJUaIVLc8EgpXZxOhlV-138vfsu37JGNkPCWGX_Pg";
        private const string OAUTH_PARAMETERS_TOKEN_TYPE = "refresh";

        private static SpreadsheetsService _spreadsheetService;

        public static AtomEntryCollection GetListEntries(string spreadsheetName, string worksheetName)
        {
            var spreadsheetFeed = GetSpreadsheetFeed();
            if (spreadsheetFeed.Entries.Count == 0)
            {
                Debug.LogError("There were no spreadsheets. Terminating");
                return null;
            }
            
            var worksheetFeed = GetWorksheetFeed(spreadsheetFeed, spreadsheetName);
            if (worksheetFeed == null || worksheetFeed.Entries.Count == 0)
            {
                Debug.LogError("There were no worksheets. Terminating");
                return null;
            }
            
            var listFeed = GetListFeed(worksheetFeed, worksheetName);
            if (listFeed == null || listFeed.Entries.Count == 0)
            {
                Debug.LogError("There were no entries. Terminating");
                return null;
            }
            return listFeed.Entries;
        }
      
        static SpreadsheetFeed GetSpreadsheetFeed()
        {
            GOAuth2RequestFactory requestFactory = RefreshAuthenticate();
            
            _spreadsheetService = new SpreadsheetsService("SpreadSheet");  
            _spreadsheetService.RequestFactory = requestFactory;

            SpreadsheetQuery query = new SpreadsheetQuery();
            return  _spreadsheetService.Query(query);
        }

        static WorksheetFeed GetWorksheetFeed(SpreadsheetFeed spreadsheetFeed, string spreadsheetTitle)
        {
            SpreadsheetEntry spreadSheetEntry = null;
            foreach (var entry in spreadsheetFeed.Entries)
            {
                if(entry.Title.Text == spreadsheetTitle && entry is SpreadsheetEntry)
                {
                    spreadSheetEntry = (SpreadsheetEntry) entry;
                    break;
                }
            }
            return spreadSheetEntry != null ? spreadSheetEntry.Worksheets : null;
        }

        static ListFeed GetListFeed(WorksheetFeed worksheetFeed, string worksheetTitle)
        {
            WorksheetEntry worksheetEntry = null;
            foreach (var entry in worksheetFeed.Entries)
            {
                if(entry.Title.Text == worksheetTitle && entry is WorksheetEntry)
                {
                    worksheetEntry = (WorksheetEntry) entry;
                    break;
                }
            };

            if (worksheetEntry == null)
            {
                return null;
            }

            AtomLink listFeedLink = worksheetEntry.Links.FindService(GDataSpreadsheetsNameTable.ListRel, null);
            ListQuery listQuery = new ListQuery(listFeedLink.HRef.ToString());
            return _spreadsheetService.Query(listQuery);
        }

        static GOAuth2RequestFactory RefreshAuthenticate() 
        {
            OAuth2Parameters parameters = GetOAuth2Parameters();
            parameters.RefreshToken = OAUTH_PARAMETERS_REFRESH_TOKEN;
            parameters.AccessToken = OAUTH_PARAMETERS_ACCESS_TOKEN;
            
            //  string authUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            return new GOAuth2RequestFactory(null, OAUTH_PARAMETERS_APPLICATION_NAME, parameters);
        }
        
        static OAuth2Parameters GetOAuth2Parameters()
        {
            var parameters = new OAuth2Parameters(){
                ClientId = OAUTH_CLIENT_ID,
                ClientSecret = OAUTH_CLIENT_SECRET,
                Scope = OAUTH_PARAMETERS_SCOPE,
                AccessType = OAUTH_PARAMETERS_ACCESS_TYPE, // IMPORTANT and was missing in the original
                TokenType = OAUTH_PARAMETERS_TOKEN_TYPE // IMPORTANT and was missing in the original
            };
            return parameters;
        }

        /*
        static void RetrieveInitialTokens()
        {
            // OAuth2Parameters holds all the parameters related to OAuth 2.0.
            OAuth2Parameters parameters = GetOAuth2Parameters();
            
            // Set your Redirect URI, which can be registered at
            // https://code.google.com/apis/console.
            parameters.RedirectUri = REDIRECT_URI;
            
            // Get the authorization url.  The user of your application must visit
            // this url in order to authorize with Google.  If you are building a
            // browser-based application, you can redirect the user to the authorization
            // url.
            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Debug.Log(authorizationUrl);
            Debug.Log("Please visit the URL above to authorize your OAuth "
                              + "request token.  Once that is complete, type in your access code to "
                              + "continue...");
            parameters.AccessCode = OAUTH_PARAMETERS_ACCESS_CODE;

            // Once the user authorizes with Google, the request token can be exchanged
            // for a long-lived access token.  If you are building a browser-based
            // application, you should parse the incoming request token from the url and
            // set it in OAuthParameters before calling GetAccessToken().
            OAuthUtil.GetAccessToken(parameters);
            string accessToken = parameters.AccessToken;
            string refreshToken = parameters.RefreshToken;
            Debug.Log("OAuth Access Token: " + accessToken + "\n");
            Debug.Log("OAuth Refresh Token: " + refreshToken + "\n");
        }
        */
	}
}