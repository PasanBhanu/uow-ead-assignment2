namespace Finance_App
{
    internal static class Variables
    {
        private static int CategoryId = 0;
        private static int TransactionId = 0;

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
    }
}
