using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.IO;

public class TestLoadComponent : MonoBehaviour {

    public RawImage imageObj;

    public Text textObj;

	// Use this for initialization
	void Start () {
        StartCoroutine(CopyData());
	}


    FastLoader.TextureLoader loader = new FastLoader.TextureLoader();

    public void TestLoad()
    {
        string path = Path.Combine( Application.temporaryCachePath , "test.bin");

        float stTime = Time.realtimeSinceStartup;

        loader.LoadToBuffer(path);
        var texture = loader.CreateTexture2DFromBuffer();
        float endTime = Time.realtimeSinceStartup;

        this.imageObj.texture = texture;

        this.textObj.text = "Texture " + texture.width + "*" + texture.height + "\n"+
            texture.format + ":" + texture.mipmapCount +"\n" + (endTime - stTime);
    }


    IEnumerator CopyData()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        string path = Path.Combine(Application.streamingAssetsPath, "test.jpg.bin");
#else
        string path = "file://" + Path.Combine(Application.streamingAssetsPath, "test.jpg.bin");
#endif
        WWW www = new WWW( path );
        yield return www;
        string outputPath = Path.Combine( Application.temporaryCachePath , "test.bin");
        File.WriteAllBytes( outputPath,www.bytes);
        
    }
}
