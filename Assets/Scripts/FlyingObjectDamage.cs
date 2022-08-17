using System.Collections;
using UnityEngine;

public class FlyingObjectDamage : MonoBehaviour
{
    [SerializeField] private int _damage;

    public void DamageOn(Health target)
    {
        target.Hurt(_damage);
    }
}
