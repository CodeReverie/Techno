using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
//AFTER PULLING

public class AATrialButton : MonoBehaviourPunCallbacks
{   //General Variables 
    public TextMeshProUGUI[] numberTexts;
    public TextMeshProUGUI[] voteTexts;
    public GameObject rolePanel,seerPanel;
    public GameObject firewallPanel, backupPanel, networkPanel, dataPanel, virusPanel, phishingPanel, spywarePanel;
    public TextMeshProUGUI roleTextElement,seerText;
   
    private int[] counters;
    private PhotonView view;
    private PlayerDevice currentPlayerDevice;
    private Dictionary<int, int> playerButtonMap; // Map player IDs to button 
    private Button[] playerButtons; // Array of player buttons
    private Button[] voteButtons; // Array of vote buttons
    

    private int[] votecounters; 
    
    
    
    //timer variables
    public float timeRemaining = 120;
    public bool timeIsRunning = true;
    public TMP_Text timeText;
    public TMP_Text PhaseText;
    public bool nightPhase = true;
    public bool gameIsRunning = true;
    public string roleEquivalent;
    public string roleDescription;
    public int voteRemaining = 1;
    public bool firstNightPhase = true;
    //once per turn and once per game
    public int optFirewall =1;
    public int opgBackup =1;
    public int optMonitor =1;
    public int optVirus =1;
    public int optSpy =1;
    



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
        Phone9
        /*Phone10,
        Phone11,
        Phone12*/
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
        for (int i = 1; i <= 9; i++)
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

            // Check if all 9 players are in the room
            if (PhotonNetwork.PlayerList.Length >= 9)
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
              roleEquivalent = "Firewall";
              roleDescription = "You can protect 1 player from being attacked each night";
              OpenFirewall();
                break;

            case "1" :
               roleEquivalent = "Backup FIle";
               roleDescription = "You can revive one player at night. You can only use this once";
               OpenBackup();
                break;
                     
            case "2" :
              roleEquivalent = "Network Monitor";
              roleDescription = "You can reveal 1 player role each night";
              OpenNetwork();
                break;

            case "3" :
              roleEquivalent = "Database";

              OpenData();
                break;

            case "4" :
              roleEquivalent = "Database";
              OpenData();
                break;

            case "5" :
                roleEquivalent = "Database";
                OpenData();
                break;

            case "6" :
                roleEquivalent = "Phishing";
                OpenPhishing();
                break;

            case "7" :
                roleEquivalent = "Virus";
                OpenVirus();
                break;

            case "8" :
                roleEquivalent =  "Spyware";
                OpenSpyware();
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


    //Phasing Logics base on timer
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
        
        optFirewall =1; 
        optMonitor =1;
        optSpy =1;
        optVirus =1; 

        if (newPhase == "Day Phase")
        {
            timeRemaining = 5;
            voteRemaining = 1;
            optFirewall =1; 
            optMonitor =1;
            optSpy =1;
            optVirus =1; 
            // Reset vote counts to 0 at the beginning of each day phase
            for (int i = 0; i < votecounters.Length; i++)
            {
            votecounters[i] = 1;
            
            view.RPC("UpdateVoteCount", RpcTarget.All, i, 1);
            }
             //ResetDisabledPlayers();
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
                /*if (CheckWormVictory())
                {
                    Debug.Log("Player 11 (Worm) has won!");
                    // Handle Worm's victory here
                }*/
            }
        }

        nightPhase = !nightPhase; // Switch the phase
    }
//timer end



    private bool IsActionButtonEnabled(int buttonIndex)
    {
        //Conditions to click other players
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
                return counters[buttonIndex] == 1; // Player 2 can only increase counter if it's = 1
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone3:
             if (nightPhase){
                return counters[buttonIndex] > 0; // Player 3 can only see alive player's roles
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone4:
             if (nightPhase){
                return counters[buttonIndex] == 1; // DATABASE NO ROLE/special ability ASSIGNED YET
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone5:
             if (nightPhase){
                return counters[buttonIndex] > 0; // DATABASE NO ROLE/special ability ASSIGNED YET
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone6:
             if (nightPhase){
                return counters[buttonIndex] >0; // DATABASE NO ROLE/special ability ASSIGNED YET
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone7:
             if (nightPhase){
                return counters[buttonIndex] == 1; // Wins if gets voted out
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone8:
             if (nightPhase){
                return counters[buttonIndex] > 0; // reduces player hp if alive
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            case PlayerDevice.Phone9:
             if (nightPhase){
                return counters[buttonIndex] > 0; // Player 9 can only see alive player's roles 
                 }
             else {
                return votecounters[buttonIndex] > 0;
             }
            /*case PlayerDevice.Phone10:
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
             }*/
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
                int displayID;
                
                switch (currentPlayerDevice)
                {
                    case PlayerDevice.Phone1:
                        // BODYGUARD FT. FIREWALL
                        if (optFirewall>0){
                        counters[buttonIndex]++;
                        optFirewall--;
                        }
                        break;
                    case PlayerDevice.Phone2:
                        // MEDIC FT. BACKUPFILE
                        if (opgBackup>0){
                        counters[buttonIndex]++;
                        opgBackup--;
                        }
                        break;
                        
                    case PlayerDevice.Phone3:
                        // SEER FT. Network Monitoring TOOL
                        if (optMonitor>0){
                        displayID = AcquireID(buttonIndex);
                        ShowPlayerIDPanel(displayID); 
                        optMonitor--;
                        }
                        break;
                    /*VILLAGERS FT. DATABASES
                    case PlayerDevice.Phone4:
                        break;
                    case PlayerDevice.Phone5: 
                        break;
                    case PlayerDevice.Phone6:
                        break;
                    */
                   /*  JESTER FT. PHISHING
                    case PlayerDevice.Phone7:
                        JESTER FT. PHISHING
                        counters[buttonIndex]++;
                        break;
                    */

                    case PlayerDevice.Phone8:
                       // WEREWOLF FT. VIRUS
                        if (optVirus>0){
                        counters[buttonIndex]--;
                        optVirus--;
                        }
                        break;

                    case PlayerDevice.Phone9:
                        //Seer Wolf FT. SPYWARE
                        if (optSpy > 0){
                        displayID = AcquireID(buttonIndex);
                        ShowPlayerIDPanel(displayID); 
                        optSpy--;}
                        break;

                    /*case PlayerDevice.Phone10:
                       // JAILER FT. QUARANTINE MANAGER
                        // Determine the player ID associated with the pressed button
                        int playerIdToDisable = GetPlayerIdForButton(buttonIndex);
                        // Disable the button for the associated player
                        DisableButton(playerIdToDisable);
                        counters[buttonIndex]= -5;
                    break;
                    case PlayerDevice.Phone11:
                            //Douser FT. WORM 
                        if (wormTargetPlayer == -1)
                            {
                                wormTargetPlayer = GetPlayerIdForButton(buttonIndex);
                                // Place the counter for the selected player
                                counters[buttonIndex]++;
                                // Notify all players about the life increase
                                view.RPC("UpdateCount", RpcTarget.All, buttonIndex, counters[buttonIndex]);
                            }
                    break;
                    case PlayerDevice.Phone12:
                    break;
                    */


                }
                view.RPC("UpdateCount", RpcTarget.All, buttonIndex, counters[buttonIndex]);
            }
            
            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex && timeIsRunning && !nightPhase)
            {
                switch (currentPlayerDevice)
                {
                    case PlayerDevice.Phone1:
                        if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                        voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone2:
                        if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone3:
                        if (voteRemaining>0){
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
                        if (voteRemaining>=0){
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
                      if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                        voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone8:
                        if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;
                    case PlayerDevice.Phone9:
                        if (voteRemaining>0){
                        votecounters[buttonIndex]++;
                         voteRemaining--;
                        }
                        break;   
                  /*  case PlayerDevice.Phone10:
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
                        break;  */      
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

/*
    //Codes for Quarantine Manager

    private List<int> disabledPlayers = new List<int>();

    private void DisableButton(int playerIdToDisable)
    {
    // Check if the player is not already disabled
    if (!disabledPlayers.Contains(playerIdToDisable))
        {
        foreach (var kvp in playerButtonMap)
            {
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

                // Add the disabled player to the list
                disabledPlayers.Add(playerIdToDisable);
                break;
                }
            }
        }
    }
    private void ResetDisabledPlayers()
{
    disabledPlayers.Clear();
}
*/


    //method to show player ID
    private void ShowPlayerIDPanel(int displayID){
        seerPanel.SetActive(true);

        int playerID = GetPlayerIdForButton(displayID);
        string roleEquivalent = GetRoleEquivalent(playerID);


        seerText.text = "Player Role ID: " + roleEquivalent;
        StartCoroutine(HidePlayerIDPanel(seerPanel));
    }



    private IEnumerator HidePlayerIDPanel(GameObject seerPanel)
    {
    yield return new WaitForSeconds(5f); // Adjust the delay as needed
    seerPanel.SetActive(false);
    }

    private string GetRoleEquivalent(int playerID)
    {
    switch (playerID)
    {
        case 0:
            return "Firewall";
        case 1:
            return "Backup File";
        case 2:
            return "Network Monitor";
        case 3:
            return "Database";
        case 4:
            return "Database";
        case 5:
            return "Database";
        case 6:
            return "Phishing";
        case 7:
            return "Virus";
        case 8:
            return "Spyware";
        // ... (add cases for other player IDs if needed)
        default:
            return "Unknown Role";
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
    //Codes for Worm
/*
    private bool CheckWormVictory()
{
    // Check if all players who have life counters also have a Worm counter
    foreach (var kvp in playerButtonMap)
    {
        int playerID = kvp.Key;
        int buttonIndex = kvp.Value;

        if (counters[buttonIndex] > 0 && playerID != 11) // Skip Worm's own counter
        {
            // Check if the player has a Worm counter
            if (counters[buttonIndex] == 0)
            {
                return false; // Found a player without a Worm counter
            }
        }
    }

    // Check if the Worm has placed a counter on each player with a life counter
    return wormTargetPlayer != -1 && counters[playerButtonMap[wormTargetPlayer]] > 0;
}
*/


    public void OpenFirewall()
    {
        firewallPanel.SetActive(true);
        backupPanel.SetActive(false);
        networkPanel.SetActive(false);
        dataPanel.SetActive(false);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(false);
    }
     public void OpenBackup()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(true);
        networkPanel.SetActive(false);
        dataPanel.SetActive(false);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(false);
    }
     public void OpenNetwork()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(false);
        networkPanel.SetActive(true);
        dataPanel.SetActive(false);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(false);
    }
     public void OpenData()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(false);
        networkPanel.SetActive(false);
        dataPanel.SetActive(true);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(false);
    }
     public void OpenVirus()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(false);
        networkPanel.SetActive(false);
        dataPanel.SetActive(false);
        virusPanel.SetActive(true);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(false);
    }
     public void OpenPhishing()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(false);
        networkPanel.SetActive(false);
        dataPanel.SetActive(false);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(true);
        spywarePanel.SetActive(false);
    }
         public void OpenSpyware()
    {
        firewallPanel.SetActive(false);
        backupPanel.SetActive(false);
        networkPanel.SetActive(false);
        dataPanel.SetActive(false);
        virusPanel.SetActive(false);
        phishingPanel.SetActive(false);
        spywarePanel.SetActive(true);
    }


}


