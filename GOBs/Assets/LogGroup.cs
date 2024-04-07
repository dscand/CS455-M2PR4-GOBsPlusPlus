using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogGroup : MonoBehaviour
{
	public GameObject LogContent;

	public void CreateLog(string log) {
		GameObject newLog = Instantiate(LogContent, transform);
		newLog.GetComponent<LogContent>().SetText(log);
		newLog.transform.SetAsFirstSibling();
	}
}
