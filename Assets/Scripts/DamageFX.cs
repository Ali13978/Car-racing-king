using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Racing Game Kit/DamageFX/DamageFX Manager")]
public class DamageFX : MonoBehaviour
{
	[Serializable]
	public class DamageReceiverData
	{
		public MeshFilter ReceiverMesh;

		public bool VisualizeDamage = true;

		public bool Detachable;

		public float Health = 100f;

		public float MassAfterDetach = 1f;

		public bool SelfDestroy;

		public float DestroyAfter = 5f;

		public bool OverrideDeform;

		public float MaxDeform;

		public float DeformManipulator;

		public bool Detached;
	}

	[Serializable]
	public class DamageFXData
	{
		public float MaxImpactPower = 10f;

		public float DeformRadius = 0.3f;

		public float DeformManipulator = 1f;

		public float MaxDeform = 0.3f;

		public float VerticalDeformBias = 0.1f;
	}

	[Serializable]
	public class DamageFXAudioData
	{
		public bool EnableAudio = true;

		public AudioRolloffMode RolloffMode;

		public float MinDistance = 1f;

		public float MaxDistance = 100f;

		public float MaxVolume = 1f;

		public float Spatial = 1f;

		public AudioClip TapCollisionAudio;

		public float LightCollisionValue = 15f;

		public AudioClip LightCollisionAudio;

		public float MedCollisionValue = 25f;

		public AudioClip MedCollisionAudio;

		public float HeavyCollisionValue = 35f;

		public AudioClip HighCollisionAudio;
	}

	[Serializable]
	public class DamageFXParticleData
	{
		public bool EnableParticleEffects = true;

		public bool GetFromScene;

		public ParticleSystem ImpactSparks;

		public ParticleSystem BrokenEngineSmoke;
	}

	[Serializable]
	public class DamageFXTextureData
	{
		public bool EnableTextureEffects;

		public GameObject ReferenceObject;

		public bool CheckEveryMaterialHaveProperty;

		public string MaterialPropertyName;
	}

	[Serializable]
	public class HealthData
	{
		public float VehicleHealth = 100f;

		public float ImpactToHealthPercent = 100f;

		public float EngineSmokePercent = 20f;
	}

	private struct CachedMeshData
	{
		public DamageReceiverData OriginalData;

		public Vector3[] OriginalVerts;
	}

	public DamageFXData Options;

	public HealthData Health;

	public LayerMask DamageIgnoreLayer;

	public bool FindReceiversOnStart;

	public bool FindExcludedsOnStart;

	public bool VisualizeAsDefault = true;

	public Transform RootTransformForReceivers;

	public Transform RootTransformForExcludeds;

	public List<DamageReceiverData> DamageReceivers;

	public List<DamageReceiverData> ExcludedReceivers;

	public DamageFXAudioData ImpactSounds;

	public DamageFXParticleData ParticleEffects;

	public DamageFXTextureData TextureEffects;

	private List<Material> CachedMaterials;

	[HideInInspector]
	public bool DebugStopOnCollision;

	[HideInInspector]
	public float LastImpactValue;

	public float RepairSpeed = 5f;

	private float ImpactRangeSquared;

	private List<MeshFilter> DamageReceiverMeshes;

	private CachedMeshData[] CachedMeshDataForRepair;

	private AudioSource AudioChannelImpact;

	private ParticleSystem m_Sparks;

	private ParticleSystem m_Smoke;

	private float healthOrig;

	private bool isEngineSmoking;

	private float RepairThreashold = 0.001f;

	private bool InitRepair;

	private bool IsRepaired = true;

	public bool DontApplyDamages;

	public bool dfxEdVarsImpact = true;

	public bool dfxEdVarsHealth = true;

	public bool dfxEdVarsParticle = true;

	public bool dfxEdVarsTexture = true;

	public bool dfxEdVarAudio = true;

	public bool dfxEdVarsReceivers = true;

	public bool dfxEdVarsReceiversLegends;

	public bool dfxEdVarReceiversAdvanced;

	public bool dfxEdVarsDebug;

	private void Start()
	{
		if (!base.enabled)
		{
			return;
		}
		healthOrig = Health.VehicleHealth;
		ImpactRangeSquared = Options.DeformRadius * Options.DeformRadius;
		if (FindExcludedsOnStart)
		{
			InitExcludedFiltersOnStart();
		}
		if (!FindReceiversOnStart)
		{
			InitMeshFilters();
		}
		else
		{
			if (DamageReceivers != null)
			{
				DamageReceivers.Clear();
			}
			InitMeshFiltersOnStart();
		}
		CreateCacheOfMeshData();
		InitSounds();
		InitParticles();
		InitTextures();
	}

	private void LateUpdate()
	{
		if (Health.VehicleHealth <= Health.EngineSmokePercent / 100f * healthOrig)
		{
			isEngineSmoking = true;
		}
		else
		{
			isEngineSmoking = false;
		}
		if (m_Smoke != null)
		{
			if (isEngineSmoking && !m_Smoke.isPlaying)
			{
				m_Smoke.Play();
			}
			else if (!isEngineSmoking && m_Smoke.isPlaying)
			{
				m_Smoke.Stop();
			}
		}
	}

	private void Update()
	{
		ProcessRepair();
	}

	private void InitMeshFilters()
	{
		if (DamageReceivers.Count > 0)
		{
			DamageReceiverMeshes = new List<MeshFilter>(DamageReceivers.Count);
			foreach (DamageReceiverData damageReceiver in DamageReceivers)
			{
				if (damageReceiver.ReceiverMesh != null)
				{
					DamageReceiverMeshes.Add(damageReceiver.ReceiverMesh);
				}
			}
		}
	}

	public void InitExcludedFiltersOnStart()
	{
		if (!(RootTransformForExcludeds == null))
		{
			MeshFilter[] componentsInChildren = RootTransformForExcludeds.GetComponentsInChildren<MeshFilter>();
			ExcludedReceivers = new List<DamageReceiverData>();
			MeshFilter[] array = componentsInChildren;
			foreach (MeshFilter receiverMesh in array)
			{
				DamageReceiverData damageReceiverData = new DamageReceiverData();
				damageReceiverData.VisualizeDamage = false;
				damageReceiverData.ReceiverMesh = receiverMesh;
				ExcludedReceivers.Add(damageReceiverData);
			}
		}
	}

	public void InitMeshFiltersOnStart()
	{
		if (!(RootTransformForReceivers == null))
		{
			MeshFilter[] componentsInChildren = RootTransformForReceivers.GetComponentsInChildren<MeshFilter>();
			DamageReceiverMeshes = new List<MeshFilter>(componentsInChildren.Length);
			DamageReceiverMeshes.AddRange(componentsInChildren);
			if (DamageReceiverMeshes.Count > 0 && ExcludedReceivers.Count > 0)
			{
				foreach (DamageReceiverData excludedReceiver in ExcludedReceivers)
				{
					if (excludedReceiver.ReceiverMesh != null)
					{
						DamageReceiverMeshes.Remove(excludedReceiver.ReceiverMesh);
					}
				}
			}
			DamageReceivers = new List<DamageReceiverData>();
			foreach (MeshFilter damageReceiverMesh in DamageReceiverMeshes)
			{
				DamageReceiverData damageReceiverData = new DamageReceiverData();
				if (!VisualizeAsDefault)
				{
					damageReceiverData.VisualizeDamage = false;
				}
				damageReceiverData.ReceiverMesh = damageReceiverMesh;
				DamageReceivers.Add(damageReceiverData);
			}
		}
	}

	private void CreateCacheOfMeshData()
	{
		CachedMeshDataForRepair = new CachedMeshData[DamageReceivers.Count];
		for (int i = 0; i < DamageReceiverMeshes.Count; i++)
		{
			CachedMeshDataForRepair[i].OriginalData = DamageReceivers[i];
			CachedMeshDataForRepair[i].OriginalVerts = DamageReceivers[i].ReceiverMesh.mesh.vertices;
		}
	}

	public void DoRepair()
	{
		InitRepair = true;
	}

	private void ProcessRepair()
	{
		if (IsRepaired || !InitRepair || !(RepairSpeed > 0f))
		{
			return;
		}
		IsRepaired = true;
		for (int i = 0; i < DamageReceivers.Count; i++)
		{
			DamageReceivers[i].Health = CachedMeshDataForRepair[i].OriginalData.Health;
			DamageReceivers[i].Detached = false;
			Vector3[] vertices = DamageReceivers[i].ReceiverMesh.mesh.vertices;
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] += (CachedMeshDataForRepair[i].OriginalVerts[j] - vertices[j]) * (Time.deltaTime * (10.1f - RepairSpeed));
				if ((CachedMeshDataForRepair[i].OriginalVerts[j] - vertices[j]).magnitude >= RepairThreashold)
				{
					IsRepaired = false;
				}
			}
			DamageReceivers[i].ReceiverMesh.mesh.vertices = vertices;
			DamageReceivers[i].ReceiverMesh.mesh.RecalculateNormals();
			DamageReceivers[i].ReceiverMesh.mesh.RecalculateBounds();
			DamageReceivers[i].ReceiverMesh.gameObject.SetActive(value: true);
			DamageReceivers[i].Health = 100f;
		}
		Health.VehicleHealth = healthOrig;
		ApplyTextureEffects(Health.VehicleHealth);
		if (IsRepaired)
		{
			InitRepair = false;
		}
	}

	private void InitSounds()
	{
		GameObject gameObject = new GameObject("audio_impact");
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.AddComponent(typeof(AudioSource));
		AudioChannelImpact = gameObject.GetComponent<AudioSource>();
		AudioChannelImpact.loop = false;
		AudioChannelImpact.playOnAwake = false;
		AudioChannelImpact.rolloffMode = ImpactSounds.RolloffMode;
		AudioChannelImpact.minDistance = ImpactSounds.MinDistance;
		AudioChannelImpact.maxDistance = ImpactSounds.MaxDistance;
		AudioChannelImpact.volume = ImpactSounds.MaxVolume;
	}

	public void UpdateImpactSoundsVolume(float Volume)
	{
		ImpactSounds.MaxVolume = Volume;
		AudioChannelImpact.volume = Volume;
	}

	private void InitParticles()
	{
		if (ParticleEffects.GetFromScene)
		{
			GameObject gameObject = GameObject.Find("_DamageFXParticles");
			if (gameObject != null)
			{
				Transform transform = gameObject.transform.Find("DamageFX_EngineSmoke");
				if (transform != null)
				{
					ParticleEffects.BrokenEngineSmoke = transform.GetComponent<ParticleSystem>();
				}
				Transform transform2 = gameObject.transform.Find("DamageFX_Sparks");
				if (transform2 != null)
				{
					ParticleEffects.ImpactSparks = transform2.GetComponent<ParticleSystem>();
				}
			}
		}
		if (ParticleEffects.ImpactSparks != null)
		{
			m_Sparks = UnityEngine.Object.Instantiate(ParticleEffects.ImpactSparks);
			m_Sparks.transform.parent = base.transform;
			m_Sparks.transform.localRotation = Quaternion.identity;
			m_Sparks.transform.localPosition = Vector3.zero;
		}
		if (ParticleEffects.BrokenEngineSmoke != null)
		{
			Transform transform3 = base.transform.Find("_EngineSmokePoint");
			if (transform3 != null)
			{
				m_Smoke = UnityEngine.Object.Instantiate(ParticleEffects.BrokenEngineSmoke);
				m_Smoke.transform.parent = transform3;
				m_Smoke.transform.localRotation = Quaternion.identity;
				m_Smoke.transform.localPosition = Vector3.zero;
			}
			else
			{
				UnityEngine.Debug.LogWarning("DamageFX Error : BrokenEngineSmoke particle assigned but \"_EngineSmokePoint\" not found! Please check your vehicle configuration.");
			}
		}
	}

	private void InitTextures()
	{
		if (TextureEffects.EnableTextureEffects)
		{
			CachedMaterials = new List<Material>();
			CacheMaterials();
			ApplyTextureEffects(Health.VehicleHealth);
		}
	}

	private void PlayAudio(float ImpactValue)
	{
		if (ImpactValue >= ImpactSounds.HeavyCollisionValue)
		{
			PlayAudioFile(ImpactSounds.HighCollisionAudio);
		}
		else if (ImpactValue > ImpactSounds.MedCollisionValue && ImpactValue < ImpactSounds.HeavyCollisionValue)
		{
			PlayAudioFile(ImpactSounds.MedCollisionAudio);
		}
		else if (ImpactValue > ImpactSounds.LightCollisionValue && ImpactValue <= ImpactSounds.MedCollisionValue)
		{
			PlayAudioFile(ImpactSounds.LightCollisionAudio);
		}
		else if (ImpactValue <= ImpactSounds.LightCollisionValue)
		{
			PlayAudioFile(ImpactSounds.TapCollisionAudio);
		}
	}

	private void PlayAudioFile(AudioClip AudioForPlay)
	{
		if (AudioForPlay != null)
		{
			AudioChannelImpact.PlayOneShot(AudioForPlay);
		}
	}

	private bool IsInLayerMask(GameObject CollisionObject, LayerMask layerMask)
	{
		int num = 1 << CollisionObject.layer;
		if ((layerMask.value & num) > 0)
		{
			return true;
		}
		return false;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!base.enabled || IsInLayerMask(collision.gameObject, DamageIgnoreLayer))
		{
			return;
		}
		float num = LastImpactValue = collision.relativeVelocity.magnitude;
		PlayAudio(LastImpactValue);
		if (DontApplyDamages)
		{
			return;
		}
		Health.VehicleHealth -= LastImpactValue * Health.ImpactToHealthPercent / 100f;
		if (Health.VehicleHealth <= 0f)
		{
			Health.VehicleHealth = 0f;
		}
		ApplyTextureEffects(Health.VehicleHealth);
		if (DamageReceiverMeshes != null)
		{
			if (DamageReceiverMeshes.Count > 0)
			{
				Vector3 relativeVelocity = collision.relativeVelocity;
				relativeVelocity.y *= Options.VerticalDeformBias;
				Vector3 vector = base.transform.position - collision.contacts[0].point;
				float num2 = relativeVelocity.magnitude * Vector3.Dot(collision.contacts[0].normal, vector.normalized);
				OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(num2 / Options.MaxImpactPower));
			}
			IsRepaired = false;
		}
	}

	private void OnCollisionStay(Collision collInfo)
	{
		if (!(m_Sparks != null))
		{
			return;
		}
		float magnitude = collInfo.relativeVelocity.magnitude;
		if (magnitude > 2f)
		{
			ContactPoint[] contacts = collInfo.contacts;
			for (int i = 0; i < contacts.Length; i++)
			{
				ContactPoint contactPoint = contacts[i];
				m_Sparks.Emit(2);
				ParticleSystem.EmitParams emitParams = default(ParticleSystem.EmitParams);
				emitParams.position = contactPoint.point;
				emitParams.velocity = collInfo.relativeVelocity / magnitude;
				m_Sparks.Emit(emitParams, 2);
			}
		}
	}

	private void OnMeshForce(Vector4 originPosAndForce)
	{
		if (base.enabled)
		{
			OnMeshForce(originPosAndForce, originPosAndForce.w);
		}
	}

	private void OnMeshForce(Vector3 originPos, float force)
	{
		if (!base.enabled)
		{
			return;
		}
		force = Mathf.Clamp01(force);
		for (int i = 0; i < DamageReceiverMeshes.Count; i++)
		{
			Vector3[] vertices = DamageReceiverMeshes[i].mesh.vertices;
			DamageReceiverData damageReceiverData = null;
			bool flag = false;
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 point = Vector3.Scale(vertices[j], base.transform.localScale);
				Vector3 vector = DamageReceiverMeshes[i].transform.position + DamageReceiverMeshes[i].transform.rotation * point;
				Vector3 a = vector - originPos;
				Vector3 b = base.transform.position - vector;
				b.y = 0f;
				if (!(a.sqrMagnitude < ImpactRangeSquared))
				{
					continue;
				}
				Vector3 vector2 = new Vector3(0f, 0f, 0f);
				if (!flag)
				{
					damageReceiverData = ApplyHealth(DamageReceiverMeshes[i], LastImpactValue);
				}
				if (damageReceiverData != null)
				{
					if (damageReceiverData.VisualizeDamage)
					{
						float num = Mathf.Clamp01(a.sqrMagnitude / ImpactRangeSquared);
						if (damageReceiverData.OverrideDeform)
						{
							float d = force * (1f - num) * damageReceiverData.MaxDeform;
							vector2 = Vector3.Slerp(a, b, damageReceiverData.DeformManipulator).normalized * d;
						}
						else
						{
							float d2 = force * (1f - num) * Options.MaxDeform;
							vector2 = Vector3.Slerp(a, b, Options.DeformManipulator).normalized * d2;
						}
						vertices[j] += Quaternion.Inverse(base.transform.rotation) * vector2;
					}
					flag = true;
				}
				else
				{
					float num2 = Mathf.Clamp01(a.sqrMagnitude / ImpactRangeSquared);
					float d3 = force * (1f - num2) * Options.MaxDeform;
					vector2 = Vector3.Slerp(a, b, Options.DeformManipulator).normalized * d3;
					vertices[j] += Quaternion.Inverse(base.transform.rotation) * vector2;
				}
				if (DebugStopOnCollision)
				{
					UnityEngine.Debug.DrawRay(vector, vector2, Color.red);
					UnityEngine.Debug.Break();
				}
			}
			DamageReceiverMeshes[i].mesh.vertices = vertices;
			DamageReceiverMeshes[i].mesh.RecalculateBounds();
		}
	}

	private void CleanUpDetached()
	{
		foreach (DamageReceiverData damageReceiver in DamageReceivers)
		{
			if (damageReceiver.ReceiverMesh != null && damageReceiver.Health <= 0f && damageReceiver.Detachable)
			{
				DamageReceiverMeshes.Remove(damageReceiver.ReceiverMesh);
			}
		}
	}

	public void DetachAllDetachables(bool shouldDestroy, float DestroyAfter)
	{
		foreach (DamageReceiverData damageReceiver in DamageReceivers)
		{
			if (damageReceiver.ReceiverMesh != null && damageReceiver.Detachable)
			{
				DetachMesh(damageReceiver.ReceiverMesh, shouldDestroy, DestroyAfter);
			}
		}
	}

	private DamageReceiverData ApplyHealth(MeshFilter Mesh, float Damage)
	{
		DamageReceiverData result = null;
		foreach (DamageReceiverData damageReceiver in DamageReceivers)
		{
			if (damageReceiver.ReceiverMesh != null && damageReceiver.ReceiverMesh == Mesh)
			{
				damageReceiver.Health -= Damage;
				result = damageReceiver;
				if (damageReceiver.Health <= 0f && damageReceiver.Detachable && !damageReceiver.Detached)
				{
					damageReceiver.Detached = true;
					DetachMesh(Mesh, damageReceiver.SelfDestroy, damageReceiver.DestroyAfter);
				}
				if (damageReceiver.Health < 0f)
				{
					damageReceiver.Health = 0f;
				}
			}
		}
		return result;
	}

	private void DetachMesh(MeshFilter Mesh, bool ShouldDestroy, float DestroyAfter)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Mesh.gameObject, Mesh.transform.position, Mesh.transform.rotation);
		Mesh.gameObject.SetActive(value: false);
		Rigidbody rigidbody = gameObject.gameObject.GetComponent<Rigidbody>();
		if (rigidbody == null)
		{
			rigidbody = (gameObject.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody);
		}
		if (rigidbody != null)
		{
			rigidbody.mass = 1f;
		}
		Collider component = gameObject.gameObject.GetComponent<BoxCollider>();
		if (component == null)
		{
			component = (gameObject.gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider);
		}
		if (ShouldDestroy)
		{
			DamageReceiverSelfDestroy damageReceiverSelfDestroy = gameObject.gameObject.GetComponent<DamageReceiverSelfDestroy>();
			if (damageReceiverSelfDestroy == null)
			{
				damageReceiverSelfDestroy = (gameObject.gameObject.AddComponent(typeof(DamageReceiverSelfDestroy)) as DamageReceiverSelfDestroy);
			}
			if (damageReceiverSelfDestroy != null)
			{
				damageReceiverSelfDestroy.DestroyAfter = DestroyAfter;
				damageReceiverSelfDestroy.StartCountdown = true;
			}
		}
	}

	private void ApplyTextureEffects(float VehicleHealth)
	{
		if (TextureEffects.EnableTextureEffects && CachedMaterials.Count > 0)
		{
			float num = healthOrig - VehicleHealth;
			if (num > 0f)
			{
				num /= healthOrig;
			}
			foreach (Material cachedMaterial in CachedMaterials)
			{
				cachedMaterial.SetFloat(TextureEffects.MaterialPropertyName, num);
			}
		}
	}

	private void CacheMaterials()
	{
		if (TextureEffects.MaterialPropertyName == string.Empty)
		{
			return;
		}
		MeshRenderer[] componentsInChildren = base.transform.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (!(meshRenderer != null))
			{
				continue;
			}
			Material[] materials = meshRenderer.materials;
			Material[] array2 = materials;
			foreach (Material material in array2)
			{
				if (material.HasProperty(TextureEffects.MaterialPropertyName))
				{
					CachedMaterials.Add(material);
				}
			}
		}
	}
}
