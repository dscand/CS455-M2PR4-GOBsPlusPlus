using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoccerField : MonoBehaviour
{
	public Transform target;
	public Animator animator;

	public IEnumerator PlayAnimation()
	{
		animator.SetTrigger("Soccer");
		yield return new WaitForSeconds(1.6f);
		animator.ResetTrigger("Soccer");
		yield break;
	}
}
