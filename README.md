# UnityComponentAutoInjector

This assets is require Unity Version 5.3.0f4 or newer.
[Tested 5.3.0f4 ~ 2018.2.0b10]

변수, 배열, 리스트의 타입에 맞춰서 자동으로 주입됩니다.
만약 상속 되어있는 클래스도 이 속성이 존재하면 모두 주입됩니다.

주입되는 방식은 스크립트가 컴파일된 후 혹은, 컴퍼넌트를 추가할 시에 자동으로 주입됩니다.
유니티 에디터에서만 적용되는 방식이라 플레이할때, 로딩할때, 빌드할때에 아무런 지장이 없습니다.

```csharp
///만약 변수가 private 일때 속성 :
[SerializeField, HideInInspector(변수를 가리고 싶은 경우), GetComponent]

[GetComponent] public Transform _class; // GameObject 지원
[GetComponent] public ClassExample[] _classes;
[GetComponent] public List<ClassExample> _classList;

[GetComponentInChildren] public ClassExample _class1;
[GetComponentInChildren] public ClassExample[] _classes1;
[GetComponentInChildren(true)] public List<ClassExample> _classList1; // 꺼져있는 오브젝트도 주입됩니다.

[GetComponentInParent] public ClassExample _class4;
[GetComponentInParent] public ClassExample[] _classes4;
[GetComponentInParent(true)] public List<ClassExample> _classList4; // 꺼져있는 오브젝트도 주입됩니다.

[GetComponentInChildrenOnly] public ClassExample _class2;           // 자식과 자식 계층구조 모두 찾습니다. 꺼져있는 오브젝트도 주입됩니다. GameObject 지원
[GetComponentInChildrenOnly] public ClassExample[] _classes2;       // 이것도 마찬가지
[GetComponentInChildrenOnly] public List<ClassExample> _classList2; // 이것도 마찬가지
[GetComponentInChildrenOnly(false)] public List<ClassExample> _classList3; //false 로 설정하면 계층구조를 제외한 자식만 찾습니다.

[GetComponentInChildrenName("ObjectExample")] public ClassExample _objectExample; // ObjectExample 오브젝트가 주입됩니다. GameObject 지원
[GetComponentInChildrenName] public ClassExample _objectExample;  // ObjectExample 오브젝트가 주입됩니다.
								  // 이름이 없으면 변수이름으로 찾습니다.
								  // 언더바는 자동으로 삭제되고 소문자로 바뀐뒤에 찾습니다.

[FindGameObject("오브젝트 이름")] public GameObject _gameObject;         // 현재 씬에 존재하는 게임오브젝트를 찾습니다.
[FindGameObject("오브젝트 이름")] public ClassExample _objectExample;

[FindGameObjectWithTag("태그 이름")] public GameObject _gameObjectTag;     // 현재 씬에서 해당 태그가 설정 되어있는 게임오브젝트를 찾습니
[FindGameObjectWithTag("태그 이름")] public GameObject[] _gameObjectsTag;   // 현재 씬에서 해당 태그가 붙어있는 게임오브젝트들을 모두 찾습니다.
[FindGameObjectWithTag("태그 이름")] public List<GameObject> _gameObjectListTag;

[FindObjectOfType] public ClassExample _classType;         // 현재 씬에 존재하는 타입을 찾아서 주입시킵니다.
[FindObjectOfType] public ClassExample[] _classesType;     // 현재 씬에 존재하는 타입들을 찾아서 모두 주입시킵니다.
[FindObjectOfType] public List<ClassExample> _classListType;
```

  
주의 사항 :
  1. private 변수는 [SerializeField] 직렬화 속성을 무조건 포함해야 합니다.
  2. 해당 컴퍼넌트의 변수들을 재 주입 시키려면 톱니바퀴를 누른 후 [Force auto inject this] 을 누릅니다.
  3. 이 에셋을 사용전에 이미 오브젝트가 Prefab 화가 되어있다면 다시 에디터로 옮겨서 재 주입을 시켜야 됩니다. (에러가 날 경우에만)
  4. 동적 오브젝트 생성(new GameObject(name)) 에서의 자동주입은 당연히 미지원 입니다. 대신 유니티 기본 내장 되어있는 GetComponent 를 사용하세요.
  5. 다른 커스텀 에디터를 사용중이고, 컴퍼넌트 추가할때 자동주입되기를 원하신다면 Editor 코드의 OnEnable 구현 혹은 상속을 받은 후
     CAutoInjectionEditor.InjectFrom_SerializedObject(serializedObject, false); 를 호출하세요.
  6. 컴파일 중일때 플레이 버튼을 누르면 컴파일이 끝난 뒤 자동주입 후 플레이됩니다.
  
기타 피드백은 및 개선사항은 Issues 에 작성해주세요.


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
