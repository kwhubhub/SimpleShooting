using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public GameController gameController;

    //ボスが発射する弾
    public BossEnemyBullet bulletPrefab;

    //破壊エフェクト
    public GameObject EXAnimation;

    //ボスと衝突したときの処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Playerの弾とボスが衝突したとき
        if (collision.CompareTag("Bullet") == true)
        {
            //爆破エフェクトを弾側に生成
            Instantiate(EXAnimation, collision.transform.position, transform.rotation);

            //100ダメージを与える
            gameObject.GetComponent<EnemyController>().TakeDamage(100);

            //弾を消す
            Destroy(collision.gameObject);
        }

        //Playerとボスが衝突したとき
        else if(collision.CompareTag("Player") == true)
        {
            //爆破エフェクトをPlayer側に生成
            Instantiate(EXAnimation, collision.transform.position, transform.rotation);

            //Playerを消す
            Destroy(collision.gameObject);

            gameController.ShowEndText();
        }

        //ボスとザコ敵が衝突したとき
        //else if(collision.CompareTag("Enemy") == true)
        //{
        //    //何もしない
        //    return;
        //}

    }
}
