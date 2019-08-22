using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	float timer = 3;

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

	[Space]
	public Image controller;
	bool isMobile;
	public GameObject MoveIcons;
	public GameObject ShootIcons;

	private void Start()
	{
		if(p1health != null)
		fullHp = (int)p1health.rectTransform.sizeDelta.x;
		fullHpActive = (int)health.rectTransform.sizeDelta.x;

		isMobile = PlayerPrefs.GetInt("Player" + 1 + "Controller") == -1 ? true : false;
	}

	private void Update()
	{
		//check if mobile
		if(isMobile)
		{
			//update icons depending on phase
			IconUpdate();

			//get screen input
			mousePos = Input.mousePosition;

			//get touch mover - adding a half so its correctly centered
			Rect r = controller.rectTransform.rect;
			r.x += r.width / 2;
			r.y += r.height / 2;

			if(r.Contains(mousePos))//check if we're pressing the move stick
			{
				pressed = Input.GetMouseButton(0);
			}
			else if(Input.GetMouseButtonUp(0))//check if we've released outside the stick
			{
				pressed = false;
			}
		}

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

			//check if players turn
			if(player.IsTurn())
			{
				int dir = 0;

				//find mouse direction
				if(pressed)
				{
					dir = mousePos.x > controller.rectTransform.position.x ? 1 : -1;
					player.GetComponent<Shoot>().conDir = mousePos - (Vector2)controller.rectTransform.position;
				}


				player.GetComponent<Movement>().horizontal = dir;

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
				if(p.actor != null)
				icon.sprite = p.actor.Icon;

				//set active health
				Vector2 hprect = health.rectTransform.sizeDelta;
				hprect.x = (p.health / 100) * fullHpActive;
				health.rectTransform.sizeDelta = hprect;
			}
		}

		text.text = "Current Phase\n" + FindObjectOfType<GameManager>().phase.ToString();


		if(FindObjectOfType<GameManager>().gameStart)
		{
			EndTurnCheck();		
			CheckGameOver();
		}

		ControlsText();
		WeaponSelected();
	}

	void EndTurnCheck()
	{
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
			if(endtext != null)
				endtext.text = "";
		}
	}

	void CheckGameOver()
	{
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

		if(red == 0) //death check red
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
		else if(blue == 0) // death check blue
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
	}

	void IconUpdate()
	{
		MoveIcons.SetActive(false);
		ShootIcons.SetActive(false);

		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Move)
		{
			MoveIcons.SetActive(true);
		}
		else if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Shoot)
		{
			ShootIcons.SetActive(true);
		}
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

	public void PickWeapon(int select)
	{
		//cycle all players
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			//find active player
			if(player.IsTurn())
			{
				player.GetComponent<Shoot>().selectedWeapon = (Shoot.Gun)select;
			}
		}
	}

	void WeaponSelected()
	{
		if(FindObjectOfType<GameManager>().phase == GameManager.TurnPhase.Shoot)
		{
			//ensure there is an object to prevent erroring
			if(gun != null)
			{
				//mobile check to enable or disable elements
				gun.enabled = isMobile ? false : true;
				melee.enabled = isMobile ? false : true;
				grenade.enabled = isMobile ? false : true;

				//cycle all players
				foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
				{
					//find active player
					if(player.IsTurn())
					{
						print(player.GetComponent<Shoot>().selectedWeapon);
						//check what weapon we're affecting
						switch(player.GetComponent<Shoot>().selectedWeapon)
						{
							case Shoot.Gun.Class:
								if(!isMobile) SetColor(gun); else gun.enabled = true;
								break;

							case Shoot.Gun.Melee:
								if(!isMobile) SetColor(melee); else melee.enabled = true;
								break;

							case Shoot.Gun.Grenade:
								if(!isMobile) SetColor(grenade); else grenade.enabled = true;
								break;
						}
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
		print("Upjump!");
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			if(player.IsTurn())
			{
				player.GetComponent<Movement>().UpJump();
			}
		}
	}

	public void SideJump()
	{
		print("SideJump!!");
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			if(player.IsTurn())
			{
				player.GetComponent<Movement>().SideJump();
			}
		}
	}

	public void Fire()
	{
		//cycle all players
		foreach(PlayerDataSP player in FindObjectsOfType<PlayerDataSP>())
		{
			//find active player
			if(player.IsTurn())
			{
				player.GetComponent<Shoot>().ShootWeapon();
			}
		}
	}

	//public void SkipTurn()
	//{
	//	FindObjectOfType<GameManager>().NextTurn();
	//}
}