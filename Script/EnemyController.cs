using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
	
	private CountDownHP.EnemyStatus status;


	void Start()
	{
		status = GetComponent<CountDownHP.EnemyStatus>();
	}

	public void TakeDamage(int damage)
	{
		status.SetDamage(damage);
	}
}