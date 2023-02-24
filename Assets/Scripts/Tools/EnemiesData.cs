using System;
using System.Collections.Generic;

public class EnemiesData
{
    public List<string> _killedGreatEnemies;
    public List<string> _killedLocationEnemies;

    public EnemiesData(List<string> killedGreat, List<string> killedLocation)
    {
        _killedGreatEnemies = killedGreat;
        _killedLocationEnemies = killedLocation;
    }

    public void AddKilled(string killedHesh, bool isGreat)
    {
        if (_killedGreatEnemies.Contains(killedHesh)
            || _killedLocationEnemies.Contains(killedHesh))
        {
            throw new System.Exception("Tryed to add already killed foe: " + killedHesh);
        }

        if (isGreat)
        {
            _killedGreatEnemies.Add(killedHesh);
        }
        _killedLocationEnemies.Add(killedHesh);
    }

    public void UpdateLocation()
    {
        _killedLocationEnemies = new List<string>();
    }

    public IEnumerator<string> GetKilled()
    {
        List<string> killed = new List<string>();
        killed.AddRange(_killedGreatEnemies);
        killed.AddRange(_killedLocationEnemies);

        return killed.GetEnumerator();
    }
}
