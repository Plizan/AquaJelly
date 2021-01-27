using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public float xValue = 0f;
    public float speed = 20f;

    public IEnumerator CameraFollow(Transform transform)
    {
        while (true)
        {
            SetCamera(transform);
            yield return null;
        }
    }

    public void SetCamera(Transform transform)
    {
        var originPosition = this.transform.position;
        var markPosition = originPosition;
        markPosition.x = transform.position.x + xValue;
        this.transform.position = Vector3.Lerp(originPosition, markPosition, Time.deltaTime * speed);
    }
}
