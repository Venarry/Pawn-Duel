using UnityEngine;

public class CharacterFactory
{
    private readonly Character _prefab = Resources.Load<Character>(ResourcesPath.Character);

    public Character Create(Vector3 position, Transform parent, bool isPlayerColor)
    {
        Character character = Object.Instantiate(_prefab, position, Quaternion.identity, parent);

        if(isPlayerColor == true)
        {
            character.SetPlayerColor();
        }
        else
        {
            character.SetEnemyColor();
        }

        character.Init(parent);
        character.SetPosition(position);

        return character;
    }
}