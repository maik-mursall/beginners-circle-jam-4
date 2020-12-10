using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioIntesityAnimation : MonoBehaviour
{
    public List<SpriteRenderer> sprites;

    [Range(0,1)]
    public float audioIntensity;

    /// <summary>
    /// for fluid animation call every frame
    /// </summary>
    /// <param name="intensity between 0 and 1"></param>
    public void animateAudio (float intensity)
    {
        foreach (SpriteRenderer s in sprites)
        {
            Color c = Color.white;
            c.a = Mathf.Clamp(audioIntensity, 0, 1);
            s.color = c;
        }
    }
}
