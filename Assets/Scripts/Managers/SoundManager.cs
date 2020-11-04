using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum SoundClip
{
    BACKGROUND,
    JELLY,
    OBJECT,
    LEVELDOWN,
    LEVELUP,

    NONE,
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundPrefab;
    private Dictionary<SoundClip, AudioClip> soundMap = new Dictionary<SoundClip, AudioClip>();

    private void Awake()
    {
        for (int i = 0; i < (int)SoundClip.NONE; i++)
        {
            SoundClip temp = (SoundClip)i;
            var loadRes = Resources.Load("Sound/Clips/" + temp.ToString());
            if (loadRes is null) continue;
            soundMap.Add(temp, loadRes as AudioClip);
        }
    }

    public void Play(SoundClip _clip)
    {
        if (!soundMap.TryGetValue(_clip, out AudioClip clip)) return;

        var source = Instantiate(soundPrefab, transform).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
}
