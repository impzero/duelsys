using MySql.Data.MySqlClient;

namespace mysql
{
    public class MySqlStore
    {
        public string ConnectionUrl { get; }

        public MySqlStore(string connectionUrl)
        {
            ConnectionUrl = connectionUrl;
        }

        public void ExecuteInTx(Action<MySqlCommand> fn)
        {
            var mysqlConn = new MySqlConnection(ConnectionUrl);
            mysqlConn.Open();

            var cmd = mysqlConn.CreateCommand();
            var myTrans = mysqlConn.BeginTransaction();

            cmd.Connection = mysqlConn;
            cmd.Transaction = myTrans;
            try
            {
                fn(cmd);
                myTrans.Commit();
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (Exception ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                                          " was encountered while attempting to roll back the transaction.");
                        throw;
                    }
                }
                Console.WriteLine("An exception of type " + e.GetType() +
                                  " was encountered while inserting the data.");
                Console.WriteLine("Neither record was written to database.");
                throw;
            }
            finally
            {
                mysqlConn.Close();
            }
        }
    }
}