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
	bool lockAnim = false;
	bool pause = false;

	//currentstate of the animation
	public enum State
	{
		Idle,
		Walk,
		Null,
		Melee,
		Aim,
		Grenade,
		Jump
	}
	public State state = State.Null;

	[HideInInspector]public Sprite[] sprites;
	int currentFrame;

	public void PlayAnimation(SpriteList sprites, State s)
	{
		PlayAnim(sprites, s, true, false, false);
	}

	public void PlayAnimation(SpriteList sprites, State s, bool loopAnim)
	{
		PlayAnim(sprites, s, loopAnim, false, false);
	}

	public void PlayAnimation(SpriteList sprites, State s, bool loopAnim, bool lockAnim)
	{
		PlayAnim(sprites, s, loopAnim, lockAnim, false);
	}

	public void PlayAnimation(SpriteList sprites, State s, bool loopAnim, bool lockAnim, bool pauseEnd)
	{
		PlayAnim(sprites, s, loopAnim, lockAnim, pauseEnd);
	}

	void PlayAnim(SpriteList sprites, State s, bool loopAnim, bool lockAnim, bool pauseEnd)
	{
		if(s != state)
		{
			CancelInvoke("Animate");

			currentFrame = 0;

			playingSprites = sprites;
			state = s;
			loop = loopAnim;
			this.lockAnim = lockAnim;
			pause = pauseEnd;

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

		//test loop
		if(currentFrame > playingSprites.Torso.Length - 1)
		{
			//loop
			if(loop)
			{
				currentFrame = 0;
			}
			//else reset and play idle
			else
			{
				currentFrame = 0;

				if(pause)
				{
					CancelInvoke("Animate");
					return;
				}

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