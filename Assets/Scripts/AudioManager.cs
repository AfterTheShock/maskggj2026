using UnityEngine;

public class AudioManager : MonoBehaviour
{ 
    #region SingletonPattern
        private static AudioManager _instance;
    
        public static AudioManager Instance { get { return _instance; } }
    
    
        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    #endregion

    [SerializeField] private GameObject soundFxObject;
    
    public void PlaySfx(AudioClip[] clips, Transform spawnTransform, float defaultVolume, bool randomPitch, bool randomVolume, AudioReverbPreset audioReverbPreset, float additionalLifeTime = 0f)
    {
        if (clips.Length == 0) return;
        
        GameObject audioObj = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);
        AudioSource audioSource = audioObj.GetComponent<AudioSource>();
        AudioReverbFilter audioReverbFilter = audioObj.GetComponent<AudioReverbFilter>();
        
        int index = Random.Range(0, clips.Length);
        
        audioSource.clip = clips[index];
        float clipLength = audioSource.clip.length;
        audioSource.volume = randomVolume ? Random.Range(0.95f,1.05f) : defaultVolume;
        audioSource.pitch =  randomPitch ? Random.Range(0.95f,1.05f) : 1f;
        audioReverbFilter.reverbPreset = audioReverbPreset;
        
        float additional = additionalLifeTime > 0f ? additionalLifeTime : 0f;
       
        Destroy(audioSource.gameObject, clipLength +  0.2f+ additional); audioSource.Play();
        
        audioSource.Play();
    } 
}
