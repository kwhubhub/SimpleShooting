using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CountDownHP
{
	public class EnemyStatus : MonoBehaviour
	{
		//GameControllerの入れ物。showendtext、showcleartextを呼ぶため。
		GameController gameController;

		//ボスのHP
		private int hp = 1000;

		void Start()
		{
			//GameControllerオブジェクトを生成
			gameController = GameObject.Find("GameController").GetComponent<GameController>();
		}

		//ダメージ値を追加する
		public void SetDamage(int damage)
		{
			//与えたダメージ分HPを減らす
			hp -= damage;

			//hpが0になったらボスを消してGAME CLEARテキストを表示する
			if(hp == 0)
            {
				Destroy(gameObject);
				gameController.ShowClearText();
            }
		}
	}
}