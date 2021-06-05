using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using TMPro;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour
{
    [SerializeField] private GameObject speechBox;
    [SerializeField] private TextMeshPro gameName;
    public string minigameName;
    public string quickName;
    public Player player;
    private int playerID = 0;
    private bool selected;
    private int nInside;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerID);
        if (minigameName != null) gameName.text = minigameName;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            nInside++;
            if (nInside > 0)
            {
                if (speechBox != null) speechBox.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            nInside--;
            if (nInside <= 0)
            {
                if (speechBox != null) speechBox.SetActive(false);
            }
        }
    }
}
