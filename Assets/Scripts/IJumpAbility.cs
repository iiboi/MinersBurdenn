public interface IJumpAbility
{
    bool CanJump { get; }                    // �u an z�playabilir mi
    void TryJump(bool pressed, bool held);   // z�plama mant��� (buffer, coyote time vs.)
}
