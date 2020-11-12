using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    public RootTransform characterPanel;
    public list<Character> characters = new list<Character>();

    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

    void Awake()
    {
        instance = this; 
    }

    public Character GetCharacter(string characterName, bool createCharacterIfDoesNotExist = true, bool enableCreatedCharacterOnStart = true)
    {
        int index = -1;
        if characterDictionary.TryGetValue (characterDictionaryName, out index))
        {
            return characters[index];
        }
        else if createCharacter(DoesNotExist)
        {
            return CreateCharacter(characterName, enableCreatedCharacterOnStart);
        }
        return null;
    }
    public Character CreateCharacter(string characterName, bool enableOnStart = true)
    {
        Character newCharacter = new Character(characterName, enableOnStart);

        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    }
    public class CHARACTERPOSITIONS
    {
        public Vector2 bottomleft = new Vector2(0, 0);
        public Vector2 topRight = new Vector2(1f, 1f);
        public Vector2 centre = new Vector2(0.5f, 0.5f);
        public Vector2 bottomRight = new Vector2(1f, 0);
        public Vector2 topLeft = new Vector2(0, 1f);
    }
    public static CHARACTERPOSITIONS characterPositions = new CHARACTERPOSITIONS();

    public class CHARACTEREXPRESSIONS
    {
        public int normal = 0;
        public int shy = 1;
        public int normalAngle = 2;
        public int cojoinedFingers = 3;
    }
    public static CHARACTEREXPRESSIONS characterExpressions = new CHARACTEREXPRESSIONS();
}

}
