public interface IJumpAbility
{
    bool CanJump { get; }                    // þu an zýplayabilir mi
    void TryJump(bool pressed, bool held);   // zýplama mantýðý (buffer, coyote time vs.)
}
