using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject bossEnemy;
    public GameObject enemyPrefab;

    //setActiveを使いたいのでGameObjectを取得する
    public GameObject endText;
    public GameObject clearText;

    public GameObject Player;
    public BossEnemyBullet bulletPrefab;

    //雑魚キャラを倒した時に加算されるスコアの初期値を設定
    int score = 0;

    //TextMeshProが全角と半角の同時入力ができないのでスコアを全角に変換する
    //画面上にスコアを表示しないならこれはいらない
    //今後のアップデートのために一応残しておく
    public class StringWidthConverter : MonoBehaviour
    {
        //全角英数の文字コードの65248個前が半角英数の文字コードのため65248を設定
        const int ConvertionConstant = 65248;

        //halfWidthStrにscore_stringが入る
        static public string ConvertToFullWidth(string halfWidthStr)
        {
            //初期化（これはなんだ？）
            string fullWidthStr = null;

            //score_stringの文字数だけ繰り返す
            for (int i = 0; i < halfWidthStr.Length; i++)
            {
                //ASCIIコードを文字コードに変換するために(char)としている
                //全角にするために文字コードに65248を足す
                fullWidthStr += (char)(halfWidthStr[i] + ConvertionConstant);
            }

            return fullWidthStr;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Spawn", 2f, 0.8f);
        bossEnemy.SetActive(false);
        endText.SetActive(false);
        clearText.SetActive(false);

        //スコアを初期表示させるときはこれを使う
        //scoreText.text = "スコア：０";
    }

    //雑魚キャラの生成
    void Spawn()
    {
        //生成位置のx座標をランダムにする
        Vector3 spawnPosition = new Vector3(
            Random.Range(-8.3f, 8.3f),
            6,
            transform.position.z
           );

        Instantiate(enemyPrefab, spawnPosition, transform.rotation);
    }

    //雑魚キャラを倒したときのスコアの加算
    public void AddScore()
    {
        //100点追加する
        score += 100;

        //スコアが1000以上になったらボスを表示して移動を開始する
        if(score >= 1000)
        {
            //ボスの移動を開始する
            bossEnemy.SetActive(true);
            StartCoroutine(BossTranslate());

            //雑魚キャラの生成を止める
            CancelInvoke();
        }

        //TextMeshProは全角文字しか反映されないので、scoreに格納された数値を文字列型に変換する
        //画面上にスコアを表示しないならこれはいらない
        //今後のアップデートのために一応残しておく
        //string score_string = score.ToString();

        //全角文字に変換したスコアを表示させる
        //画面上にスコアを表示しないならこれはいらない
        //今後のアップデートのために一応残しておく
        //scoreText.text = "スコア：" + StringWidthConverter.ConvertToFullWidth(score_string);
    }

    //ボスを移動させる
    IEnumerator BossTranslate()
    {
        //ボスがy座標で3以上の間は移動を繰り返す
        while (bossEnemy.transform.position.y > 3f)
        {
            //deltatimeにかける数字で移動速度を調整可能
            bossEnemy.transform.position -= new Vector3(0, 1f, 0) * 2 * Time.deltaTime;

            //移動した後に1フレーム待ってから再度移動を繰り返す
            yield return null;
        }

        //移動が完了したら弾の発射を開始する
        StartCoroutine(CPU());
    }

    //ボスの弾を発射する関数
    void Shot(float angle, float speed)
    {
        BossEnemyBullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.Setting(angle, speed);
    }

    //ボスが発射する弾の方向と弾数を決める
    IEnumerator CPU()
    {
        while (true)
        {
            yield return WaveNShotM(2, 4);
            yield return new WaitForSeconds(2f);
            yield return WaveNShotMCurve(4, 8);
            yield return new WaitForSeconds(2f);
            yield return WaveNPlayerAimShot(2, 4);
            yield return new WaitForSeconds(2f);
        }

    }

    //真っ直ぐ発射される弾
    IEnumerator WaveNShotM(int n, int m)
    {
        //n回m方向に撃つ
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(0.3f);
            ShotN(m, 2);
        }
    }

    //弾の弾数とスピードを決める
    void ShotN(int count, float speed)
    {
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            //2πを弾数で割って照射角度を作る
            float angle = i * (2 * Mathf.PI / bulletCount);
            Shot(angle, speed);
        }
    }

    //波のように発射される弾
    IEnumerator ShotNCurve(int count, float speed)
    {
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            //2πを弾数で割って照射角度を作る
            float angle = i * (2 * Mathf.PI / bulletCount);

            //弾を対称的に発射させるために-angleの方も発射する
            Shot(angle - Mathf.PI / 2f, speed);
            Shot(-angle - Mathf.PI / 2f, speed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WaveNShotMCurve(int n, int m)
    {
        //n回m方向に撃つ
        for (int w = 0; w < n; w++)
        {
            yield return ShotNCurve(m, 2);
            yield return new WaitForSeconds(0.3f);
        }

    }

    //弾の弾数とスピードを決める
    IEnumerator WaveNPlayerAimShot(int n, int m)
    {
        //n回m方向に撃つ
        for (int w = 0; w < n; w++)
        {
            PlayerAimShot(m, 2);
            yield return new WaitForSeconds(0.3f);
        }

    }

    //Playerを狙う弾を発射する
    //Playerの位置を把握する
    void PlayerAimShot(int count, float speed)
    {
        //ボスから見たプレイヤーの位置を相対的に取得する（ベクトルの差）
        Vector3 diffPosition = Player.transform.position - bossEnemy.transform.position;

        //角度を出す：diffpositionのx座標とy座標から傾きを出す。アークタンジェントを使う
        float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);
        
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = (i - bulletCount / 2f) * (Mathf.PI / 2f / bulletCount);
            Shot(angleP + angle, speed);
        }
    }


    //GAME OVERの時のメッセージを表示する
    public void ShowEndText()
    {
        endText.SetActive(true);
        clearText.SetActive(false);
    }

    //GAME CLEAR時のメッセージを表示する
    public void ShowClearText()
    {
        clearText.SetActive(true);
        endText.SetActive(false);
    }


    private void Update()
    {
        if (endText.activeSelf == true || clearText.activeSelf == true)
        {
            //GAME OVERまたはGAME CLEARをしたときは弾の発射を止める
            StopAllCoroutines();

            //Enterキーを押すともう一度プレイできる
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("SampleScene");
            }        
        }
    }
}
