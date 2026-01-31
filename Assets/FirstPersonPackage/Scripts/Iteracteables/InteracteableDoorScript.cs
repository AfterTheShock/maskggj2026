using UnityEngine;
using UnityEngine.Events;

public class InteracteableDoorScript : MonoBehaviour
{
    [SerializeField] float openSpeed = 8f;
    [SerializeField] float closeSpeed = 5f;

    [SerializeField] GameObject[] toBeOffWhenDoorIsOpen = new GameObject[0];

    [SerializeField] bool cantBeClosed = false;

    [SerializeField] bool isOpen = false;

    [SerializeField] Transform openedTransform;
    private Quaternion startRotation;

    [Header("Sounds")]
    [SerializeField] AudioSource openCloseSource;
    [SerializeField] AudioClip[] openClips = new AudioClip[0];
    [SerializeField] AudioClip[] closeClips = new AudioClip[0];
    [SerializeField] float openCloseDeffautPitch = 1f;
    [SerializeField] float openClosePitchShift = 0.05f;
    [SerializeField] Transform soundPositionParent;
    [SerializeField] bool changeParentToSoundPositionAndZeroLocal = false;

    [SerializeField] UnityEvent onDoorOpen;
    [SerializeField] UnityEvent afterDoorFullyClosed;

    private void Start()
    {
        startRotation = transform.localRotation;
    }

    void Update()
    {
        Quaternion targetRotation = startRotation;
        float rotationSpeed = closeSpeed;

        if (isOpen)
        {
            targetRotation = openedTransform.localRotation;
            rotationSpeed = openSpeed;
        }

        this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void OpenCloseDoor()
    {
        if (isOpen && cantBeClosed) return;

        isOpen = !isOpen;

        TurnOnOffToBeOffWhenDoorIsOpen(!isOpen);

        CancelInvoke("DoorFullyClosed");
        if (isOpen) onDoorOpen.Invoke();
        else Invoke("DoorFullyClosed", 3f);

        //Trigger Sound Here
        PlayOpenCloseSound();
    }

    private void DoorFullyClosed()
    {
        afterDoorFullyClosed.Invoke();
    }

    private void TurnOnOffToBeOffWhenDoorIsOpen(bool turn = false)
    {
        foreach (GameObject go in toBeOffWhenDoorIsOpen)
        {
            go.SetActive(turn);
        }
    }

    private void PlayOpenCloseSound()
    {
        if (openCloseSource == null || openClips.Length <= 0) return;

        AudioSource source = new GameObject("OpenCloseSound").AddComponent<AudioSource>();

        source.outputAudioMixerGroup = openCloseSource.outputAudioMixerGroup;
        source.volume = openCloseSource.volume;
        source.loop = openCloseSource.loop;
        source.spatialBlend = openCloseSource.spatialBlend;

        if (!isOpen && closeClips.Length > 0)
        {
            source.clip = closeClips[Random.Range(0, closeClips.Length)];
            source.pitch = Random.Range(openCloseDeffautPitch - openClosePitchShift, openCloseDeffautPitch + openClosePitchShift);
        }
        else
        {
            source.clip = openClips[Random.Range(0, openClips.Length)];
            source.pitch = Random.Range(openCloseDeffautPitch - openClosePitchShift, openCloseDeffautPitch + openClosePitchShift);
        }

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

        source.Play();
    }
}
