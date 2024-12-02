using UnityEngine;
using TMPro;

public class ResultController : MonoBehaviour
{
    [SerializeField] private GameObject clear;
    [SerializeField] private GameObject gameOver;

    [SerializeField] private TextMeshProUGUI txtPlayTime;
    [SerializeField] private TextMeshProUGUI txtDefeatCount;

    private bool isActive = false;
    private bool toTitle = false;
    public bool ToTitle { get { return toTitle; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 自オブジェクトを非表示
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            // 画面タッチされたら、リザルト画面を非表示
            if(Input.GetMouseButtonDown(0))
            {
                this.gameObject.SetActive(false);
                isActive = false;
                toTitle = true;
            }
        }
    }

    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="isClear">クリアフラグ</param>
    /// <param name="playTime">プレイ時間</param>
    /// <param name="defeatCount">討伐数</param>
    public void Show(bool isClear, float playTime, int defeatCount)
    {
        // 自オブジェクトを表示
        this.gameObject.SetActive(true);
        clear.SetActive(isClear);
        gameOver.SetActive(!isClear);

        // クリア時間(00:00 フォーマット)
        txtPlayTime.text = ((int)playTime / 60).ToString("00") + ":" + ((int)playTime % 60).ToString("00");
        // 討伐数
        txtDefeatCount.text = defeatCount.ToString();

        isActive = true;
    }
}
