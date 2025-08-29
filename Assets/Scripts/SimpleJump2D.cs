using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleJump2D : MonoBehaviour, IJumpAbility
{
    [Header("Forces")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float lowJumpMultiplier = 2.5f; // erken tu� b�rak�nca daha h�zl� d��

    [Header("Windows")]
    [SerializeField] private float coyoteTime = 0.1f; // zeminden ayr�ld�ktan sonra k�sa s�re daha z�plama hakk�
    [SerializeField] private float jumpBuffer = 0.1f; // z�pla erken bas�l�rsa k�sa s�re sakla

    [Header("Limits")]
    [SerializeField] private int maxAirJumps = 0; // 0 = sadece yer z�plamas�

    private Rigidbody2D rb;
    private IGroundChecker ground;
    private float lastGroundedTime;
    private float lastJumpPressedTime;
    private int airJumpsUsed;

    public bool CanJump => (Time.time - lastGroundedTime <= coyoteTime) || (airJumpsUsed < maxAirJumps);

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<IGroundChecker>();
    }

    void FixedUpdate()
    {
        // erken tu� b�rakma ile daha k�sa z�plama (better jump feel)
        if (rb.velocity.y > 0f && _wantLowJump)
        {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.fixedDeltaTime);
        }
        _wantLowJump = false;
    }

    private bool _wantLowJump;

    public void TryJump(bool pressed, bool held)
    {
        if (ground != null && ground.IsGrounded)
        {
            lastGroundedTime = Time.time;
            airJumpsUsed = 0;
        }

        if (pressed)
            lastJumpPressedTime = Time.time;

        bool buffered = (Time.time - lastJumpPressedTime) <= jumpBuffer;

        if (buffered && CanJump)
        {
            // Y eksenini s�f�rla (daha tutarl� z�plama)
            Vector2 v = rb.velocity;
            v.y = 0f;
            rb.velocity = v;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (!(ground != null && ground.IsGrounded))
                airJumpsUsed++;

            // buffer�� t�ket
            lastJumpPressedTime = -999f;
        }

        // tu�u erken b�rakt�ysa d���k z�plama iste
        if (!held && rb.velocity.y > 0f)
            _wantLowJump = true;
    }
}
