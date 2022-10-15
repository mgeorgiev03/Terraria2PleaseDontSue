using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [HideInInspector] public GameObject player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = this.transform.gameObject;
    }

}
