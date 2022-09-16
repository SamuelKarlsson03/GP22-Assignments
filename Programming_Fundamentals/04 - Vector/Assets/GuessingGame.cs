using UnityEngine;

public class GuessingGame : ProcessingLite.GP21
{
    public Vector2 nextGuessTextCoords;
    public Vector2 scoreTextCoords;
    public Vector2 lastGuessScoreTextCoords;

    public float scoreOnGuess;
    public float safeDistance;
    public float scoreFalloff;

    private Vector2 targetCoords;
    private Vector2 lastGuess;
    private Vector2 lastCorrect;
    private float scoreOnLastGuess;
    private float score;

    private void Start()
    {
        ChooseNewTargetLocation();   
    }

    void Update()
    {
        Background(255);
        Vector2 mousePos = new Vector2((Input.mousePosition.x / Screen.width) * Width, (Input.mousePosition.y / Screen.height) * Height);
        Stroke(0, 255, 0);
        Circle(lastCorrect.x, lastCorrect.y, 0.1f); //Last correct answer
        Stroke(255, 255, 0);
        Circle(lastGuess.x,lastGuess.y,0.1f);//Last guess
        Stroke(0);
        Circle(Width/2, Height/2, 0.1f);//Center circle marker
        Text($"Guess the location of the vector[{targetCoords.x:0.00}:{targetCoords.y:0.00}]", nextGuessTextCoords.x, nextGuessTextCoords.y);
        Text($"Score: {score}", scoreTextCoords.x, scoreTextCoords.y);
        Text($"Last guess: {scoreOnLastGuess}",lastGuessScoreTextCoords.x,lastGuessScoreTextCoords.y);
        
        if (Input.GetMouseButton(0)) //Draw line from center to mouse
        {
            Line(Width/2,Height/2, mousePos.x, mousePos.y);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            Guess(mousePos);
            ChooseNewTargetLocation();
        }
    }

    private void Guess(Vector2 guessPosition)
    {
        Vector2 recalculatedGuess = new Vector2(guessPosition.x - Width / 2, guessPosition.y - Height / 2);
        float distance = Vector2.Distance(recalculatedGuess, targetCoords);
        lastGuess = guessPosition;
        lastCorrect = new Vector2(targetCoords.x + Width/2, targetCoords.y + Height/2);
        scoreOnLastGuess = scoreOnGuess / Mathf.Pow(2, scoreFalloff * (Mathf.Max((distance - safeDistance), 0)));
        score += scoreOnLastGuess;
    }

    private void ChooseNewTargetLocation()
    {
        targetCoords = new Vector2(Random.Range(0, Width), Random.Range(0, Height));
        targetCoords.x -= Width / 2;
        targetCoords.y -= Height / 2;
    }
}
