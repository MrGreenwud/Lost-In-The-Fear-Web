using UnityEngine;

namespace BCTSTool.Math
{
    public static class RectMath
    {
        public static Rect Add(Rect a, Rect b)
        {
            return new Rect(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height);
        }

        public static Rect Substract(Rect a, Rect b)
        {
            return new Rect(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height);
        }

        public static Rect Multiplay(Rect rect, float value)
        {
            return new Rect(rect.x * value, rect.y * value, rect.width * value, rect.height * value);
        }
    }
}
