using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOrganizer : MonoBehaviour
{
    [SerializeField][Tooltip("Prefab for when Yamlet first spawns")] GameObject YamletSpawnPrefab;
    [SerializeField] [Tooltip("Prefab for when Yamlet is defeated by acts")] GameObject YamletSparedPrefab;

    [SerializeField] private GameObject CatmanSpawnPrefab;
    [SerializeField] private GameObject SerifSpawnPrefab;
    [SerializeField][Tooltip("Used to place Serif at a specific spot in the overworld after an event")] GameObject SerifAfterMoving;
    [SerializeField] private Vector3 SerifMoveLocation;


    public static WorldOrganizer instance;

    //checks player prefs and moves characters / destroys them whenever called

    //if an enemy is killed in combat, remove them from the overworld
    //if an enemy is spared, move them to another part of the world
    //needs a reference to all enemies

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    public void updateWorld()
    {
        //this has to be called after a certain point. 
        //CharacterStatus is either Alive or Dead
        //Serif is either Stay or Moved 
        //since the game is simple right now, just move characters to the position theyre supposed to be at after an event happens
        //characters will just spawn at natural positions otherwise (as determined by the editor locations)
        if (PlayerPrefs.GetString("YamletStatus") == "Dead")
        {
            Destroy(GameObject.Find("Serif"));
            Instantiate(SerifAfterMoving, SerifMoveLocation, Quaternion.identity);
            
            Destroy(GameObject.Find("Yamlet"));
        }
        else if (PlayerPrefs.GetString("YamletStatus") == "Alive")
        {
            Vector3 yamletPos = GameObject.Find("Yamlet").transform.position;
            Destroy(GameObject.Find("Yamlet"));
            Instantiate(YamletSparedPrefab, yamletPos, Quaternion.identity);
        }

        //if (PlayerPrefs.GetString("SerifStatus") == "Moved")
        //{
        //    Instantiate()
        //}
    }



}
