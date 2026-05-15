using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


public class PokemonBattleSystem : MonoBehaviour
{
    [System.Serializable]
    public class Move
    {
        public string moveName;
        public int damage;
    }

    [System.Serializable]
    public class Pokemon
    {
        public string name;
        public int maxHP;
        public int currentHP;
        public Move[] moves;
    }

    [Header("Pok�mon Data")]
    public Pokemon playerPokemon;
    public Pokemon enemyPokemon;

    [Header("UI Elements")]
    public TMP_Text playerNameText;
    public Slider playerHPBar;
    public TMP_Text enemyNameText;
    public Slider enemyHPBar;
    public TMP_Text battleLog;
    public Button[] moveButtons;

    private bool playerTurn = true;
    private bool battleOver = false;

    public string newSceneName;

    void Start()
    {
        // Initialize HP
        playerPokemon.currentHP = playerPokemon.maxHP;
        enemyPokemon.currentHP = enemyPokemon.maxHP;

        // Set UI
        playerNameText.text = playerPokemon.name;
        enemyNameText.text = enemyPokemon.name;
        playerHPBar.maxValue = playerPokemon.maxHP;
        enemyHPBar.maxValue = enemyPokemon.maxHP;
        UpdateHPBars();

        // Assign move buttons
        for (int i = 0; i < moveButtons.Length; i++)
        {
            int index = i; 
            moveButtons[i].GetComponentInChildren<TMP_Text>().text = playerPokemon.moves[i].moveName;
            moveButtons[i].onClick.AddListener(() => OnPlayerMove(index));
        }

        battleLog.text = $"A wild {enemyPokemon.name} appeared!";
    }

    void OnPlayerMove(int moveIndex)
    {
        if (!playerTurn || battleOver) return;

        Move move = playerPokemon.moves[moveIndex];
        StartCoroutine(PlayerAttack(move));
    }

    IEnumerator PlayerAttack(Move move)
    {
        battleLog.text = $"{playerPokemon.name} used {move.moveName}!";
        yield return new WaitForSeconds(1f);

        enemyPokemon.currentHP -= move.damage;
        if (enemyPokemon.currentHP < 0) enemyPokemon.currentHP = 0;
        UpdateHPBars();

        if (CheckBattleEnd()) yield break;

        playerTurn = false;
        yield return new WaitForSeconds(1f);
        StartCoroutine(EnemyTurn());
    }

    IEnumerator EnemyTurn()
    {
        // Enemy picks a random move
        Move move = enemyPokemon.moves[Random.Range(0, enemyPokemon.moves.Length)];
        battleLog.text = $"{enemyPokemon.name} used {move.moveName}!";
        yield return new WaitForSeconds(1f);

        playerPokemon.currentHP -= move.damage;
        if (playerPokemon.currentHP < 0) playerPokemon.currentHP = 0;
        UpdateHPBars();

        if (CheckBattleEnd()) yield break;

        playerTurn = true;
        battleLog.text = $"What will {playerPokemon.name} do?";
    }

    void UpdateHPBars()
    {
        playerHPBar.value = playerPokemon.currentHP;
        enemyHPBar.value = enemyPokemon.currentHP;
    }


    bool CheckBattleEnd()
    {
        if (playerPokemon.currentHP <= 0)
        {
            battleLog.text = $"{playerPokemon.name} fainted! You lose!";
            battleOver = true;
            StartCoroutine(EndBattleDelay());
            return true;
        }
        else if (enemyPokemon.currentHP <= 0)
        {
            battleLog.text = $"{enemyPokemon.name} fainted! You win!";
            battleOver = true;
            StartCoroutine(EndBattleDelay());
            return true;
        }
        return false;
    }



    void LoadNextScene()
    {
        SceneManager.LoadScene(newSceneName);
    }


    IEnumerator EndBattleDelay()
    {
        yield return new WaitForSeconds(2f); // wait 2 seconds
        LoadNextScene();
    }


}
