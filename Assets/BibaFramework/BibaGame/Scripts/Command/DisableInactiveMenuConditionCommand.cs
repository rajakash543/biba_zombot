using BibaFramework.BibaMenu;
using strange.extensions.command.impl;
using System;

namespace BibaFramework.BibaGame
{
    public class DisableInactiveMenuConditionCommand : Command
    {
        [Inject]
        public SetMenuStateConditionSignal SetMenuStateConditionSignal { get; set; }

        public override void Execute ()
        {
            SetMenuStateConditionSignal.Dispatch(MenuStateCondition.ShowInactive, false);
        }
    }
}