
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 playerVelocity;
    public float playerSpeed = 200.0f;
    private float hoz, vert;
    public CharacterController _cc;

    public Vector3 wishDirection;
    public Vector3 lastWishDirection;
    public float turnRate = 0.1f; 
    private float turnSmoothVelocity; 

    void Update()
    {
        hoz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        if (vert != 0 || hoz != 0)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();
            Vector3 cameraRight = Camera.main.transform.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            cameraForward *= vert;
            cameraRight *= hoz;

            wishDirection = (cameraForward + cameraRight).normalized;

            // Smooth turning if moving forward or sideways

            float targetAngle = Mathf.Atan2(wishDirection.x, wishDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnRate);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            _cc.Move(wishDirection * Time.deltaTime * playerSpeed);
        }
    }
}
