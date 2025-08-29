using UnityEngine;

public class RaycastGroundChecker2D : MonoBehaviour, IGroundChecker
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float extraRayLength = 0.05f;

    private Rigidbody2D rb;
    public bool IsGrounded { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (groundCheck == null)
        {
            // otomatik altýna küçük bir nokta ekleyelim (istersen sahnede kendin koy)
            GameObject gc = new GameObject("GroundCheck");
            gc.transform.SetParent(transform);
            gc.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheck = gc.transform;
        }
    }

    public void UpdateGrounded()
    {
        bool overlap = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundMask) != null;

        // kýsa bir ray da atalým (faydalý olur)
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down,
                                             checkRadius + extraRayLength, groundMask);

        IsGrounded = overlap || hit.collider != null;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
#endif
}
