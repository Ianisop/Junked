using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 playerVelocity;
    public float playerSpeed = 200.0f;
    private float hoz, vert;
    public CharacterController _cc;
<<<<<<< Updated upstream

=======
    public Vector3 wishDirection;
    public Vector3 lastWishDirection;
    public Vector3 corss;
    public float angCurrent;
    public float angWish;
    private void Start()
    {
       
    }
>>>>>>> Stashed changes

    void Update()
    {
        hoz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        if(Input.anyKey)
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
            _cc.Move(wishDirection * Time.deltaTime * playerSpeed);

            if (vert != 0f || hoz != 0f)
            {
                lastWishDirection = wishDirection;
            }
            var currentRotation = transform.rotation.eulerAngles;
            angCurrent = Vector3.Angle(currentRotation, transform.forward);
            angWish = Vector3.Angle(lastWishDirection, transform.forward);

            currentRotation.y = angWish;
            transform.rotation = Quaternion.Euler(currentRotation);
            transform.LookAt(wishDirection + transform.position);
        }

    }
}