using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    private Image _healthbarImage;
    private float _newFillAmount;

    private void Start()
    {
        _healthbarImage = GetComponent<Image>();

        // for some stupid reason, unity requires that there is a sprite for an image in order for it to use the partial fill feature
        // create a blank sprite to use
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.wrapMode = TextureWrapMode.Repeat;
        tex.Apply();
        _healthbarImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        
        _healthbarImage.type = Image.Type.Filled;
        _healthbarImage.fillMethod = (int)Image.FillMethod.Horizontal;
        _healthbarImage.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    private void Update()
    {
        //_healthbarImage.fillAmount = Mathf.Lerp(_healthbarImage.fillAmount, _newFillAmount, Time.deltaTime);
    }

    public void UpdateHealth(int current, int max)
    {
        //_newFillAmount = Mathf.Clamp(current / (float)max, 0.0f, 1.0f);

        _healthbarImage.fillAmount = Mathf.Clamp(current / (float)max, 0.0f, 1.0f);
    }
}
