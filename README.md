# FileManager

Unity 프로젝트에서 JSON 및 XML 데이터를 손쉽게 가져오고 내보낼 수 있는 기능을 제공합니다.

# 설치

![image](https://github.com/user-attachments/assets/f8a6a65e-f832-4eeb-bfff-8bcfe9238b5f)

1. URL 복사

![image](https://github.com/user-attachments/assets/ac89b149-8c9b-40f5-bfe8-2d38b6b95383)

2. 패키지 매니저에서 Add Package from Git URL 선택

![image](https://github.com/user-attachments/assets/3f8ceeba-7e35-4044-961d-c8f9ff53b1d7)

3. 복사한 URL로 설치

# 함수

### JSON

`ExportJson` - 객체를 JSON 파일로 내보내기

`ImportJson` - JSON 파일에서 객체 불러오기

### XML

`ExportXmlSchema` - 객체 스키마를 XML로 내보내기

`ImportXml` - XML 파일에서 객체 배열 불러오기

### 기본 제공 경로

`PersistentDataPath` - 데이터 저장에 사용

`ResourcePath` - 외부에서 접근할 수 없는 데이터에 사용

`StreamingAssetsPath` - 외부에서 접근할 수 있는 데이터에 사용

# 사용법
### JSON
``` C#
//JSON 내보내기
FileManager.ExportJson(FileManager.PersistentDataPath, data);

//JSON 불러오기
var data = FileManager.ImportJson<MyClass>(FileManager.PersistentDataPath);
```

### XML
``` C#
///XML 스키마 내보내기
FileManager.ExportXmlSchema<MyClass>(FileManager.StreamingAssetsPath);

///XML 불러오기
var data = FileManager.ImportXml<MyClass>(FileManager.StreamingAssetsPath);
```
