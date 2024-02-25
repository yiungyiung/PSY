using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class readdata : MonoBehaviour
{
    public holdrotate hr;

    private int currentIndex = 0;

    [SerializeField]
    public Dropdown tmpDropdown; //TMP_Dropdown

    [SerializeField]
    TMP_InputField Inputname;
    [SerializeField]
    TMP_Text name;
    [SerializeField]
    TMP_Text age;
    [SerializeField]
    TMP_Text gender;
    [SerializeField]
    TMP_Text scenes;
    [SerializeField]
    TMP_Text type;
    [SerializeField]
    TMP_Text joint;
    [SerializeField]
    TMP_Text angle;
    public firebase fb;

    public IEnumerable<DataSnapshot> alldata;

    List<DataSnapshot> childlastdata = new List<DataSnapshot>();

    public List<Dictionary<string, object>>
        shotdata = new List<Dictionary<string, object>>();

    [SerializeField]
    public GameObject spot;

    GameObject legpain;

    GameObject Painpoint;

    [SerializeField]
    string names = "";

    [SerializeField]
    GameObject Knee1;

    [SerializeField]
    GameObject Elbow;

    string scene;
    bool ranged= false;
    bool acute = false;
    // Update is called once per frame
    Quaternion originalRotation;
    public void callReadName()
    {
        names=Inputname.text;
        fb.ReadName(Inputname.text, OnReadNameComplete);
    }

    private void OnReadNameComplete(IEnumerable<DataSnapshot> childrenList)
    {
        tmpDropdown.ClearOptions();
        alldata = childrenList;
        string first = "Select an option"; // Default text for the blank value

        // Add a blank option at index 0
        tmpDropdown.options.Add(new Dropdown.OptionData(first));
        if (alldata != null)
        {
            List<string> options = new List<string>();
            childlastdata.Clear();
            foreach (var childSnapshot in alldata)
            {
                string entryId = childSnapshot.Key;
                string timestamp =
                    childSnapshot.Child("timestamp").Value.ToString();
                if (first == "")
                {
                    first = timestamp;
                }
                string entryData = $"ID: {entryId}, Timestamp: {timestamp}";
                var op = new Dropdown.OptionData(timestamp);
                tmpDropdown.options.Add (op);
                childlastdata.Add (childSnapshot);
            }
            Debug.Log("exe");
            
            tmpDropdown.RefreshShownValue();

        }
    }

    public void OnDropdownValueChanged()
    {
        int index = tmpDropdown.value-1;
        if (index >= 0 && index < childlastdata.Count)
        {
            shotdata.Clear();
            Debug.Log("Selected index: " + index);
            currentIndex = 0;

            // Access the corresponding DataSnapshot from the child list
            DataSnapshot selectedSnapshot = childlastdata[index];
   
            // Access the "data" child
            DataSnapshot dataSnapshot = selectedSnapshot.Child("data");
            scene = selectedSnapshot.Child("scene").Value.ToString();
            name.text=names;
            age.text=selectedSnapshot.Child("age").Value.ToString();
            gender.text=selectedSnapshot.Child("gender").Value.ToString();
            scenes.text= scene;
            joint.text="Current Joint:";
            foreach (var typeSnapshot in dataSnapshot.Children)
            {
                string dataType = typeSnapshot.Key;
                foreach (var entrySnapshot in typeSnapshot.Children)
                {
                    string entryKey = entrySnapshot.Key;
                    var entryValue = entrySnapshot.Value;
                    Dictionary<string, object> keyValuePairs =
                        new Dictionary<string, object> {
                            { entryKey, entryValue }
                        };
                    shotdata.Add (keyValuePairs);
                    //Debug.Log( entryKey + " " + entryValue );
                }
            }
            Knee1.SetActive(false);
            Elbow.SetActive(false);

            switch (scene)
            {
                case "knee 1":
                    Knee1.SetActive(true);
                    legpain = FindChildWithTag(Knee1, "painloc");
                    hr.hd = Knee1.GetComponent<Rotation>();
                    break;
                case "elbow":
                    Elbow.SetActive(true);
                    legpain = FindChildWithTag(Elbow, "painloc");
                    hr.hd = Elbow.GetComponent<Rotation>();
                    break;
            }
            DisplayCurrentIndex();
        }
    }

    public void ReduceIndex()
    {
        if (shotdata.Count > 0)
        {
            currentIndex = (currentIndex - 1 + shotdata.Count) % shotdata.Count;
            DisplayCurrentIndex();
        }
    }

    public void IncreaseIndex()
    {
        if (shotdata.Count > 0)
        {
            currentIndex = (currentIndex + 1) % shotdata.Count;
            DisplayCurrentIndex();
        }
    }

    private void DisplayCurrentIndex()
    {
        Debug.Log($"Current Index: {currentIndex}");

        if (shotdata.Count > 0)
        {
            legpain.transform.localRotation = originalRotation;
            ranged = false;
            acute = false;
            angle.text = "";
            Dictionary<string, object> currentEntry = shotdata[currentIndex];

            if (Painpoint != null)
            {
                Destroy (Painpoint);
            }
            originalRotation = legpain.transform.localRotation;
            if (currentEntry.ContainsKey("maxmin"))
            {  
                type.text="Max ROM";
                Debug.Log("Type: MaxMin");
                // Perform actions specific to MaxMin type
                List<int> range = new List<int>();
                Debug.Log(currentEntry["maxmin"]);
                range=ParseList(currentEntry["maxmin"].ToString(),0);
                if (range != null)
                {
                    angle.text = range[0] + "-" + range[1];
                    int init = range[0];
                    int fin = range[1];
                    ranged=true;
                    StartCoroutine(MoveLegPain(init, fin));
                }
            }
            else if (currentEntry.ContainsKey("position"))
            {  
                type.text="Pain Position";
                Debug.Log("Type: Position");
                string positionString = currentEntry["position"].ToString();
                Vector3 position = ParseVector3(positionString);
                var spotf = Instantiate(spot, position, Quaternion.identity);
                spotf.transform.parent = legpain.transform;
                spotf.transform.localPosition = position;
                spotf.transform.localScale = 4 * Vector3.one;
                Painpoint = spotf;
            }
            else if (currentEntry.ContainsKey("acute_pain"))
            {  
                type.text="Acute Pain";
                Debug.Log("Type: Acute Pain");
                // Perform actions specific to Acute Pain type
                List<int> range = new List<int>();
                Debug.Log(currentEntry["acute_pain"]);
                range=ParseList(currentEntry["acute_pain"].ToString(),2);
                acute = true;
                angle.text = range[0]+"";
                StartCoroutine(AcuteLegPain(range[0]));
                //legpain.transform.localEulerAngles = new Vector3(range[0], 0, 0);

            }
            else if (currentEntry.ContainsKey("ranged_pain"))
            {  
                type.text="Ranged Pain";
                Debug.Log("Type: Ranged Pain");
                List<int> range = new List<int>();
                Debug.Log(currentEntry["ranged_pain"]);
                range=ParseList(currentEntry["ranged_pain"].ToString(),1);
                if (range != null)
                {
                    angle.text = range[0] + "-" + range[1];
                    int init = range[0];
                    int fin = range[1];
                    ranged=true;
                    StartCoroutine(MoveLegPain(init, fin));
                }
                // Perform actions specific to Ranged Pain type
            }
        }
        else
        {
            Debug.Log("No data available.");
        }
    }

    private IEnumerator AcuteLegPain(int targetAngle)
    {
        // Save the original rotation
     

        // Set the rotation instantly to the target angle
        legpain.transform.localEulerAngles = new Vector3(targetAngle, 0, 0);

        // Continuously check the condition (e.g., a variable change)
        while (acute) // Replace VariableChanged with your actual condition
        {
            yield return null; // Wait for the next frame
        }
        // Code here will execute after the variable changes
        Debug.Log("Leg pain rotation complete!");

        // If you need to perform any actions after the rotation, you can do it here...
    }
    private IEnumerator MoveLegPain(int init, int fin)

{
       
        float duration = 2f; // Adjust the duration as needed
    float elapsed = 0f;
    Debug.Log("moving");
    while (ranged)
    {
        float t = Mathf.Sin(elapsed / duration * Mathf.PI * 2) * 0.5f + 0.5f;

        // Interpolate between init and fin
        int currentPos = (int)Mathf.Lerp(init, fin, t);

        // Move legpain to the current position
        // Replace "legpain.transform.position.x" with the actual property you want to modify
        legpain.transform.localEulerAngles = new Vector3(currentPos, 0, 0);

        elapsed += Time.deltaTime;
            yield return null;

        }

    }
    private Vector3 ParseVector3(string vectorString)
    {
        string[] components = vectorString.Trim('(', ')').Split(',');

        if (components.Length == 3)
        {
            float x = float.Parse(components[0]);
            float y = float.Parse(components[1]);
            float z = float.Parse(components[2]);

            return new Vector3(x, y, z);
        }

        return Vector3.zero; //Return a default value if parsing fails
    }

    private List<int> ParseList(string str,int type)
    {
        string[] components = str.Trim('(', ')').Split(',');
        List<int> value = new List<int>();
   
            if(type==0)
            {
            value.Add(int.Parse(components[0]));
            value.Add(int.Parse(components[1]));
            return value;
            }
            else if(type==1)
            {
            value.Add(int.Parse(components[0]));
            value.Add(int.Parse(components[1]));
            value.Add(int.Parse(components[2]));
            return value;
            }
            else if(type==2)
            {
            value.Add(int.Parse(components[0]));
            value.Add(int.Parse(components[1]));
            return value;
            }
            return null;
    }    
       

    GameObject FindChildWithTag(GameObject parent, string tag)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in children)
        {
            if (
                child != null &&
                child.gameObject != null &&
                child.gameObject.tag == tag
            )
            {
                return child.gameObject;
            }
        }

        // Child with the specified tag not found
        return null;
    }
}