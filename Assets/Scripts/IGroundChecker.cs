public interface IGroundChecker
{
    bool IsGrounded { get; }
    void UpdateGrounded(); // FixedUpdate’te çaðrýlacak
}
