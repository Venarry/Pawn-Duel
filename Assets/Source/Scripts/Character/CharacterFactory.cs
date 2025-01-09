using UnityEngine;

public class CharacterFactory
{
    private readonly Character _prefab = Resources.Load<Character>(ResourcesPath.Character);

    public Character Create(Vector3 position, Vector2Int gridPosition, Transform parent, bool isPlayer)
    {
        Character character = Object.Instantiate(_prefab, position, Quaternion.identity, parent);

        character.Init(parent, isPlayer);
        character.SetPosition(position, gridPosition);

        return character;
    }
}