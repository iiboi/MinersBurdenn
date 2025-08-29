public interface IInputProvider
{
    float Horizontal { get; }
    bool JumpPressed { get; }
    bool DashPressed { get; }
    void ConsumeDashPressed();
    bool JumpHeld { get; }
    void ConsumeJumpPressed();
    bool DigHeld { get; }
}