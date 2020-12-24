using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Ta Daaa
        transform.rotation = Quaternion.Euler(new Vector3(angle, 90f, -90f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return 180f - (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg);
    }
}
