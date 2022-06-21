using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //弾を発射する位置を取得する
    public Transform firePoint;

    //Playerの弾のprefabを取得する
    public GameObject bulletPrefab;

    //破壊エフェクト
    public GameObject EXAnimation;

    //GameControllerの入れ物。AddScoreを呼ぶため。
    GameController gameController;

    //連射禁止制御をするための時間計測
    private float time;

    // Update is called once per frame

    void Start()
    {
        //最初は待ち時間なしで発射してOK
        time = 1.0f;
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        //前回のフレームからの経過時間をtimeに代入する
        time += Time.deltaTime;
        Move();
        Shot();
    }

    //Playerの移動
        void Move()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * 8f * Time.deltaTime;

            //Mathf.clampでnextpositionの範囲を制限する
            nextPosition = new Vector3(
                Mathf.Clamp(nextPosition.x, -8.3f, 8.3f),
                Mathf.Clamp(nextPosition.y, -4.1f, 4.3f),
                nextPosition.z
                );

            transform.position = nextPosition;
        }

        //弾の発射
        void Shot()
        {
            //連射禁止のため一定時間経過しないと打てないようにする
            if(time >= 0.5f)
            {
                //スペースキーを押したらPlayerが弾を発射する
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                    time = 0.0f;
                }

            }
        }

    //Playerと衝突したときの処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ボスの弾に当たった時の処理
        if(collision.CompareTag("EnemyBullet") == true)
        {
            //弾側の爆発エフェクトを生成
            Instantiate(EXAnimation, collision.transform.position, transform.rotation);

            //Player側の爆発エフェクトを生成
            Instantiate(EXAnimation, transform.position, transform.rotation);

            //Playerを消す
            Destroy(gameObject);

            //ボスの弾を消す
            Destroy(collision.gameObject);

            //GAME OVERテキスト表示する
            gameController.ShowEndText();
        }
    }
}
