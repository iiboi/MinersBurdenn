using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SimpleDash2D : MonoBehaviour, IDashAbility
{
    [Header("Dash Settings")]
    [SerializeField] private bool enableDash = true;
    [SerializeField] private float dashSpeed = 15f;        // dash boyunca yatay h�z
    [SerializeField] private float dashDuration = 0.18f;   // dash s�rer (saniye)
    [SerializeField] private float dashCooldown = 0.6f;    // dashlar aras� bekleme (saniye)

    private Rigidbody2D rb;
    private bool isDashing;
    private float dashTimer;
    private float lastDashTime = -999f;
    private int dashDirection = 1;      // -1 veya +1
    private float lastFacing = 1f;      // son g�r�len yatay y�n (k�rse +1)

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
            // Dash boyunca sabit yatay h�z uygula, y h�z�n� koru
            rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);

            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                EndDash();
            }
        }
    }

    /// <summary>
    /// �a��ran k�s�mdan: TryDash(inputProvider.DashPressed, inputProvider.Horizontal)
    /// </summary>
    public void TryDash(bool pressed, float horizontalInput)
    {
        // G�ncelle: son facing (y�n) bilgisi
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

        // Ba�lang�� an�nda yatay momentumu s�f�rlay�p tutarl� dash uygulamak istersen:
        Vector2 v = rb.velocity;
        v.x = dashDirection * dashSpeed;
        rb.velocity = v;

        // dash ba�lama zaman� (cooldown i�in)
        lastDashTime = Time.time;

        // iste�e ba�l�: �arp��ma/yer �ekimi vb. davran��lar� buradan de�i�tirebilirsin
        // �rn: rb.gravityScale = 0; // istersen ge�ici olarak yer�ekimini kapat
    }

    private void EndDash()
    {
        isDashing = false;
        // Dash bitince yatay h�z� s�f�rlamak istersen:
        Vector2 v = rb.velocity;
        v.x = 0f;
        rb.velocity = v;

        // E�er gravityScale de�i�tirdiysen restore et burada
    }

    // Opsiyonel: dash durumunu d��ar�ya g�sterebilmek i�in event/metod ekleyebilirsin
}
