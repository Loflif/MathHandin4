using UnityEditor;
using UnityEngine;


public class CoilDrawer : MonoBehaviour
{
    public static float TAU = 6.28318f;
    
    [Range(10, 1000)] public int PointsPerRevolution = 50;
    [Range(1, 200)] public int RevolutionCount = 5;
    [Range(0.1f, 30.0f)] public float Height = 1.0f;
    [Range(0.1f, 5.0f)] public float Radius = 0.5f;

    public Color StartColor = Color.red;
    public Color EndColor = Color.green;
    
    private void OnDrawGizmos()
    {
        Vector3[] points = GetCoilVerts(Radius, PointsPerRevolution, RevolutionCount, Height);

        for (int i = 0; i < points.Length; i++)
        {
            Vector3 currentPoint = points[i];
            Vector3 nextPoint = points[(i + 1)%points.Length];
            
            Color drawColor = Color.Lerp(StartColor, EndColor, (float)i / points.Length);
            Handles.color = drawColor;
            Handles.DrawAAPolyLine(currentPoint, nextPoint);
        }
    }

    private Vector3[] GetCoilVerts(float Radius, int PointCountPerRevolution, int RevolutionCount, float Height)
    {
        int pointCount = PointCountPerRevolution * RevolutionCount;
        
        Vector3[] verts = new Vector3[pointCount+1];
        for (int i = 0; i < verts.Length; i++)
        {
            float winding = i / (float) PointCountPerRevolution;
            float length = i / (float) pointCount;
            
            Vector2 torusDirection = AngleToDirection(length * TAU) * Height;
            
            //Point on torus, rotated to fit in 3D space
            Vector3 torusPoint;
            torusPoint.x = torusDirection.x;
            torusPoint.z = -torusDirection.y;
            torusPoint.y = 0.0f;
            
            Vector2 coilDirection = AngleToDirection(winding * TAU) * Radius;

            Vector3 coilPoint;
            coilPoint.x = coilDirection.x;
            coilPoint.y = coilDirection.y;
            coilPoint.z = 0.0f;

            //Rotate coil around circle
            coilPoint = Quaternion.AngleAxis(length * TAU * Mathf.Rad2Deg, Vector3.up) * coilPoint;

            verts[i] = coilPoint + torusPoint;
        }
        
        return verts;
    }
    
    private Vector2 AngleToDirection(float AngleRadians)
    {
        Vector2 direction;
        direction.x = Mathf.Cos(AngleRadians);
        direction.y = Mathf.Sin(AngleRadians);
        return direction;
    }
}
