using duelsys.Interfaces;

namespace duelsys.Services
{
    public class TournamentService
    {
        public ITournamentStore TournamentStore { get; private set; }
        public IMatchStore MatchStore { get; private set; }

        public TournamentService(ITournamentStore tournamentStore, IMatchStore matchStore)
        {
            TournamentStore = tournamentStore;
            MatchStore = matchStore;
        }

        public List<TournamentBase> GetTournaments() => TournamentStore.GetTournaments();
        public Tournament GetTournamentById(int id) => TournamentStore.GetTournamentById(id);
        public Tournament EditTournament(bool isAdmin, TournamentBase t, int tsId)
        {
            if (!isAdmin)
                throw new Exception("User must be an admin in order to edit update");


            if (!TournamentStore.UpdateTournamentById(t, tsId))
                throw new Exception("There was an error updating the tournament");

            return TournamentStore.GetTournamentById(t.Id);
        }

        public void GenerateSchedule(MatchPair mp, int tId)
        {
            var t = TournamentStore.GetTournamentById(tId);

        }
    }
}
