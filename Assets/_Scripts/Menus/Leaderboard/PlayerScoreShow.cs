using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerScoreShow : MonoBehaviour
{

    [SerializeField] NumberDisplay rankDisplay, scoreDisplay;

    [SerializeField] TMP_Text playerNameText, scoreText;


    [SerializeField] Image bgImage;


    [SerializeField] Sprite[] firstRanksSprites;


    [SerializeField] int manualDashReduction = 8;

    internal void SetPlayer(Leaderboard.Player player, int rank)
    {
        playerNameText.text = player.name;// + GetNeededDashes(player.name);
        // scoreDisplay.SetNumber(player.score);
        scoreText.text = player.score.ToString();

        if (rank <= firstRanksSprites.Length) bgImage.sprite = firstRanksSprites[rank - 1];

        rankDisplay.SetNumber(rank);
    }

    private string GetNeededDashes(string playerName)
    {
        string originalText = playerName;
        string dashes = "";

        playerNameText.text = originalText;
        float maxWidth = playerNameText.rectTransform.rect.width;

        while (playerNameText.preferredWidth < maxWidth)
        {
            dashes += "-";
            playerNameText.text = originalText + dashes;

            if (playerNameText.preferredWidth >= maxWidth)
            {
                dashes = dashes.Remove(dashes.Length - 1);
                break;
            }
        }

        // should not be here, is some kind of wrong reading of the size of the text field
        for (int i = 0; i < manualDashReduction; i++)
        {
            if(dashes.Length == 0) break;
            dashes = dashes.Remove(dashes.Length - 1);
        }


        return dashes;
    }
}
