using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITexts : MonoBehaviour {

	private int CollectedCoins { get; set; }

	public Texture2D DucatIcon;
	public float DucatIconPositionX = 1.0f;
	public float DucatIconPositionY = 22.0f;

	public bool ShowDucatIcon = true;

	public static GUITexts GetInstance()
	{
		return GameObject.FindWithTag("GUI").GetComponent<GUITexts>();
	}

	private void Start()
	{
		CollectedCoins = 0;
	}

	private void OnGUI()
	{
		DrawDucatIcon();
	}

	private void DrawDucatIcon()
	{
		if (!ShowDucatIcon) return;

		var iconShape = new Rect(DucatIconPositionX / 100.0f * Screen.width, DucatIconPositionY / 100.0f * Screen.height, 64.0f, 64.0f);
		var textShape = new Rect(iconShape.x + 64.0f + 5.0f, iconShape.y, 100.0f, 64.0f);
		var textShadowShape = new Rect(textShape);
		textShadowShape.y += 2;
		textShadowShape.x += 2;

		GUI.skin.box.normal.background = DucatIcon;
		GUI.Box(iconShape, GUIContent.none);

		var fontStyle = new GUIStyle();
		fontStyle.fontSize = 50;
		fontStyle.normal.textColor = Color.white;
		fontStyle.alignment = TextAnchor.MiddleLeft;
		fontStyle.richText = true;
		fontStyle.wordWrap = false;

		var fontStyleShadow = new GUIStyle(fontStyle);
		fontStyleShadow.normal.textColor = Color.gray;

		GUI.Label(textShadowShape, CollectedCoins.ToString(), fontStyleShadow);
		GUI.Label(textShape, CollectedCoins.ToString(), fontStyle);
	}

	public void CollectCoin()
	{
		CollectedCoins++;
	}
}
