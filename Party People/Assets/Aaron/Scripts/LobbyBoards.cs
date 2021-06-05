using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBoards : MonoBehaviour
{
    [SerializeField] private GameObject crystalCavernsPopup;
    [SerializeField] private string boardName;
    private LobbyControls playerZero;
    private GameObject playerObj;
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player") 
        {
            LobbyControls guy = other.GetComponent<LobbyControls>();
            if (guy.playerID == 0)
            {
                playerObj = guy.gameObject;
                playerZero = guy;
                guy.touchingBoard = true;
                guy.boardToPlay = boardName;
                crystalCavernsPopup.SetActive(true);
                Debug.Log("Play " + boardName + "?");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "Player" && other.gameObject == playerObj) 
        {
            playerZero.touchingBoard = false;
            crystalCavernsPopup.SetActive(false);
        }
    }
}
