using UnityEngine;
using TMPro;
public class HeaderController : MonoBehaviour
{
    [SerializeField] private Transform objExpGauge;
    private struct ExpGauge
    {
        public int cur;
        public int max;
    }
    private ExpGauge expGauge = new ExpGauge();

    [SerializeField] private TextMeshProUGUI txtMinute;
    [SerializeField] private TextMeshProUGUI txtSecond;
    private float timer = 0.0f;
    public float TimeValue { get { return timer; } }

    [SerializeField] private TextMeshProUGUI txtDefeatCount;
    private int defeatCount = 0;
    public int DefeatCount { get { return defeatCount; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.isPause)
        {
            timer += Time.deltaTime;
            txtMinute.text = ((int)timer / 60).ToString("00");
            txtSecond.text = ((int)timer % 60).ToString("00");
        }
    }

    /// <summary>
    /// 経験値ゲージを設定する
    /// </summary>
    /// <param name="cur">現在値</param>
    /// <param name="max">最大値</param>
    public void SetExpGauge(int cur, int max)
    {
        expGauge.cur = cur;
        expGauge.max = max;
        // スケール値を算出（1.0 を超える場合は 1.0 にする）
        float scl = (float)expGauge.cur / expGauge.max;
        objExpGauge.localScale = new Vector3(scl > 1.0f ? 1.0f : scl, 1.0f, 1.0f);
    }

    /// <summary>
    /// 討伐数カウント
    /// </summary>    
    public void AddDefeatCount()
    {
        defeatCount++;
        txtDefeatCount.text = defeatCount.ToString();
    }

}
