using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System.IO;

public class CurveZone : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler {

	[SerializeField] private CurveSlot curveSlot;
	[SerializeField] private AnimationCurve curveValue;

	[SerializeField] public float minValue, maxValue;

	[SerializeField] private Color backColor;
	[SerializeField] private Color curveColor;
	[SerializeField] private Color graduationColor;
	[SerializeField] private Color keyColor;
	[SerializeField] private Color selectedKeyColor;

	[SerializeField] private CurvePresetButton[] presetButtons;

	[SerializeField] private RawImage previewImage;

	[SerializeField] private int rWidth, rHeight;

	RectTransform rectTransform;
	RawImage curveZoneTexture;


	bool hasSelectedKey;
	int selectKeyIndex;


	private int pixelSensibility = 10;


	public void Initialize()
	{
		rectTransform = GetComponent<RectTransform> ();
		curveZoneTexture = GetComponent<RawImage> ();
		UpdateCurve ();
		InitializePresetButtons ();
	}

	public void SetCurve(AnimationCurve curve)
	{
		curveValue = curve;
		UpdateCurve ();

	}

	public void InitializePresetButtons()
	{
		for (int i = 0; i < presetButtons.Length; i++) {
			presetButtons [i].Initialize (this);
		}
	}

	public void UpdateCurve()
	{
		//Debug.Log ("Update curve");
		int height = (int)(rHeight / 4f);
		int width = (int)(rWidth / 4f);

		//Debug.Log (width + "  " + height);

		Texture2D t = new Texture2D (width, height);

		t.filterMode = FilterMode.Point;

		int h0 = (int)(Mathf.InverseLerp(minValue, maxValue, 0f) * height);
		int h1 = (int)(Mathf.InverseLerp(minValue, maxValue, 1f) * height);
		int h2 = (int)(Mathf.InverseLerp(minValue, maxValue, 2f) * height);
		int hm1 = (int)(Mathf.InverseLerp(minValue, maxValue, -1f) * height);


		for (int i = 0; i < width; i++) {

			for (int j = 0; j < height; j++)
				t.SetPixel (i, j, backColor);
			
			if (h0 > -1 && h0 < height)
				t.SetPixel (i, h0, graduationColor);
			if (h1 > -1 && h1 < height)
				t.SetPixel (i, h1, graduationColor);
			if (h2 > -1 && h2 < height)
				t.SetPixel (i, h2, graduationColor);
			if (hm1 > -1 && hm1 < height)
				t.SetPixel (i, hm1, graduationColor);

			float ratio = (float)i / (float)width;
			float fX = curveValue.Evaluate (ratio);

			//float h = (fX / (maxValue - minValue)) + minValue;


			float h = Mathf.InverseLerp(minValue, maxValue, fX);

			h *= (height - 1);
			//Debug.Log (h);

			t.SetPixel (i, (int)h, curveColor);



		}


		Color[] selectedBlock = new Color[9];
		Color[] unselectedBlock = new Color[9];

		for (int i = 0; i < selectedBlock.Length; i++) {
			unselectedBlock [i] = keyColor;
			selectedBlock [i] = selectedKeyColor;
		}

		for (int i = 0; i < curveValue.keys.Length; i++) {

			Keyframe k = curveValue.keys [i];

			int kX = (int)(k.time * width);
			int kY = (int)(Mathf.InverseLerp(minValue, maxValue, k.value) * height);

			if (kX < width - 1 && kX > 0 && kY < height - 1 && kY > 0) {
				if (selectKeyIndex == i && hasSelectedKey)
					t.SetPixels (kX - 1, kY - 1, 3, 3, selectedBlock);
				else
					t.SetPixels (kX - 1, kY - 1, 3, 3, unselectedBlock);
			}

		}

		t.Apply ();
		Destroy (previewImage.texture);
		Destroy (curveZoneTexture.texture);

		curveZoneTexture.texture = t;
		previewImage.texture = t;

		curveSlot.CurveValue = curveValue;
		curveSlot.UpdateValue ();

	}

	public void OnPointerDown(PointerEventData data)
	{

		Vector2 screenPos = data.position;

		float width = rectTransform.rect.width;
		float height = rectTransform.rect.height;

		Vector2 position;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, screenPos, null, out position);
		position += new Vector2 (width, height) * 0.5f;

		//Debug.Log ("pos : " + position);

		hasSelectedKey = false;
		for (int i = 0; i < curveValue.keys.Length && !hasSelectedKey; i++) {

			Keyframe k = curveValue.keys [i];

			int kX = (int)(k.time * width);
			int kY = (int)(Mathf.InverseLerp (minValue, maxValue, k.value) * height);


			if (kX + pixelSensibility > position.x && kX - pixelSensibility < position.x
			   && kY + pixelSensibility > position.y && kY - pixelSensibility < position.y) {
				if (data.button == PointerEventData.InputButton.Left) {
					selectKeyIndex = i;
					hasSelectedKey = true;
					Debug.Log ("SELECTED KEY : " + i);
				} else if (data.button == PointerEventData.InputButton.Right) {

					curveValue.RemoveKey (i);
					UpdateCurve ();
					return;
				}
			}
		}

		if (!hasSelectedKey && data.button == PointerEventData.InputButton.Right) {

			float ktime = position.x / (float)width;
			float kvalue = Mathf.Lerp ((float)minValue, (float)maxValue, position.y / height);

			curveValue.AddKey (ktime, kvalue);

			UpdateCurve ();

		}
	}





	public void OnBeginDrag(PointerEventData data)
	{

		Debug.Log ("Début dragging");


	}


	public void OnDrag(PointerEventData data)
	{
		if (data.dragging) {
			Debug.Log ("dragging....");

			if (!RectTransformUtility.RectangleContainsScreenPoint (rectTransform, data.position)) {
				data.dragging = false;
				Debug.Log ("exit curve zone");
				
			}

			if (hasSelectedKey) {

				Debug.Log ("JE BOUGE UNE CLé");

				Vector2 screenPos = data.position;

				float width = rectTransform.rect.width;
				float height = rectTransform.rect.height;

				Vector2 position;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, screenPos, null, out position)) {
					position += new Vector2 (width, height) * 0.5f;


					if (position.x >= 0 && position.y >= 0 && position.x < width && position.y < height) {
 
						float ktime = position.x / (float)width;
						float kvalue = Mathf.Lerp ((float)minValue, (float)maxValue, position.y / (height + 1f));

						//Debug.Log (position.y + "   " + ktime + " , " + kvalue);
						CurveValueMoveKey (selectKeyIndex, ktime, kvalue);
						//curveValue.MoveKey (selectKeyIndex, new Keyframe (ktime, kvalue));
						UpdateCurve ();
					}
				}
			}
		}
	}


	public void CurveValueMoveKey(int index, float time, float value)
	{
		Keyframe[] nKeys = new Keyframe[curveValue.keys.Length];
		for (int i = 0; i < curveValue.keys.Length; i++) {
			if (index == i) {
				nKeys [i] = new Keyframe (time, value);
			} else
				nKeys [i] = curveValue.keys [i];
		}
		curveValue.keys = nKeys;
	}

	public void OnEndDrag(PointerEventData data)
	{
		Debug.Log ("Fin dragging");
		hasSelectedKey = false;

		UpdateCurve ();

		curveSlot.CurveValue = curveValue;

		curveSlot.UpdateValue ();

	}


	public void SaveTexture()
	{

		File.WriteAllBytes (Application.persistentDataPath + "/texture.png", ((Texture2D)curveZoneTexture.texture).EncodeToPNG ()); 

		Debug.Log ("saved at '" + Application.persistentDataPath + "/texture.png'");
	}

}