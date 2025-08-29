using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleDash2D : MonoBehaviour, IDashAbility
{
    [Header("Dash Settings")]
    [SerializeField] private bool enableDash = true;
    [SerializeField] private float dashSpeed = 15f;        // dash boyunca yatay hýz
    [SerializeField] private float dashDuration = 0.18f;   // dash sürer (saniye)
    [SerializeField] private float dashCooldown = 0.6f;    // dashlar arasý bekleme (saniye)

    private Rigidbody2D rb;
    private bool isDashing;
    private float dashTimer;
    private float lastDashTime = -999f;
    private int dashDirection = 1;      // -1 veya +1
    private float lastFacing = 1f;      // son görülen yatay yön (körse +1)

    public bool CanDash => enableDash && (!isDashing) && (Time.time - lastDashTime >= dashCooldown);
    public bool IsDashing => isDashing;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            // Dash boyunca sabit yatay hýz uygula, y hýzýný koru
            rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    /// <summary>
    /// Çaðýran kýsýmdan: TryDash(inputProvider.DashPressed, inputProvider.Horizontal)
    /// </summary>
    public void TryDash(bool pressed, float horizontalInput)
    {
        // Güncelle: son facing (yön) bilgisi
        if (Mathf.Abs(horizontalInput) > 0.01f)
            lastFacing = Mathf.Sign(horizontalInput);

        if (!pressed) return;
        if (!enableDash) return;
        if (!CanDash) return;

        StartDash((int)Mathf.Sign(lastFacing));
    }

    private void StartDash(int dir)
    {
        if (dir == 0) dir = 1;
        dashDirection = dir;
        isDashing = true;
        dashTimer = dashDuration;

        // Baþlangýç anýnda yatay momentumu sýfýrlayýp tutarlý dash uygulamak istersen:
        Vector2 v = rb.velocity;
        v.x = dashDirection * dashSpeed;
        rb.velocity = v;

        // dash baþlama zamaný (cooldown için)
        lastDashTime = Time.time;

        // isteðe baðlý: çarpýþma/yer çekimi vb. davranýþlarý buradan deðiþtirebilirsin
        // örn: rb.gravityScale = 0; // istersen geçici olarak yerçekimini kapat
    }

    private void EndDash()
    {
        isDashing = false;
        // Dash bitince yatay hýzý sýfýrlamak istersen:
        Vector2 v = rb.velocity;
        v.x = 0f;
        rb.velocity = v;

        // Eðer gravityScale deðiþtirdiysen restore et burada
    }

    // Opsiyonel: dash durumunu dýþarýya gösterebilmek için event/metod ekleyebilirsin
}
