using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  
    public PlayerController controller;

    private void Awake()
    {
        if (CharacterManager.Instance != null)
        {
            CharacterManager.Instance.Player = this;
        }

        controller = GetComponent<PlayerController>();
    }
}
