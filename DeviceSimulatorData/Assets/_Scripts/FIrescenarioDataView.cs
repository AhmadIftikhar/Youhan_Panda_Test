using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FIrescenarioDataView : MonoBehaviour
{
	public Text deviceid;
	public Text devicetype;
	public Text devicetime;

	[Header("O2")]
	public Text o2value;
	public Text o2units;
	public Text o2max;
	public Text o2min;

[Header("CO")]
	public Text covalue;
	public Text counits;
	public Text comin;

	[Header("H2")]
	public Text H2value;
	public Text H2units;
	public Text H2min;

	[Header("HCN")]
	public Text HCNvalue;
	public Text HCNunits;
	public Text HCNmin;


	[Header("Lel")]
	public Text Lelvalue;
	public Text Lelunits;
	public Text Lelmin;

	[Header("Particulate")]
	public Text Particulatevalue;
	public Text Particulateunits;
	public Text Particulatemin;


	[Header("Coordinates")]
	public Text latitudes;
	public Text longitude;




	public void UpdateThisView
		
		(string devid,
		string devtype,
		string devtime,

	string o2val,
	string o2unit,
	string o2ma,
	string o2mi,

	string coval,
	string counit,
	string comi,

	string H2val,
	string H2unit,
	string H2mi,

	string HCNval,
	string HCNuni,
	string HCNmi,

	string Lelval,
	string Leluni,
	string Lelmi,

	string Particulatevalu,
	string Particulateunit,
	string Particulatemi,

	string latitude,
	string longitud
		) 
	{
	deviceid.text = devid;
	devicetype.text= devtype;
	devicetime.text = devtime;

	o2value.text =o2val;
	o2units.text =o2unit;
	o2max.text =o2ma;
	o2min.text =o2mi;

	covalue.text =coval;
	counits.text =counit;
	comin.text =comi;

	H2value.text =H2val;
	H2units.text =H2unit;
	H2min.text = H2mi;

	HCNvalue.text =HCNval;
	HCNunits.text =HCNuni;
	HCNmin.text = HCNmi;
	
	Lelvalue.text =Lelval;
	Lelunits.text =Leluni;
	Lelmin.text = Lelmi;

	Particulatevalue.text =Particulatevalu;
	Particulateunits.text =Particulateunit;
	Particulatemin.text =Particulatemi;

	latitudes.text =latitude;
	longitude.text =longitud;

}





}
