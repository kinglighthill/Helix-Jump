using System.Threading;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private float rotationSpeed = 10;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isGameStarted)
            return;

        //if (Input.GetMouseButton(0))
        //{
        //    float mouseX = Input.GetAxisRaw("Mouse X");
        //    transform.Rotate(0, -mouseX * rotationSpeed * 1500 * Time.deltaTime, 0);
        //}

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float xDelta = Input.GetTouch(0).deltaPosition.x;
            transform.Rotate(0, -xDelta * rotationSpeed * Time.deltaTime, 0);
        }
    }
}
