using System.Collections.Generic;
using System;
using UnityEngine;

namespace BibaFramework.BibaGame
{
	public class BibaDeviceSession : IResetModel
    {
		public DateTime Start { get; set; }

		public string QuadTileId { get; set; }
		public Vector2 Location { get; set; }

		public bool TagEnabled { get; set; }
		public bool TagScanned { get; set; }

		public BibaTagType TagToScan { get; set; }
		public List<BibaEquipment> SelectedEquipments { get; set; }

		public BibaDeviceSession()
		{
			Reset ();
		}

		public void Reset()
		{
			Start = DateTime.UtcNow;
			QuadTileId = string.Empty;
			Location = Vector2.zero;

			TagEnabled = false;
			TagScanned = false;

			TagToScan = BibaTagType.none;
			SelectedEquipments = new List<BibaEquipment> ();
		}
    }
}