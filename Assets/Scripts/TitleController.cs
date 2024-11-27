using UnityEngine;

public class TitleController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // 画面タッチでゲームシーンに遷移
        if (Input.GetMouseButtonDown(0))
        {
            changeScene();
        }   
    }

    void changeScene()
    {
        // Load the scene with the name "GameScene"
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

}
