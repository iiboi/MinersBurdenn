public interface IDashAbility
{
    bool CanDash { get; }
    void TryDash(bool pressed, float horizontalInput);
}
