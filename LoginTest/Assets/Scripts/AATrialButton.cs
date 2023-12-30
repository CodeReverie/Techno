using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class AATrialButton : MonoBehaviourPunCallbacks
{   //General Variables 
    public TextMeshProUGUI[] numberTexts;
    public TextMeshProUGUI[] voteTexts;
    public GameObject rolePanel,seerPanel;
    public TextMeshProUGUI roleTextElement,seerText;
   
    private int[] counters;
    private PhotonView view;
    private PlayerDevice currentPlayerDevice;
    private Dictionary<int, int> playerButtonMap; // Map player IDs to button indices
    private Button[] playerButtons; // Array of player buttons
    private Button[] voteButtons; // Array of vote buttons
    
    private int[] votecounters;
    private bool[] wormCountersPlaced;
    
    //timer
    public float timeRemaining = 120;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public TMP_Text PhaseText;
    public bool nightPhase = true;
    public bool gameIsRunning = true;
    public string roleEquivalent;
    public int voteRemaining = 1;
    public bool firstNightPhase = true;



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
        firstNightPhase = true; 

        view = GetComponent<PhotonView>();
        counters = new int[numberTexts.Length];
        votecounters = new int[voteTexts.Length];
        wormCountersPlaced = new bool[numberTexts.Length];

        playerButtonMap = new Dictionary<int, int>();
        playerButtons = new Button[numberTexts.Length];
        voteButtons = new Button[voteTexts.Length];
       

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
        voteButtons = new Button[voteTexts.Length];
        for (int i = 0; i < voteTexts.Length; i++)
        {
            availableButtonIndices.Add(i);
            votecounters[i] = 1;
            voteTexts[i].text = votecounters[i].ToString();
            voteButtons[i] = playerButtons[i];
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
            case "0" :
                roleEquivalent = "Virus";
                break;
            case "1" :
              roleEquivalent = "Firewall";
                break;
            case "2" :
            
               roleEquivalent = "Backup FIle";
                break;
            case "3" :
            
               roleEquivalent = "Database";
                break;
            case "4" :
            
                roleEquivalent = "Quarantine Manager";
                break;
            case "5" :
            
              roleEquivalent = "Network Monitor";
                break;
            case "6" :
            
              roleEquivalent = "Database";
                break;
            case "7" :
            
            
              roleEquivalent = "Database";
                break;
            case "8" :
            
                roleEquivalent = "Database";
                break;
            case "9" :
                roleEquivalent = "Phishing";
                break;
            case "10" :
            
               roleEquivalent =  "Worm";
                break;
            case "11" :
            
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
        voteRemaining = 1;

        if (newPhase == "Day Phase")
        {
            timeRemaining = 5;
            voteRemaining = 2;
            // Reset vote counts to 0 at the beginning of each day phase
            for (int i = 0; i < votecounters.Length; i++)
            {
            votecounters[i] = 1;
            
            view.RPC("UpdateVoteCount", RpcTarget.All, i, 1);
            }



        }
        else if (newPhase == "Night Phase")
        {
            timeRemaining = 5;
            if (PhotonNetwork.IsConnected && view.IsMine)
            {
            int playerToEliminate = GetPlayerWithHighestVotes();
            
            if (playerToEliminate != -1)
            {

                if (firstNightPhase){
                    Debug.Log("First Voting Phase - No player eliminated");
                }
                // Reduce the life of the player with the highest votes to 0
                else{
                    counters[playerButtonMap[playerToEliminate]] -= 10;

                    // Notify all players about the life reduction
                    view.RPC("UpdateCount", RpcTarget.All, playerButtonMap[playerToEliminate], 0);
                }
                firstNightPhase = false; 
            }
            }
            

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
                return counters[buttonIndex] == 1; // Player 2 can only increase counter if it's =1
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
                return counters[buttonIndex] >0; // Player 6 can reveal player's role
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
                        // WEREWOLF FT. VIRUS
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
                    /*case PlayerDevice.Phone4:
                        // VILLAGER FT. DATABASE
                        counters[buttonIndex]++;
                        break;
                    */
                
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

                        int displayID = AcquireID(buttonIndex);
                        ShowPlayerIDPanel(displayID); 
                        break;

                    /*case PlayerDevice.Phone7:
                        // VILLAGER FT. DATABASE
                        counters[buttonIndex]++;
                        break;*/

                    /* case PlayerDevice.Phone8:
                        //
                        // Implement logic to see other players and perform action here
                        counters[buttonIndex]++;
                        break;*/

                    /*case PlayerDevice.Phone9:
                        //Virus
                        // Implement logic to see other players and perform action here
                        counters[buttonIndex]--;
                        break;*/    
                    /*case PlayerDevice.Phone10:
                        //Jester FT. Phishing
                    break;*/

                    case PlayerDevice.Phone11:
                            //Douser FT. WORM 
                        int targetPlayerIndex = buttonIndex;
                        // Check if Player 11 has already placed a counter on the target player
                        if (!wormCountersPlaced[targetPlayerIndex])
                            {
                            counters[targetPlayerIndex]++;
                            wormCountersPlaced[targetPlayerIndex] = true;

                            // Update UI and notify other players
                            view.RPC("UpdateCount", RpcTarget.All, targetPlayerIndex, counters[targetPlayerIndex]);

                            // Check if Player 11 has placed a counter on every player with life left
                            if (wormCountersPlaced.All(x => x))
                            {
                                // Player 11 wins, handle victory condition here
                                Debug.Log("Player 11 (Worm) has won!");
                            }
                        }
                    else
                    {
                        // Player 11 has already placed a counter on this player
                        Debug.Log("Player 11 (Worm) has already placed a counter on this player.");
                    }
                    
                    break;

                

                    case PlayerDevice.Phone12:
                            //Seer Wolf FT. SPYWARE
                            displayID = AcquireID(buttonIndex);
                            ShowPlayerIDPanel(displayID); 
                    break;



                }
                view.RPC("UpdateCount", RpcTarget.All, buttonIndex, counters[buttonIndex]);
            }
            
            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex && timeIsRunning && !nightPhase)
            {
                switch (currentPlayerDevice)
                {
                    case PlayerDevice.Phone1:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                        voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone2:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone3:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone4:
                       if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone5:
                        if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone6:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                     case PlayerDevice.Phone7:
                      if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone8:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone9:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;   
                    case PlayerDevice.Phone10:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone11:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;  
                    case PlayerDevice.Phone12:
                        if (voteRemaining>=0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
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


    //codes for VotingElimination
    private int GetPlayerWithHighestVotes()
    {
    int maxVotes = -1;
    int playerWithHighestVotes = -1;

    for (int i = 0; i < votecounters.Length; i++)
    {
        if (votecounters[i] > maxVotes)
        {
            maxVotes = votecounters[i];
            playerWithHighestVotes = GetPlayerIdForButton(i);
        }
    }
    
    if (playerWithHighestVotes == 10 && roleEquivalent == "Phishing")
    {
        Debug.Log("Player 10 (Phishing) has won!");
        // You may want to handle the victory condition here
    }


    return playerWithHighestVotes;
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
    //method to show player ID
    private void ShowPlayerIDPanel(int displayID){
        seerPanel.SetActive(true);
        seerText.text = "Player Role ID: " + displayID;
        StartCoroutine(HidePlayerIDPanel(seerPanel));
    }

    private IEnumerator HidePlayerIDPanel(GameObject seerPanel)
    {
    yield return new WaitForSeconds(5f); // Adjust the delay as needed
    seerPanel.SetActive(false);
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
    private int AcquireID(int buttonIndex)
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




}


