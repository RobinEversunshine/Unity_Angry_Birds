using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _secondsToCheck = 3f;
    [SerializeField] private GameObject _restartScene;
    [SerializeField] private SlingShotHandler _shotHandler;

    public static GameManager instance;


    public int MaxShots = 4;

    private int _usedShots;

    private IconHandler _iconHandler;

    private List<Enemy> _enemies = new List<Enemy>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _iconHandler = FindFirstObjectByType<IconHandler>();

        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i = 0; i < enemies.Length; i++)
        {
            _enemies.Add(enemies[i]);
        }


    }



    public void UseShot()
    {
        _usedShots++;
        _iconHandler.UseShot(_usedShots);
        CheckLastShot();
    }


    public bool HasEnoughShots()
    {
        return _usedShots < MaxShots;

    }


    private void CheckLastShot()
    {
        if (_usedShots == MaxShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }

    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToCheck);

        if (_enemies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }

    }


    public void RemoveEnemy(Enemy enemy)
    {
        _enemies.Remove(enemy);
        CheckForAllEnemies();
    }

    private void CheckForAllEnemies()
    {
        if (_enemies.Count == 0)
        {
            WinGame();
        }
    }


    #region Win/Lose
    private void WinGame()
    {
        Debug.Log("win");
        _restartScene.SetActive(true);
        _shotHandler.enabled = false;

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("lose");
    }


    #endregion
}
