using UnityEngine;
using System.Collections;

[AddComponentMenu("Image Effects/UChromaKey")]
public class UChromaKey : MonoBehaviour {

	public bool imageEffect = true;
	public bool threeColors = false;
	public Color SelectedColor;
	public Color SelectedColor2;
	public Color SelectedColor3;
	public float Range = 0.1f;
	public float HueRange = 0.1f;
	public float opacity = 1;
	public float edgeSharpness = 20;
	public bool shaderModel2 = false;
	public Vector2 uvShift = Vector2.zero;
	public Vector2 uvCoef = Vector2.one;
	public Vector4 crop;
	public bool flipHorizontal;
	public bool flipVertical;

	public bool autoSetDevice;


	public Texture chromaKeyTexture;

	private bool oldShaderModel;
	private bool old3Colors;
	private Material curMaterial;
	public WebCamTexture webCamTexture;
	[SerializeField]
	private string devName;
	public bool devicesExist = false;

	[SerializeField]
	private ChromaKeySource srcType;


	public enum ChromaKeySource
	{
		Device = 0,
		Texture = 1
	}

	protected static Material alphaMat
	{
		get
		{
			if (_alphaMat == null)
				_alphaMat = new Material(Shader.Find("Hidden/UChromaKeyAlpha"));
			return _alphaMat;
		}
	}
	protected static Material _alphaMat;

	Material material
	{
		get
		{
			if(curMaterial == null || oldShaderModel != shaderModel2 || old3Colors != threeColors)
			{
				if(curMaterial != null)
				{
					DestroyImmediate(curMaterial);
				}
				if (shaderModel2)
					curMaterial = new Material(Shader.Find("Hidden/UChromaKey_mobile"));
				else
				{
					if (!threeColors)
					{						
						curMaterial = new Material(Shader.Find("Hidden/UChromaKey"));
					}
					else
					{						
						curMaterial = new Material(Shader.Find("Hidden/UChromaKey_3colors"));
					}
				}
				curMaterial.hideFlags = HideFlags.HideAndDontSave;	
				oldShaderModel = shaderModel2;
				old3Colors = threeColors;
			}
			return curMaterial;
		}
	}

	public string DeviceName
	{
		get
		{
			return devName;
		}
		set
		{
			if (value != devName)
			{
				devName = value;
				if (srcType == ChromaKeySource.Device && Application.isPlaying)
					SetTexture();	
			}
		}
	}

	public ChromaKeySource SourceType
	{
		get
		{
			return srcType;
		}
		set
		{
			if (value != srcType)
			{
				srcType = value;
				if (srcType == ChromaKeySource.Texture && Application.isPlaying && webCamTexture != null && webCamTexture.isPlaying)
					webCamTexture.Stop();
				if (srcType == ChromaKeySource.Device && Application.isPlaying)
				{
					SetTexture();
					if (!webCamTexture.isPlaying)
						webCamTexture.Play();
				}
			}
		}
	}

	[ContextMenu("SetFirstDevice")]
	public void SetFirstDevice()
	{
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > 0)
			DeviceName = devices[0].name;
		else
			Debug.LogError("No devices found.");
	}

	public static void ChromaKeyAlphaBlit(Texture2D input, RenderTexture output, Color color, float range, float hueRange, float opacity = 1.0f, float edgeSharpness = 20.0f)
	{
		ChromaKeyAlphaBlit(input, output, color, range, hueRange, opacity , edgeSharpness, Vector2.zero, Vector2.one, false, false, Vector4.zero);
	}

	public static void ChromaKeyAlphaBlit(Texture2D input, RenderTexture output, Color color, float range, float hueRange, float opacity , float edgeSharpness, Vector2 shift, Vector2 multiplier,  bool flipH, bool flipV, Vector4 crop)
	{
		Material mat = alphaMat;
		SetShiftAndMultiplier(mat, shift, multiplier, flipH, flipV);
		mat.SetColor("_CKCol", color);
		mat.SetFloat("_Range", range);
		mat.SetFloat("_HueRange", hueRange);
		mat.SetFloat("_Opacity", opacity);
		mat.SetFloat("_EdgeSharp", edgeSharpness);
		mat.SetVector("_Crop", crop);
		Graphics.Blit(input,output,mat);
	}

	public static void SetShiftAndMultiplier(Material mat, Vector2 shift, Vector2 multiplier,  bool flipH, bool flipV)
	{
		if (flipH)
		{
			mat.SetFloat("_uvDefX",1.0f + shift.x);
			mat.SetFloat("_uvCoefX",-1.0f / multiplier.x);
		}
		else
		{
			mat.SetFloat("_uvDefX",0.0f - shift.x);
			mat.SetFloat("_uvCoefX",1.0f / multiplier.x);
		}
		if (flipV)
		{
			mat.SetFloat("_uvDefY",1.0f + shift.y);
			mat.SetFloat("_uvCoefY",-1.0f / multiplier.y);
		}
		else
		{
			mat.SetFloat("_uvDefY",0.0f - shift.y);
			mat.SetFloat("_uvCoefY",1.0f / multiplier.y);
		}
	}



	public void SetTexture()
	{
		if (webCamTexture != null && webCamTexture.isPlaying)
		{
			webCamTexture.Stop();
		}
		if (webCamTexture != null)
			webCamTexture.deviceName = devName;
		else
			webCamTexture = new WebCamTexture(devName);
		webCamTexture.Play();
	}


	void OnRenderImage (RenderTexture sourceTexture, RenderTexture destTexture) //sourceTexture is the source texture, destTexture is the final image that gets to the screen
	{
		switch (srcType)
		{
		case ChromaKeySource.Device:
			Shader.SetGlobalTexture("_UChromaKeyTex", webCamTexture);
			break;
		case ChromaKeySource.Texture:
			Shader.SetGlobalTexture("_UChromaKeyTex", chromaKeyTexture);
			break;
		}

		Shader.SetGlobalVector("_Crop",crop);

		if (flipHorizontal)
		{
			Shader.SetGlobalFloat("_uvDefX",1.0f + uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",-1.0f / uvCoef.x);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefX",0.0f - uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",1.0f / uvCoef.x);
		}
		if (flipVertical)
		{
			Shader.SetGlobalFloat("_uvDefY",1.0f + uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",-1.0f / uvCoef.y);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefY",0.0f - uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",1.0f / uvCoef.y);
		}

		if (imageEffect)
		{
			material.SetColor("_PatCol",SelectedColor);
			if (threeColors)
			{
				material.SetColor("_PatCol2",SelectedColor2);
				material.SetColor("_PatCol3",SelectedColor3);
			}
			material.SetFloat("_Range", Range);
			if (!shaderModel2)
				material.SetFloat("_HueRange", HueRange);
			material.SetFloat("_opacity",opacity);
			material.SetFloat("_smoothing",edgeSharpness);
			RenderTexture tempRT = RenderTexture.GetTemporary(sourceTexture.width,sourceTexture.height,0);
			Graphics.Blit(sourceTexture,tempRT,curMaterial);
			Graphics.Blit(tempRT,destTexture);
			RenderTexture.ReleaseTemporary(tempRT);

			//Graphics.Blit(sourceTexture, destTexture, curMaterial);
		}
		else
		{
			Graphics.Blit(sourceTexture,destTexture);
		}
	}

	// Use this for initialization
	void Start () {
		if(!SystemInfo.supportsImageEffects)
		{
			enabled = false;
			return;
		}
		devicesExist = true;
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0)
			devicesExist = false;
		if (SourceType == ChromaKeySource.Device && autoSetDevice && devicesExist)
		{
			DeviceName = devices[0].name;
		}
	}

	void Update () 
	{		
		switch (srcType)
		{
		case ChromaKeySource.Device:
			Shader.SetGlobalTexture("_UChromaKeyTex", webCamTexture);
			break;
		case ChromaKeySource.Texture:
			Shader.SetGlobalTexture("_UChromaKeyTex", chromaKeyTexture);
			break;
		}

		Shader.SetGlobalVector("_Crop",crop);

		if (flipHorizontal)
		{
			Shader.SetGlobalFloat("_uvDefX",1.0f + uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",-1.0f / uvCoef.x);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefX",0.0f - uvShift.x);
			Shader.SetGlobalFloat("_uvCoefX",1.0f / uvCoef.x);
		}
		if (flipVertical)
		{
			Shader.SetGlobalFloat("_uvDefY",1.0f + uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",-1.0f / uvCoef.y);
		}
		else
		{
			Shader.SetGlobalFloat("_uvDefY",0.0f - uvShift.y);
			Shader.SetGlobalFloat("_uvCoefY",1.0f / uvCoef.y);
		}
	}


	void OnEnable ()
	{
		if (SourceType == ChromaKeySource.Device)
			SetTexture();
	}

	//When we disable or delete the effect.....
	void OnDisable ()
	{
		if(curMaterial != null)
		{
			DestroyImmediate(curMaterial);	//Destroys the material when not used so it won't cause leaks
		}
		if (webCamTexture != null)
		{
			if (webCamTexture.isPlaying)
				webCamTexture.Stop();
			DestroyImmediate(webCamTexture);
		}
	}

}
