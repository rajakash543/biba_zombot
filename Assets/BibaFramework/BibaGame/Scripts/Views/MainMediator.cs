using BibaFramework.BibaMenu;

namespace BibaFramework.BibaGame
{
	public class MainMediator : SceneMenuStateMediator
	{
        [Inject]
        public MainView MainView { get; set; }

        public override SceneMenuStateView View { get { return MainView; } }

        public override void SetupMenu (BaseMenuState menuState)
        {
        }

        public override void RegisterSceneDependentSignals ()
        {
        }
        
        public override void UnRegisterSceneDependentSignals ()
        {
        }
	}
}