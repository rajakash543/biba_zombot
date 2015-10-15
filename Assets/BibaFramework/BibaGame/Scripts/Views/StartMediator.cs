using BibaFramework.BibaMenu;

namespace BibaFramework.BibaGame
{
    public class StartMediator : SceneMenuStateMediator
	{
        [Inject]
        public StartView StartView { get; set; }

        public override SceneMenuStateView View { get { return StartView; } }

        public override void SetupMenu (BaseMenuState menuState)
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