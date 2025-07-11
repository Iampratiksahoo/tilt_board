using Source.TiltBoard.Board;
using UnityEngine;
using UnityEngine.Rendering;

public class TestBoardInput : MonoBehaviour
{
    [SerializeField] private BoardObject boardPrefab = null;
    [SerializeField] private LeanTweenType boardSpawnEaseType = LeanTweenType.easeInSine;
    [SerializeField] private float boardSpawnAnimTime = .1f;
    private BoardObject _boardController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnBoard();
        }
        
        if (_boardController == null)
            return;

        // Player 1 input (WASD)
        Vector2 p1 = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) p1.y = +1f;
        if (Input.GetKey(KeyCode.S)) p1.y = -1f;
        if (Input.GetKey(KeyCode.D)) p1.x = +1f;
        if (Input.GetKey(KeyCode.A)) p1.x = -1f;

        // Player 2 input (Arrow keys)
        Vector2 p2 = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow)) p2.y = +1f;
        if (Input.GetKey(KeyCode.DownArrow)) p2.y = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) p2.x = +1f;
        if (Input.GetKey(KeyCode.LeftArrow)) p2.x = -1f;

        // Send to board
        _boardController.Tilt(p1, p2);
    }

    public void SpawnBoard()
    {
        if (_boardController != null)
        {
            Destroy(_boardController.gameObject);
        }

        _boardController = Instantiate(boardPrefab);
        _boardController.transform.localScale = Vector3.zero;

        LeanTween.scale(
            _boardController.gameObject,
            Vector3.one,
            boardSpawnAnimTime
        ).setEase(boardSpawnEaseType);
    }
}
