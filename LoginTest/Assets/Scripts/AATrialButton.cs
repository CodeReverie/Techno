using System.Collections;
using UnityEngine.SceneManagement;
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
    public GameObject firewallPanel, backupPanel, networkPanel, dataPanel, virusPanel, phishingPanel, spywarePanel, CyberTWin, NetworkTWin, PhishingWin;
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
         List<int> availableButtonIndices1 = new List<int>();


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
            availableButtonIndices1.Add(i);
            votecounters[i] = 1;
            voteTexts[i].text = votecounters[i].ToString();
            voteButtons[i] = playerButtons[i];
        }

        //Number of Players with buttons of course 
        /*for (int i = 1; i <= 9; i++)
        {
            if (availableButtonIndices.Count > 0){
            int randomButtonIndex = Random.Range(0, availableButtonIndices.Count);
            int playerID = i; // Adjust this if your player IDs don't start from 1
            int buttonIndex = availableButtonIndices[randomButtonIndex];

            playerButtonMap.Add(playerID, buttonIndex);
            availableButtonIndices.RemoveAt(randomButtonIndex);
            }
            else
            {
                // Handle the case where there are not enough available buttons
                Debug.LogError("Not enough available buttons for player assignment.");
                break; // Exit the loop or handle the situation accordingly
            }
        }*/
        if (availableButtonIndices.Count < 9) {
        Debug.LogError("Not enough available buttons for player assignment.");
        }   else {
            for (int i = 0; i < 9; i++) {
                int playerID = i + 1; // Adjust this if your player IDs don't start from 1
                int buttonIndex = availableButtonIndices[i];

                playerButtonMap.Add(playerID, buttonIndex);
            }
                // Clear the available buttons list since all buttons are assigned
                availableButtonIndices.Clear();
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
              roleDescription = "You can vote twice";

              OpenData();
                break;

            case "4" :
              roleEquivalent = "Database";
              roleDescription = "You can vote twice";
              OpenData();
                break;

            case "5" :
                roleEquivalent = "Database";
                roleDescription = "You can vote twice";
                OpenData();
                break;

            case "6" :
                roleEquivalent = "Phishing";
                roleDescription = "You win if you get voted out";
                OpenPhishing();
                break;

            case "7" :
                roleEquivalent = "Virus";
                roleDescription = "Kill every other player to win";
                OpenVirus();
                break;

            case "8" :
                roleEquivalent =  "Spyware";
                roleDescription = "Your goal is to help virus and win";
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
                    counters[playerButtonMap[playerToEliminate]] = 0;
                    if (playerToEliminate==7){
                        Debug.Log($"You have been scammed by Phishing, he wins");
                        PhishingWin.SetActive(true);
                        StartCoroutine(Quit());
                    }
                    Debug.Log($"Player {playerToEliminate} is dead!");
                   
                    // Notify all players about the life reduction
                    view.RPC("UpdateCount", RpcTarget.All, playerButtonMap[playerToEliminate], 0);
                    WinCondition();
                    
                }
                firstNightPhase = false; 
            }
 
            }
        }
      

        nightPhase = !nightPhase; // Switch the phase
        
    }
//timer end

public void WinCondition()
{
    int NetworkLeft = 6;
    int ThreatLeft = 2;
    bool PhishingDead = false;

    for (int i = 6; i >= 1; i--)
    {
        int playerID = GetPlayerIdForButton(i);
        
        if (playerID != -1 && playerButtonMap.ContainsKey(playerID) && counters[playerButtonMap[playerID]] == 0)
        {
            NetworkLeft--;
        }
    }

    for (int i = 9; i >= 8; i--)
    {
        int playerID = GetPlayerIdForButton(i);
        
        if (playerID != -1 && playerButtonMap.ContainsKey(playerID) && counters[playerButtonMap[playerID]] == 0)
        {
            ThreatLeft--;
        }
    }

    // Check for Player 7 (Phishing)
    int phishingPlayerID = GetPlayerIdForButton(7);
    if (phishingPlayerID != -1 && playerButtonMap.ContainsKey(phishingPlayerID) && counters[playerButtonMap[phishingPlayerID]] == 0)
    {
        Debug.Log($"You have been scammed by Phishing, he wins");
        PhishingDead = true;

        return;  // Phishing wins, no need to check other conditions
    }

    // Check win conditions for Cybersecurity Threats and Network Tools
    if (NetworkLeft <= ThreatLeft /*&& PhishingDead*/)
    {
        Debug.Log($"Cybersecurity Threats win");
        CyberTWin.SetActive(true);
        StartCoroutine(Quit());

    }

    if (ThreatLeft == 0 /*&& PhishingDead*/)
    {
        Debug.Log($"Networking Tools win");
        NetworkTWin.SetActive(true);
        StartCoroutine(Quit());
        
    }
}

    private bool IsActionButtonEnabled(int buttonIndex)
    {
        int playerID = GetPlayerIdForButton(buttonIndex);
        if (counters[playerButtonMap[playerID]]==0){
            return false;
        }
          
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

            default:
                return false;     
        }
    }

    public void OnButtonPress(int buttonIndex)
    {
        int playerID = GetPlayerIdForButton(buttonIndex);
        if (counters[playerButtonMap[playerID]]==0){
            return;
        }
        
        if (PhotonNetwork.IsConnected)
        {

            if (view.IsMine && IsActionButtonEnabled(buttonIndex) && currentPlayerDevice != (PlayerDevice)buttonIndex && timeIsRunning && nightPhase)
            {
                
                
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
                            PlayerDevice targetPlayerDevice = AcquireDevice(buttonIndex);
                            ShowPlayerIDPanel(targetPlayerDevice);
                            optMonitor--;
                        }
                        break;


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
                            PlayerDevice targetPlayerDevice = AcquireDevice(buttonIndex);
                            ShowPlayerIDPanel(targetPlayerDevice);
                            optSpy--;}
                        break;

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
                   
                }
                view.RPC("UpdateVoteCount", RpcTarget.All, buttonIndex, votecounters[buttonIndex]);
            }
        }     
                Debug.Log($"currentPlayerDevice: {currentPlayerDevice}");
                Debug.Log($"Clicked button assigned to: {(PlayerDevice)buttonIndex}");
    }

  public void UpdateVoteCount(int buttonIndex, int newVoteCounter)
    {
        votecounters[buttonIndex] = newVoteCounter;
        voteTexts[buttonIndex].text = newVoteCounter.ToString();
    }

    //update to all players
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




    //method to show player ID
    private void ShowPlayerIDPanel(PlayerDevice targetPlayerDevice)
{
    seerPanel.SetActive(true);

    string roleEquivalent = GetRoleEquivalent(targetPlayerDevice);

    seerText.text = "Player Role: " + roleEquivalent;
    StartCoroutine(HidePlayerIDPanel(seerPanel));
}




    private IEnumerator HidePlayerIDPanel(GameObject seerPanel)
    {
    yield return new WaitForSeconds(5f); // Adjust the delay as needed
    seerPanel.SetActive(false);
    }

   private string GetRoleEquivalent(PlayerDevice playerDevice)
{
    switch (playerDevice)
    {
        case PlayerDevice.Phone1:
            return "Firewall";
        case PlayerDevice.Phone2:
            return "Backup File";
        case PlayerDevice.Phone3:
            return "Network Monitor";
        case PlayerDevice.Phone4:
            return "Database";
        case PlayerDevice.Phone5:
            return "Database";
        case PlayerDevice.Phone6:
            return "Database";
        case PlayerDevice.Phone7:
            return "Phishing";
        case PlayerDevice.Phone8:
            return "Virus";
        case PlayerDevice.Phone9:
            return "Spyware";
        // ... (add cases for other player devices if needed)
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



    private PlayerDevice AcquireDevice(int buttonIndex)
    {
    foreach (var kvp in playerButtonMap)
    {
        if (kvp.Value == buttonIndex)
        {
            return (PlayerDevice)kvp.Key;
        }
    }

    return PlayerDevice.Phone1; // Return a default value if the button index is not found 
    }
    
    

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

     
        

    public IEnumerator Quit()
    {
        yield return new WaitForSeconds(5f); 
        // Check if connected to Photon before loading the scene
        if (PhotonNetwork.IsConnected)
        {
            // Disconnect from Photon
            PhotonNetwork.Disconnect();
        }
        // Load the scene
        SceneManager.LoadScene("FirebaseAuth");
    }

}