using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum EffectSoundClip
{
    Fever,
    GameStart,
    Item,
    Jump,
    SizeChange,
    Hit,
    Attack,

    NONE,
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject soundPrefab;
    [SerializeField] private AudioSource bgmSource;

    [SerializeField] private AudioClip defaultClip;
    [SerializeField] private AudioClip fevelClip;

    private Dictionary<EffectSoundClip, AudioClip> soundMap = new Dictionary<EffectSoundClip, AudioClip>();

    private void Awake()
    {
        for (int i = 0; i < (int)EffectSoundClip.NONE; i++)
        {
            EffectSoundClip temp = (EffectSoundClip)i;
            var loadRes = Resources.Load("Sound/Effect/" + temp.ToString() + "_Effect");
            if (loadRes is null) continue;
            soundMap.Add(temp, loadRes as AudioClip);
        }


    }

    public void FeverPlay()
    {
        Managers.Sound.Play(EffectSoundClip.Fever);

        bgmSource.clip = fevelClip;
        bgmSource.Play();
    }

    public void DefaultPlay()
    {
        bgmSource.clip = defaultClip;
        bgmSource.Play();
    }

    public void Play(EffectSoundClip _clip)
    {
        if (!soundMap.TryGetValue(_clip, out AudioClip clip)) return;

        var source = Instantiate(soundPrefab, transform).GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
}
