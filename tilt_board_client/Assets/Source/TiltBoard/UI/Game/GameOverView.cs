using TMPro;
using UnityEngine;

namespace Source.TiltBoard.UI.Game
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField]
        private GameObject content = null;

        [SerializeField]
        private TMP_Text nameText = null;

        [SerializeField]
        private TMP_Text scoreText = null;

        public void Show(string winnerName = "", int winnerScore = 0)
        {
            // first set the name and score 
            nameText.text = winnerName;
            scoreText.text = winnerScore.ToString();

            // then show the content
            content.SetActive(true);
        }

        public void Hide()
        {
            content.SetActive(false);
        }
    }
}