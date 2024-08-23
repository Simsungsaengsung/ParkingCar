using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Sound // 사운드 종류, 사운드 추가하고 싶으면 여기서 enum을 추가해야됨
{
    InGameBgm,
    IntroBgm,
    ButtonClickSfx,
    ClearSfx,
    GameOverBgm,
    StageSelectBgm,
}

public enum AudioType // 점프나 총 소리 같은 SFX인지 아님 BGM인지
{
    SFX, BGM
}

[Serializable]
public struct Audio // 사운드 출력 시 필요한 정보들
{
    public Sound sound;
    public AudioType audioType;
    public AudioClip clip;
    [Range(0, 1)] public float volume; // 기본 volume
    [Range(-3, 3)] public float pitch; // 기본 pitch
    [Range(0, 1)] public float pitchRandomness; // PlayWithVariablePitch로 실행할 떄 pitch 얼마나 랜덤하게 실행할지
}

public class SoundManager : MonoSingleton<SoundManager>
{
    [HideInInspector] public string enumName;
    
    private Dictionary<AudioType, AudioSource> _audioSource = new();
    
    [SerializeField] private Audio[] _audios;
    private Dictionary<Sound, Audio> _audioClips = new();

    public override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        // AudioSource 가져오고 AUdioType별로 loop 설정
        var sources = GetComponents<AudioSource>();
        _audioSource.Add(AudioType.BGM, sources[0]);
        _audioSource.Add(AudioType.SFX, sources[1]);
        _audioSource[AudioType.BGM].loop = true;
        _audioSource[AudioType.SFX].loop = false;
        
        foreach (var audio in _audios)
        {
            // Dictionary에 같은 key값으로 2개 이상의 값 들어가는 거 방지
            if (_audioClips.ContainsKey(audio.sound))
            {
                Debug.LogError($"Sound {audio.sound} has multiple AudioClips!");
                continue;
            }

            // audio의 사운드 종류 별로 Dictionary에 추가
            _audioClips.Add(audio.sound, audio);
        }
        
        // 브금은 시작할 때 실행해줌
        PlayWithBasePitch(Sound.InGameBgm);
    }
    
    // 총 소리 같은 경우 pitch가 왔다갔다 해야 듣기에? 좋기 떄문에 PlayWithVariablePitch로
    // 그게 아니라 사운드의 음과 속도가 정확해야하는 경우 PlayWithBasePitch
    public void PlayWithVariablePitch(Sound sound)
    {
        var audio = _audioClips[sound];
        float randomPitch = Random.Range(-audio.pitchRandomness, audio.pitchRandomness);
        _audioSource[audio.audioType].volume = audio.volume;
        _audioSource[audio.audioType].pitch = audio.pitch + randomPitch;
        PlayClip(audio.clip, audio.audioType);
    }
    
    public void PlayWithBasePitch(Sound sound)
    {
        var audio = _audioClips[sound];
        _audioSource[audio.audioType].volume = audio.volume;
        _audioSource[audio.audioType].pitch = audio.pitch;
        PlayClip(audio.clip, audio.audioType);
    }
    
    private void PlayClip(AudioClip clip, AudioType audioType)
    {
        if (audioType == AudioType.BGM)
        {
            _audioSource[audioType].clip = clip;
            _audioSource[audioType].Play();
        }
        else
        {
            _audioSource[audioType].PlayOneShot(clip);
        }
    }
}