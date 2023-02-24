using System.Collections.Generic;

public class EnemiesData
{
    private List<string> _killedGreatEnemies;
    private List<string> _killedLocationEnemies;

    public EnemiesData(List<string> killedGreat, List<string> killedLocation)
    {
        _killedGreatEnemies = killedGreat;
        _killedLocationEnemies = killedLocation;
    }

    public void AddKilled(string killedHesh)
    {
        _killedGreatEnemies.Add(killedHesh);
        _killedLocationEnemies.Add(killedHesh);
    }

    public void UpdateLocation()
    {
        _killedLocationEnemies = new List<string>();
    }

    public IEnumerator<string> GetKilledGreat()
    {
        return _killedGreatEnemies.GetEnumerator();
    }

    public IEnumerator<string> GetKilledLocation()
    {
        return _killedLocationEnemies.GetEnumerator();
    }
}
