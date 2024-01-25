using UnityEngine;

public class MyCodeStyle : MonoBehaviour {
    // Constants: UpperCase Snake_Case
    public const int MAX_HEALTH = 100;

    // Properties: PascalCase
    public static MyCodeStyle Instance { get; private set; }

    // Fields: camelCase
    private float memberVariable;

    // Function Names: PascalCase 
    private void Awake() {
        Instance = this;

        DoSomething(10f);
    }

    // Function Params: camelCase
    private void DoSomething(float time) {
        // Do something...
        memberVariable = time + Time.deltaTime;
        if (memberVariable > 0) {
            // Do something else...
        }
    }
}