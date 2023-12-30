using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ToLobby : MonoBehaviour
{
    public void LoadScene()
    {
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
