public static class PlayerDeathCounter
{
    public static uint DeathCount { get; private set; }

    public static void IncreaseDeathCount()
    {
        DeathCount++;
    }

    public static void ResetDeathCount()
    {
        DeathCount = 0;
    }
}
