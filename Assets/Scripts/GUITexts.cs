using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITexts : MonoBehaviour {

	private int CollectedCoins { get; set; }

	public float MessageMaximumDisplayLength = 5.0f;

	public Texture2D DucatIcon;
	public float DucatIconPositionX = 1.0f;
	public float DucatIconPositionY = 22.0f;

	private string TextMessage = "";
	private float MessageTime = 0;

	public bool ShowHUDElements = true;

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
		DrawKeyboardInteractionInfo();
		DrawTextMessage();
		TextMessageTimeAdvance();
	}

	private void DrawDucatIcon()
	{
		if (!ShowHUDElements) return;

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

	private void DrawKeyboardInteractionInfo()
	{
		if (!ShowHUDElements) return;

		if (Scripts.PlayerController.getInstance().enableKeyboardInteraction) return;

		var textShape = new Rect(5, Screen.height - 30, 100, 25);
		var textShapeShadow = new Rect(textShape);
		textShapeShadow.x++;
		textShapeShadow.y++;

		var fontStyle = new GUIStyle();
		fontStyle.fontSize = 25;
		fontStyle.normal.textColor = Color.red;
		fontStyle.alignment = TextAnchor.UpperLeft;

		var fontStyleShadow = new GUIStyle(fontStyle);
		fontStyleShadow.normal.textColor = Color.gray;

		string text = "Interackie vypnuté ...";

		GUI.Label(textShapeShadow, text, fontStyleShadow);
		GUI.Label(textShape, text, fontStyle);
	} 

	private void DrawTextMessage()
	{
		if (!ShowHUDElements) return;
		if (TextMessage.Equals("")) return;
		if (MessageTime > MessageMaximumDisplayLength) return;

		var messageShape = new Rect(0, Screen.height - 80, Screen.width, 50);
		var shadowShape = new Rect(messageShape);
		shadowShape.x++;
		shadowShape.y++;

		var fontStyle = new GUIStyle();
		fontStyle.normal.textColor = Color.white;
		fontStyle.alignment = TextAnchor.MiddleCenter;
		fontStyle.fontSize = 20;
		fontStyle.wordWrap = true;
		fontStyle.richText = true;

		var fontStyleShadow = new GUIStyle(fontStyle);
		fontStyleShadow.normal.textColor = Color.black;

		GUI.Label(shadowShape, TextMessage, fontStyleShadow);
		GUI.Label(messageShape, TextMessage, fontStyle);
	}

	private void TextMessageTimeAdvance()
	{
		if (MessageTime > MessageMaximumDisplayLength) return;
		MessageTime += Time.deltaTime;
	}

	public void CollectCoin()
	{
		CollectedCoins++;
	}

	public int GetCoinsCount()
	{
		return CollectedCoins;
	}

	public void NewTextMessage(string Message)
	{
		TextMessage = Message;
		MessageTime = 0.0f;
	}
}
