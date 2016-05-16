using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour {

	public GameObject mappingsUi;

	private ProjectManagerScript projectManager;
	private FirstPersonController controller;
    private NavigationController navController;

	public void Start() {
		projectManager = Component.FindObjectOfType<ProjectManagerScript> ();
		controller = Component.FindObjectOfType<FirstPersonController> ();
        navController = Component.FindObjectOfType<NavigationController>();
    }

    void Update() {
        if (Input.GetButtonDown("Exit"))
        {
            Application.Quit();
        }

        if (Input.GetButtonDown("OpenMappingEditor"))
        {
            ShowMappingsUi();
        }
    }

	public void CloseUi() {
		controller.enabled = true;
        navController.enabled = true;
        mappingsUi.SetActive (false);
	}

	public void ShowMappingsUi() {
		controller.enabled = false;
        navController.enabled = false;
        mappingsUi.SetActive (true);
		Dropdown dropdownMappingHeight = GameObject.Find ("DropdownMappingHeight").GetComponent<Dropdown> ();
		Dropdown dropdownMappingSize = GameObject.Find ("DropdownMappingSize").GetComponent<Dropdown> ();
		Dropdown dropdownMappingColor = GameObject.Find ("DropdownMappingColor").GetComponent<Dropdown> ();

		List<string> metricNames = projectManager.currentProject.metricNames;

		foreach (Dropdown dropdown in new Dropdown[] {dropdownMappingHeight, dropdownMappingSize, dropdownMappingColor}) {
			dropdown.options.Clear ();
			dropdown.AddOptions (metricNames);
		}

        if (one++ < 1)
        {
            // FOR DEMO
            dropdownMappingHeight.value = 7;
            dropdownMappingColor.value = 10;
        }
        
    }

    private int one;

    public void OnMappingsApplyClicked()  {
		Dictionary<BuildingProperty, MetricMapping> mappings = new Dictionary<BuildingProperty, MetricMapping>();

		Dropdown dropdownMappingHeight = GameObject.Find ("DropdownMappingHeight").GetComponent<Dropdown> ();
		Dropdown dropdownMappingSize = GameObject.Find ("DropdownMappingSize").GetComponent<Dropdown> ();
		Dropdown dropdownMappingColor = GameObject.Find ("DropdownMappingColor").GetComponent<Dropdown> ();


		mappings.Add(BuildingProperty.Height, new MetricMapping(dropdownMappingHeight.options[dropdownMappingHeight.value].text, BuildingProperty.Height,
			CreateConverter(GameObject.Find("MappingHeight").GetComponent<InputField>().text, false), null));
		mappings.Add(BuildingProperty.Width, new MetricMapping(dropdownMappingSize.options[dropdownMappingSize.value].text, BuildingProperty.Width, 
			CreateConverter(GameObject.Find("MappingSize").GetComponent<InputField>().text, false), null));

        ColorGradient col = new ColorGradient(new ColorGradient.ColorPoint(0, new Vector3(1, 0, 0)),  new ColorGradient.ColorPoint(1, new Vector3(0, 1, 0)));
		ValueConverter colorValueConverter = CreateConverter (GameObject.Find ("MappingColor").GetComponent<InputField> ().text, true);
		mappings.Add(BuildingProperty.Red, new MetricMapping(dropdownMappingColor.options[dropdownMappingColor.value].text, BuildingProperty.Red, colorValueConverter, col));
		mappings.Add(BuildingProperty.Green, new MetricMapping(dropdownMappingColor.options[dropdownMappingColor.value].text, BuildingProperty.Green, colorValueConverter, col));
		mappings.Add(BuildingProperty.Blue,  new MetricMapping(dropdownMappingColor.options[dropdownMappingColor.value].text, BuildingProperty.Blue, colorValueConverter, col));

		projectManager.OnMappingChanged (mappings);
		CloseUi ();
	}

	private ValueConverter CreateConverter(string mappings, bool clamp) {
		Regex split = new Regex(" *, *");
		string[] values = split.Split (mappings);
		return new ValueConverter (float.Parse (values [0]), float.Parse (values [1]), float.Parse (values [2]), float.Parse (values [3]), clamp);
	}

}
