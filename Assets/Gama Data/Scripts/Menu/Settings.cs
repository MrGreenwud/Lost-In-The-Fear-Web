public static class Settings
{
    public enum lenguage
    {
        ru,
        en
    }

    public static float s_Sensetivity = 200;
    public static float s_MaxSensetivity = 400;

    public static lenguage s_Lenguage;

    public static void SetSensetivity(float value)
    {
        if (value > 1)
            value = 1;
        else if(value < 0)
            value = 0;

        s_Sensetivity = value * s_MaxSensetivity;
    }

    public static void SwichLenguage(lenguage lenguage)
    {
        s_Lenguage = lenguage;
    }
}
