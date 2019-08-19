using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	float timer = 3;
	//public void Skip()
	//{
	//	GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CmdEndTurn();
	//}

	public Text text;
	public Text endtext;
	public Text controls;
	public Button nextPhaseButton;

	GameObject win;

	public Image gun;
	public Image melee;
	public Image grenade;

	[Space]
	public Image p1health;
	public Image p2health;
	int fullHp = 670;

	[Space]
	public Image icon;
	public Image health;
	int fullHpActive = -1;

	//Mobile Input
	[HideInInspector] public Vector2 mousePos;
	[HideInInspector] public bool pressed;

	private void Start()
	{
		if(p1health != null)
		fullHp = (int)p1health.rectTransform.sizeDelta.x;
		fullHpActive = (int)health.rectTransform.sizeDelta.x;
	}

	private void Update()
	{
		//Get Mouse Input
		pressed = Input.GetMouseButton(0);
		mousePos = Input.mousePosition;

		//init healths
		float redHealth = 0;
		float blueHealth = 0;

		//calc total health per team
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			if(player.team == 0)
			{
				redHealth += player.health;
			}
			else
			{
				blueHealth += player.health;
			}

			if(player.IsTurn())
			{
				player.GetComponent<Movement>().horizontal = pressed ? (mousePos.x > 0 ? (1) : (-1)) : 0;
			}
		}

		//adjust team 1 healthbar
		if(p1health != null)
		{
			Vector2 p1Rect = p1health.rectTransform.sizeDelta;
			p1Rect.x = (redHealth / 300) * fullHp;
			p1health.rectTransform.sizeDelta = p1Rect;
		}

		//adjust team 2 healthbar
		if(p2health)
		{
			Vector2 p2Rect = p2health.rectTransform.sizeDelta;
			p2Rect.x = (blueHealth / 300) * fullHp;
			p2health.rectTransform.sizeDelta = p2Rect;
		}

		//define if button is clickable
		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.End || FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Damage)
		{
			nextPhaseButton.interactable = false;
		}
		else
		{
			nextPhaseButton.interactable = true;
		}

		//active player icon and health
		foreach(PlayerDataSP p in FindObjectsOfType<PlayerDataSP>())
		{
			if(p.IsTurn())
			{
				//set active icon
				icon.sprite = p.actor.Icon;

				//set active health
				Vector2 hprect = health.rectTransform.sizeDelta;
				hprect.x = (p.health / 100) * fullHpActive;
				health.rectTransform.sizeDelta = hprect;
			}
		}

		text.text = "Current Phase\n" + FindObjectOfType<GameManager>().phase.ToString();

		//TODO: move to game manager
		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.End)
		{
			endtext.text = "Next Turn: " + timer.ToString("f0");

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				NextPhase();
				timer = 3;

				foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
				{
					if(player.health <= 0)
					{
						Destroy(player.gameObject);
					}
				}
			}
		}
		else
		{
			if(endtext!= null)
			endtext.text = "";
		}

		int red = 0;
		int blue = 0;
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			switch(player.team)
			{
				case SpawnPoint.Team.Blue:
					blue++;
					break;
				case SpawnPoint.Team.Red:
					red++;
					break;
			}
		}

		if(red == 0)
		{
			if(win == null)
			{
				FindObjectOfType<GameManager>().enabled = false;
				win = (GameObject)Instantiate(Resources.Load("GameOver"));
				win.GetComponentInChildren<Text>().color = Color.blue;
				win.GetComponentInChildren<Text>().text = "Game Over!\nBlue Team Wins!";
			}

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}
		else if(blue == 0)
		{
			if(win == null)
			{
				FindObjectOfType<GameManager>().enabled = false;
				win = (GameObject)Instantiate(Resources.Load("GameOver"));
				win.GetComponentInChildren<Text>().color = Color.red;
				win.GetComponentInChildren<Text>().text = "Game Over!\nRed Team Wins!";
			}

			if(timer >= 0)
			{
				timer -= Time.deltaTime;
			}
			else
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene(0);
			}
		}

		ControlsText();
		WeaponSelected();
	}

	public void ControlsText()
	{
		switch(FindObjectOfType<GameManager>().phase)
		{
			case GameManager.TurnPhase.Move:
				controls.text = 
					"A: Left\n" +
					"D: Right\n\n" +
					"Jump\n" +
					"W: Up\n" +
					"Space: Forward";
				break;

			case GameManager.TurnPhase.Shoot:
				controls.text =
					"Mouse: Aim\n" +
					"LMB: Shoot";
				break;
		}
	}

	public void NextPhase()
	{
		if(FindObjectOfType<GameManager>().phase != GameManager.TurnPhase.End || FindObjectOfType<GameManager>().phase != GameManager.TurnPhase.Damage)
		{
			FindObjectOfType<GameManager>().NextPhase();
		}
	}

	void WeaponSelected()
	{
		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Shoot)
		{
			if(!gun.enabled && gun != null)
			{
				gun.enabled = true;
				melee.enabled = true;
				grenade.enabled = true;
			}

			foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
			{
				if(player.IsTurn())
				{
					switch(player.GetComponent<Shoot>().selectedWeapon)
					{
						case Shoot.Gun.Class:
							SetColor(gun);
							break;

						case Shoot.Gun.Melee:
							SetColor(melee);
							break;

						case Shoot.Gun.Grenade:
							SetColor(grenade);
							break;
					}
				}
			}
		}
		else
		{
			if(gun != null)
			{
				gun.enabled = false;
				melee.enabled = false;
				grenade.enabled = false;
			}
		}
	}

	void SetColor(Image img)
	{
		gun.color = Color.white;
		melee.color = Color.white;
		grenade.color = Color.white;

		img.color = Color.yellow;
	}

	public void UpJump()
	{
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			if(player.IsTurn())
			{
				player.GetComponent<Movement>().jump = true;
				player.GetComponent<Movement>().upJump = true;
			}
		}
	}

	public void SideJump()
	{
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			if(player.IsTurn())
			{
				player.GetComponent<Movement>().jump = true;
				player.GetComponent<Movement>().upJump = false;
			}
		}
	}

	public void Fire()
	{

	}

	//public void SkipTurn()
	//{
	//	FindObjectOfType<GameManager>().NextTurn();
	//}
}