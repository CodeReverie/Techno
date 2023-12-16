using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class AATrialButton : MonoBehaviourPunCallbacks
{   //General Variables 
    public TextMeshProUGUI[] numberTexts;
    private int[] counters;
    private PhotonView view;
    private PlayerDevice currentPlayerDevice;
    private Dictionary<int, int> playerButtonMap; // Map player IDs to button indices
    private Button[] playerButtons; // Array of player buttons



    public enum PlayerDevice
    {//variables for players
        Phone1,
        Phone2,
        Phone3,
        Phone4,
        Phone5,
        Phone6
    }
    
    void Start()
    {
        view = GetComponent<PhotonView>();
        counters = new int[numberTexts.Length];
        playerButtonMap = new Dictionary<int, int>();
        playerButtons = new Button[numberTexts.Length];

        // Life Counters
        List<int> availableButtonIndices = new List<int>();
        for (int i = 0; i < numberTexts.Length; i++)
        {
            availableButtonIndices.Add(i);
            counters[i] = 1;
            numberTexts[i].text = counters[i].ToString();
            playerButtons[i] = GetComponent<Button>(); // Assuming your buttons are on the same GameObject
        }
        //Number of Players with buttons of course 
        for (int i = 1; i <= 6; i++)
        {
            int randomButtonIndex = Random.Range(0, availableButtonIndices.Count);
            int playerID = i; // Adjust this if your player IDs don't start from 1
            int buttonIndex = availableButtonIndices[randomButtonIndex];

            playerButtonMap.Add(playerID, buttonIndex);
            availableButtonIndices.RemoveAt(randomButtonIndex);
        }

        // Player POV and assign roles
        if (PhotonNetwork.IsConnected && view.IsMine)
        {
            int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            if (playerButtonMap.ContainsKey(actorNumber))
            {
                currentPlayerDevice = (PlayerDevice)playerButtonMap[actorNumber];
            }
            else
            {
                // Handle the case where the player's button is not assigned
            }

            // Check if all 6 players are in the room
            if (PhotonNetwork.PlayerList.Length >= 6)
            {
                // Close the room
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private bool IsActionButtonEnabled(int buttonIndex)
    {
        //Conditions to click 
        // Logic to determine if the action button is enabled based on the current player's device
        switch (currentPlayerDevice)
        {
            case PlayerDevice.Phone1:

                return counters[buttonIndex] > 1; // Player 1 can only increase counter if it's > 0
            case PlayerDevice.Phone2:
                return counters[buttonIndex] == 1; // Player 2 can only decrease counter if it's > 0
            case PlayerDevice.Phone3:
                return counters[buttonIndex] == 0; // Player 3 can only increase if counter is 0
            case PlayerDevice.Phone4:
                            // Implement logic to copy ability of the player
                return counters[buttonIndex] == 1; // Adjust this condition based on copied ability
            case PlayerDevice.Phone5:
                return counters[buttonIndex] != 0; // player 5 can disable button if they're not deado
            case PlayerDevice.Phone6:
                return counters[buttonIndex] == 1; // Player 6 can reveal player's role
            default:
                return false;
        }
    }

    public void OnButtonPress(int buttonIndex)
    {
        if (PhotonNetwork.IsConnected)
        {
            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex)
            {
                switch (currentPlayerDevice)
                {
                    case PlayerDevice.Phone1:
                        // WEREWOLF FT. THREAT
                        counters[buttonIndex]--;
                        break;
                    case PlayerDevice.Phone2:
                        // BODYGUARD FT. FIREWALL
                        counters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone3:
                        // ANGEL FT. BACKUPFILE
                        counters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone4:
                        // ??????
                        // For now, let's just increase the counter
                        counters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone5:
                        //  JAILER FT. QUARANTINE MANAGER
                        // Determine the player ID associated with the pressed button
                        int playerIdToDisable = GetPlayerIdForButton(buttonIndex);
                        // Disable the button for the associated player
                        DisableButton(playerIdToDisable);
                        counters[buttonIndex]= -5;
                        break;
                    case PlayerDevice.Phone6:
                        // SEER FT. Network Monitoring TOOL
                        // Implement logic to see other players and perform action here
                        counters[buttonIndex]++;
                        break;
                }
                view.RPC("UpdateCount", RpcTarget.All, buttonIndex, counters[buttonIndex]);
            }
        }
        else
        {
            counters[buttonIndex]++;
            numberTexts[buttonIndex].text = counters[buttonIndex].ToString();
        }
    }

    [PunRPC]
    private void UpdateCount(int buttonIndex, int newCounter)
    {
        counters[buttonIndex] = newCounter;
        numberTexts[buttonIndex].text = newCounter.ToString();
    }
    //Codes for Quarantine Manager
    private void DisableButton(int playerIdToDisable)
    {
        foreach (var kvp in playerButtonMap)
        { // codes just to separate the ID and button index
            int playerID = kvp.Key;
            int buttonIndex = kvp.Value;

            if (playerID == playerIdToDisable)
            {
                // Disable the button associated with the specified player ID
                if (buttonIndex >= 0 && buttonIndex < playerButtons.Length)
                {
                    playerButtons[buttonIndex].interactable = false;
                    Debug.Log($"Disabled button for player ID {playerIdToDisable}");
                }
                else
                {
                    Debug.LogError($"Invalid button index for player ID {playerIdToDisable}");
                }
                break;
            }
        }
    }

    // method to get the player ID associated with a button index
    private int GetPlayerIdForButton(int buttonIndex)
    {
        foreach (var kvp in playerButtonMap)
        {
            if (kvp.Value == buttonIndex)
            {
                return kvp.Key;
            }
        }

        return -1; // Return -1 if the button index is not found 
    }

// Roles Assignment Transition Screen 

    




}


