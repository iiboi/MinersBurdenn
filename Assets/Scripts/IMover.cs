using UnityEngine;

public interface IMover
{
    void Move(float horizontal, float fixedDelta);
    void Face(float dirX); // sprite �evirmek i�in
}
