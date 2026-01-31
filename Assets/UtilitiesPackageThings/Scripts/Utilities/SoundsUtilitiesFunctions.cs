using UnityEngine;

public static class SoundsUtilitiesFunctions
{
    public static void PlaySoundOneShot(AudioClip clip, Vector3 position = new Vector3(), float volume = 1, bool pitchShiftedRandom = false,
        float spatialBlend = 0)
    {
        //Create Game object and position it
        GameObject sourceObj = new GameObject();
        sourceObj.transform.position = position;

        //Add audio source
        AudioSource source = sourceObj.AddComponent<AudioSource>();

        //Set all the variables to the onces passed
        source.clip = clip;
        source.volume = volume;
        if (pitchShiftedRandom) source.pitch = Random.Range(0.9f, 1.1f);
        source.spatialBlend = spatialBlend;

        //Play the audioSource
        source.Play();

        //Add a component to destroy the source once it finished playing
        DestroyAfterXSeconds destroyX = sourceObj.AddComponent<DestroyAfterXSeconds>();
        destroyX.timeToDestroyObject = clip.length + 0.5f;
    }
}

