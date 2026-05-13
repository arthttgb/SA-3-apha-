using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Referências")]
    public CharacterController controller;
    public Transform cameraHolder;
    public Transform groundCheck;
    public LayerMask groundMask;

    [Header("Movimento")]
    public float speed = 6f;
    public float smoothSpeed = 5f;

    [Header("Pulo e Gravidade")]
    public float gravity = -20f;
    public float jumpForce = 4f;

    [Header("Chão")]
    public float groundDistance = 0.5f;

    [Header("Head Bob")]
    public float bobSpeed = 10f;
    public float bobAmount = 0.05f;

    private Vector3 velocity;
    private bool isGrounded;
    private float bobTimer;
    private Vector3 currentMove;

    void Update()
    {
        // 🟢 Verifica chão
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f; // gruda no chão
        }

        // 🎮 Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 targetMove = transform.right * x + transform.forward * z;

        // 🧊 Movimento suave (inércia)
        currentMove = Vector3.Lerp(currentMove, targetMove * speed, smoothSpeed * Time.deltaTime);

        controller.Move(new Vector3(currentMove.x, 0, currentMove.z) * Time.deltaTime);

        // 🦘 Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // 🌍 Gravidade
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 💥 Colisão lateral (impacto parede)
        if (controller.collisionFlags == CollisionFlags.Sides)
        {
            currentMove *= 0.5f;
        }

        // 🎥 Head Bob
        HandleHeadBob(new Vector3(x, 0, z).magnitude);

        // 🎯 FOV dinâmico
        HandleFOV(x, z);
    }

    void HandleHeadBob(float moveAmount)
    {
        if (isGrounded && moveAmount > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bob = Mathf.Sin(bobTimer) * bobAmount;

            cameraHolder.localPosition = new Vector3(0, 1.6f + bob, 0);
        }
        else
        {
            bobTimer = 0;
            cameraHolder.localPosition = new Vector3(0, 1.6f, 0);
        }
    }

    void HandleFOV(float x, float z)
    {
        Camera cam = Camera.main;

        float targetFOV = 65f;

        if (Mathf.Abs(x) > 0.1f || Mathf.Abs(z) > 0.1f)
        {
            targetFOV = 70f;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, 5f * Time.deltaTime);
    }
}