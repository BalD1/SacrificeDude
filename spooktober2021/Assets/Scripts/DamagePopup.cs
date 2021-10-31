using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    private Color textColor;

    [SerializeField] private float moveYspeed = 20f;
    [SerializeField] private float disappearTimer = 3f;
    [SerializeField] private float disappearSpeed = 3f;

    [SerializeField] private float increaseScaleAmount = 1f;
    [SerializeField] private float decreaseScaleAmount = 1f;

    private Vector3 moveVector;

    private float disappearTimeMax;

    private static int sortingOrder;

    public static DamagePopup Create(Vector2 pos, float value)
    {
        Transform damagePopupTransform = Instantiate(GameManager.Instance.DamagePopup, pos, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
        damagePopup.Setup(value);

        return damagePopup;
    }
    private void Awake()
    {
        disappearTimeMax = disappearTimer;
        textColor = text.color;
    }

    public void Setup(float amount)
    {
        text.SetText(amount.ToString());
        text.color = textColor;

        sortingOrder++;
        text.sortingOrder = sortingOrder;

        moveVector = new Vector2(0, moveYspeed);
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;

        if (disappearTimer > disappearTimeMax * 0.5f)
        {
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;
        if (disappearTimer <= 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;
            if (textColor.a <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
