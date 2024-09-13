using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public int playerScore = 0;
    public bool isPlayerDead = false;  // ��¼��ɫ�Ƿ�����

    void Awake()//ȷ��ʼ�մ���game controller
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        if (!isPlayerDead)
        {
            playerScore += points;
            Debug.Log("Score: " + playerScore);
        }
    }

    public void ResetScore()
    {
        playerScore = 0;
        Debug.Log("Score reset. Current Score: " + playerScore);
    }

    public void PlayerDied()
    {
        isPlayerDead = true;
        Debug.Log("Player is dead.!!!!!!");
    }

    public void PlayerRevived()
    {
        isPlayerDead = false;
        Debug.Log("Player revived.");
    }
}
