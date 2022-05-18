namespace mysql
{
    public class MySql
    {
        public string ConnectionUrl { get; }

        public MySql(string connectionUrl)
        {
            ConnectionUrl = connectionUrl;
        }
    }
}