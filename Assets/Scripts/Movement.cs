using UnityEngine;

public interface Blockable
{
    public void Block();

    public void Unblock();
}

public abstract class Movement : MonoBehaviour, Blockable
{
    public bool NotBlocked = true;

    public virtual void Block()
    {
        NotBlocked = false;
    }

    public virtual void Unblock()
    {
        NotBlocked = true;
    }
}
