using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject[] characters;
    private int selectedCharacterIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject character in characters) {
            character.SetActive(false);
        }

        characters[selectedCharacterIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCharacter(int newCharacterIndex)
    {
        characters[selectedCharacterIndex].SetActive(false);
        characters[newCharacterIndex].SetActive(true);
        selectedCharacterIndex = newCharacterIndex;
    }
}
