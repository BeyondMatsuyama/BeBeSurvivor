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
        Game = 0,

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
        Coin1,
        Coin2,

        Num
    }

    [SerializeField] private AudioClip[] se;
    private AudioSource seSource;

    // ヒット音とコイン音が大量にならないためのリミッター
    private float seBaseTime = 0f;
    private int seCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = 0.2f;

        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
        seSource.volume = 0.7f;

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
        if(canSePlayed(se))
        {
            seSource.PlayOneShot(this.se[(int)se]);
        }
    }
    public void StopSE()
    {
        seSource.Stop();
    }

    /// <summary>
    /// SEが再生可能か（単位時間内に再生できる上限を設ける）
    /// </summary>
    /// <param name="se">SE 番号</param>
    /// <returns>true で再生可能</returns>
    private bool canSePlayed(SE se)
    {
        bool canPlayed = true;
        if(se == SE.Hit0 || se == SE.Hit1 || se == SE.Coin1 || se == SE.Coin2)
        {
            if(seCount == 0)
            {
                seBaseTime = Time.time;
            }

            if(Time.time - seBaseTime < 0.2f)
            {
                seCount++;
                if(seCount > 2)
                {
                    canPlayed = false;
                }
            }
            else
            {
                seCount = 0;
            }
        }
        return canPlayed;
    }

}
