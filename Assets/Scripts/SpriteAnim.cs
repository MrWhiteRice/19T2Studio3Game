using UnityEngine;
using System.Collections;

public class SpriteAnim : MonoBehaviour
{
	//0 = torso, 1 = f/l arm, 2 = b/r arm, 3 = legs
	public SpriteRenderer[] spriteObjects;

	[HideInInspector] public SpriteList Idle_Sprites;
	[HideInInspector] public SpriteList Walk_Sprites;

	[HideInInspector] public SpriteList Melee_Sprites;
	[HideInInspector] public SpriteList Melee_Idle_Sprites;

	[HideInInspector] public SpriteList Grenade_Sprites;

	[HideInInspector] public SpriteList Hurt_Sprites;
	[HideInInspector] public SpriteList Jump_Sprites;
	[HideInInspector] public SpriteList Aim;

	public Sprite weapon;
	public SpriteRenderer weaponSlot;

	SpriteList playingSprites;

	bool loop = true;
	bool lockAnim;

	//currentstate of the animation
	public enum State
	{
		Idle,
		Walk,
		Null,
		Melee,
		Aim,
		Grenade
	}
	[HideInInspector]public State state = State.Null;

	[HideInInspector]public Sprite[] sprites;
	int currentFrame;

	public void PlayAnimation(SpriteList sprites, State s)
	{
		PlayAnim(sprites, s, true, false);
	}

	public void PlayAnimation(SpriteList sprites, State s, bool set)
	{
		PlayAnim(sprites, s, set, false);
	}

	public void PlayAnimation(SpriteList sprites, State s, bool set, bool lockAnim)
	{
		PlayAnim(sprites, s, set, lockAnim);
	}

	void PlayAnim(SpriteList sprites, State s, bool set, bool lockAnim)
	{
		loop = set;
		this.lockAnim = lockAnim;

		if(s != state)
		{
			CancelInvoke("Animate");
			currentFrame = 0;
			playingSprites = sprites;
			state = s;

			Invoke("Animate", 0);
		}
	}

	void Animate()
	{
		spriteObjects[0].sprite = playingSprites.Torso[currentFrame];
		spriteObjects[1].sprite = playingSprites.Left[currentFrame];
		spriteObjects[2].sprite = playingSprites.Right[currentFrame];
		spriteObjects[3].sprite = playingSprites.Legs[currentFrame];

		if(lockAnim)
		{
			CancelInvoke("Animate");
			return;
		}

		currentFrame++;
		if(currentFrame > playingSprites.Torso.Length - 1)
		{
			if(loop)
			{
				currentFrame = 0;
			}
			else
			{
				currentFrame = 0;
				PlayAnimation(Idle_Sprites, State.Idle, false);
			}
		}
		
		Invoke("Animate", Time.deltaTime*2);
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