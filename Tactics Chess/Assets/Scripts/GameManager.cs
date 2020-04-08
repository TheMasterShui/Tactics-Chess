using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Unit selectedUnit;
    public Texture2D cursorPrefab;

    public int playerTurn = 1;

    public GameObject selectedUnitSquare;

    public Image playerIndicator;
    public Sprite player1Indicator;
    public Sprite player2Indicator;

    public int player1Gold = 100;
    public int player2Gold = 100;

    public Text player1GoldText;
    public Text player2GoldText;

    public BarrackItem purchasedItem;

    public GameObject statsPanel;
    public Vector2 statsPanelShift;
    public Unit viewedUnit;

    public Text healthText;
    public Text armorText;
    public Text attackDamageText;
    public Text defenseDamageText;

    public AudioClip endTurnClip;
    private AudioSource source;

    private void Awake()
    {
        Cursor.SetCursor(cursorPrefab, Vector2.zero, CursorMode.Auto);
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        GetGoldIncome(1);
    }

    public void ToggleStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit) == false)
        {
            statsPanel.SetActive(true);
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
            viewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if (viewedUnit != null)
        {
            healthText.text = viewedUnit.health.ToString();
            armorText.text = viewedUnit.armor.ToString();
            attackDamageText.text = viewedUnit.attackDamage.ToString();
            defenseDamageText.text = viewedUnit.defenseDamage.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.transform.position = (Vector2)unit.transform.position + statsPanelShift;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit.Equals(viewedUnit))
        {
            statsPanel.SetActive(false);
            viewedUnit = null;
        }
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach(Village village in FindObjectsOfType<Village>())
        {
            if (village.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += village.goldPerTurn;
                }
                else
                {
                    player2Gold += village.goldPerTurn;
                }
            }
        }

        UpdateGoldText();
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            source.clip = endTurnClip;
            source.Play();
            EndTurn();
        }

        if (selectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitSquare.SetActive(false);
        }
    }

    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
        }

        GetGoldIncome(playerTurn);

        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }

        GetComponent<Barrack>().CloseMenus();
    }

    public void PlayAudioOnButtonClick(AudioClip audioClip)
    {
        source.clip = audioClip;
        source.Play();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


