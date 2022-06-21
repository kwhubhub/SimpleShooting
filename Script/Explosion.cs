using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //爆破エフェクトを0.5秒後に消す
        Destroy(gameObject, 0.5f);
    }
}
