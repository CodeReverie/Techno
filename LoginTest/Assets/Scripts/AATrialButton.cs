using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class AATrialButton : MonoBehaviourPunCallbacks
{   //General Variables 
    public TextMeshProUGUI[] numberTexts;
    public TextMeshProUGUI[] voteTexts;
    public GameObject rolePanel;
    public TextMeshProUGUI roleTextElement;
   
    private int[] counters;
    private PhotonView view;
    private PlayerDevice currentPlayerDevice;
    private Dictionary<int, int> playerButtonMap; // Map player IDs to button indices
    private Button[] playerButtons; // Array of player buttons
    
    private int[] votecounters;
    
    //timer
    public float timeRemaining = 120;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public TMP_Text PhaseText;
    public bool nightPhase = true;
    public bool gameIsRunning = true;
    public string roleEquivalent;



    public enum PlayerDevice
    {//variables for players
        Phone1,
        Phone2,
        Phone3,
        Phone4,
        Phone5,
        Phone6,
        Phone7,
        Phone8,
        Phone9,
        Phone10,
        Phone11,
        Phone12
    }
    
    void Start()
    {
        //timer
        timeIsRunning = true;
        nightPhase = false;
        timeRemaining = 5; 

        view = GetComponent<PhotonView>();
        counters = new int[numberTexts.Length];
        votecounters = new int[voteTexts.Length];
        playerButtonMap = new Dictionary<int, int>();
        playerButtons = new Button[numberTexts.Length];
        playerButtons = new Button[voteTexts.Length];

        // Life Counters
        List<int> availableButtonIndices = new List<int>();
        for (int i = 0; i < numberTexts.Length; i++)
        {
            availableButtonIndices.Add(i);
            counters[i] = 1;
            numberTexts[i].text = counters[i].ToString();
            playerButtons[i] = GetComponent<Button>(); // Assuming your buttons are on the same GameObject
        }
        //vote counters
        for (int i = 0; i < voteTexts.Length; i++)
        {
            availableButtonIndices.Add(i);
            votecounters[i] = 1;
            voteTexts[i].text = votecounters[i].ToString();
            playerButtons[i] = GetComponent<Button>(); // Assuming your buttons are on the same GameObject
        }
        //Number of Players with buttons of course 
        for (int i = 1; i <= 12; i++)
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
                ShowRolePanel(playerButtonMap[actorNumber].ToString());

            }
            else
            {
                // Handle the case where the player's button is not assigned
            }

            // Check if all 6 players are in the room
            if (PhotonNetwork.PlayerList.Length >= 12)
            {
                // Close the room
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

       
    }
        //UI For ROLES Assignment 
        private void ShowRolePanel(string roleText){

          
            switch (roleText){
            case "1" :
                roleEquivalent = "Virus";
                break;
            case "2" :
              roleEquivalent = "Firewall";
                break;
            case "3" :
            
               roleEquivalent = "Backup FIle";
                break;
            case "4" :
            
               roleEquivalent = "Database";
                break;
            case "5" :
            
                roleEquivalent = "Quarantine Manager";
                break;
            case "6" :
            
              roleEquivalent = "Network Monitor";
                break;
            case "7" :
            
              roleEquivalent = "Database";
                break;
            case "8" :
            
            
              roleEquivalent = "Database";
                break;
            case "9" :
            
                roleEquivalent = "Database";
                break;
            case "10" :
            
               roleEquivalent = "Database";
                break;
            case "11" :
            
               roleEquivalent =  "Worm";
                break;
            case "12" :
            
                roleEquivalent =  "Spyware";
                break;
                }
        
      
             roleTextElement.text = "Your Role is: " + roleEquivalent;
             rolePanel.SetActive(true);

             StartCoroutine(HideRolePanel(rolePanel));
        }
        
        private IEnumerator HideRolePanel(GameObject rolePanel){
                
                yield return new WaitForSeconds(5f); // Adjust the delay as needed
                rolePanel.SetActive(false);
                
        }





    
    //timer
    
    
    void Update()
    {
        if (timeIsRunning && nightPhase)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                SwitchPhase("Day Phase");
            }
        }
        else if (timeIsRunning && !nightPhase)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                SwitchPhase("Night Phase");
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SwitchPhase(string newPhase)
    {
        timeIsRunning = true;
        timeRemaining = 0;
        DisplayTime(timeRemaining);
        PhaseText.text = newPhase;

        if (newPhase == "Day Phase")
        {
            timeRemaining = 5;
        }
        else if (newPhase == "Night Phase")
        {
            timeRemaining = 5;
        }

        nightPhase = !nightPhase; // Switch the phase
    }
//timer end


    private bool IsActionButtonEnabled(int buttonIndex)
    {
        //Conditions to click 
        // Logic to determine if the action button is enabled based on the current player's device
        switch (currentPlayerDevice)
        {
            case PlayerDevice.Phone1:
             if (nightPhase){
                return counters[buttonIndex] > 0; // Player 1 can only decrease counter if it's > 0
             }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone2:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 2 can only increase counter if it's > 0
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone3:
             if (nightPhase){
                return counters[buttonIndex] == 0; // Player 3 can only increase if counter is 0
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone4:
             if (nightPhase){
                return counters[buttonIndex] == 1; // NO ROLE ASSIGNED YET
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone5:
             if (nightPhase){
                return counters[buttonIndex] > 0; // player 5 can disable button if they're not deado
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone6:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 6 can reveal player's role
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone7:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 7 Villager
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone8:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 8 can reveal player's role
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone9:
             if (nightPhase){
                return counters[buttonIndex] > 0; // Player 9 can can only decrease counter if it's > 0
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone10:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 10 Villager
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone11:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 11 Villager
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone12:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Player 12 Villager
             }
             else {
                return votecounters[buttonIndex] > 0;
             }
            default:
                return false;     
        }
    }

    public void OnButtonPress(int buttonIndex)
    {
        if (PhotonNetwork.IsConnected)
        {

            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex && timeIsRunning && nightPhase)
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
                        // MEDIC FT. BACKUPFILE
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
                     case PlayerDevice.Phone7:
                        // VILLAGER FT. DATABASE
                        counters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone8:
                        //
                        // Implement logic to see other players and perform action here
                        counters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone9:
                        //Virus
                        // Implement logic to see other players and perform action here
                        counters[buttonIndex]--;
                        break;          
                }
                view.RPC("UpdateCount", RpcTarget.All, buttonIndex, counters[buttonIndex]);
            }
            
            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex && timeIsRunning && !nightPhase)
            {
                switch (currentPlayerDevice)
                {
                    case PlayerDevice.Phone1:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone2:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone3:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone4:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone5:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone6:
                        votecounters[buttonIndex]++;
                        break;
                     case PlayerDevice.Phone7:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone8:
                        votecounters[buttonIndex]++;
                        break;
                    case PlayerDevice.Phone9:
                        votecounters[buttonIndex]++;
                        break;          
                }
                view.RPC("UpdateVoteCount", RpcTarget.All, buttonIndex, votecounters[buttonIndex]);
            }
        }
       

    }

  public void UpdateVoteCount(int buttonIndex, int newVoteCounter)
    {
        votecounters[buttonIndex] = newVoteCounter;
        voteTexts[buttonIndex].text = newVoteCounter.ToString();
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


