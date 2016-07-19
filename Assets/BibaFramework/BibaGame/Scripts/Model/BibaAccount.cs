﻿using System.Collections.Generic;
using LitJson;
using System;

namespace BibaFramework.BibaGame
{
	public class BibaAccount
	{
		public string Id { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }

		public BibaProfile SelectedProfile { get; set; }

		[JsonIgnore]
		public int TotalPoints {
			get {
				var result = 0;
				foreach (var profile in BibaProfiles) 
				{
					result += profile.Points;
				}
				return result;
			}
		}

		public void ResetLMVScores()
		{
			BibaProfiles.ForEach (profile => {
				profile.LScore = 0;
				profile.MScore = 0;
				profile.VScore = 0;
			});
		}

		public List<BibaProfile> BibaProfiles { get; set; }

		public BibaAccount()
		{
			SelectedProfile = new BibaProfile ();
			SelectedProfile.Id = Guid.NewGuid ().ToString ();

			BibaProfiles = new List<BibaProfile> ();
			BibaProfiles.Add(SelectedProfile);
		}
	}
}