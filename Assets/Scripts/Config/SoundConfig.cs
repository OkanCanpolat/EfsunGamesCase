using UnityEngine;

[CreateAssetMenu (fileName = "SoundConfig", menuName = "Sound Configs/ General")]
public class SoundConfig : ScriptableObject
{
    public AudioClip ButtonClickSound;
    [Range(0, 1)]
    public float ButtonClickVolume = 1;

    public AudioClip CollectResourceSound;
    [Range(0, 1)]
    public float CollectResourceSoundVolume = 1;
}
