
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

	#region Declaration 
	[SerializeField] GameObject floor;

	[SerializeField] GameObject[] walls  ;
	[SerializeField]float[] bounds = new float[4];
	[SerializeField] Vector3[] floorSize = new Vector3[1];
	[SerializeField] GameObject[] items;
	public static LevelGenerator instance;

    public Vector3[] FloorSize { get => floorSize; set => floorSize = value; }
    #endregion



    private void Awake ()
	{
        if (instance == null)
        {

            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

		levelGen();
        floorSize[0] = new Vector2(floor.GetComponent<MeshRenderer>().bounds.min.x, floor.GetComponent<MeshRenderer>().bounds.min.z);
		floorSize[1] = new Vector2(floor.GetComponent<MeshRenderer>().bounds.max.x, floor.GetComponent<MeshRenderer>().bounds.max.z);

    }
    private void Start ()
    {
        Player player = Player.instance;

    }
    void levelGen () 
	{
        floor.transform.localScale = new Vector3(Random.Range((bounds[0]), bounds[1]), floor.transform.localScale.y, Random.Range((bounds[2]), bounds[3]));
		foreach (GameObject i in walls)
		{
			
			if (i.transform.rotation.y !=0 ) 
			{
				print("asdsad");
				if (i.transform.position.x < 0) 
				{
					
					i.transform.position =new Vector3 (floor.GetComponent<MeshRenderer>().bounds.min.x, i.transform.position.y,i.transform.position.z);


                }
				else 
				
				{

                    i.transform.position = new Vector3(floor.GetComponent<MeshRenderer>().bounds.max.x, i.transform.position.y, i.transform.position.z);

                }



            }
			else 
			{
				if (i.GetComponent<SpriteRenderer>() != null)
				{
					i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y, floor.GetComponent<MeshRenderer>().bounds.max.z);
				}
				else 
				{
					i.transform.position = new Vector3(i.transform.position.x, i.transform.position.y, floor.GetComponent<MeshRenderer>().bounds.min.z);



                }
            }
        }
    }
}
