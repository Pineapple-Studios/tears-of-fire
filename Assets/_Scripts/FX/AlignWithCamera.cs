using UnityEngine;

public class AlignWithCamera : MonoBehaviour
{
    void Update()
    {
        Camera cam = Camera.main;

        if (cam != null)
        {
            // Find the direction from this object to the target
            Vector3 direction = cam.transform.position - transform.position;

            // Calculate the rotation angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate this object to face the target
            transform.rotation = Quaternion.AngleAxis(angle, Vector2.right);
        }
    }
}
