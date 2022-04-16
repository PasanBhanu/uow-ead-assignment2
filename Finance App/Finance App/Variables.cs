using Finance_App.Api;

namespace Finance_App
{
    internal static class Variables
    {
        private static int CategoryId = 0;
        private static int TransactionId = 0;
        private static bool ServerOnline = false;
        private static bool PendingSync = false;

        public static int GetCategoryId()
        {
            CategoryId = CategoryId - 1;
            return CategoryId;
        }

        public static int GetTransactionId()
        {
            TransactionId = TransactionId - 1;
            return TransactionId;
        }

        public static void SetServerStatus(bool status)
        {
            ServerOnline = status;
        }

        public static bool CheckServerHealth()
        {
            HealthApiClient client = new HealthApiClient();
            ServerOnline = client.GetHealth();
            return ServerOnline;
        }

        public static void SetPendingSync()
        {
            PendingSync = true;
        }

        public static bool CanGoOnline()
        {
            return !PendingSync;
        }

        public static bool IsAppOnline()
        {
            if (ServerOnline && !PendingSync)
            {
                return true;
            }

            return false;
        }
    }
}
