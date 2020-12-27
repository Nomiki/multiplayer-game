using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public bool IsSelf = false;

    private float remoteAngle = 0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(GetAngle(), 90f, -90f));
    }

    private float GetAngle()
    {
        if (IsSelf)
        {
            //Get the Screen positions of the object
            Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the Screen position of the mouse
            Vector2 mouseOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            //Get the angle between the points
            return AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);
        }
        else
        {
            return remoteAngle;
        }
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return 180f - (Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg);
    }

    public void SetRemoteAngle(float angle)
    {
        this.remoteAngle = angle;
    }
}
