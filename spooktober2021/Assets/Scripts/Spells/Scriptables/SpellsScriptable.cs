using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "ScriptableObject/ Spells Configuration")]
public class SpellsScriptable : ScriptableObject
{
    [System.Serializable]
    public struct stats
    {
        public string name;
        public float damages;
        public float speed;
        public float cost;
    }
    public stats SpellStats;

    #region prints
    /// <summary>
    /// Prints the current spell stats
    /// </summary>
    public void PrintSpell()
    {
        Debug.Log(SpellStats.name + " : \n" +
                  "Damages : " + SpellStats.damages + "                " +
                  "Speed : " + SpellStats.speed + "                " +
                  "Cost : " + SpellStats.cost);
    }
    /// <summary>
    /// Prints the <paramref name="targetStats"/> of a given spell
    /// </summary>
    /// <param name="targetStats"></param>
    public void PrintSpell(CharactersScriptable.stats targetStats)
    {
        Debug.Log(targetStats.name + " : \n" +
                  "Damages : " + targetStats.damages + "                " +
                  "Speed : " + targetStats.speed + "                " +
                  "Cost : " + SpellStats.cost);
    }
    #endregion
}
