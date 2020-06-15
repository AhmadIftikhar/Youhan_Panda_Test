using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DropableItem : MonoBehaviour ,IDragHandler,IEndDragHandler,IBeginDragHandler
{
	public Image imageholder;
	public Sprite unavailableImage;
	public bool Hittingplane;

	public bool startDrag;

	public Text TextDescription;

	Transform parrenttoretuento;
	int indextoretuento;

	public GameObject Quad;
	public Material QuadMaterial;

	Vector2 mousePos;
	Vector3 point;


	public float offset;

	void Start()
	{
		startDrag = false;
		   Hittingplane = false;
	}

	void Update()
	{
		if (Input.GetMouseButton(0) && startDrag)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin,ray.direction*1000,Color.red);

			mousePos.x = Input.mousePosition.x;
			mousePos.y = Input.mousePosition.y;

			point = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane + 5 ));
		//	point.y = point.y * -1 +2;
			Quad.transform.position = point;
			Quad.transform.localScale = Vector3.one;
			Quad.transform.LookAt(Camera.main.transform.forward);

			//suppose i have two objects here named obj1 and obj2.. how do i select obj1 to be transformed 
			if (Physics.Raycast( ray , out hit, Mathf.Infinity))
			{
				if (hit.transform != null)
				{
					Debug.DrawRay(transform.position, transform.TransformDirection(hit.transform.up) * hit.distance, Color.yellow);
				

					Debug.Log("somewhere i collided " + hit.transform.gameObject.name);
					if (hit.transform.gameObject.tag != "Quads")
					{
						Hittingplane = true;

						Debug.Log("yo i hit " + hit.transform.gameObject.name + " changimg pos to " + hit.transform.position);
						this.transform.position = hit.point;
						Quad.transform.position = hit.point;


						Debug.Log("pos of x is "+ hit.transform.rotation.eulerAngles.x+ "pos of y is " + hit.transform.rotation.eulerAngles.y + "pos of z is " + hit.transform.rotation.eulerAngles.z );


					//	Vector3 direction = hit.transform.forward;
						// Change child.forward to child.up if you want the up vectors to "look at" the other child object
					//	Quaternion rotation = Quaternion.FromToRotation(Quad.transform.position, direction);
					//	Quad.transform.rotation = rotation * Quad.transform.rotation;


						
						Quad.transform.rotation = 
							Quaternion.Euler(
								hit.transform.rotation.x , 
								hit.transform.rotation.y , 
								hit.transform.rotation.z 
								);

						Quad.transform.rotation = Quaternion.FromToRotation(Quad.transform.forward,-1*Quad.transform.up);

						Quad.transform.localPosition = Quad.transform.localPosition+ Quad.transform.forward *-1* offset;
					}
				}
			}
			else
			{
				Hittingplane = false;
			}
		}
	}
	public void SetSprite(Sprite sprite = null)
	{
		if (sprite == null)
		{
			imageholder.sprite = unavailableImage;
		}
		else
		{
			imageholder.sprite = sprite;
		}
	}
	public void SetName(string Name)
	{
		TextDescription.text = Name;
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (!Hittingplane)
		{
			this.transform.position = eventData.position;
		}
	
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		this.transform.localPosition = Vector3.zero;
		this.transform.SetParent(parrenttoretuento);
		this.transform.SetSiblingIndex(indextoretuento);

		if (!Hittingplane)
			{
				Quad.SetActive(false);
			}
		if (Hittingplane)
		{
			GameObject Quadinstance = Instantiate(Quad, null);

			Material mat = new Material(Quadinstance.GetComponent<Renderer>().sharedMaterial);
			mat.mainTexture = imageholder.sprite.texture;
			Quadinstance.GetComponent<Renderer>().material = mat;

			Quad.SetActive(false);
		}

		imageholder.enabled = true;
		TextDescription.enabled = true;
		startDrag = false;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		startDrag = true;
		SideBarmanager.instance.Changeanimatorstate();
		parrenttoretuento = this.transform.parent;
		indextoretuento    = this.transform.GetSiblingIndex();
		this.transform.SetParent(this.transform.parent.parent);
		Left2DSpace();
	}

	public void Left2DSpace()
	{
		imageholder.enabled = false;
		TextDescription.enabled = false;
		QuadMaterial.mainTexture = imageholder.sprite.texture;
		Quad.SetActive(true);
		Quad.transform.SetParent(null);
		Quad.transform.position = Input.mousePosition;

	}

}
