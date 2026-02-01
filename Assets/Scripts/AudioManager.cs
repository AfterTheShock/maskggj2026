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

    [SerializeField] private AudioSource soundFxObject;
    
    public void PlaySfx(AudioClip clip, Transform spawnTransform, float defaultVolume, bool randomPitch, bool randomVolume)
    {
        AudioSource audioSource = Instantiate(soundFxObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        float clipLength = audioSource.clip.length;
        audioSource.volume = randomVolume ? Random.Range(0.95f,1.05f) : defaultVolume;
        audioSource.pitch =  randomPitch ? Random.Range(0.95f,1.05f) : 1f;
        
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength + 0.2f);
    } 
}
