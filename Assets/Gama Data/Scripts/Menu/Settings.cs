using BCTSTool.Localization;

public static class Settings
{
    public static float s_Sensetivity = 200;
    public static float s_MaxSensetivity = 400;

    public static LenguageLocalization s_Lenguage { get; private set; }

    public static void SetSensetivity(float value)
    {
        if (value > 1)
            value = 1;
        else if(value < 0)
            value = 0;

        s_Sensetivity = value * s_MaxSensetivity;
    }

    public static void SwichLenguage(LenguageLocalization lenguage)
    {
        if (lenguage == null) return;
        s_Lenguage = lenguage;
    }
}
