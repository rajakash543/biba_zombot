using strange.extensions.command.impl;
using BibaFramework.BibaData;
using BibaFramework.BibaMenu;

namespace BibaFramework.BibaGame
{
    public class PrivacyStatementAcceptedCommand : Command
    {
        [Inject]
        public BibaGameModel BibaGameModel { get; set; }

        [Inject]
        public IDataService DataService { get; set; }

        [Inject]
        public SetMenuStateConditionSignal SetMenuStateConditionSignal { get; set; }

        public override void Execute ()
        {
            BibaGameModel.PrivacyPolicyAccepted = true;
            DataService.WriteGameModel();

            SetMenuStateConditionSignal.Dispatch(MenuStateCondition.PrivacyAgreementAccepted, true);
        }
    }
}