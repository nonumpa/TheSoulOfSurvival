using UnityEngine;
using System.Collections;

public class MiniMap : MonoBehaviour {
  
    // Use this for initialization  
    public GameObject point;  
    public GameObject map;  
    public GameObject hero;  
	public GameObject red1;
	public GameObject ai1;
    private float miniMapScaleRatio_x,miniMapScaleRatio_y;  
      
    void Start () {  
        //map.transform.localScale = new Vector3(Screen.height, Screen.height, 1);  
        GameObject terrain = GameObject.Find("Terrain");  
        Terrain script = terrain.GetComponent<Terrain>();  
        miniMapScaleRatio_x = 600 / script.terrainData.size.x; 
		miniMapScaleRatio_y = 598 / script.terrainData.size.x; 
	 
    }  
      
    // Update is called once per frame  
    void Update () {  
         // const float miniMapScaleRatio = 10000 / 2000f;
        if (hero && point && map && red1 && ai1) {  
            point.transform.rotation = Quaternion.Euler(0, 0, -90-hero.transform.rotation.eulerAngles.y); 
			red1.transform.localPosition = new Vector3()  
            {  
                x = ai1.transform.position.z * miniMapScaleRatio_x,  
                y = -ai1.transform.position.x * miniMapScaleRatio_y,  
                z = 0,  
            };  
            map.transform.localPosition = new Vector3()  
            {  
                x = -hero.transform.position.z * miniMapScaleRatio_x,  
                y = hero.transform.position.x * miniMapScaleRatio_y,  
                z = 0,  
            };  
        }  
    }  
}  