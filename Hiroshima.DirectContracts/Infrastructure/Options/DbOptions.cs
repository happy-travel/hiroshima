namespace Hiroshima.DirectContracts.Infrastructure.Options
{
    public readonly struct DbOptions
    {
        public DbOptions(string host, int port, string database, string userId, string password)
        {
            Host = host;
            Port = port;
            Database = database;
            UserId = userId;
            Password = password;
        }


        public string Host { get; }
        public int Port { get; }
        public string Database { get; }
        public string UserId { get; }
        public string Password { get; }
    }
}