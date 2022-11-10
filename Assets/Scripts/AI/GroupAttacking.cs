using UnityEngine;

public class GroupAttacking : MonoBehaviour
{
    private PlayerObject _player;
    private Surround _surround;

    private void Start()
    {
        _player = Resources.Load<PlayerObject>("Player");
        _surround = new Surround();
    }

    public Vector2 GetDestination(int enemyIndex, Transform target, float spacing)
    {
        Vector2[] destinations = _surround.FindDestinations(
                                            _surround.FindAngles(
                                                Trigonometric.AddAngle(target.eulerAngles.z, 180),
                                                _player.NumberEnemiesSeeYou
                                                ),
                                            spacing
        );

        return (Vector3)destinations[enemyIndex] + target.position;
    }
}
