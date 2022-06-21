using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //破壊エフェクトを呼び出す
    public GameObject EXAnimation;

    //GameControllerの入れ物。AddScoreを呼ぶため。
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //ヒエラルキー上のGameControllerオブジェクトを取得
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        //雑魚キャラを画面下に移動させる
        transform.position += new Vector3(0, -3, 0) * Time.deltaTime;

        //画面から消えた雑魚キャラは消滅させる
        if(transform.position.y < -8)
        {
            Destroy(gameObject);
        }
    }

    //Triggerがついているものが雑魚キャラに衝突した時にこれが呼ばれる
    //Player側のistirggerを外すとこのイベントが発生しなくなるので注意
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //雑魚キャラがPlayerに当たった時の処理
        if(collision.CompareTag("Player") == true)
        {
            //Player側に爆発エフェクトを生成
            Instantiate(EXAnimation, collision.transform.position, transform.rotation);

            //GAME OVER時のテキストを表示する
            gameController.ShowEndText();
            
        }
        //Playerの弾に当たったら
        else if(collision.CompareTag("Bullet") == true)
        {
            //Scoreを更新
            gameController.AddScore();
        }

        //雑魚キャラ側に爆発エフェクトを生成
        Instantiate(EXAnimation, transform.position, transform.rotation);

        //雑魚キャラを消す
        Destroy(gameObject);

        //PlayerとPlayerの弾を消す
        Destroy(collision.gameObject);
    }
}
