using BibaFramework.BibaMenu;

namespace BibaFramework.BibaGame
{
	public class MainMediator : BaseSceneBasedMediator
	{
        [Inject]
        public MainView MainView { get; set; }

        public override BaseSceneBasedView View { get { return MainView; } }

        public override void SetupMenu (BibaMenuState menuState)
        {
        }

        public override void RegisterSceneDependentSignals ()
        {
        }
        
        public override void RemoveSceneDependentSignals ()
        {
        }
	}
}