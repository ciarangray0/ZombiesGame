using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    private Rigidbody rb; // Reference to the Rigidbody
    private Animator animator; // Reference to the Animator

    // Start is called before the first frame update
    void Start()
    {
        // Initialize position
        transform.position = new Vector3(0f, 0f, 0f);
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    // Update is called once per frame
    void Update()
    {
        // Get input for rotation
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, -10f * Time.deltaTime, 0f));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, 10f * Time.deltaTime, 0f));
        }
    }

    // FixedUpdate is called on a fixed time interval
    void FixedUpdate()
    {
        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows
        float moveVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrows

        // Create movement vector based on input
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        // Apply movement
        if (movement.magnitude > 0)
        {
            Vector3 moveDirection = movement * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection);

            // Update animator speed if you have animations
            if (animator != null)
            {
                animator.SetFloat("Speed", movement.magnitude);
            }

            // Rotate to face movement direction
            if (movement != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
                rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, toRotation, 720 * Time.fixedDeltaTime));
            }
        }
    }
}

