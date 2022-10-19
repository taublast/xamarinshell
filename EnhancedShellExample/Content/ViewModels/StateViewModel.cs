namespace EnhancedShellExample.Content.ViewModels
{
    /// <summary>
    /// For holding notifications and other globally shared data
    /// </summary>
    public class StateViewModel : BaseViewModel
    {

        public StateViewModel()
        {
            NotificationsSystem = 2;
        }





        public void UpdateControls()
        {

            //StatusBarHeightRequest = CalculateStatusBar(Orientation);
            //NavBarHeightRequest = CalculateNavBar(Orientation);
            //ClippedNavBarHeightRequest = CalculateClippedNavBar(Orientation);
            //PaddingHeightRequest = NavBarHeightRequest + StatusBarHeightRequest;
            //PaddingClippedHeightRequest = ClippedNavBarHeightRequest + StatusBarHeightRequest;

            //BottomTabsHeightRequest = Super.BottomTabsHeight + 3;

            //BottomTabsUnderPadding = BottomTabsHeightRequest - 2;

            //NavAndTabsMargin = new Thickness(0, PaddingHeightRequest, 0, 0);
        }

        private int _NotificationsSystem;
        public int NotificationsSystem
        {
            get { return _NotificationsSystem; }
            set
            {
                if (_NotificationsSystem != value)
                {
                    _NotificationsSystem = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _NotificationsUser;
        public bool NotificationsUser
        {
            get { return _NotificationsUser; }
            set
            {
                if (_NotificationsUser != value)
                {
                    _NotificationsUser = value;
                    OnPropertyChanged();
                }
            }
        }



    }
}
