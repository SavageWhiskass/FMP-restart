
using UnityEngine;

// Ensures the GameObject always has a Rigidbody2D component
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The maximum speed the player can move.")]
    [SerializeField] private float moveSpeed = 5f; // Public variable to adjust speed in the Inspector

    // Private references to components
    private Rigidbody2D rb;
    private Vector2 movementInput; // Stores the raw input vector

    // Awake is called when the script instance is being loaded (before Start)
    void Awake()
    {
        // Get and store the Rigidbody2D component attached to this GameObject
        // Caching components like this is good practice for performance
        rb = GetComponent<Rigidbody2D>();

        // Double-check gravity scale is zero, just in case it wasn't set in the Inspector
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // --- Input Handling ---
        // Get raw input values (-1, 0, or 1) for horizontal and vertical axes
        // GetAxisRaw provides immediate response without smoothing
        float horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D keys or Left/Right arrows
        float verticalInput = Input.GetAxisRaw("Vertical");   // W/S keys or Up/Down arrows

        // Store the input in a Vector2
        movementInput = new Vector2(horizontalInput, verticalInput);

        // --- Normalization ---
        // If the magnitude of the input vector is greater than 1 (e.g., diagonal movement),
        // normalize it. This ensures the player moves at the same speed diagonally
        // as they do horizontally or vertically.
        if (movementInput.sqrMagnitude > 1)
        {
            movementInput.Normalize();
            // Alternatively, you can normalize directly: movementInput = movementInput.normalized;
            // However, checking sqrMagnitude first avoids unnecessary calculations when input is zero or purely axial.
        }
    }

    // FixedUpdate is called at a fixed interval, independent of frame rate (ideal for physics)
    void FixedUpdate()
    {
        // --- Applying Movement ---
        // Calculate the desired velocity based on input and move speed
        Vector2 targetVelocity = movementInput * moveSpeed;

        // Set the Rigidbody2D's velocity directly. 
        // This provides responsive, physics-based movement.
        rb.linearVelocity = targetVelocity;
    }
}

