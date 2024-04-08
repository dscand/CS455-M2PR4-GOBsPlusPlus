using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class CharacterAnim : Character
{
	public Indicator IndicatorFood;
	public Indicator IndicatorSleep;
	public Indicator IndicatorFun;
	public TMP_Text CurrentAction;
	public GameObject body;
	void Update()
	{
		IndicatorFood.IndicatorLevel = 1f-Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0, 10f, Goal_Eat.value));
		IndicatorSleep.IndicatorLevel = 1f-Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0, 10f, Goal_Sleep.value));
		IndicatorFun.IndicatorLevel = 1f-Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0, 10f, Goal_Fun.value));
	}


	public EnvironmentTargets targets;

	public class ActionAnim
	{
		public CharacterAnim character;
		public EnvironmentTargets targets;
		public GOBs.Action action;
		public virtual IEnumerator DoAction() { yield break; }
	}

	public class ActionAnim_EatSnack : ActionAnim
	{
		public override IEnumerator DoAction()
		{
			Debug.Log("StartAction (" + action.getDuration() + " sec): " + action.name);

			yield return character.MoveTo(targets.TargetCabinet, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration()/2f);

			yield return character.MoveTo(targets.TargetTable, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration()/2f);

			Debug.Log("Complete: " + action.name);
			yield break;
		}
	}

	public class ActionAnim_EatMeal : ActionAnim
	{
		public override IEnumerator DoAction()
		{
			Debug.Log("StartAction (" + action.getDuration() + " sec): " + action.name);

			yield return character.MoveTo(targets.TargetFridge, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration()/3f);

			yield return character.MoveTo(targets.TargetCabinet, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration()/3f);

			yield return character.MoveTo(targets.TargetTable, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration()/3f);

			Debug.Log("Complete: " + action.name);
			yield break;
		}
	}

	public class ActionAnim_BedSleep : ActionAnim
	{
		public override IEnumerator DoAction()
		{
			Debug.Log("StartAction (" + action.getDuration() + " sec): " + action.name);

			yield return character.MoveTo(targets.TargetBed, character.CharacterSpeed * character.SpeedFactor);
			yield return new WaitForSeconds(action.getDuration());

			Debug.Log("Complete: " + action.name);
			yield break;
		}
	}

	public class ActionAnim_PlaySoccer : ActionAnim
	{
		public override IEnumerator DoAction()
		{
			Debug.Log("StartAction (" + action.getDuration() + " sec): " + action.name);

			yield return character.MoveTo(targets.SoccerField.target, character.CharacterSpeed * character.SpeedFactor);
			character.body.SetActive(false);
			yield return targets.SoccerField.PlayAnimation();
			character.body.SetActive(true);
			//yield return new WaitForSeconds(action.getDuration());

			Debug.Log("Complete: " + action.name);
			yield break;
		}
	}


	public ActionAnim_EatSnack actionAnim_EatSnack;
	public ActionAnim_EatMeal actionAnim_EatMeal;
	public ActionAnim_BedSleep actionAnim_BedSleep;
	public ActionAnim_PlaySoccer actionAnim_PlaySoccer;
	public ActionAnim[] ActionAnims;

	void Start()
	{
		Setup();

		string state = "";
		foreach (GOBs.Goal goal in Goals)
		{
			state += goal.name + " " + goal.value + ",";
		}
		Debug.Log("StartState: " + state);

		StartCharacter();
	}
	public void StartCharacter() { StartCoroutine(CharacterRun()); }
	public void StopCharacter() { StopAllCoroutines(); }

	new void Setup()
	{
		base.Setup();

		actionAnim_EatSnack = new() {
			character = this,
			targets = targets,
			action = Action_EatSnack,
		};

		actionAnim_EatMeal = new() {
			character = this,
			targets = targets,
			action = Action_EatMeal,
		};

		actionAnim_BedSleep = new() {
			character = this,
			targets = targets,
			action = Action_BedSleep,
		};

		actionAnim_PlaySoccer = new() {
			character = this,
			targets = targets,
			action = Action_PlaySoccer,
		};
		
		ActionAnims = new ActionAnim[] {
			actionAnim_EatSnack,
			actionAnim_EatMeal,
			actionAnim_BedSleep,
			actionAnim_PlaySoccer,
		};
	}


	public IEnumerator CharacterActionStep()
	{
		GOBs.Action action = GOBs.ChooseAction(Actions, Goals);
		foreach (ActionAnim actionAnim in ActionAnims)
		{
			if (action == actionAnim.action) {
				Debug.Log("DoingAction: " + action.name);
				//TextLog.CreateLog("DoingAction: " + action.name);
				CurrentAction.text = action.name;
				yield return actionAnim.DoAction();
				CurrentAction.text = "";

				for (int i = 0; i < action.targetGoals.Length; i++)
				{
					action.targetGoals[i].value += action.changes[i];
				}
				yield break;
			}
		}
		Debug.LogError("Did not find action, " + action.name);
	}

	public IEnumerator CharacterRun()
	{
		float total = 1f;
		while (total > 0f)
		{
			yield return CharacterActionStep();

			string state = "";
			foreach (GOBs.Goal goal in Goals)
			{
				state += goal.name + " " + goal.value + ",";
			}

			Debug.Log("NewState: " + state);
			//TextLog.CreateLog("NewState: " + state);


			total = 0f;
			foreach (GOBs.Goal goal in Goals)
			{
				total += goal.value;
			}

			yield return MoveTo(targets.TargetCenter, CharacterSpeed*SpeedFactor);
			yield return new WaitForSeconds(2f);
		}
		//TextLog.CreateLog("All goals <0");
		yield break;
	}



	IEnumerator MoveTo(Transform target, float moveSpeed)
	{
		while (Vector3.Distance(transform.position, target.position) > 0.1f)
		{
			transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * moveSpeed);
			yield return null;
		}
		transform.position = target.position;
		yield break;
	}
}