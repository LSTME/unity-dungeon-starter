using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsController : MonoBehaviour {

	public bool IsOpen = false;

	void Start() {
		this.Close();
	}

	// Update is called once per frame
	void Update () {
		var player = GameObject.FindGameObjectWithTag("Player");

		if (Vector3.Distance(transform.position, player.transform.position) < 1.1)
		{
			Open();
		}
		else
		{
			Close();
		}
	}

	void Open() {
		if (IsOpen) return;

		IsOpen = true;
		var animator = GetComponent<Animator>();
		animator.Play("open");
	}

	void Close() {
		if (!IsOpen) return;

		IsOpen = false;
		var animator = GetComponent<Animator>();
		animator.Play("close");
	}
}
