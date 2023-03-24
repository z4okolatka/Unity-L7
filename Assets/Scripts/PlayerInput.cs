using System.Collections;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlatformCreator platformCreator;
    [SerializeField] Score scoreText;
    [SerializeField] float collapseTime = .05f;
    [SerializeField] AnimationCurve dyingCurve;
    [SerializeField] float dyingTime = 1f;

    private bool alive = true;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) && alive)
        {
            StartCoroutine(Dash(1, platformCreator.Move(1)));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && alive)
        {
            StartCoroutine(Dash(2, platformCreator.Move(2)));
        }
    }
    
    private IEnumerator Dash(int distance, bool die)
    {
        Vector3 initialScale = transform.localScale;
        for (float t = 0; t < 1f; t += Time.deltaTime / collapseTime)
        {
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
            yield return null;
        }
        Vector3 position = transform.position;
        position += platformCreator.unitDistance * distance;
        transform.position = position;
        yield return null;
        for (float t = 0; t < 1f; t += Time.deltaTime / collapseTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }
        if (die) {
            alive = false;
            StartCoroutine(Die());
            scoreText.setTrigger("Death");
            StartCoroutine(DelayedScoreReset());
        } else {
            scoreText.addScore(distance);
        }
    }
    private IEnumerator Die()
    {
        for (float t = 0; t < 1f; t += Time.deltaTime / dyingTime)
        {
            transform.localScale = Vector3.one * dyingCurve.Evaluate(t);
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }

    private IEnumerator DelayedScoreReset()
    {
        yield return new WaitForSeconds(2);
        scoreText.resetTrigger("Death");

        float initialScore = scoreText.score;
        float initialTopScore = scoreText.topScore.score;
        for (float t = 0; t < 2f; t += Time.deltaTime)
        {
            int score = Mathf.RoundToInt(Mathf.Lerp(initialScore, 0f, t));
            scoreText.setScore(score);
            if (initialScore > initialTopScore)
            {
                int topscore = Mathf.RoundToInt(Mathf.Lerp(initialTopScore, initialScore, t));
                scoreText.topScore.setScore(topscore);
            }
            yield return null;
        }

        scoreText.setTrigger("Revival");
        yield return new WaitForSeconds(2);
        scoreText.resetTrigger("Revival");

        // Возрождение куба
        platformCreator.Move(1);
        Vector3 position = transform.position;
        position += platformCreator.unitDistance;
        transform.position = position;
        for (float t = 0; t < 1f; t += Time.deltaTime / dyingTime)
        {
            transform.localScale = Vector3.one * dyingCurve.Evaluate(1 - t);
            yield return null;
        }
        transform.localScale = Vector3.one;
        alive = true;
    }
}
