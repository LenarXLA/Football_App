namespace Football_App.Service
{
    public interface IWindowService
    {
        void OpenAddClubWindow();
        void CloseAddClubWindow();
        
        void OpenPlayersListWindow();
        
        void OpenAddPlayerToClubWindow();
        void CloseAddPlayerToClubWindow();
        
        void OpenAddFootballPlayerWindow();
        void CloseAddFootballPlayerWindow();

        void OpenEditPlayerClubWindow();
        void CloseEditPlayerClubWindow();

        void OpenAddFanWindow();
        void CloseAddFanWindow();

        void OpenFanListWindow();

        void OpenEditFanClubWindow();
        void CloseEditFanClubWindow();
    }
}