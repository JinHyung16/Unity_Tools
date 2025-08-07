using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using HughCommon.Util;

namespace HughGame.HEditor
{
    public class CanvasScalerChangeEditor
        : EditorWindow
    {
        enum EFilterMode
        {
            ShowAll = 0,
            ShowSpecificResolution
        }

        AssetObjectData[] _allPrefabs;

        // Canvas �⺻ ����
        CanvasScaler.ScaleMode _uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        Vector2 _resolution;
        CanvasScaler.ScreenMatchMode _screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        float _matchWidthOrHeight = 0.0f;

        // ���͸� (�˻�)
        EFilterMode _filterMode = EFilterMode.ShowAll;
        Vector2 _filterResolution = new Vector2(1080, 1920);

        string _searchText = "";

        Vector2 _prefabScrollPosition;
        Vector2 _logScrollPosition;

        List<CanvasScaler> _canvasScalerList = new List<CanvasScaler>();
        List<bool> _selectList = new List<bool>();

        List<CanvasScaler> _filteredCanvasScalerList = new List<CanvasScaler>();
        List<bool> _filteredSelectList = new List<bool>();
        List<AssetObjectData> _filteredPrefabList = new List<AssetObjectData>();

        List<string> _logs = new List<string>();

        public static void OpenWindow()
        {
            var win = GetWindow<CanvasScalerChangeEditor>("CanvasScalerChangeEditor");
            if (win != null)
                win.Show();
        }

        void OnEnable()
        {
            FindCanvasScalers();
            ApplyFiltering();
        }

        void ReloadWindow()
        {
            try
            {
                _canvasScalerList.Clear();
                _selectList.Clear();
                _filteredCanvasScalerList.Clear();
                _filteredSelectList.Clear();
                _filteredPrefabList.Clear();
                _logs.Clear();

                _prefabScrollPosition = Vector2.zero;
                _logScrollPosition = Vector2.zero;

                _filterMode = EFilterMode.ShowAll;
                _searchText = "";

                FindCanvasScalers();
                ApplyFiltering();

                Debug.Log("Canvas Scaler â�� ���������� ��ε�Ǿ����ϴ�.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"â ��ε� �� ���� �߻�: {e.Message}");
            }
        }

        void OnGUI()
        {
            GUILayout.Space(5);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Canvas Scaler - Set Change Value", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Canvas Scaler ����", EditorStyles.boldLabel);
            GUILayout.Space(5);
            _uiScaleMode = (CanvasScaler.ScaleMode)EditorGUILayout.EnumPopup("UI Scale Mode", _uiScaleMode);
            _resolution = EditorGUILayout.Vector2Field("Reference Resolution", _resolution);
            _screenMatchMode = (CanvasScaler.ScreenMatchMode)EditorGUILayout.EnumPopup("Screen Match Mode", _screenMatchMode);

            if (_screenMatchMode == CanvasScaler.ScreenMatchMode.MatchWidthOrHeight)
            {
                _matchWidthOrHeight = EditorGUILayout.Slider("Match Width Or Height", _matchWidthOrHeight, 0.0f, 1.0f);
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Slider("Match Width Or Height (��Ȱ��)", _matchWidthOrHeight, 0.0f, 1.0f);
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("������ ����", EditorStyles.boldLabel);
            GUILayout.Space(3);
            GUILayout.Label("CanvasScaler�� ������ �ִ� ��ü ������ ���� : " + _canvasScalerList.Count);
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("���͸� �� �˻� �ɼ�", EditorStyles.boldLabel);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = _filterMode == EFilterMode.ShowAll ? Color.green : Color.white;
            if (GUILayout.Button("��ü ����", GUILayout.Height(25)))
            {
                if (_filterMode != EFilterMode.ShowAll)
                {
                    _filterMode = EFilterMode.ShowAll;
                    ApplyFiltering();
                }
            }
            GUI.backgroundColor = _filterMode == EFilterMode.ShowSpecificResolution ? Color.green : Color.white;
            if (GUILayout.Button("Ư�� �ػ󵵰� �ƴ� �͸� ����", GUILayout.Height(25)))
            {
                if (_filterMode != EFilterMode.ShowSpecificResolution)
                {
                    _filterMode = EFilterMode.ShowSpecificResolution;
                    ApplyFiltering();
                }
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            if (_filterMode == EFilterMode.ShowSpecificResolution)
            {
                EditorGUILayout.BeginHorizontal();
                Vector2 newFilterResolution = EditorGUILayout.Vector2Field("������ �ػ� (W x H)", _filterResolution);
                if (newFilterResolution != _filterResolution)
                {
                    _filterResolution = newFilterResolution;
                    ApplyFiltering();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            string newSearchText = EditorGUILayout.TextField("������ �˻� (�̸�, ��� ��������)", _searchText);
            if (newSearchText != _searchText)
            {
                _searchText = newSearchText;
                ApplyFiltering();
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"���͸��� ������ ���� : {GetFilteredCount()}/{_canvasScalerList.Count}");
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Canvas Scaler ���� ����", GUILayout.Height(30)))
            {
                Change();
            }

            GUILayout.Space(10);

            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("������â ��ε�", GUILayout.Height(30), GUILayout.Width(100)))
            {
                ReloadWindow();
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("���� ��� ���", EditorStyles.boldLabel);
            GUILayout.Space(5);

            _prefabScrollPosition = EditorGUILayout.BeginScrollView(_prefabScrollPosition, GUILayout.Height(800f));

            var filteredCount = GetFilteredCount();

            if (filteredCount > 0 &&
                (filteredCount != _filteredSelectList.Count || filteredCount != _filteredPrefabList.Count))
            {
                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("������ ����ġ ������ �߻��߽��ϴ�.", EditorStyles.boldLabel);
                GUILayout.Space(5);

                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("���� â ��ε��ϱ�", GUILayout.Height(25)))
                {
                    ReloadWindow();
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndVertical();
            }
            else
            {
                for (int i = 0; i < filteredCount; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    var prefab = _filteredPrefabList[i];
                    _filteredSelectList[i] = EditorGUILayout.Toggle(_filteredSelectList[i], GUILayout.Width(20));
                    GUILayout.Label(prefab.Path);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            GUILayout.Space(15);

            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("��ȯ ����", EditorStyles.boldLabel);
            GUILayout.Space(5);

            var logScrollViewHeight = 150f;
            _logScrollPosition = EditorGUILayout.BeginScrollView(
                _logScrollPosition,
                GUILayout.Height(logScrollViewHeight)
            );

            for (int i = 0; i < _logs.Count; i++)
            {
                var log = _logs[i];
                GUILayout.Label(log);
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            GUILayout.Space(10);
        }

        void FindCanvasScalers()
        {
            //var allPrefab = AssetDatabase.FindAssets("t:Prefab");
            var allPrefab = Finder.GetAllPrefabs();
            var list = new List<AssetObjectData>();

            foreach (var assetData in allPrefab)
            {
                var obj = assetData.Object;
                if (obj == null ||
                    obj is not GameObject go)
                    continue;

                var path = AssetDatabase.GetAssetPath(obj);
                if (path.StartsWith("Packages"))
                    continue;

                var canvasScaler = go.GetComponent<CanvasScaler>();
                if (canvasScaler == null)
                    continue;

                _canvasScalerList.Add(canvasScaler);
                _selectList.Add(true);
                list.Add(assetData);
            }

            _allPrefabs = list.ToArray();
        }

        void ApplyFiltering()
        {
            _filteredCanvasScalerList.Clear();
            _filteredSelectList.Clear();
            _filteredPrefabList.Clear();

            var count = _canvasScalerList.Count;
            if (count != _allPrefabs.Length || count != _selectList.Count)
            {
                Debug.LogError($"����Ʈ ���� ����ġ: CanvasScaler={count}, AllPrefabs={_allPrefabs.Length}, SelectList={_selectList.Count}");
                return;
            }

            for (int i = 0; i < count; i++)
            {
                var canvasScaler = _canvasScalerList[i];
                var prefab = _allPrefabs[i];
                var selected = _selectList[i];

                bool passResolutionFilter = true;
                bool passSearchFilter = true;

                if (_filterMode == EFilterMode.ShowSpecificResolution)
                {
                    var resolution = canvasScaler.referenceResolution;
                    passResolutionFilter = Mathf.Approximately(resolution.x, _filterResolution.x) == false ||
                                           Mathf.Approximately(resolution.y, _filterResolution.y) == false;
                }

                if (string.IsNullOrEmpty(_searchText) == false)
                {
                    var searchLower = _searchText.ToLower();
                    var pathLower = prefab.Path.ToLower();
                    var nameLower = prefab.Object.name.ToLower();

                    passSearchFilter = pathLower.Contains(searchLower) || nameLower.Contains(searchLower);
                }

                if (passResolutionFilter && passSearchFilter)
                {
                    _filteredCanvasScalerList.Add(canvasScaler);
                    _filteredSelectList.Add(selected);
                    _filteredPrefabList.Add(prefab);
                }
            }
        }

        int GetFilteredCount()
        {
            return _filteredCanvasScalerList.Count;
        }

        void Change()
        {
            if (_resolution == Vector2.zero)
            {
                Debug.Assert(false, "�����Ϸ��� Canvas Scaler�� ���� (0,0)�� �� �����ϴ�");
                return;
            }

            _logs.Clear();

            var count = GetFilteredCount();

            if (count != _filteredSelectList.Count || count != _filteredPrefabList.Count)
            {
                Debug.LogError($"���͸��� ����Ʈ ���� ����ġ: Count={count}, SelectList={_filteredSelectList.Count}, PrefabList={_filteredPrefabList.Count}");
                return;
            }

            var save = 0 < count;
            var changedCount = 0;

            for (int i = 0; i < count; ++i)
            {
                if (!_filteredSelectList[i])
                    continue;

                var assetData = _filteredPrefabList[i];
                var canvasScaler = _filteredCanvasScalerList[i];

                canvasScaler.uiScaleMode = _uiScaleMode;
                canvasScaler.referenceResolution = _resolution;
                canvasScaler.screenMatchMode = _screenMatchMode;
                canvasScaler.matchWidthOrHeight = _matchWidthOrHeight;

                _logs.Add(assetData.Path);
                EditorUtility.SetDirty(assetData.Object);
                changedCount++;

                // ���� ����Ʈ������ �ش� �׸��� ã�Ƽ� ������Ʈ
                for (int j = 0; j < _canvasScalerList.Count; j++)
                {
                    if (_canvasScalerList[j] == canvasScaler)
                    {
                        _selectList[j] = _filteredSelectList[i];
                        break;
                    }
                }
            }

            if (save)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log($"Canvas Scaler ���� �Ϸ�: {changedCount}�� �������� ������Ʈ�Ǿ����ϴ�.");
            }
        }
    }
}