using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Race Progress UI")]
	public class RaceProgressUI : UIBehaviour, ICanvasElement
	{
		[Serializable]
		public class ProgressRacers
		{
			public RectTransform RacerIcon;

			public float ProgressValue;

			public Racer_Detail RacerDetail;

			public bool IsPlayer;

			public bool IsDestroyed;

			public ProgressRacers()
			{
			}

			public ProgressRacers(RectTransform Icon, Racer_Detail RD)
			{
				RacerIcon = Icon;
				RacerDetail = RD;
			}
		}

		[SerializeField]
		private RectTransform m_PlayerProgressFill;

		[SerializeField]
		private RectTransform m_PlayerIcon;

		[SerializeField]
		private RectTransform m_RivalIcon;

		[SerializeField]
		private RectTransform m_DestroyedRivalIcon;

		private Sprite m_DestroyedRivalIconSprite;

		[SerializeField]
		private RectTransform m_LapSeperator;

		[SerializeField]
		public List<ProgressRacers> m_Racers;

		private float m_MinValue;

		private float m_MaxValue = 100f;

		private Image m_FillImage;

		private Transform m_FillTransform;

		private RectTransform m_FillContainerRect;

		private Transform m_HandleTransform;

		private RectTransform m_IconSliderRect;

		private Race_Manager m_RaceManager;

		private float m_RaceLenght;

		private float m_RaceLaps;

		private float m_TotalLenght;

		private float m_ProgressResolution;

		public new Transform transform => transform;

		public new bool IsDestroyed()
		{
			return base.IsDestroyed();
		}

		protected override void Awake()
		{
			base.Awake();
			GameObject gameObject = GameObject.Find("_RaceManager");
			if (gameObject != null)
			{
				m_RaceManager = gameObject.GetComponent<Race_Manager>();
				m_RaceManager.OnRaceInitiated += m_RaceManager_OnRaceInitiated;
			}
			if (m_Racers == null)
			{
				m_Racers = new List<ProgressRacers>();
			}
			else
			{
				m_Racers.Clear();
			}
			if (m_PlayerIcon != null)
			{
				m_PlayerIcon.gameObject.SetActive(value: false);
			}
			if (m_RivalIcon != null)
			{
				m_RivalIcon.gameObject.SetActive(value: false);
			}
			if (m_DestroyedRivalIcon != null)
			{
				m_DestroyedRivalIconSprite = m_DestroyedRivalIcon.GetComponent<Image>().sprite;
				m_DestroyedRivalIcon.gameObject.SetActive(value: false);
			}
			if (m_LapSeperator != null)
			{
				m_LapSeperator.gameObject.SetActive(value: false);
			}
		}

		private void m_RaceManager_OnRaceInitiated()
		{
			m_RaceLenght = m_RaceManager.RaceLength;
			m_RaceLaps = m_RaceManager.RaceLaps;
			m_TotalLenght = m_RaceLenght * m_RaceLaps;
			m_ProgressResolution = 100f / m_TotalLenght;
			foreach (Racer_Detail registeredRacer in m_RaceManager.RegisteredRacers)
			{
				RectTransform rectTransform;
				if (registeredRacer.IsPlayer)
				{
					rectTransform = UnityEngine.Object.Instantiate(m_PlayerIcon);
					rectTransform.SetParent(m_IconSliderRect, worldPositionStays: false);
					rectTransform.localPosition = m_PlayerIcon.localPosition;
					rectTransform.localScale = Vector3.one;
					rectTransform.SetAsLastSibling();
				}
				else
				{
					rectTransform = UnityEngine.Object.Instantiate(m_RivalIcon);
					rectTransform.SetParent(m_IconSliderRect, worldPositionStays: false);
					rectTransform.localPosition = m_RivalIcon.localPosition;
					rectTransform.localScale = Vector3.one;
				}
				rectTransform.transform.name = "RacerIcon-" + registeredRacer.RacerName;
				rectTransform.gameObject.SetActive(value: true);
				m_Racers.Add(new ProgressRacers(rectTransform, registeredRacer));
			}
			if (m_LapSeperator != null)
			{
				for (int i = 1; (float)i < m_RaceLaps; i++)
				{
					RectTransform rectTransform2 = UnityEngine.Object.Instantiate(m_LapSeperator);
					rectTransform2.SetParent(m_LapSeperator.parent, worldPositionStays: false);
					rectTransform2.name = "LapSeperator-Lap" + i;
					rectTransform2.localPosition = m_LapSeperator.localPosition;
					rectTransform2.localScale = Vector3.one;
					rectTransform2.gameObject.SetActive(value: true);
					Vector2 zero = Vector2.zero;
					Vector2 one = Vector2.one;
					float num2 = zero[0] = (one[0] = GetNormalizedValue(100f - m_RaceLenght * (float)i * m_ProgressResolution));
					rectTransform2.anchorMin = zero;
					rectTransform2.anchorMax = one;
				}
			}
		}

		private void Update()
		{
			if (Application.isPlaying && m_RaceManager != null && m_RaceManager.IsRaceStarted)
			{
				UpdateVisuals();
			}
		}

		public float GetNormalizedValue(float Value)
		{
			if (Mathf.Approximately(m_MinValue, m_MaxValue))
			{
				return 0f;
			}
			return Mathf.InverseLerp(m_MinValue, m_MaxValue, Value);
		}

		public virtual void GraphicUpdateComplete()
		{
		}

		public virtual void LayoutComplete()
		{
		}

		protected override void OnDisable()
		{
			base.OnDisable();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateCachedReferences();
			UpdateVisuals();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (IsActive())
			{
				UpdateVisuals();
			}
		}

		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing != 0)
			{
			}
		}

		private void UpdateCachedReferences()
		{
			if (!m_PlayerProgressFill)
			{
				m_FillContainerRect = null;
				m_FillImage = null;
			}
			else
			{
				m_FillTransform = m_PlayerProgressFill.transform;
				m_FillImage = m_PlayerProgressFill.GetComponent<Image>();
				if (m_FillTransform.parent != null)
				{
					m_FillContainerRect = m_FillTransform.parent.GetComponent<RectTransform>();
				}
			}
			if (!m_PlayerIcon)
			{
				m_IconSliderRect = null;
				return;
			}
			m_HandleTransform = m_PlayerIcon.transform;
			if (m_HandleTransform.parent != null)
			{
				m_IconSliderRect = m_HandleTransform.parent.GetComponent<RectTransform>();
			}
		}

		private void UpdateVisuals()
		{
			if (!Application.isPlaying)
			{
				UpdateCachedReferences();
				if (m_IconSliderRect != null && m_Racers != null)
				{
					foreach (ProgressRacers racer in m_Racers)
					{
						if (racer.RacerIcon != null)
						{
							if (racer.IsPlayer && m_FillContainerRect != null)
							{
								Vector2 zero = Vector2.zero;
								Vector2 one = Vector2.one;
								if (m_FillImage != null && m_FillImage.type == Image.Type.Filled)
								{
									m_FillImage.fillAmount = GetNormalizedValue(racer.ProgressValue);
								}
								else
								{
									one[0] = GetNormalizedValue(racer.ProgressValue);
								}
								m_PlayerProgressFill.anchorMin = zero;
								m_PlayerProgressFill.anchorMax = one;
							}
							Vector2 zero2 = Vector2.zero;
							Vector2 one2 = Vector2.one;
							float num2 = zero2[0] = (one2[0] = GetNormalizedValue(racer.ProgressValue));
							racer.RacerIcon.anchorMin = zero2;
							racer.RacerIcon.anchorMax = one2;
						}
					}
				}
			}
			else
			{
				UpdateRuntimeVisuals();
			}
		}

		private void UpdateRuntimeVisuals()
		{
			if (m_IconSliderRect != null && m_Racers != null)
			{
				foreach (ProgressRacers racer in m_Racers)
				{
					if (racer.RacerIcon != null)
					{
						float normalizedValue = GetNormalizedValue(100f - racer.RacerDetail.RacerDistance * m_ProgressResolution);
						if (racer.RacerDetail.IsPlayer)
						{
							racer.IsPlayer = true;
							if (m_FillContainerRect != null)
							{
								Vector2 zero = Vector2.zero;
								Vector2 one = Vector2.one;
								if (m_FillImage != null && m_FillImage.type == Image.Type.Filled)
								{
									m_FillImage.fillAmount = normalizedValue;
								}
								else
								{
									one[0] = normalizedValue;
								}
								m_PlayerProgressFill.anchorMin = zero;
								m_PlayerProgressFill.anchorMax = one;
							}
						}
						else if (racer.RacerDetail.RacerDestroyed && !racer.IsDestroyed)
						{
							racer.RacerIcon.GetComponent<Image>().sprite = m_DestroyedRivalIconSprite;
							racer.IsDestroyed = true;
						}
						Vector2 zero2 = Vector2.zero;
						Vector2 one2 = Vector2.one;
						racer.ProgressValue = normalizedValue;
						one2[0] = normalizedValue;
						zero2[0] = normalizedValue;
						racer.RacerIcon.anchorMin = zero2;
						racer.RacerIcon.anchorMax = one2;
					}
				}
			}
		}
	}
}
