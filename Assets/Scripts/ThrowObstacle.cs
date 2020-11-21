using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacle : MonoBehaviour
{
    public bool isTargetPlayer;
    public float speed;
    public bool isMove;
    private void Start()
    {
        if (!isTargetPlayer) return;

        var targetPos = Managers.Game.playerCtrl.transform.position - transform.position;
        var angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        //GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.left * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (isMove)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}
