﻿using System;
using UnityEngine;
using System.Collections.Generic;
using LitJson;

namespace BibaFramework.BibaGame
{
	public class BibaProfile : IResetModel
	{
		public string Id { get; set; }
		public string Nickname { get; set; }
		public byte[] Avatar { get; set; }
		public Gender Gender { get; set; }
		public DateTime Birthday { get; set; }
		public int Points { get; set; }
		public List<string> CompletedPointEvents { get; set; }

		[JsonIgnore]
		public BibaProfileSession BibaProfileSession { get; set; }

		public BibaProfile()
		{
			Reset ();
		}

		public void Reset()
		{
			Id = Guid.NewGuid().ToString ();
			Nickname = string.Empty;
			Gender = Gender.na;

			Points = 0;
			CompletedPointEvents = new List<string> ();
			BibaProfileSession = new BibaProfileSession ();
		}
	}

	public enum Gender
	{
		male,
		female,
		na
	}
}