using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class UGUIJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {

    public RectTransform bar;
    public delegate void UGUIJoystickDelegate(bool isActive, float radian);
    static UGUIJoystickDelegate OnJoystickDrag;
    RectTransform trans;
    bool isLock, isActive;
    float maxDistance;
    public bool Lock {
        get { return isLock; }
        set {
            isLock = value;
            if (isLock) {
                Init();
            }
        }
    }
    Vector2 defaultPos;
    float defaultAlpha, barDefaultAlpha;
    Image image, barImage;
    RawImage rawImage, barRawImage;
    float screenSizeRate, screenUIWidth;

    void Awake() {
        trans = GetComponent<RectTransform>();
        if (bar) {
            Button barBtn = bar.GetComponent<Button>();
            if (barBtn) {
                Destroy(barBtn);
            }
        } else {
            Debug.LogError("Joystick Bar is null");
        }
        maxDistance = trans.sizeDelta.x * 0.35f * trans.localScale.x;
        GetDefaultInfo();
    }

    void Start() {
        CanvasScaler canvasScaler = GameObject.FindObjectOfType<CanvasScaler>();
        screenSizeRate = canvasScaler.referenceResolution.y / (float)Screen.height;
        screenUIWidth = canvasScaler.referenceResolution.y * (float)Screen.width / (float)Screen.height;
    }

    void OnEnable() {
        Init();
    }

    void OnApplicationFocus(bool focusStatus) {
        Init();
    }

    void Init() {
        isActive = false;
        SetBorder(defaultPos, defaultAlpha);
        SetBar(Vector2.zero, barDefaultAlpha);
        if (OnJoystickDrag != null) {
            OnJoystickDrag(false, 0);
        }

    }

    void Update() {
        if (isActive) {
            if (OnJoystickDrag != null) {
                float radian = Mathf.Atan2(bar.anchoredPosition.y, bar.anchoredPosition.x);
                OnJoystickDrag(true, radian);
            }
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        if (isLock) {
            return;
        }
        Vector2 touchPos = eventData.position * screenSizeRate;
        if (defaultPos.x < 0) {
            touchPos.x -= screenUIWidth;
        }
        SetBorder(touchPos, 1f);
        SetBar(Vector2.zero, 1f);
        isActive = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData) {
        if (isLock) {
            return;
        }
        Vector2 touchPos = eventData.position * screenSizeRate;
        if (defaultPos.x < 0) {
            touchPos.x -= screenUIWidth;
        }
        bar.anchoredPosition = touchPos - trans.anchoredPosition;
        float dis = Vector2.Distance(bar.anchoredPosition, Vector2.zero);
        if (dis >= maxDistance) {
            Vector2 vec = bar.anchoredPosition * maxDistance / dis;
            bar.anchoredPosition = vec;
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) {
        Init();
    }

    public static void RegisterEvent(UGUIJoystickDelegate OnUGUIJoystickDrag) {
        OnJoystickDrag = OnUGUIJoystickDrag;
    }
    public static void UnregisterEvent() {
        RegisterEvent(null);
    }

    void GetDefaultInfo() {
        defaultPos = trans.anchoredPosition;
        image = GetComponent<Image>();
        barImage = bar.GetComponent<Image>();
        rawImage = GetComponent<RawImage>();
        barRawImage = bar.GetComponent<RawImage>();
        defaultAlpha = image ? image.color.a : rawImage.color.a;
        barDefaultAlpha = barImage ? barImage.color.a : barRawImage.color.a;
    }
    void SetBorder(Vector2 pos, float alpha) {
        trans.anchoredPosition = pos;
        if (image) {
            Color color = image.color;
            color.a = alpha;
            image.color = color;
        } else {
            Color color = rawImage.color;
            color.a = alpha;
            rawImage.color = color;
        }
    }
    void SetBar(Vector2 pos, float alpha) {
        bar.anchoredPosition = pos;
        if (barImage) {
            Color color = barImage.color;
            color.a = alpha;
            barImage.color = color;
        } else {
            Color color = barRawImage.color;
            color.a = alpha;
            barRawImage.color = color;
        }
    }
}