using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour
{
	public SpriteRenderer[] spriteObjects;

	public SpriteList Idle_Sprites;
	public SpriteList Walk_Sprites;

	public enum State
	{
		Idle,
		Walk
	}
	public State state;
	public State lastState;

	private int Cur_Frame = 0;
	private float SecsPerFrame = 1;

	void Awake()
	{
		PlayAnimation(Idle_Sprites, Time.deltaTime);
	}

	private void Update()
	{
		if(Input.GetAxisRaw("Horizontal") != 0)
		{
			if(GetComponent<PlayerDataSP>().IsTurn() && GetComponent<Movement>().enabled)
			{
				state = State.Walk;
			}
		}
		else
		{
			state = State.Idle;
		}

		if(lastState != state)
		{
			switch(state)
			{
				case State.Idle:
					PlayAnimation(Idle_Sprites, Time.deltaTime);
					break;

				case State.Walk:
					PlayAnimation(Walk_Sprites, Time.deltaTime);
					break;
			}
		}

		lastState = state;
	}

	public void PlayAnimation(SpriteList sprites, float secPerFrame)
	{
		SecsPerFrame = secPerFrame;
		StopCoroutine("AnimateSprite");

		Cur_Frame = 0;
		StartCoroutine("AnimateSprite", sprites);
	}

	IEnumerator AnimateSprite(SpriteList sprites)
	{
		yield return new WaitForSeconds(SecsPerFrame);

		spriteObjects[0].sprite = sprites.Torso[Cur_Frame];
		spriteObjects[1].sprite = sprites.Left[Cur_Frame];
		spriteObjects[2].sprite = sprites.Right[Cur_Frame];
		spriteObjects[3].sprite = sprites.Legs[Cur_Frame];

		Cur_Frame++;

		if(Cur_Frame > sprites.Legs.Length-1)
		{
			Cur_Frame = 0;
		}

		StartCoroutine("AnimateSprite", sprites);
	}
}

[System.Serializable]
public class SpriteList
{
	public Sprite[] Torso;
	public Sprite[] Legs;
	public Sprite[] Left;
	public Sprite[] Right;
}