using UnityEngine;

public class KeyboardInputProvider : MonoBehaviour, IInputProvider
{
    [SerializeField] private KeyCode jumpKey1 = KeyCode.Space;
    [SerializeField] private KeyCode jumpKey2 = KeyCode.UpArrow;
    [SerializeField] private KeyCode dashKey = KeyCode.X; // <-- dash tuþu
    [SerializeField] private KeyCode digKey = KeyCode.Z; // <-- dash tuþu

    public float Horizontal { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool DigHeld { get; private set; }
    public bool DashPressed { get; private set; }

    void Update()
    {
        float a = Input.GetKey(KeyCode.A) ? -1f : 0f;
        float d = Input.GetKey(KeyCode.D) ? 1f : 0f;
        float left = Input.GetKey(KeyCode.LeftArrow) ? -1f : 0f;
        float right = Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;

        Horizontal = Mathf.Clamp(a + d + left + right, -1f, 1f);

        bool nowHeld = Input.GetKey(jumpKey1) || Input.GetKey(jumpKey2);
        JumpPressed |= Input.GetKeyDown(jumpKey1) || Input.GetKeyDown(jumpKey2);
        JumpHeld = nowHeld;

        DigHeld = Input.GetMouseButton(0) || Input.GetKey(digKey);

        DashPressed |= Input.GetKeyDown(dashKey);
    }

    public void ConsumeJumpPressed()
    {
        JumpPressed = false;
    }

    public void ConsumeDashPressed()
    {
        DashPressed = false;
    }
}
