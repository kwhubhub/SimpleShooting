using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    void Update()
    {
        //弾を上に移動させる
        transform.position += new Vector3(0, 8, 0) * Time.deltaTime;
    }
}
