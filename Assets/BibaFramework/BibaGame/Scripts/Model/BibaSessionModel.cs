using System.Collections.Generic;
using System;
using LitJson;
using UnityEngine;

namespace BibaFramework.BibaGame
{
    public class BibaSessionModel
    {
		public SessionInfo SessionInfo { get; set; }
        public bool TagScanned { get; set; }
    }

	public class SessionInfo	
	{
		public string UUID { get; set; }
        public string SessionID { get; set; }
		public string DeviceModel { get; set; }
		public string DeviceOS { get; set; }
        public string QuadTileId { get; set; }
        public Vector2 Location { get; set; }
	}
}