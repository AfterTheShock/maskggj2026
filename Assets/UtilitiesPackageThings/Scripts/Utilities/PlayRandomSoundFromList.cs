using UnityEngine;
using UnityEngine.Audio;

public class PlayRandomSoundFromList : MonoBehaviour
{
    [SerializeField] Transform soundPositionParent;
    [SerializeField] bool changeParentToSoundPositionAndZeroLocal = false;
    [SerializeField] AudioClip[] randomClips = new AudioClip[0];
    [SerializeField] float deffautPitch = 1f;
    [SerializeField] float pitchShift = 0.05f;

    [SerializeField] bool shoudAddDistortionFilter = false;
    [SerializeField] float distortionForDistortionFilter = 0.5f;

    [SerializeField] AudioSource audioSourceToCopy;

    [Header("Audio Source Settings (If no source present above)")]
    [SerializeField] float volume = 1;
    [SerializeField] AudioMixerGroup audioMixerGroup;
    [Tooltip("0 is no 3d sound, 1 is full 3d sound")]
    [SerializeField] float spatialBlend = 1;

    public void PlayOneShotSound()
    {
        if (randomClips.Length <= 0) return;

        AudioSource source = new GameObject("RandomSoundFromList").AddComponent<AudioSource>();

        if(audioSourceToCopy != null)
        {
            source.outputAudioMixerGroup = audioSourceToCopy.outputAudioMixerGroup;
            source.volume = audioSourceToCopy.volume;
            source.loop = audioSourceToCopy.loop;
            source.spatialBlend = audioSourceToCopy.spatialBlend;
        }
        else
        {
            source.volume = volume;
            source.outputAudioMixerGroup = audioMixerGroup;
            source.spatialBlend = spatialBlend;
        }

        source.clip = randomClips[Random.Range(0, randomClips.Length)];
        source.pitch = Random.Range(deffautPitch - pitchShift, deffautPitch + pitchShift);

        source.gameObject.AddComponent<DestroyAfterXSeconds>().timeToDestroyObject = source.clip.length + 1f;

        if (soundPositionParent != null)
        {
            source.transform.position = soundPositionParent.position;

            if (changeParentToSoundPositionAndZeroLocal)
            {
                source.transform.parent = soundPositionParent.transform;
                source.transform.localPosition = Vector3.zero;
            }
        }

        if (shoudAddDistortionFilter)
        {
            source.gameObject.AddComponent<AudioDistortionFilter>().distortionLevel = distortionForDistortionFilter;
        }

        source.Play();
    }


}
