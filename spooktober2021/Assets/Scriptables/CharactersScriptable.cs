using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "ScriptableObject/ Character Configuration")]
public class CharactersScriptable : ScriptableObject
{
    [System.Serializable]
    public struct stats
    {
        public string name;
        public float maxHP;
        public float currentHP;
        public float speed;
        public float damages;
        public float invincibleTime;
    }
    public stats CharacterStats;

    #region prints
    /// <summary>
    /// Prints the current character stats
    /// </summary>
    public void PrintCharacter()
    {
        Debug.Log(CharacterStats.name + " : \n" +
                  "HP : " + CharacterStats.currentHP + " / " + CharacterStats.maxHP + "                " +
                  "Speed : " + CharacterStats.speed + "                " +
                  "Damages : " + CharacterStats.damages + "                " +
                  "Invincibility time : " + CharacterStats.invincibleTime);
    }
    /// <summary>
    /// Prints the <paramref name="targetStats"/> of a given character
    /// </summary>
    /// <param name="targetStats"></param>
    public void PrintCharacter(CharactersScriptable.stats targetStats)
    {
        Debug.Log(targetStats.name + " : \n" +
                  "HP : " + targetStats.currentHP + " / " + targetStats.maxHP + "                " +
                  "Speed : " + targetStats.speed + "                " +
                  "Damages : " + targetStats.damages + "                " +
                  "Invincibility time : " + targetStats.invincibleTime);
    }
    /// <summary>
    /// Prints the current character's HPs
    /// </summary>
    public void PrintCharacterHP()
    {
        Debug.Log(CharacterStats.name + " : " + CharacterStats.currentHP + " / " + CharacterStats.maxHP);
    }
    /// <summary>
    /// Prints the given character's HPs
    /// </summary>
    /// <param name="targetStats"></param>
    public void PrintCharacterHP(CharactersScriptable.stats targetStats)
    {
        Debug.Log(targetStats.name + " : " + targetStats.currentHP + " / " + targetStats.maxHP);
    }
    #endregion
}
