using UnityEngine;

/// <summary>
/// Bile�enleri birbirine ba�layan, SOLID�e uygun kompozisyon kontrolc�s�.
/// Update: girdi toplar. FixedUpdate: fizik ve hareket uygular.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    [Header("References (bo� b�rak�l�rsa otomatik bulunur)")]
    [SerializeField] private MonoBehaviour inputProviderBehaviour;
    [SerializeField] private MonoBehaviour moverBehaviour;
    [SerializeField] private MonoBehaviour groundCheckerBehaviour;
    [SerializeField] private MonoBehaviour jumpAbilityBehaviour;
    [SerializeField] private MonoBehaviour digAbilityBehaviour;
    [SerializeField] private MonoBehaviour dashAbilityBehaviour;

    private IInputProvider input;
    private IMover mover;
    private IGroundChecker ground;
    private IJumpAbility jumper;
    private IDashAbility dasher;
    private Rigidbody2D rb;

    private float cachedHorizontal;
    private bool cachedJumpPressed;
    private bool cachedJumpHeld;
    private bool cachedDashPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        input = (inputProviderBehaviour as IInputProvider) ?? GetComponent<IInputProvider>();
        mover = (moverBehaviour as IMover) ?? GetComponent<IMover>();
        ground = (groundCheckerBehaviour as IGroundChecker) ?? GetComponent<IGroundChecker>();
        jumper = (jumpAbilityBehaviour as IJumpAbility) ?? GetComponent<IJumpAbility>();
        dasher = (dashAbilityBehaviour as IDashAbility) ?? GetComponent<IDashAbility>();

        if (input == null) Debug.LogError("IInputProvider eksik!");
        if (mover == null) Debug.LogError("IMover eksik!");
        if (ground == null) Debug.LogError("IGroundChecker eksik!");
        if (jumper == null) Debug.LogError("IJumpAbility eksik!");
        if (dasher == null) Debug.LogError("IDashAbility eksik!");

    }

    void Update()
    {
        if (input == null) return;
        cachedHorizontal = input.Horizontal;
        cachedJumpPressed = cachedJumpPressed || input.JumpPressed; // JumpPressed TRUE olursa sakla
        cachedJumpHeld = input.JumpHeld;
        cachedDashPressed = cachedDashPressed || input.DashPressed;
    }

    void FixedUpdate()
    {
        ground?.UpdateGrounded();

        mover?.Move(cachedHorizontal, Time.fixedDeltaTime);
        mover?.Face(cachedHorizontal);

        jumper?.TryJump(cachedJumpPressed, cachedJumpHeld);

        dasher?.TryDash(cachedDashPressed, cachedHorizontal);

        if (input != null)
        {
            if (cachedJumpPressed)
            {
                input.ConsumeJumpPressed();
                cachedJumpPressed = false;
            }
            if (cachedDashPressed)
            {
                input.ConsumeDashPressed();
                cachedDashPressed = false;
            }
        }

    }
}
