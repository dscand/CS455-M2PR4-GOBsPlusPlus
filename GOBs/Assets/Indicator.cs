using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[ExecuteInEditMode]
public class Indicator : MonoBehaviour
{
	public FillLevel level;
	public TMP_Text text;

	public string IndicatorText = "text";
	[Range(0.0f, 1.0f)]
	public float IndicatorLevel = 0f;
	public Color IndicatorColor = Color.cyan;

	void Update()
	{
		text.text = IndicatorText;
		level.FilledLevel = IndicatorLevel;
		level.GetComponent<Image>().color = IndicatorColor;
	}
}
