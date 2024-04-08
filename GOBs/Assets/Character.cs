using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float SpeedFactor = 60f;
	public float CharacterSpeed = 1f;

	public GOBs.Goal Goal_Eat;
	public GOBs.Goal Goal_Sleep;
	public GOBs.Goal Goal_Fun;
	public GOBs.Goal[] Goals;

	public GOBs.Action Action_EatSnack;
	public GOBs.Action Action_EatMeal;
	public GOBs.Action Action_BedSleep;
	public GOBs.Action Action_PlaySoccer;
	public GOBs.Action[] Actions;

	
	void FixedUpdate()
	{
		foreach (GOBs.Goal goal in Goals)
		{
			goal.value += goal.changeRate * Time.deltaTime;
		}
	}

	
	void Start() { Setup(); }
	public void Setup()
	{
		Goal_Eat = new()
		{
			name = "Eat",
			value = 10f, //4f,
			changeRate = 4f*60f /SpeedFactor,
		};

		Goal_Sleep = new()
		{
			name = "Sleep",
			value = 8f, //3f,
			changeRate = 1f*60f /SpeedFactor,
		};

		Goal_Fun = new()
		{
			name = "Fun",
			value = 8f, //3f,
			changeRate = 3f*60f /SpeedFactor,
		};

		Goals = new GOBs.Goal[] {
			Goal_Eat,
			Goal_Sleep,
			Goal_Fun,
		};


		Action_EatSnack = new()
		{
			name = "EatSnack",
			targetGoals = new GOBs.Goal[] {
				Goal_Eat,
			},
			changes = new float[] {
				-2f,
			},
			duration = 15f*60f /SpeedFactor, // sec
		};

		Action_EatMeal = new()
		{
			name = "EatMeal",
			targetGoals = new GOBs.Goal[] {
				Goal_Eat,
			},
			changes = new float[] {
				-4f,
			},
			duration = 1f*60f*60f /SpeedFactor,
		};

		Action_BedSleep = new()
		{
			name = "BedSleep",
			targetGoals = new GOBs.Goal[] {
				Goal_Sleep,
			},
			changes = new float[] {
				-8f,
			},
			duration = 4f*60f*60f /SpeedFactor,
		};

		Action_PlaySoccer = new()
		{
			name = "PlaySoccer",
			targetGoals = new GOBs.Goal[] {
				Goal_Fun,
			},
			changes = new float[] {
				-4f,
			},
			duration = 30f*60f /SpeedFactor,
		};

		Actions = new GOBs.Action[] {
			Action_EatSnack,
			Action_EatMeal,
			Action_BedSleep,
			Action_PlaySoccer,
		};
	}
}