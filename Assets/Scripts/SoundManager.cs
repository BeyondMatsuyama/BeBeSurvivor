using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = (SoundManager)FindAnyObjectByType(typeof(SoundManager));
                if(instance == null)
                {
                    Debug.LogError("SoundManager Instance Error");
                }
            }
            return instance;
        }
    }

    public enum BGM
    {
        Title = 0,

        Num
    }
    [SerializeField] private AudioClip[] bgm;
    private AudioSource bgmSource;

    public enum SE
    {
        Hit0 = 0,
        Hit1,
        LevelUp,
        Select,
        Melee0,
        Melee1,
        Range,
        Win,
        Lose,
        Dead,

        Num
    }

    [SerializeField] private AudioClip[] se;
    private AudioSource seSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.5f;

        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
        seSource.volume = 0.8f;

        DontDestroyOnLoad(this);
    }

    public void PlayBGM(BGM bgm)
    {
        bgmSource.clip = this.bgm[(int)bgm];
        bgmSource.Play();
    }
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySE(SE se)
    {
        seSource.PlayOneShot(this.se[(int)se]);
    }
    public void StopSE()
    {
        seSource.Stop();
    }


}
