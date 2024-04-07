using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOBs
{
	public class Goal
	{
		public string name;
		public float value;

		public float getDiscontentment(float newValue)
		{
			return newValue * newValue;
		}

		public float changeRate = 0.1f; // per sec
		public float getChange()
		{
			//rateSinceLastTime = changeSinceLastTime / timeSinceLast;
			//basicRate = 0.95 * basicRate + 0.05 * rateSinceLastTime;
			return changeRate;
		}
	}

	public class Action
	{
		public string name;
		public Goal[] targetGoals;
		public float[] changes;
		public virtual float getGoalChange(Goal goal)
		{
			for (int i = 0; i < targetGoals.Length; i++)
			{
				Goal targetGoal = targetGoals[i];
				if (goal.name == targetGoal.name) return changes[i];
			}
			return 0;
		}

		public float duration;
		public virtual float getDuration()
		{
			// TODO?
			return duration;
		}
	}

	public static Action ChooseAction(Action[] actions, Goal[] goals)
	{
		// Find the most valuable goal to try and fulfil.
		Goal topGoal = goals[0];
		foreach (Goal goal in goals)
		{
			if (goal.value > topGoal.value) {
				topGoal = goal;
			}
		}

		// Find the best action to take.
		Action bestAction = actions[0];
		float bestValue = Discontentment(bestAction, goals);

		foreach (Action action in actions)
		{
			// We invert the change because a low change value is good
			// (we want to reduce the value for the goal)
			// but utilities are typically scale so high values are good.
			float value = Discontentment(action, goals);

			// We look for the lowest change (highest utility).
			if (value < bestValue) {
				bestValue = value;
				bestAction = action;
			}
		}

		// Return the best action, to be carried out.
		return bestAction;
	}

	public static float Discontentment(Action action, Goal[] goals)
	{
		// Keep a running total.
		float discontentment = 0;
		
		// Loop through each goal.
		foreach (Goal goal in goals)
		{
			// Calculate the new value after the action.
			float newValue = goal.value + action.getGoalChange(goal);

			// Calculate the change due to time alone.
			newValue += action.getDuration() * goal.getChange();

			// Get the discontentment of this value.
			discontentment += goal.getDiscontentment(newValue);
		}

		return discontentment;
	}
}