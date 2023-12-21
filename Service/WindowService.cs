using System.Linq;
using System.Windows;
using Football_App.View;
using Football_App.ViewModel;

namespace Football_App.Service
{
    public class WindowService : IWindowService
    {

        #region Add new club window
        
        public void OpenAddClubWindow()
        {
            // создаем экземпляр нового окна-добавления клуба
            var window = new AddClubWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        public void CloseAddClubWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<AddClubWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели ClubVM, с помощью статичного clubVmStatic
                ClubVM.ClubVmStatic.LoadData();
                window.Close();
            }
        }

        #endregion
        
        #region Players in club List window

        public void OpenPlayersListWindow()
        {
            // создаем экземпляр нового окна просмотра игроков клуба
            var window = new PlayersListWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        #endregion
        
        #region Link player to club window

        public void OpenAddPlayerToClubWindow()
        {
            // создаем экземпляр нового окна просмотра свободных игроков
            var window = new LinkPlayerWindow();

            // покажем новое окно
            window.ShowDialog();
        }
        
        public void CloseAddPlayerToClubWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<LinkPlayerWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели PlayerVM, с помощью статичного playerVmStatic
                PlayerVM.PlayerVmStatic.UpdatePlayerLists();

                window.Close();
            }
        }

        #endregion
        
        #region Add new football player window

        public void OpenAddFootballPlayerWindow()
        {
            // создаем экземпляр нового окна-добавления футболиста
            var window = new AddPlayerWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        public void CloseAddFootballPlayerWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<AddPlayerWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели PlayerVM, с помощью статичного PlayerVMStatic
                PlayerVM.PlayerVmStatic.UpdateAllPlayerLists();
                window.Close();
            }
        }

        #endregion
        
        #region Edit player club window

        public void OpenEditPlayerClubWindow()
        {
            // создаем экземпляр нового окна изменения привязки клуба
            var window = new LinkClubWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        public void CloseEditPlayerClubWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<LinkClubWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели PlayerVM, с помощью статичного PlayerVMStatic
                PlayerVM.PlayerVmStatic.UpdateAllPlayerLists();
                window.Close();
            }
        }

        #endregion
        
        #region Add new fan window

        public void OpenAddFanWindow()
        {
            // создаем экземпляр нового окна-добавления болельщика
            var window = new AddFanWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        public void CloseAddFanWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<AddFanWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели FanVM, с помощью статичного FanVMVmStatic
                FanVM.FanVMVmStatic.UpdateFanList();
                window.Close();
            }
        }

        #endregion
        
        #region Fan clubs List window

        public void OpenFanListWindow()
        {
            // создаем экземпляр нового окна просмотра избранных клубов болельщика
            var window = new FanListWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        #endregion
        
        #region Edit fan clubs window

        public void OpenEditFanClubWindow()
        {
            // создаем экземпляр нового окна изменения привязки клуба
            var window = new LinkFanWindow();

            // покажем новое окно
            window.ShowDialog();
        }

        public void CloseEditFanClubWindow()
        {
            // получим ссылку на текущее окно
            var window = Application.Current.Windows.OfType<LinkFanWindow>().SingleOrDefault(a => a.IsActive);
            
            // закрываем окно
            if (window != null)
            {
                // обновляем список во вьюмодели FanVM, с помощью статичного FanVMVmStatic
                FanVM.FanVMVmStatic.UpdateFanClubLists();
                window.Close();
            }
        }

        #endregion

    }
}