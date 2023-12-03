namespace Transactions.Common
{
    public class Constants
    {
        public static readonly string AccountsDbConnection = "DataConnection";

        //Procedures constants
        public static string TRANSACTION_GET_LIST = "dbo.transaction_get_list";
        public static readonly string TRANSACTION_CREATE = "dbo.transaction_create";

        //Configuration constants
        public static string QUEUE_SETTINGS = "QueueSettings";
        public static readonly string QUEUE_SETTINGS_ADDRESS = "QueueSettings:Address";
        public static readonly string TRANSACTION_INSERT_PROCESS_QUEUE_SETTINGS_QUEUE = "QueueSettings:TransactionInsertProcessQueueSettings:Queue";
    }
}