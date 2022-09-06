using UnityEngine;

public interface Blockable
{
    public void Block();

    public void Unblock();
}

public abstract class Movement : MonoBehaviour, Blockable
{
    protected bool _notBlocked = true;

    public virtual void Block()
    {
        _notBlocked = false;
    }

    public virtual void Unblock()
    {
        _notBlocked = true;
    }
}
