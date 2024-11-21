using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerScoreUI : MonoBehaviour
{
  
    PlayerScore playerScore;

    [SerializeField] Text scoreText;

    int currentDisplayScore = 0;
    int targetDisplayScore = 0;
    
    float maxCountUpTime = 1.5f;

    [SerializeField] Text addedPointsText;


    public void Init(PlayerScore _playerScore)
    {
        playerScore = _playerScore;
    }
    
    void Start()
    {
        UpdateText(false);
    }

    public void UpdatePlayerScore(bool animate)
    {
        UpdateText(animate);
    }

    public void Reset()
    {
        if (countUpScoreCoroutine != null)
        {
            StopCoroutine(countUpScoreCoroutine);
        }
        SetText();
        addedPointsText.text = "";
    }

    Coroutine countUpScoreCoroutine;

    void UpdateText(bool animate)
    {

        if (animate)
        {
            if (countUpScoreCoroutine != null)
            {
                StopCoroutine(countUpScoreCoroutine);
                currentDisplayScore = targetDisplayScore;
            }
            targetDisplayScore = playerScore.score;
            int scoreDiff = Mathf.Abs(targetDisplayScore - currentDisplayScore);
            countUpScoreCoroutine = StartCoroutine(CountUpScore(Mathf.Min(scoreDiff * 0.015f, maxCountUpTime)));
        }
        else 
        {
            currentDisplayScore = playerScore.score;
            targetDisplayScore = playerScore.score; 
            SetText();
        }

        if (playerScore.lastAddedScore > 0)
        {
            addedPointsText.text = "+" + playerScore.lastAddedScore.ToString();
            addedPointsText.transform.localScale = new Vector3(1.15f, 1.15f, 1.15f);
        }
        else
        {
            addedPointsText.text = "";
        }

    }

    IEnumerator CountUpScore(float scoreCountDuration)
    {

        float t = 0.0f;
        int startingScore = currentDisplayScore;

        while (t < scoreCountDuration)
        {
            t += Time.deltaTime;
            currentDisplayScore = (int)Mathf.Lerp(startingScore, targetDisplayScore, t / scoreCountDuration);

            addedPointsText.transform.localScale = Vector3.Lerp(addedPointsText.transform.localScale, Vector3.one, t / (scoreCountDuration * 0.6f));

            SetText();
            yield return null;
        }

        currentDisplayScore = targetDisplayScore;
        SetText();
        addedPointsText.text = "";

        yield return null;
    }

    void SetText()
    {
        scoreText.text = currentDisplayScore.ToString("n0");
    }

}