using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogContent : MonoBehaviour
{
	public TMP_Text TextObject;

	public void SetText(string text)
	{
		TextObject.text = text;
	}
}
