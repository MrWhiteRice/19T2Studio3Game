using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteAnim : MonoBehaviour
{
	//0 = torso, 1 = f/l arm, 2 = b/r arm, 3 = legs
	public SpriteRenderer[] spriteObjects;

	public SpriteList Idle_Sprites;
	public SpriteList Walk_Sprites;
	public SpriteList Melee_Sprites;

	SpriteList playingSprites;

	bool loop = true;

	//currentstate of the animation
	public enum State
	{
		Idle,
		Walk,
		Null,
		Melee
	}
	public State state;

	public Sprite[] sprites;
	int currentFrame;

	public void PlayAnimation(SpriteList sprites, State s)
	{
		if(s != state)
		{
			CancelInvoke("Animate");
			currentFrame = 0;
			playingSprites = sprites;
			state = s;

			Invoke("Animate", 0);
		}
	}

	public void PlayAnimation(SpriteList sprites, State s, bool set)
	{
		loop = set;

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

		currentFrame++;
		if(currentFrame > playingSprites.Torso.Length - 1)
		{
			currentFrame = 0;
		}
		
		if(loop)
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