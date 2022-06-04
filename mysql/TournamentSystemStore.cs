﻿using duelsys;
using MySql.Data.MySqlClient;

namespace mysql
{
    public class TournamentSystemStore : MySql
    {
        public TournamentSystemStore(string connectionUrl) : base(connectionUrl)
        {
        }

        public List<TournamentSystem> GetTournamentSystems()
        {
            const string query = "SELECT id, name FROM tournament_system";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query);

            if (!reader.HasRows) return new();

            var ts = new List<TournamentSystem>();
            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);

                ts.Add(TournamentSystemFactory.Create(name, id));
            }
            return ts;
        }
        public TournamentSystem GetTournamentSystemById(int id)
        {
            const string query = "SELECT id, name FROM tournament_system WHERE id = @id";
            using var reader = MySqlHelper.ExecuteReader(ConnectionUrl, query, new MySqlParameter[] { new("id", id) });

            if (!reader.HasRows) return null;

            reader.Read();
            var tsId = reader.GetInt32(0);
            var name = reader.GetString(1);
            return TournamentSystemFactory.Create(name, tsId);
        }

    }
}

