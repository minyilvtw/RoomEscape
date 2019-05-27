using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{

    public bool TestMode;

    void Awake(){
        if(TestMode){
            PhotonNetwork.offlineMode = true;
            PhotonNetwork.JoinOrCreateRoom("OFFLINE", new RoomOptions() { MaxPlayers = 8 }, null);
        }
    }

    PlayerMovement m_Movement;
    public PlayerMovement PlayerMovement
    {
        get
        {
            if (m_Movement == null)
            {
                m_Movement = GetComponent<PlayerMovement>();
            }

            return m_Movement;
        }

    }

    PlayerStatus m_Status;
    public PlayerStatus PlayerStatus
    {
        get
        {
            if (m_Status == null)
            {
                m_Status = GetComponent<PlayerStatus>();
            }

            return m_Status;
        }

    }
    
    PlayerAnimation m_Animation;
    public PlayerAnimation PlayerAnimation {
        get {
            if (m_Animation == null) {
                m_Animation = GetComponent<PlayerAnimation>();
            }

            return m_Animation;
        }
    }


}
