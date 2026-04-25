using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public DataContainer container;
    public Dictionary<string, Texture> tcPairs = new Dictionary<string, Texture>();
    public SymbolSelector selector;
    public Animator animator;
    public int spinningSpeed, stoppingDelay;
    public Vector2 displayPosition, initialPosAdjust;
    public TransactionManager transactionManager;
    public GameObject buttonParent;

    private State _spinState = State.Idle;
    private List<List<RectTransform>> _rectTransforms = new List<List<RectTransform>>();
    private List<List<Image>> _images = new List<List<Image>>();
    private Vector2 _initialPosition, _lastPosition;
    private int _currentSlot = 0;
    private Texture _selectedTexture;
    private RectTransform _selectedTransform;
    private List<Texture> _textures = new List<Texture>();

    //Initializes required lists and dictonaries
    void Start()
    {
        if (container.symbolDatas.Count == 0)
        {
            Debug.Log("Symbol List is Empty!");
        }
        foreach (var item in slots)
        {
            if (item.transform.childCount != container.symbolDatas.Count)
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
        foreach (var item in container.symbolDatas)
        {
            tcPairs[item.Category] = item.Symbol;
        }
        _initialPosition = _rectTransforms[0][0].anchoredPosition + initialPosAdjust;
        _lastPosition = _rectTransforms[0][container.symbolDatas.Count - 1].anchoredPosition;
        Debug.Log(_lastPosition);
    }

    //Handles the spinning
    void Update()
    {
        if (_spinState == State.Spining)
        {
            float moveStep = spinningSpeed * Time.deltaTime;

            for (int j = _currentSlot; j < slots.Count; j++)
            {
                for (int i = 0; i < _rectTransforms[j].Count; i++)
                {
                    RectTransform TempTransform = _rectTransforms[j][i];
                    TempTransform.anchoredPosition = new Vector2(TempTransform.anchoredPosition.x, TempTransform.anchoredPosition.y - moveStep);
                    if (TempTransform.anchoredPosition.y <= _lastPosition.y)
                    {
                        float overshoot = TempTransform.anchoredPosition.y - _lastPosition.y;
                        TempTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y + overshoot);
                    }
                }
            }
        }
    }

    // Initiates the slot machine spin sequence. 
    //Ignores the call if the reels are already in motion.
    public void StartSpin(int betAmount)
    {
        Debug.Log(container.remainingAmount);
        Debug.Log(betAmount);
        if (_spinState == State.Spining || container.remainingAmount < betAmount)
        {

            return;
        }
        StartCoroutine(SlotSpinController());
    }

    //Handles the sequencing of stopping the reels one by one based on the StopingDelay.
    // Evaluates the winning symbol against the cached image lists to snap the correct UI element into place.
    IEnumerator SlotSpinController()
    {
        buttonParent.SetActive(false);
        animator.SetTrigger("Pull");

        yield return new WaitForSeconds(0.5f);
        _spinState = State.Spining;

        yield return new WaitForSeconds(stoppingDelay);
        WaitForSeconds slotDelay = new WaitForSeconds(1.4f);
        for (int j = 0; j < slots.Count; j++)
        {
            _selectedTexture = tcPairs[selector.Select()];
            _textures.Add(_selectedTexture);
            Debug.Log(_selectedTexture);
            for (int i = 0; i < _images[j].Count; i++)
            {
                var TempImage = _images[j][i];
                if (TempImage.mainTexture == _selectedTexture)
                {
                    _selectedTransform = _rectTransforms[j][i];
                    break;
                }
            }
            yield return new WaitUntil(() => Vector2.Distance(_selectedTransform.anchoredPosition, displayPosition) <= (spinningSpeed * Time.deltaTime * 1.5f));

            _currentSlot++;

            Vector2 offset = displayPosition - _selectedTransform.anchoredPosition;
            for (int i = 0; i < _rectTransforms[j].Count; i++)
            {
                _rectTransforms[j][i].anchoredPosition += offset;
            }

            yield return slotDelay;
        }
        transactionManager.CreditMoney(_textures);
        _textures.Clear();
        buttonParent.SetActive(true);
        _spinState = State.Idle;
        _currentSlot = 0;
    }
}