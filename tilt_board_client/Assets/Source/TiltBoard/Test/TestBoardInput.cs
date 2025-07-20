using Source.TiltBoard.Board;
using Source.TiltBoard.Global;
using Source.TiltBoard.Global.Signal;
using Source.TiltBoard.Signal;
using UnityEngine;

public class TestBoardInput : MonoBehaviour
{
    private BoardObject _boardController;
    void Update()
    {
        if (_boardController == null)
        {
            _boardController = FindAnyObjectByType<BoardObject>();
        }
        
        if (_boardController != null)
        {
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
    }
}
