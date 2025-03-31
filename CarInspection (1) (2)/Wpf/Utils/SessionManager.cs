using BusinessObject.Models;

namespace Wpf.Utils
{
    public static class SessionManager
    {
        private static User? _currentUser;

        public static User? CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public static void ClearSession()
        {
            _currentUser = null;
        }

        public static bool IsLoggedIn => _currentUser != null;
    }
}
