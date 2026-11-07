using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources Settings")]

    [SerializeField] private Source[] main_sources;
    private AudioClip[] current_clip_on_main_sources;

    [SerializeField] private AudioSource[] position_sources;
    private AudioClip[] current_clip_on_position_sources;
    Transform[] position_source_transforms;

    [Header("Instace")]

    [SerializeField] private AudioSource created_position_source; // NIE UŻYWAĆ DOPÓKI NIE DOSTARCZĘ POTRZEBNEGO OBIEKTU

    private int last_used_main_source;
    private int last_used_position_source;
    
    const float ZOffset = 0;

    [SerializeField] private AudioSource ui_source_instance;
    public static AudioSource UI_source { get; private set; }

    [System.Serializable]
    public class Source {
        public AudioSource _src;
        public bool playing_important_sfx;
    }

    #region Initializing Data

    private void Awake() {
        current_clip_on_main_sources = new AudioClip[main_sources.Length];
        current_clip_on_position_sources = new AudioClip[position_sources.Length];
        position_source_transforms = new Transform[position_sources.Length];

        if (Instance != this)
            Instance = this;

        for (int i = 0; i < position_source_transforms.Length; i++)
            position_source_transforms[i] = position_sources[i].GetComponent<Transform>();

        UI_source = ui_source_instance;
    }

    private void Start() {
        last_used_main_source = 0;
        last_used_position_source = 0;

        transform.SetParent(Camera.main.transform);
    }

    #endregion Initializing Data

    public void Play_Sound_By_Clip(AudioClip clip, AudioSource _source) {
        _source = SetSourceToAvaliableMain(_source);

        if (current_clip_on_main_sources[last_used_main_source] == clip)
            _source.Stop();

        _source.PlayOneShot(clip);
    }

    #region Main Sources

    /// <summary>
    /// Playes sound from given Audio Datapack Scriptable Object. 
    /// Requires inserting an index of the sound we want to be played. 0 is default
    /// </summary>
    public void Play_Sound_By_AudioData_With_Index(AudioData_SO data, int sound_effect_index, AudioSource _source) {
        if (data == null) {
            if (Application.isEditor)
                Debug.LogError("No Audio Datapack Included");
            return;
        }

        _source = SetSourceToAvaliableMain(_source);
        SetCurrentClipOnMainSources(_source, data.sound_effects[sound_effect_index]);

        _source.volume = data.vol_multiplier;
        _source.pitch = 1 + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        _source.outputAudioMixerGroup = data.used_mixer;
        _source.bypassEffects = data.settings.bypass_filters;

        _source.PlayOneShot(data.sound_effects[sound_effect_index]);
    }

    /// <summary>
    /// Playes sound from given Audio Datapack Scriptable Object. 
    /// Plays sound taken randomly from Datapack's array
    /// </summary>
    public AudioSource Play_Sound_By_AudioData(AudioData_SO data, AudioSource _source = null) {
        if (data == null) {
            if (Application.isEditor)
                Debug.LogError("No Audio Datapack Included");
            return null;
        }

        int index = Random.Range(0, data.sound_effects.Length);

        if(data.settings.prevent_repeating && data.sound_effects.Length > 1) {
            while (data.sound_effects[index] == data.previous_clip) {
                index = Random.Range(0, data.sound_effects.Length);
            }

            //Debug.Log(index + "..." + data.name);
        }

        _source = SetSourceToAvaliableMain(_source);
        SetCurrentClipOnMainSources(_source, data.sound_effects[index]);
        data.previous_clip = data.sound_effects[index];

        if (data.settings.important_sound)
            main_sources[last_used_main_source].playing_important_sfx = true;

        _source.volume = data.vol_multiplier;
        _source.pitch = 1f + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        _source.outputAudioMixerGroup = data.used_mixer;
        _source.bypassListenerEffects = data.settings.bypass_filters;
        _source.bypassEffects = data.settings.bypass_filters;
        _source.rolloffMode = data.settings.rolloff_type;
        _source.dopplerLevel = 0f;

        if (data.settings.use_3d && !data.force_2d && _source != null) {
            _source.minDistance = data.settings.min_distance;
            _source.maxDistance = data.settings.max_distance;
            _source.spatialBlend = 1f;
            _source.spread = 180f;
        }
        else {
            _source.spatialBlend = 0f;
            _source.spread = 0f;
        }

        if (data.settings.loop) {
            _source.clip = data.sound_effects[index];
            _source.Play();
        }
        else
            _source.PlayOneShot(data.sound_effects[index]);

        return _source;
    }

    public void SetupSource(AudioData_SO data, AudioSource _source, bool with_position = false) {
        if (with_position) {
            SetAudioSourceToProperPosition(_source.transform);
        }

        if (data == null)
            return;

        _source.clip = data.sound_effects[Random.Range(0, data.sound_effects.Length)];
        _source.volume = data.vol_multiplier;
        _source.pitch = 1 + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        _source.outputAudioMixerGroup = data.used_mixer;
        _source.bypassListenerEffects = data.settings.bypass_filters;
        _source.bypassEffects = data.settings.bypass_filters;
        _source.rolloffMode = data.settings.rolloff_type;
        _source.dopplerLevel = 0f;

        if (data.settings.loop)
            _source.loop = true;

        if (data.settings.use_3d) {
            _source.minDistance = data.settings.min_distance;
            _source.maxDistance = data.settings.max_distance;
            _source.spatialBlend = 1f;
            _source.spread = 180f;
        }

        if (data.force_2d)
            _source.spatialBlend = 0f;
    }

    public void SetupSource(AudioData_SO data, AudioSource _source, Transform parent) {
        _source.transform.SetParent(parent);
        SetAudioSourceToProperPosition(_source.transform);

        if (data == null)
            return;

        _source.clip = data.sound_effects[Random.Range(0, data.sound_effects.Length)];
        _source.volume = data.vol_multiplier;
        _source.pitch = 1 + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        _source.outputAudioMixerGroup = data.used_mixer;
        _source.bypassListenerEffects = data.settings.bypass_filters;
        _source.bypassEffects = data.settings.bypass_filters;
        _source.rolloffMode = data.settings.rolloff_type;
        _source.dopplerLevel = 0f;

        if (data.settings.loop)
            _source.loop = true;

        if (data.settings.use_3d) {
            _source.minDistance = data.settings.min_distance;
            _source.maxDistance = data.settings.max_distance;
            _source.spatialBlend = 1f;
            _source.spread = 180f;
        }

        if(data.force_2d)
            _source.spatialBlend = 0f;
    }

    private AudioSource SetSourceToAvaliableMain(AudioSource _source) {
        if (_source == null) {
            do {
                if (last_used_main_source == main_sources.Length - 1)
                    last_used_main_source = 0;
                else
                    last_used_main_source += 1;
            }
            while (main_sources[last_used_main_source].playing_important_sfx && main_sources[last_used_main_source]._src.isPlaying);

            main_sources[last_used_main_source].playing_important_sfx = false;
            return main_sources[last_used_main_source]._src;
        }

        return _source;
    }
    private void SetCurrentClipOnMainSources(AudioSource _source, AudioClip _clip) {
        if (current_clip_on_main_sources[last_used_main_source] == _clip)
            _source.Stop();

        current_clip_on_main_sources[last_used_main_source] = _clip;
    }

    #endregion Main Sources

    #region Position Sources

    /// <summary>
    /// Why even using "At Position"? Because that function generates 3D audio, meaning that further away player is from the source, smaller volume is.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="position"></param>
    public AudioSource Play_Sound_By_AudioData_At_Position(AudioData_SO data, Vector3 position) {
        if (data == null) {
            if(Application.isEditor) 
                Debug.LogError("No Audio Datapack Included");

            return null;
        }

        if (data.force_2d) {
            Play_Sound_By_AudioData(data);
            return null;
        }

        //if (Vector3.Distance(position, PlayerMovement.Instance._transform.position) > data.settings.max_distance)
        //    return null;

        int index = Random.Range(0, data.sound_effects.Length);

        if (data.settings.prevent_repeating && data.sound_effects.Length > 1) {
            while (data.sound_effects[index] == data.previous_clip) {
                index = Random.Range(0, data.sound_effects.Length);
            }

            //Debug.Log(index + "..." + data.name);
        }

        SetCurrentClipOnPositionSources(SetSourceToAvaliablePositionSource(), data.sound_effects[index]);
        data.previous_clip = data.sound_effects[index];

        Transform position_source_transform = position_source_transforms[last_used_position_source];
        AudioSource position_source = position_sources[last_used_position_source];

        position_source.volume = data.vol_multiplier;
        position_source.pitch = 1 + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        position_source.outputAudioMixerGroup = data.used_mixer;
        position_source.bypassEffects = data.settings.bypass_filters;
        position_source.rolloffMode = data.settings.rolloff_type;
        position_source.dopplerLevel = 0f;

        if (data.settings.use_3d) {
            position_source.minDistance = data.settings.min_distance;
            position_source.maxDistance = data.settings.max_distance;
            position_source.spatialBlend = 1f;
            position_source.spread = 180f;
        }

        position.z = ZOffset + 0.25f;
        position_source_transform.position = position;

        position_source.PlayOneShot(data.sound_effects[index]);
        return position_source;
    }

    public void SetAudioSourceToProperPosition(Transform _source) {
        Vector3 pos = _source.position;
        pos.z = ZOffset + 0.25f;
        _source.position = pos;
    }

    /// <summary>
    /// New Experimental Feature to AudioManager. Plays a loop on specified gameobject
    /// </summary>
    /// <param name="data">Audio Data SO</param>
    /// <param name="parent_object">Object that will be a parent of sound effect</param>
    /// <returns></returns>
    public AudioSource Setup_Loop_Position_Source(AudioData_SO data, Transform parent_object, bool disable_at_the_start = false) {
        if (data == null) {
            if (Application.isEditor)
                Debug.LogError("No Audio Datapack Included");
            return null;
        }

        int index = Random.Range(0, data.sound_effects.Length);

        AudioSource created_source = Instantiate(created_position_source, parent_object.position, Quaternion.identity, parent_object);
        Transform position_source_transform = created_source.transform;
        position_source_transform.position = position_source_transform.position.WithAxis(Axis.Z, ZOffset + 0.25f);

        created_source.clip = data.sound_effects[index];
        created_source.volume = data.vol_multiplier;
        created_source.outputAudioMixerGroup = data.used_mixer;
        created_source.bypassEffects = data.settings.bypass_filters;
        created_source.pitch = 1 + Random.Range(-data.pitch_multiplier, data.pitch_multiplier);
        created_source.rolloffMode = data.settings.rolloff_type;
        created_source.dopplerLevel = 0f;

        if (data.settings.use_3d) {
            created_source.minDistance = data.settings.min_distance;
            created_source.maxDistance = data.settings.max_distance;
            created_source.spatialBlend = 1f;
            created_source.spread = 180f;
        }
        else if (data.force_2d) {
            created_source.spatialBlend = 0f;
        }

        created_source.loop = data.settings.loop;

        if (!disable_at_the_start) {
            created_source.playOnAwake = true;
            created_source.Play();
        }
        else
            created_source.Stop();

        return created_source;
    }

    /// <summary>
    /// Sets up a position source without need for audio_data
    /// </summary>
    /// <param name="parent_object">Object that will be a parent of sound effect</param>
    /// <returns></returns>
    public AudioSource Setup_Loop_Position_Source(Transform parent_object) {
        AudioSource created_source = Instantiate(created_position_source, parent_object.position, Quaternion.identity, parent_object);
        Transform position_source_transform = created_source.transform;
        position_source_transform.position = position_source_transform.position.WithAxis(Axis.Z, ZOffset + 0.25f);

        return created_source;
    }

    private AudioSource SetSourceToAvaliablePositionSource() {
        if (last_used_position_source == position_sources.Length - 1)
            last_used_position_source = 0;
        else
            last_used_position_source += 1;

        return position_sources[last_used_position_source];
    }
    private void SetCurrentClipOnPositionSources(AudioSource _source, AudioClip _clip) {
        if (current_clip_on_position_sources[last_used_position_source] == _clip)
            _source.Stop();

        current_clip_on_position_sources[last_used_position_source] = _clip;
    }

    #endregion Position Sources

    #region Other Helping Functions

    public void LerpSourceVolumeOff(AudioSource _src, float time) => StartCoroutine(LerpingSourceVolumeOff(_src, time));

    private IEnumerator LerpingSourceVolumeOff(AudioSource _src, float time) {
        float time_elapsed = 0f;
        float start_vlm = _src.volume;

        while(_src.volume > 0f) {
            _src.volume = Mathf.Lerp(start_vlm, -0.05f, time_elapsed / time);
            time_elapsed += Time.deltaTime;
            yield return null;
        }

        _src.Stop();
    }

    #endregion Other Helping Functions
}

public enum Axis {
    X, Y, Z
}

public static class VectorsExtension {
    public static Vector3 WithAxis(this Vector3 vector, Axis axis, float value) {
        return new Vector3(
            axis == Axis.X ? value : vector.x,
            axis == Axis.Y ? value : vector.y,
            axis == Axis.Z ? value : vector.z
            );
    }

    public static Quaternion LookAt2D(Vector2 forward) => Quaternion.Euler(0f, 0f, Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg);

    public static float EasedTracking(float current, float target, AnimationCurve _curve, float catchup_speed, float catchup_range = 1f) {
        var deltaRaw = target - current;
        var delta = Mathf.Abs(deltaRaw);

        var catchupRaw = _curve.Evaluate(delta / catchup_range);
        var catchup = catchupRaw * catchup_speed * Time.deltaTime;

        return Mathf.MoveTowards(current, target, catchup);
    }

    //public static Vector2 EasedTracking(Vector2 current, Vector2 desired, AnimationCurve easingCurve, float catchup_speed, float catchupRange = 1f) {
    //    var deltaRaw = desired - current;
    //    var delta = new Vector2(Mathf.Abs(deltaRaw.x), Mathf.Abs(deltaRaw.y));

    //    float catchup_x = easingCurve.Evaluate(delta.x / catchupRange);
    //    float catchup_y = easingCurve.Evaluate(delta.y / catchupRange);
    //    var catchup = new Vector2(catchup_x, catchup_y) * catchup_speed * Time.deltaTime;

    //    return Vector2.MoveTowards(current, desired, catchup);
    //}
}