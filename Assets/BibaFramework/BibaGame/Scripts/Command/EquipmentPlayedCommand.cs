using System;
using strange.extensions.command.impl;

namespace BibaFramework.BibaGame
{
    public class EquipmentPlayedCommand : Command
    {
        [Inject]
        public BibaEquipmentType BibaEquipmentType { get; set; }

        [Inject]
		public BibaDevice BibaDevice { get; set; }

        [Inject]
        public IDataService DataService { get; set; }

        public override void Execute ()
        {
			var equipment = BibaDevice.PlayedEquipments.Find(equip => equip.EquipmentType == BibaEquipmentType);
			if (equipment == null) 
			{
				throw new ArgumentNullException ();
			}
			equipment.Play();

			DataService.Save();
        }
    }
}