using UnityEngine;

namespace BCTSTool.Editor
{
    public static class CustemGizmos
    {
        public static void DrowArrow(Vector3 position, Vector3 direction, float headLength = 2f, float headAngle = 20f)
        {
            Vector3 arrowHeadRight = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + headAngle, 0) * new Vector3(0, 0, 1);
            Vector3 arrowHeadLeft = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - headAngle, 0) * new Vector3(0, 0, 1);

            Gizmos.DrawRay(position + direction, arrowHeadRight * headLength);
            Gizmos.DrawRay(position + direction, arrowHeadLeft * headLength);
        }

        public static void DrowWireDisc(Vector3 center, float radius)
        {
            Vector3[] vertex = new Vector3[8];

            float deaganalLength = Mathf.Sqrt(Mathf.Pow(radius / 2, 2) + Mathf.Pow(radius / 2, 2));

            vertex[0] = center + new Vector3(deaganalLength, 0, deaganalLength);
            vertex[1] = center + new Vector3(deaganalLength, 0, -deaganalLength);
            vertex[2] = center + new Vector3(-deaganalLength, 0, -deaganalLength);
            vertex[3] = center + new Vector3(-deaganalLength, 0, deaganalLength);

            vertex[4] = center + new Vector3(radius, 0, 0);
            vertex[5] = center + new Vector3(0, 0, -radius);
            vertex[6] = center + new Vector3(-radius, 0, 0);
            vertex[7] = center + new Vector3(0, 0, radius);

            for (int i = 0; i <= vertex.Length - 4; i++)
            {
                if (i + 4 < vertex.Length)
                {
                    Gizmos.DrawLine(vertex[i], vertex[i + 4]);

                    if (i + 4 < vertex.Length - 1)
                        Gizmos.DrawLine(vertex[i + 1], vertex[i + 4]);
                    else
                        Gizmos.DrawLine(vertex[vertex.Length - 1], vertex[0]);
                }
            }
        }

        public static void DrowLineByMultipleNumber(Vector3 a, Vector3 b, Color posetive, Color negavive, float multipleNumber, float inaccuracy)
        {
            float distence = Vector3.Distance(a, b);

            if (distence % multipleNumber < inaccuracy)
                Gizmos.color = posetive;
            else
                Gizmos.color = negavive;

            Gizmos.DrawLine(a, b);
        }
    }
}