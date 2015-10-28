using strange.extensions.mediation.impl;
using UnityEngine;
using BibaFramework.BibaMenu;

namespace BibaFramework.BibaGame
{
    public class ARScanStartMediator : BaseObjectMenuStateMediator 
	{
        public override BaseObjectMenuStateView BaseObjectMenuStateView { get { return ARScanStartView; } }

        [Inject]
        public ARScanStartView ARScanStartView { get; set; }
        
        [Inject]
        public IBibaTagService BibaTagService { get; set; }
        
        [Inject]
        public TagScannedSignal TagScannedSignal { get; set; }
        
        [Inject]
        public BibaSessionModel BibaSessionModel { get; set; }

        [Inject]
        public BibaGameModel BibaGameModel { get; set; }
        
        [Inject]
        public SetMenuStateTriggerSignal SetMenuStateTriggerSignal { get; set; }

        [Inject]
        public TagServiceInitFailedSignal TagServiceInitFailedSignal { get; set; }

        [Inject]
        public LogCameraReminderTimeSignal LogCameraReminderTimeSignal { get; set; }

        private BibaTagType _tagToScan;

		public override void OnRegister ()
		{
            base.OnRegister();

            TagScannedSignal.AddListener(TagScanned);
            TagServiceInitFailedSignal.AddListener(TagServiceInitFailed);

            SetupTagToScan();
        }

        public override void OnRemove ()
        {
            base.OnRemove();

            TagScannedSignal.RemoveListener(TagScanned);
            TagServiceInitFailedSignal.RemoveListener(TagServiceInitFailed);
        }

        void OnEnable()
        {
            if (ARScanStartView != null)
            {
                ARScanStartView.SetupCamera();
                BibaTagService.StartScan();
            }
        }

        void OnDisable()
        {
            if (ARScanStartView != null)
            {
                BibaTagService.StopScan();
                ARScanStartView.DestroyCamera();
            }
        }

        void SetupTagToScan()
        {
            var rndIndex = Random.Range(0, BibaGameModel.SelectedEquipments.Count);
            var equipmentToScan = new BibaEquipment(BibaGameModel.SelectedEquipments[rndIndex].EquipmentType);
            
            _tagToScan = equipmentToScan.TagType;
            ARScanStartView.SetupTag(_tagToScan);
        }

        void TagScanned(BibaTagType tagType)
        {
            //TODO: implement scanning logic
            Debug.Log(tagType.ToString() + " tag is scanned. We are expecting " + _tagToScan.ToString());
            if (tagType == _tagToScan)
            {
                BibaTagService.StopScan();

                SetMenuStateTriggerSignal.Dispatch(MenuStateTrigger.Yes);
                BibaSessionModel.TagScanned = true;
            }
        }

        void TagServiceInitFailed()
        {
            LogCameraReminderTimeSignal.Dispatch();
            SetMenuStateTriggerSignal.Dispatch(MenuStateTrigger.No);
        }
	} 
}