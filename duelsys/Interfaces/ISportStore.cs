﻿namespace duelsys.Interfaces;

public interface ISportStore
{
    List<Sport> GetSports();
    void SaveSport(string name, int minPlayers, int maxPlayers);
}

