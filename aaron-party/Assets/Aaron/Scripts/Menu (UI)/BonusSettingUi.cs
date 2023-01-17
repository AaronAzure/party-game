using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusSettingUi : MonoBehaviour
{
	public LobbyControls player;
	public Image img;
	public string bonusName;
	public bool available=true;


    public void TOGGLE_BONUS()
	{
		TOGGLE();
		if (player != null)
			player.TOGGLE_BONUS(bonusName);
	}

	public void TOGGLE()
	{
		if (img != null)
		{
			if (available)
			{
				available = !available;
				img.color = new Color(1, 0.1f, 0.1f, 1);
			}
			else
			{
				available = !available;
				img.color = new Color(1, 1, 1, 1);
			}
		}
	}
}
