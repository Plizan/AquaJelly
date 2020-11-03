using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] private float speed = 1f;

    public IEnumerator CameraFollow(Transform transform)
    {
        while (true)
        {
            var originPosition = this.transform.position;
            var markPosition = originPosition;
            markPosition.x = transform.position.x;
            this.transform.position = Vector3.Lerp(originPosition, markPosition, Time.deltaTime * speed);

            yield return null;
        }
    }
}
