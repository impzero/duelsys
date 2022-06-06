namespace mysql
{
    public class MySqlStore
    {
        public string ConnectionUrl { get; }

        public MySqlStore(string connectionUrl)
        {
            ConnectionUrl = connectionUrl;
        }
    }
}