﻿using UnityEngine;

namespace BibaFramework.BibaMenu
{
	public abstract class BibaMenuState : StateMachineBehaviour 
	{
        public GameScene GameScene;
        public bool Popup;
        public bool LoadingScreen;
		public bool EntryAnimation;
		public bool ExitAnimation;

    	// OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
        {
            animator.GetComponent<BibaMenuStateMachineView>().EnteredMenuState(this);
    	}

    	// OnStateExit is called before OnStateExit is called on any state inside this state machine
    	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
        {
            animator.GetComponent<BibaMenuStateMachineView>().ExitedMenuState(this);
    	}

        public override string ToString()
        {
            return string.Format("[BibaMenuState: GameScene={0}, Popup={1}, EntryAnimation={2}, ExitAnimation={3}]", GameScene, Popup, EntryAnimation, ExitAnimation);
        }
        
	}
}