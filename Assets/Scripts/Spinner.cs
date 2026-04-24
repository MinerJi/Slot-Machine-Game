using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TextureCategory
{
    public Texture Symbol;
    public string Category;
}
public enum State
{
    Idle,
    Spining,
}
//Summary
//Controls the visual spinning logic and symbol selection for a slot machine reel system.
//Requires pre-configured UI GameObjects for the slots and a SymbolSelector for determining outcomes.
public class Spinner : MonoBehaviour
{
    public List<GameObject> slots;
    public List<TextureCategory> textureCategories;
    public Dictionary<string, Texture> tcPairs = new Dictionary<string, Texture>();
    public SymbolSelector selector;
    public int spinningSpeed, stoppingDelay;
    public Vector2 displayPosition,initialPosAdjust;

    private List<List<RectTransform>> _rectTransforms = new List<List<RectTransform>>();
    private List<List<Image>> _images = new List<List<Image>>();
    private Vector2 _initialPosition, _lastPosition;
    private State _spinState = State.Idle;
    private int _currentSlot = 0;
    private Texture _selectedTexture;
    private RectTransform _selectedTransform;

    void Start()
    {
      
        foreach (var item in slots)
        {
            if (item.transform.childCount != textureCategories.Count)
            {
                Debug.LogError("Symbol count is not matching!");
                return;
            }
            List<RectTransform> currentSlotTransforms = new List<RectTransform>();
            List<Image> currentSlotImages = new List<Image>();

            // Cache components on initialization to prevent expensive GetComponent calls during Update
            for (int i = 0; i < item.transform.childCount; i++)
            {
                currentSlotTransforms.Add(item.transform.GetChild(i).GetComponent<RectTransform>());
                currentSlotImages.Add(item.transform.GetChild(i).GetComponent<Image>());
            }

            _rectTransforms.Add(currentSlotTransforms);
            _images.Add(currentSlotImages);
        }
        foreach (var item in textureCategories)
        {
            tcPairs[item.Category] = item.Symbol;
        }
        _initialPosition = _rectTransforms[0][0].anchoredPosition + initialPosAdjust;
        _lastPosition = _rectTransforms[0][textureCategories.Count - 1].anchoredPosition;
        Debug.Log(_lastPosition);
    }

    void Update()
    {
        if (_spinState == State.Spining)
        {
            for (int j = _currentSlot; j < slots.Count; j++)
            {
                for (int i = 0; i < _rectTransforms[j].Count; i++)
                {
                    RectTransform TempTransform = _rectTransforms[j][i];
                    TempTransform.anchoredPosition = Vector2.MoveTowards(TempTransform.anchoredPosition, _lastPosition, spinningSpeed * Time.deltaTime);
                    // Comparing Y-axis float values directly instead of using Vector2.Distance to save CPU cycles
                    if (TempTransform.anchoredPosition.y <= _lastPosition.y)
                    {
                        TempTransform.anchoredPosition = _initialPosition;
                    }
                }
            }
        }
    }
    // Initiates the slot machine spin sequence. 
    //Ignores the call if the reels are already in motion.
    public void StartSpin()
    {
        if (_spinState == State.Spining)
        {
            return;
        }
        _spinState = State.Spining;
        StartCoroutine(SlotSpinController());
    }

    //Handles the sequencing of stopping the reels one by one based on the StopingDelay.
    // Evaluates the winning symbol against the cached image lists to snap the correct UI element into place.
    IEnumerator SlotSpinController()
    {
        yield return new WaitForSeconds(stoppingDelay);
        WaitForSeconds slotDelay = new WaitForSeconds(2f);
        for (int j = 0; j < slots.Count; j++)
        {
            _selectedTexture = tcPairs[selector.Select()];
            for (int i = 0; i < _images[j].Count; i++)
            {
                var TempImage = _images[j][i];
                if (TempImage.mainTexture == _selectedTexture)
                {
                    _selectedTransform = _rectTransforms[j][i];
                    break;
                }
            }
            yield return new WaitUntil(() => Vector2.Distance(_selectedTransform.anchoredPosition, displayPosition) < 1f && _selectedTransform.anchoredPosition.y >= displayPosition.y);
            yield return slotDelay;
            _currentSlot++;
        }

        _spinState = State.Idle;
        _currentSlot = 0;
    }
}