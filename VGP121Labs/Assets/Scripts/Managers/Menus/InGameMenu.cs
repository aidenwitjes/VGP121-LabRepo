using TMPro;

public class InGameMenu : BaseMenu
{
    public TMP_Text livesText;
    public TMP_Text scoreText;
    public override void InitState(MenuController ctx)
    {
        base.InitState(ctx);
        state = MenuController.MenuStates.InGame;
        livesText.text = "Lives: " + GameManager.Instance.lives.ToString();
        GameManager.Instance.OnLifeValueChanged += OnLifeValueChanged;
        scoreText.text = "Score: " + GameManager.Instance.score.ToString();
        GameManager.Instance.OnScoreValueChanged += OnScoreValueChanged;
    }

    private void OnLifeValueChanged(int lives)
    {
        livesText.text = "Lives: " + lives.ToString();
    }
    private void OnScoreValueChanged(int score)
    {
        scoreText.text = "Score " + score.ToString();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnLifeValueChanged -= OnLifeValueChanged;
    }
}
