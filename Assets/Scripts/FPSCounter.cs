using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public class FPSCounter : MonoBehaviour
    {
        float _deltaTime = 0.0f;

        void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
        }

        void OnGUI()
        {
            var style = new GUIStyle();

            var rect = new Rect(0, 0, Screen.width, Screen.height * 2.0f / 100.0f);
            style.alignment = TextAnchor.UpperRight;
            style.fontSize = Screen.height * 2 / 100;
            style.normal.textColor = Color.green;
            var msec = _deltaTime * 1000.0f;
            var fps = 1.0f / _deltaTime;
            var text = string.Format("{0:0.} fps / {1:0.0} ms", fps, msec);
            GUI.Label(rect, text, style);
        }
    }
}
