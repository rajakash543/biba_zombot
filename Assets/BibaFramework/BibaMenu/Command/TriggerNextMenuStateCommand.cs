using strange.extensions.command.impl;
using UnityEngine;

namespace BibaFramework.BibaMenu
{
    public class TriggerNextMenuStateCommand : Command
    {
        [Inject]
        public MenuStateTrigger MenuStateTrigger { get; set; }

        [Inject(BibaConstants.BIBA_STATE_MACHINE)]
        public Animator StateMachine { get; set; }

        public override void Execute ()
        {
            StateMachine.SetTrigger(MenuStateTrigger.ToString());
        }
    }
}