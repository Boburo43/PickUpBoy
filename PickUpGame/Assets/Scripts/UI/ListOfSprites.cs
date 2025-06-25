using TMPro;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ListOfSprites", menuName = "ListOfSprites")]
public class ListOfSprites : ScriptableObject
{
    public List<TMP_SpriteAsset> SpriteAssets;
}
