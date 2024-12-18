using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TitleController : MonoBehaviour
{
    private enum TestSeButton
    {
        Play = 0,
        Prev,
        Next
    }
    [SerializeField] private Button[] btnTestSe;
    private int seIndex = 0;

    [SerializeField] private TextMeshProUGUI txtSoundIndex;

    void Start()
    {
        // ボタン操作（リスナー）
        btnTestSe[(int)TestSeButton.Play].onClick.AddListener(() => 
        {
            SoundManager.Instance.PlaySE((SoundManager.SE)seIndex);
        });
        btnTestSe[(int)TestSeButton.Prev].onClick.AddListener(() => 
        {
            seIndex = (seIndex - 1 + (int)SoundManager.SE.Num) % (int)SoundManager.SE.Num;
            txtSoundIndex.text = string.Format("Play SE [{0}]", seIndex);
        });
        btnTestSe[(int)TestSeButton.Next].onClick.AddListener(() => 
        {
            seIndex = (seIndex + 1) % (int)SoundManager.SE.Num;
            txtSoundIndex.text = string.Format("Play SE [{0}]", seIndex);            
        });
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // ボタンタッチ
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }

            // シーン遷移
            GameController.isPause = false;
            changeScene();
        }
    }

    void changeScene()
    {
        // Load the scene with the name "GameScene"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
