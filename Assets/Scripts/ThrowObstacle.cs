using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : MonoBehaviour
{
    private void Start()
    {
        var targetPos = Managers.Game.playerCtrl.transform.position - transform.position;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 10, ForceMode2D.Impulse);
    }
}
