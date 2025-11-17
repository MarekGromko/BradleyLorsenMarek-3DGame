using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotion : MonoBehaviour
{
    [SerializeField] private float lookMotionSensitivity= 0.1f;
    [SerializeField] private float backwardSlowFactor = 0.5f;
    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float jumpStrenght = 3f;
    [SerializeField] private GameObject mainCamera;
    private Animator camAnim;
    private Vector2 lookRotation;
    private Rigidbody rb;
    private Vector3 shouldMove;
    private float shouldJump = 0.0f;
    private bool shouldSprint = false;
    private bool isGrounded = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        lookRotation = mainCamera.transform.rotation.eulerAngles;
        if(!mainCamera.TryGetComponent(out camAnim))
            throw new Exception("Main camera must have an Animator component.");
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void OnCollisionStay(Collision collision)
    {
        if(rb.linearVelocity.y*rb.linearVelocity.y > .1f)
        {
            isGrounded = false;
            return;
        }
        foreach (ContactPoint contact in collision.contacts)
        {
            if(Vector3.Dot(Vector3.up, contact.normal) > 0.5)
            {
                isGrounded = true;
                break;
            }
        }
        
    }
    void OnLook(InputValue value)
    {
        var delta = value.Get<Vector2>();
        lookRotation.x += -delta.y * lookMotionSensitivity;
        lookRotation.y += delta.x * lookMotionSensitivity;
    }
    void OnMove(InputValue value)
    {
        shouldMove = value.Get<Vector2>().normalized;
        PropagateCameraMove();
    }
    void OnSprint(InputValue value) {
        shouldSprint = value.Get<float>() > 0.5;
        PropagateCameraMove();
    }
    void OnJump(InputValue value)
    {
        shouldJump = 1.0f;
    }
    void DefaultUpdate()
    {
        if(shouldMove.sqrMagnitude > 0.0001)
        {
            var right   = mainCamera.transform.right;
            var forward = Vector3.Cross(right, Vector3.up).normalized;
            var moveDir = (forward * shouldMove.y) + (right * shouldMove.x);
            var speed   = ComputeSpeed();
            rb.MovePosition(rb.position + (Time.deltaTime * speed * moveDir));
        }
        if(shouldJump >= 1.0f)
        {
            if(isGrounded)
            {
                rb.AddForce(Vector3.up * jumpStrenght, ForceMode.VelocityChange);
                shouldJump = 0;
                isGrounded = false;
            }
            shouldJump += Time.deltaTime;
            if(shouldJump >= 1.2)
                shouldJump = 0;
        }
    }
    void Update()
    {
        var lookAt = Quaternion.Euler(lookRotation);
        mainCamera.transform.rotation = lookAt;
        //
        DefaultUpdate();
    }
    private float ComputeSpeed()
    {
        float speed = shouldSprint ? sprintSpeed : walkingSpeed;
        float backFactor = (-shouldMove.y)*.5f + .5f;
        backFactor = 1f-Mathf.Pow(1-backFactor, 4);
        speed *= 1-(backFactor*backwardSlowFactor);
        return speed;
    }
    private void PropagateCameraMove()
    {
        var willMove = shouldMove.sqrMagnitude > 0.001;
        camAnim.SetBool("isWalking", willMove && !shouldSprint);
        camAnim.SetBool("isSprinting",  willMove && shouldSprint);
    }
}
