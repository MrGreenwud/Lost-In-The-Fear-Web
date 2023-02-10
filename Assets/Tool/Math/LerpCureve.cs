using UnityEngine;

namespace BCTSTool.Math
{
    public static class LerpCureve
    {
        public static Vector3 ColculatePositions(Vector3 a, Vector3 b, float t, float ampletude)
        {
            Vector3 center = new Vector3(Vector3.Lerp(a, b, 0.5f).x, Vector3.Lerp(a, b, 0.5f).y + ampletude, Vector3.Lerp(a, b, 0.5f).z);

            Vector3 v1 = Vector3.Lerp(a, b, t);
            Vector3 v2 = Vector3.Lerp(center, b, t);
            Vector3 v3 = Vector3.Lerp(v1, v2, t);

            return v3;
        }

        public static void Draw(Vector3 a, Vector3 b, Vector3 startPosition, float ampletude)
        {
            Vector3 firstPosition = startPosition;

            for (int i = 0; i < 20; i++)
            {
                float parameter = (float)i / 20;
                Vector3 point = ColculatePositions(a, b, parameter, ampletude);

                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(firstPosition, point);

                firstPosition = point;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(b, Vector3.one * 0.5f);
        }
    }
}

