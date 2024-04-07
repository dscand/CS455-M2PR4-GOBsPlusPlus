using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public float SpeedFactor = 60f;

	public GOBs.Goal Goal_Eat;
	public GOBs.Goal Goal_Bathroom;
	public GOBs.Goal[] Goals;

	public GOBs.Action Action_EatSnack;
	public GOBs.Action Action_EatMeal;
	public GOBs.Action Action_VisitBathroom;
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

		Goal_Bathroom = new()
		{
			name = "Bathroom",
			value = 8f, //3f,
			changeRate = 2f*60f /SpeedFactor,
		};

		Goals = new GOBs.Goal[] {
			Goal_Eat,
			Goal_Bathroom,
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

		Action_VisitBathroom = new()
		{
			name = "VisitBathroom",
			targetGoals = new GOBs.Goal[] {
				Goal_Bathroom,
			},
			changes = new float[] {
				-4f,
			},
			duration = 15f*60f /SpeedFactor,
		};

		Actions = new GOBs.Action[] {
			Action_EatSnack,
			Action_EatMeal,
			Action_VisitBathroom,
		};
	}
}