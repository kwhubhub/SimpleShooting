using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyBullet : MonoBehaviour
{
    //爆破エフェクト
    public GameObject EXAnimation;

    //弾の軌跡
    float dx;
    float dy;

    private void Start()
    {
        //ボスの弾の発射位置をボスの位置と合わせる
        Vector3 pos = transform.position;
        pos.y = 3f;
        transform.position = pos;    
    }

    //弾の発射方向を決める
    public void　Setting(float angle, float speed)
    {
        dx = Mathf.Cos(angle) * speed;
        dy = Mathf.Sin(angle) * speed;
    }


    // Update is called once per frame
    void Update()
    {
        //ボスの弾の移動速度をdeltatimeの値で変更可能
        transform.position += new Vector3(dx, dy, 0) * 4 * Time.deltaTime;

        //画面から消えたボスの弾は消滅させる
        if (transform.position.y < -8 || transform.position.y > 6 ||
            transform.position.x < -10 || transform.position.x > 10)
        {
            Destroy(gameObject);
        }

    }

    //ボスの弾と当たったときの処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ボスの弾とPlayerの弾が衝突したとき
        if (collision.CompareTag("Bullet") == true)
        {
            //爆破エフェクトをPlayerの弾側に生成する
            Instantiate(EXAnimation, collision.transform.position, transform.rotation);

            //爆破エフェクトをボスの弾側に生成する
            Instantiate(EXAnimation, transform.position, transform.rotation);

            //Playerの弾を消す
            Destroy(collision.gameObject);

            //ボスの弾を消す
            Destroy(gameObject);
        }
  
    }

}