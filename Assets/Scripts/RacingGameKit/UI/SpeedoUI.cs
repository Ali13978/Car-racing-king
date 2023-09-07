using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RacingGameKit.UI
{
	[AddComponentMenu("Racing Game Kit/UI/Speedo UI")]
	public class SpeedoUI : UIBehaviour, ICanvasElement
	{
		public bool m_ShowSpeedoNeedle = true;

		public RectTransform m_SpeedoNeedle;

		public float m_SpeedoNeedleMinAngle;

		public float m_SpeedoNeedleMaxAngle;

		public float m_SpeedoMaxValue;

		public bool m_SpeedReversed;

		public bool m_EnableSpeedoFill = true;

		public RectTransform m_SpeedFill;

		public float m_SpeedFillAmountMin;

		public float m_SpeedFillAmountMax = 1f;

		[Space]
		public bool m_ShowRpmNeedle = true;

		public RectTransform m_RpmNeedle;

		public float m_RpmNeedleMinAngle;

		public float m_RpmNeedleMaxAngle;

		public float m_RpmMaxValue;

		public bool m_RpmReversed;

		public bool m_EnableRpmFill = true;

		public RectTransform m_RpmFill;

		public float m_RpmFillAmountMin;

		public float m_RpmFillAmountMax = 1f;

		[Space]
		public bool m_ShowNitroNeedle = true;

		public RectTransform m_NitroNeedle;

		public float m_NitroNeedleMinAngle;

		public float m_NitroNeedleMaxAngle;

		public float m_NitroMaxValue = 1f;

		public bool m_NitroReversed;

		public bool m_EnableNitroFill = true;

		public RectTransform m_NitroFill;

		public float m_NitroFillAmountMin;

		public float m_NitroFillAmountMax = 1f;

		[Space]
		public bool m_ShowRepairNeedle = true;

		public RectTransform m_RepairNeedle;

		public float m_RepairNeedleMinAngle;

		public float m_RepairNeedleMaxAngle;

		public float m_RepairMaxValue = 1f;

		public bool m_RepairReversed;

		public bool m_EnableRepairFill = true;

		public RectTransform m_RepairFill;

		public float m_RepairFillAmountMin;

		public float m_RepairFillAmountMax = 1f;

		[Space]
		public bool m_EnableShiftHelper;

		public RectTransform m_ShiftHelperControl;

		public Sprite m_ShiftHelperNaturalSprite;

		public Sprite m_ShiftHelperSweetSprite;

		public Sprite m_ShiftHelperOverRevSprite;

		public Vector2 m_ShiftHelperSweetRpmRange = new Vector2(6001f, 7000f);

		public Vector2 m_ShiftHelperOverRevRpmRange = new Vector2(7001f, 9000f);

		[Space]
		public Text m_SpeedText;

		public Text m_RpmText;

		public Text m_GearText;

		[Space]
		[SerializeField]
		private float m_Speed;

		[SerializeField]
		private float m_Rpm;

		[SerializeField]
		private string m_Gear;

		[SerializeField]
		private float m_Nitro;

		[SerializeField]
		private float m_Repair;

		private float m_SpeedoTotalMove;

		private float m_SpeedoResolution;

		private float m_SpeedoResolutionFill;

		private float m_RpmResolutionFill;

		private float m_RpmTotalMove;

		private float m_RpmResolution;

		private float m_NitroResolutionFill;

		private float m_NitroTotalMove;

		private float m_NitroResolution;

		private float m_RepairResolutionFill;

		private float m_RepairTotalMove;

		private float m_RepairResolution;

		private Image m_CachedSpeedFill;

		private Image m_CachedRpmFill;

		private Image m_CachedNitroFill;

		private Image m_CachedRepairFill;

		private Image m_CachedShiftHelper;

		public bool m_EdVars_Control = true;

		public bool m_EdVars_Speedo = true;

		public bool m_EdVars_Rpm = true;

		public bool m_EdVars_Nitro = true;

		public bool m_EdVars_Repair = true;

		public bool m_EdVars_Shift = true;

		public new Transform transform => transform;

		public float Speed
		{
			get
			{
				return m_Speed;
			}
			set
			{
				m_Speed = value;
			}
		}

		public float Rpm
		{
			get
			{
				return m_Rpm;
			}
			set
			{
				m_Rpm = value;
			}
		}

		public float Nitro
		{
			get
			{
				return m_Nitro;
			}
			set
			{
				m_Nitro = value;
			}
		}

		public float Repair
		{
			get
			{
				return m_Repair;
			}
			set
			{
				m_Repair = value;
			}
		}

		public string Gear
		{
			get
			{
				return m_Gear;
			}
			set
			{
				m_Gear = value;
			}
		}

		public new bool IsDestroyed()
		{
			return base.IsDestroyed();
		}

		protected override void Start()
		{
			base.Start();
			UpdateCached();
		}

		private void Update()
		{
			if (Application.isPlaying)
			{
				UpdateVisuals();
			}
		}

		public void UpdateCached()
		{
			if (!m_SpeedReversed)
			{
				m_SpeedoTotalMove = Mathf.Abs(m_SpeedoNeedleMinAngle) + Mathf.Abs(m_SpeedoNeedleMaxAngle);
			}
			else
			{
				m_SpeedoTotalMove = Mathf.Abs(m_SpeedoNeedleMinAngle) - Mathf.Abs(m_SpeedoNeedleMaxAngle);
			}
			m_SpeedoResolution = m_SpeedoTotalMove / m_SpeedoMaxValue;
			m_SpeedoResolutionFill = (m_SpeedFillAmountMax - m_SpeedFillAmountMin) / m_SpeedoMaxValue;
			if (!m_RpmReversed)
			{
				m_RpmTotalMove = Mathf.Abs(m_RpmNeedleMinAngle) + Mathf.Abs(m_RpmNeedleMaxAngle);
			}
			else
			{
				m_RpmTotalMove = Mathf.Abs(m_RpmNeedleMinAngle) - Mathf.Abs(m_RpmNeedleMaxAngle);
			}
			m_RpmResolution = m_RpmTotalMove / m_RpmMaxValue;
			m_RpmResolutionFill = (m_RpmFillAmountMax - m_RpmFillAmountMin) / m_RpmMaxValue;
			if (!m_NitroReversed)
			{
				m_NitroTotalMove = Mathf.Abs(m_NitroNeedleMinAngle) + Mathf.Abs(m_NitroNeedleMaxAngle);
			}
			else
			{
				m_NitroTotalMove = Mathf.Abs(m_NitroNeedleMinAngle) - Mathf.Abs(m_NitroNeedleMaxAngle);
			}
			m_NitroResolution = m_NitroTotalMove / m_NitroMaxValue;
			m_NitroResolutionFill = (m_NitroFillAmountMax - m_NitroFillAmountMin) / m_NitroMaxValue;
			if (!m_RepairReversed)
			{
				m_RepairTotalMove = Mathf.Abs(m_RepairNeedleMinAngle) + Mathf.Abs(m_RepairNeedleMaxAngle);
			}
			else
			{
				m_RepairTotalMove = Mathf.Abs(m_RepairNeedleMinAngle) - Mathf.Abs(m_RepairNeedleMaxAngle);
			}
			m_RepairResolution = m_RepairTotalMove / m_RepairMaxValue;
			m_RepairResolutionFill = (m_RepairFillAmountMax - m_RepairFillAmountMin) / m_RepairMaxValue;
			if (m_SpeedFill != null)
			{
				m_CachedSpeedFill = m_SpeedFill.GetComponent<Image>();
			}
			if (m_RpmFill != null)
			{
				m_CachedRpmFill = m_RpmFill.GetComponent<Image>();
			}
			if (m_NitroFill != null)
			{
				m_CachedNitroFill = m_NitroFill.GetComponent<Image>();
			}
			if (m_RepairFill != null)
			{
				m_CachedRepairFill = m_RepairFill.GetComponent<Image>();
			}
			if (m_ShiftHelperControl != null)
			{
				m_CachedShiftHelper = m_ShiftHelperControl.GetComponent<Image>();
			}
		}

		public void UpdateVisuals()
		{
			if (m_SpeedoNeedle != null && m_ShowSpeedoNeedle)
			{
				float num = m_Speed * m_SpeedoResolution * -1f;
				if (!m_SpeedReversed)
				{
					m_SpeedoNeedle.localRotation = Quaternion.Euler(0f, 0f, num + m_SpeedoNeedleMinAngle);
				}
				else
				{
					m_SpeedoNeedle.localRotation = Quaternion.Euler(0f, 0f, m_SpeedoNeedleMinAngle - num);
				}
				if (!m_SpeedoNeedle.gameObject.activeInHierarchy)
				{
					m_SpeedoNeedle.gameObject.SetActive(value: true);
				}
			}
			else if (m_SpeedoNeedle != null && m_SpeedoNeedle.gameObject.activeInHierarchy)
			{
				m_SpeedoNeedle.gameObject.SetActive(value: false);
			}
			if (m_SpeedFill != null && m_CachedSpeedFill != null && m_EnableSpeedoFill)
			{
				m_CachedSpeedFill.fillAmount = m_SpeedFillAmountMin + m_Speed * m_SpeedoResolutionFill;
				if (!m_SpeedFill.gameObject.activeInHierarchy)
				{
					m_SpeedFill.gameObject.SetActive(value: true);
				}
			}
			else if (m_SpeedFill != null && m_SpeedFill.gameObject.activeInHierarchy)
			{
				m_SpeedFill.gameObject.SetActive(value: false);
			}
			if (m_SpeedText != null)
			{
				m_SpeedText.text = $"{m_Speed:0}";
			}
			if (m_RpmNeedle != null && m_ShowRpmNeedle)
			{
				float num2 = m_Rpm * m_RpmResolution * -1f;
				if (!m_RpmReversed)
				{
					m_RpmNeedle.localRotation = Quaternion.Euler(0f, 0f, num2 + m_RpmNeedleMinAngle);
				}
				else
				{
					m_RpmNeedle.localRotation = Quaternion.Euler(0f, 0f, m_RpmNeedleMinAngle - num2);
				}
				if (!m_RpmNeedle.gameObject.activeInHierarchy)
				{
					m_RpmNeedle.gameObject.SetActive(value: true);
				}
			}
			else if (m_RpmNeedle != null && m_RpmNeedle.gameObject.activeInHierarchy)
			{
				m_RpmNeedle.gameObject.SetActive(value: false);
			}
			if (m_RpmFill != null && m_CachedRpmFill != null && m_EnableRpmFill)
			{
				m_CachedRpmFill.fillAmount = m_RpmFillAmountMin + m_Rpm * m_RpmResolutionFill;
				if (!m_RpmFill.gameObject.activeInHierarchy)
				{
					m_RpmFill.gameObject.SetActive(value: true);
				}
			}
			else if (m_RpmFill != null && m_RpmFill.gameObject.activeInHierarchy)
			{
				m_RpmFill.gameObject.SetActive(value: false);
			}
			if (m_RpmText != null)
			{
				m_RpmText.text = $"{m_Rpm:0}";
			}
			if (m_NitroNeedle != null && m_ShowNitroNeedle)
			{
				float num3 = m_Nitro * m_NitroResolution * -1f;
				if (!m_NitroReversed)
				{
					m_NitroNeedle.localRotation = Quaternion.Euler(0f, 0f, num3 + m_NitroNeedleMinAngle);
				}
				else
				{
					m_NitroNeedle.localRotation = Quaternion.Euler(0f, 0f, m_NitroNeedleMinAngle - num3);
				}
				if (!m_NitroNeedle.gameObject.activeInHierarchy)
				{
					m_NitroNeedle.gameObject.SetActive(value: true);
				}
			}
			else if (m_NitroNeedle != null && m_NitroNeedle.gameObject.activeInHierarchy)
			{
				m_NitroNeedle.gameObject.SetActive(value: false);
			}
			if (m_CachedNitroFill != null && m_CachedNitroFill != null && m_EnableNitroFill)
			{
				m_CachedNitroFill.fillAmount = m_NitroFillAmountMin + m_Nitro * m_NitroResolutionFill;
				if (!m_NitroFill.gameObject.activeInHierarchy)
				{
					m_NitroFill.gameObject.SetActive(value: true);
				}
			}
			else if (m_NitroFill != null && m_NitroFill.gameObject.activeInHierarchy)
			{
				m_NitroFill.gameObject.SetActive(value: false);
			}
			if (m_RepairNeedle != null && m_ShowRepairNeedle)
			{
				float num4 = m_Repair * m_RepairResolution * -1f;
				if (!m_RepairReversed)
				{
					m_RepairNeedle.localRotation = Quaternion.Euler(0f, 0f, num4 + m_RepairNeedleMinAngle);
				}
				else
				{
					m_RepairNeedle.localRotation = Quaternion.Euler(0f, 0f, m_RepairNeedleMinAngle - num4);
				}
				if (!m_RepairNeedle.gameObject.activeInHierarchy)
				{
					m_RepairNeedle.gameObject.SetActive(value: true);
				}
			}
			else if (m_RepairNeedle != null && m_RepairNeedle.gameObject.activeInHierarchy)
			{
				m_RepairNeedle.gameObject.SetActive(value: false);
			}
			if (m_CachedRepairFill != null && m_CachedRepairFill != null && m_EnableRepairFill)
			{
				m_CachedRepairFill.fillAmount = m_RepairFillAmountMin + m_Repair * m_RepairResolutionFill;
				if (!m_RepairFill.gameObject.activeInHierarchy)
				{
					m_RepairFill.gameObject.SetActive(value: true);
				}
			}
			else if (m_RepairFill != null && m_RepairFill.gameObject.activeInHierarchy)
			{
				m_RepairFill.gameObject.SetActive(value: false);
			}
			if (m_GearText != null)
			{
				m_GearText.text = m_Gear;
			}
			if (m_CachedShiftHelper != null && m_EnableShiftHelper)
			{
				if (m_Rpm > m_ShiftHelperSweetRpmRange.x && m_Rpm <= m_ShiftHelperSweetRpmRange.y)
				{
					m_CachedShiftHelper.sprite = m_ShiftHelperSweetSprite;
				}
				else if (m_Rpm >= m_ShiftHelperOverRevRpmRange.x && m_Rpm < m_ShiftHelperOverRevRpmRange.y)
				{
					m_CachedShiftHelper.sprite = m_ShiftHelperOverRevSprite;
				}
				else
				{
					m_CachedShiftHelper.sprite = m_ShiftHelperNaturalSprite;
				}
				if (!m_CachedShiftHelper.gameObject.activeInHierarchy)
				{
					m_CachedShiftHelper.gameObject.SetActive(value: true);
				}
			}
			else if (m_CachedShiftHelper != null && m_CachedShiftHelper.gameObject.activeInHierarchy)
			{
				m_CachedShiftHelper.gameObject.SetActive(value: false);
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			UpdateCached();
			UpdateVisuals();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			base.OnRectTransformDimensionsChange();
			if (IsActive())
			{
			}
		}

		public virtual void Rebuild(CanvasUpdate executing)
		{
			if (executing != 0)
			{
			}
		}

		public void GraphicUpdateComplete()
		{
		}

		public void LayoutComplete()
		{
		}
	}
}
