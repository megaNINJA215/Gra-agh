using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "New Audio Datapack", menuName = "Audio/Audio Datapack")]
public class AudioData_SO : ScriptableObject 
{
    [System.Serializable]
    public class AdvancedSettings {
        public bool use_3d = true;
        public float min_distance = 7.05f;
        public float max_distance = 20f;
        public bool important_sound = false;
        public bool loop = false;
        public bool bypass_filters = false;
        public bool prevent_repeating = false;
        public AudioRolloffMode rolloff_type = AudioRolloffMode.Logarithmic;
    }

    [Header("Audio Settings")]

    [Range(0.0f, 1f)] public float vol_multiplier = 1; //volume mutiplier
    [Range(0.0f, 0.3f)] public float pitch_multiplier = 0; //random pitch multiplier
    public bool force_2d = false;
    public AdvancedSettings settings;

    [Header("Audio Instances")]

    public AudioClip[] sound_effects;
    [HideInInspector] public AudioClip previous_clip;
    public AudioMixerGroup used_mixer;
}