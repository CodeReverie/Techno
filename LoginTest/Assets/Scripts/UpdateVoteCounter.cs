using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UpdateVoteCounter : MonoBehaviourPunCallbacks
{
    [PunRPC]
    public void UpdateVoteCount(int buttonIndex, int newVoteCounter)
    {
        AATrialButton instance = FindObjectOfType<AATrialButton>();
        if (instance != null)
        {
            instance.UpdateVoteCount(buttonIndex, newVoteCounter);
        }
    }
}