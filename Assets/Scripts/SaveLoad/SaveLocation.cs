[System.Serializable]
public class SaveLocation
{
    public bool unlocked;
    public bool bossDefeated;
    public SaveLocation()
    {
        unlocked = false;
        bossDefeated = false;
    }
    public SaveLocation(bool _unlocked, bool _bossDefeated)
    {
        unlocked = _unlocked;
        bossDefeated = _bossDefeated;
    }
}
