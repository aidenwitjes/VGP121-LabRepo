using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : BaseMenu
{
    public Button mainMenuButton;
    public Button quitButton;
    public override void InitState(MenuController ctx)
    {
        base.InitState(ctx);
        state = MenuController.MenuStates.GameOver;
        mainMenuButton.onClick.AddListener(() => GameManager.Instance.LoadScene("TitleScreen"));
        quitButton.onClick.AddListener(QuitGame);
    }
}