/*
 * Enhancer.cs
 * Created by: Ambrosia
 */

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Enhancer : MonoBehaviour
{
	[Header("First Trigger Variables")]

	[SerializeField] bool _FirstTrigger = false;
	// Chromatic Aberration Film Grain
	[SerializeField] Volume _CAFGVolume;
	[SerializeField] bool _Enabling = true;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (_FirstTrigger)
			{
				_CAFGVolume.profile.TryGet(out ChromaticAberration ca);
				ca.active = _Enabling;
				_CAFGVolume.profile.TryGet(out FilmGrain fg);
				fg.active = _Enabling;

				_CAFGVolume.profile.TryGet(out WhiteBalance wb);
				wb.active = !_Enabling;
				_CAFGVolume.profile.TryGet(out LensDistortion ld);
				ld.active = !_Enabling;
				_CAFGVolume.profile.TryGet(out LiftGammaGain lgg);
				lgg.active = !_Enabling;

				Destroy(gameObject);
			}
		}
	}
}
