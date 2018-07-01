# UnityComponentAutoInjector

## This assets is require Unity Version 5.3.0f4 or newer.
> [Tested 5.3.0f4 ~ 2018.2.0b10]

It is automatically injected according to the type of variable, array, and list.
If an inherited class also has this attribute, it is injected.

* How injected
The injection method is automatically injected after the script is compiled or when you add a component.
It works only in Unity Editor, so there is no problem when you play, load, or build.


```csharp
[SerializeField, HideInInspector(If you want to hiding variables), GetComponent] // If the variable is private,

[GetComponent] public Transform _class; // GameObject type supported
[GetComponent] public ClassExample[] _classes;
[GetComponent] public List<ClassExample> _classList;

[GetComponentInChildren] public ClassExample _class1;
[GetComponentInChildren] public ClassExample[] _classes1;
[GetComponentInChildren(true)] public List<ClassExample> _classList1; // Include hide in active

[GetComponentInParent] public ClassExample _class4;
[GetComponentInParent] public ClassExample[] _classes4;
[GetComponentInParent(true)] public List<ClassExample> _classList4; //  Include hide in active

[GetComponentInChildrenOnly] public ClassExample _class2;           // Locate both the child and child hierarchies. Objects that are off are also injected.
[GetComponentInChildrenOnly] public ClassExample[] _classes2;
[GetComponentInChildrenOnly] public List<ClassExample> _classList2;
[GetComponentInChildrenOnly(false)] public List<ClassExample> _classList3; // If set to false, only the children except the hierarchy are searched.

[GetComponentInChildrenName("ObjectExample")] public ClassExample _objectExample; // The ObjectExample object is injected.
[GetComponentInChildrenName] public ClassExample _objectExample; // If the name does not exist, it looks for the variable name..
								 // _ And m_ are automatically deleted and looked for after they are changed to lowercase.

[FindGameObject("Object name")] public GameObject _gameObject;         // Finds game objects that exist in the current scene.
[FindGameObject("Object name")] public ClassExample _objectExample;

[FindGameObjectWithTag("Tag name")] public GameObject _gameObjectTag;     // Find the game object that has the tag in the current scene.
[FindGameObjectWithTag("Tag name")] public GameObject[] _gameObjectsTag;
[FindGameObjectWithTag("Tag name")] public List<GameObject> _gameObjectListTag;

[FindObjectOfType] public ClassExample _classType;         // Finds the type in the current scene and injects it.
[FindObjectOfType] public ClassExample[] _classesType;
[FindObjectOfType] public List<ClassExample> _classListType;
```

* Caution
  1. The private variable must contain the [SerializeField] serialization attribute unconditionally.
  2. To reinject the components' variables, press the gear and then [Force auto inject this].
  3. Automatic injection in dynamic object creation (new GameObject (name)) is not supported. Instead, use GetComponent, which is built-in to Unity.
  4. If you are using another custom editor and want to be injected automatically when you add components, you can use the OnEnable implementation of the Editor code,
     Call this method CAutoInjectionEditor.InjectFrom_SerializedObject (serializedObject, false);
  5. If you press the play button while compiling, it will play automatically after completing the auto injection.
  
* If you this assets have a bug or want to request an improvement, use the Issues menu.

MIT License

Copyright (c) 2018 KJH

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
