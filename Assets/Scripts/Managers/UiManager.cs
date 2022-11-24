using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject healthNumberPrefab;
    public Color damageNumberColor = new Color(1.0f, 0.0f, 0.0f);
    public Color healNumberColor = new Color(0.0f, 1.0f, 0.0f);
    public GameObject youDiedText;
    [SerializeField] private GameObject _killCounterText;

    private void Start()
    {
        StartCoroutine(UpdateKillCounterText());
    }

    public void AddDamageNumber(Vector3 position, int amount, bool isCrit)
    {
        GameObject damageNumber = Instantiate(healthNumberPrefab, transform);
        damageNumber.GetComponent<HealthNumber>().worldPosition = position;
        damageNumber.GetComponent<Text>().text = "-" + amount.ToString();
        damageNumber.GetComponent<Text>().color = damageNumberColor;
        //damageNumber.GetComponent<Text>().fontSize = isCrit ? 32 : 16; // make the font bigger if its a crit

        float textScale = isCrit ? 1.0f : 1.2f;
        damageNumber.GetComponent<RectTransform>().localScale = new Vector3(textScale, textScale, textScale);
    }

    public void AddHealNumber(Vector3 position, int amount)
    {
        GameObject healNumber = Instantiate(healthNumberPrefab, transform);
        healNumber.GetComponent<HealthNumber>().worldPosition = position;
        healNumber.GetComponent<Text>().text = "+" + amount.ToString();
        healNumber.GetComponent<Text>().color = healNumberColor;
        float textScale = 1.2f;
        healNumber.GetComponent<RectTransform>().localScale = new Vector3(textScale, textScale, textScale);
        
            //healNumber.GetComponent<Text>().fontSize = 32;
    }

    public void CallShowDeathStatusCoroutine()
    {
        StartCoroutine(ShowDeathStatus());
    }

    IEnumerator ShowDeathStatus()
    {
        yield return new WaitForSeconds(0.1f);
        Color c = youDiedText.GetComponent<Text>().color = damageNumberColor;
        Vector3 s = Vector3.one;

        for (float alpha = 0f; alpha < 1f; alpha += 0.5f * Time.deltaTime)
        {
            c.a = alpha;
            s.x += 0.2f * Time.deltaTime;
            s.y += 0.2f * Time.deltaTime;
            youDiedText.GetComponent<Text>().color = c;
            youDiedText.GetComponent<RectTransform>().localScale = s;
            yield return null;
        }
    }

    IEnumerator UpdateKillCounterText()
    {
        for(; ; )
        {
            _killCounterText.GetComponent<Text>().text = EnemyManager.Instance.EnemiesKilled.ToString();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // singleton
    private static UiManager _instance;

    public static UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UiManager();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
}
