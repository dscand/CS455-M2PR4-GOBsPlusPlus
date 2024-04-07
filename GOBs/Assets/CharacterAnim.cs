using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : Character
{
	public LogGroup TextLog;
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

            character.transform.position = targets.TargetCabinet.position;
			yield return new WaitForSeconds(action.getDuration()/2f);

			character.transform.position = targets.TargetTable.position;
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

            character.transform.position = targets.TargetFridge.position;
			yield return new WaitForSeconds(action.getDuration()/3f);

            character.transform.position = targets.TargetCabinet.position;
			yield return new WaitForSeconds(action.getDuration()/3f);

			character.transform.position = targets.TargetTable.position;
			yield return new WaitForSeconds(action.getDuration()/3f);

			Debug.Log("Complete: " + action.name);
			yield break;
        }
    }

	public class ActionAnim_VisitBathroom : ActionAnim
	{
        public override IEnumerator DoAction()
        {
			Debug.Log("StartAction (" + action.getDuration() + " sec): " + action.name);

			character.transform.position = targets.TargetBathroom.position;
			yield return new WaitForSeconds(action.getDuration());

			Debug.Log("Complete: " + action.name);
			yield break;
        }
    }


	public ActionAnim_EatSnack actionAnim_EatSnack;
	public ActionAnim_EatMeal actionAnim_EatMeal;
	public ActionAnim_VisitBathroom actionAnim_VisitBathroom;
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

		actionAnim_VisitBathroom = new() {
			character = this,
			targets = targets,
			action = Action_VisitBathroom,
		};
		
		ActionAnims = new ActionAnim[] {
			actionAnim_EatSnack,
			actionAnim_EatMeal,
			actionAnim_VisitBathroom,
		};
	}


	public IEnumerator CharacterActionStep()
	{
		GOBs.Action action = GOBs.ChooseAction(Actions, Goals);
		foreach (ActionAnim actionAnim in ActionAnims)
		{
			if (action == actionAnim.action) {
				Debug.Log("DoingAction: " + action.name);
				TextLog.CreateLog("DoingAction: " + action.name);
				yield return actionAnim.DoAction();

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
			TextLog.CreateLog("NewState: " + state);


			total = 0f;
			foreach (GOBs.Goal goal in Goals)
			{
				total += goal.value;
			}
		}
		TextLog.CreateLog("All goals <0");
		yield break;
	}
}