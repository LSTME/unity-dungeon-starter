using System.Collections;
using System.Collections.Generic;
using Scripts.Map;
using UnityEngine;

namespace Scripts.Controllers
{
	public class MultiLightFlickerController : AbstractGameObjectController, Scripts.Interfaces.ISwitchable
    {
    
    	public List<Light> Lights;
    	
    	public GameObject TorchFlame;
    
    	public float BaseIntensity = 3f;
    	public float MaxReduction = 0.75f;
    	public float MaxIncrease = 0.3f;
    	public float RateDamping = 0.05f;
    	public float Strength = 20.0f;
    	
    	private bool Enabled = true;
    	
    	private bool initialized = false;
    	
    	void Start()
    	{
    		UpdateLight();
    		StartCoroutine(FlickerIntensity());
    	}
    
    	private void Update()
    	{
    		Initialize();
    	}
    
    	private void Initialize()
    	{
    		if (initialized) return;
		    
    		if (ObjectConfig == null) return;
    		if (ObjectConfig.Torch == null) return;
    
    		Enabled = ObjectConfig.Torch.State;
    		
    		Debug.Log(Enabled);
    		Debug.Log(ObjectConfig.Torch.State);
    		
    
    		UpdateLight();
    
    		initialized = true;
    	}
    	
    	private void UpdateLight()
    	{
    		TorchFlame.SetActive(Enabled);
    		foreach (var light in Lights)
    		{
    			light.enabled = Enabled;
    		}
    	}
    	
    	IEnumerator FlickerIntensity()
    	{
    		while (true)
    		{
    			foreach (var light in Lights)
    			{
    				var intensity = Random.Range(BaseIntensity - MaxReduction, BaseIntensity + MaxIncrease);
    				
    				light.intensity = Mathf.Lerp(light.intensity, intensity, Strength * Time.deltaTime);
    			}
    
    			yield return new WaitForSeconds(RateDamping);
    		}
    	}
    
    	public void ActionSwitchOn(string target)
    	{
    		if (ObjectConfig == null) return;
    		if (ObjectConfig.Name == null || !ObjectConfig.Name.Equals(target)) return;
    
    		Enabled = true;
    		UpdateLight();
    	}
    
    	public void ActionSwitchOff(string target)
    	{
    		if (ObjectConfig == null) return;
    		if (ObjectConfig.Name == null || !ObjectConfig.Name.Equals(target)) return;
    
    		Enabled = false;
    		UpdateLight();
    	}
    }
}


