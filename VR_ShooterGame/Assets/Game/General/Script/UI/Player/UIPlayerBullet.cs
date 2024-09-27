using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIPlayerBullet : MonoBehaviour
{
    public TMP_Text magazineAmmoText;
    public TMP_Text stashAmmoText;

    public void RefreshPlayerAmmoText(int magazine, int stash)
    {
        magazineAmmoText.text = magazine.ToString();
        stashAmmoText.text = stash.ToString();
    }
}
