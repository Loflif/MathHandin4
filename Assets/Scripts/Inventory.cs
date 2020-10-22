using System.Linq;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public float[] itemRadii = new float[3] {0.05f, 0.05f, 0.05f};
    public float arcRadius = 0.5f;
    private int itemCount = 3;

    private void OnDrawGizmos()
    {
        using (new Handles.DrawingScope(transform.localToWorldMatrix))
        {
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.right, 45, arcRadius);
            Handles.DrawWireArc(Vector3.zero, Vector3.forward, Vector3.right, -45, arcRadius);

            itemCount = itemRadii.Length;
            float[] anglesBetweenItems = new float[itemCount - 1];
            for (int i = 0; i < anglesBetweenItems.Length; i++)
            {
                float itemARadius = itemRadii[i];
                float itemBRadius = itemRadii[i + 1];
                float bothItemRadii = itemARadius + itemBRadius;
                anglesBetweenItems[i] = Mathf.Acos(1f - (bothItemRadii * bothItemRadii) / (2 * arcRadius * arcRadius));
            }

            float angleToNextItemRad = -anglesBetweenItems.Sum() / 2f;
            for (int i = 0; i < itemCount; i++)
            {
                float radius = itemRadii[i];
                Vector3 itemCenter = AngleToDirection(angleToNextItemRad) * arcRadius;
                Handles.DrawWireDisc(itemCenter, Vector3.forward, radius);
                if (i < itemCount - 1)
                    angleToNextItemRad += anglesBetweenItems[i];
            }
        }
    }
    
    private Vector2 AngleToDirection(float AngleRadians)
    {
        Vector2 direction;
        direction.x = Mathf.Cos(AngleRadians);
        direction.y = Mathf.Sin(AngleRadians);
        return direction;
    }
}