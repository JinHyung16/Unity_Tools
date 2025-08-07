using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using HughCommon.HGui;


namespace HughCommon.Util
{
    public class AssetDatas
    {
    }

    public class AssetObjectData
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FolderPath { get; set; }
        public string Extension { get; set; }
        public string Guid;
        public UnityEngine.Object Object;
        public Component[] Components;
        public UnityEngine.Object[] Dependencies;

        public bool CanResourceLoad()
        {
            if (Path.Contains("/Resources/"))
                return true;

            return false;
        }

        public void DrawGui(int index, Action editAction)
        {
            SImpleDrawGui(index, editAction);

            if (Dependencies != null && Dependencies.Length > 0)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal(GUILayout.Height(80f));
                {
                    using (new ScrollBlock().Open(this, "dependence"))
                    {
                        for (int i = 0; i < Dependencies.Length; i++)
                        {
                            var obj = Dependencies[i];
                            EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), true);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        public void SImpleDrawGui(int index, Action editAction)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(string.Format("{0}) {1}", index, Name));
                GUILayout.FlexibleSpace();
                var objType = Object == null ? typeof(UnityEngine.Object) : Object.GetType();
                EditorGUILayout.ObjectField(Object, objType, true);
                if (GUILayout.Button("Edit"))
                {
                    AssetDatabase.OpenAsset(Object);
                    editAction.Fire();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }
    }
    public class AssetTextureData
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FolderPath { get; set; }
        public string ResourceLoadPath { get; set; }
        public string Guid { get; set; }
        public UnityEngine.Object Texture { get; set; }
        public List<AssetObjectData> RefrenceAssetPrefabDatas { get; set; } = new List<AssetObjectData>();

        protected bool _foldOut;

        public void SimpleDrawGui(int index)
        {
            using (new EditorHorizontalBlock().Open())
            {
                EditorGUILayout.LabelField(string.Format("{0} ) Name : {1}", index, Name));
                EditorGUILayout.Space();
                EditorGUILayout.ObjectField(Texture, typeof(UnityEngine.Object), true);
            }

            using (new EditorHorizontalBlock().Open())
            {
                GUILayout.FlexibleSpace();
            }
        }

        public virtual void DrawGui(int index, Action editAction)
        {
            SimpleDrawGui(index);

            var count = RefrenceAssetPrefabDatas.COUNT();
            if (count > 0)
            {
                EditorGUI.indentLevel++;
                _foldOut = EditorGUILayout.Foldout(_foldOut, "Use Prefab List : " + count);
                if (_foldOut)
                {
                    using (new EditorVerticalBlock().Open())
                    {
                        for (int i = 0; i < RefrenceAssetPrefabDatas.Count; i++)
                        {
                            var data = RefrenceAssetPrefabDatas[i];
                            data.DrawGui(i, null);
                        }
                    }
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUILayout.LabelField("Use Prefab List : " + 0);
            }
        }
    }

    public class AssetTextureData<T>
        : AssetTextureData
    {
        public T Extra { get; set; }
    }

    public class Finder
    {
        public static List<T> GetAllByType<T>()
            where T : UnityEngine.Object
        {
            var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name));
            if (guids == null || guids.Length == 0)
                return null;

            List<T> list = new List<T>(guids.Length);

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);

                if (assetPath.IsNullOrEmpty())
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset == null)
                    continue;

                list.Add(asset);
            }

            return list;
        }

        public static List<AssetObjectData> GetObjectListByExtensions(string[] extensins, string folderPath)
        {
            var guids = AssetDatabase.FindAssets("", new string[] { folderPath });
            if (guids == null || guids.Length == 0)
                return null;

            List<AssetObjectData> list = new List<AssetObjectData>(guids.Length);

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty() ||
                    IsFileWithExtension(assetPath, extensins) == false)
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (asset == null)
                    continue;

                var assetData = new AssetObjectData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    Extension = System.IO.Path.GetExtension(assetPath),
                    Path = assetPath,
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Guid = guid,
                    Object = asset,
                };

                list.Add(assetData);
            }

            return list;
        }

        public static AssetObjectData GetObject(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            var guid = asset.GetAssetGUID();

            var assetData = new AssetObjectData()
            {
                Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                Extension = System.IO.Path.GetExtension(assetPath),
                Path = assetPath,
                FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                Guid = guid,
                Object = asset,
            };

            return assetData;
        }

        private static bool IsFileWithExtension(string filePath, string[] extensions)
        {
            string fileExtension = System.IO.Path.GetExtension(filePath).Replace(".", "");
            if (fileExtension.IsNullOrEmpty())
                return false;

            foreach (string ext in extensions)
            {
                if (fileExtension.Equals(ext, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        public static AssetObjectData[] GetAllPrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab");
            var length = guids.Length;
            var list = new List<AssetObjectData>(length);

            for (int i = 0; i < length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                    continue;

                var dependencies = AssetDatabase.GetDependencies(assetPath);
                var objs = new UnityEngine.Object[dependencies.COUNT()];

                var objLength = objs.Length;
                for (int j = 0; j < objLength; j++)
                {
                    var path = dependencies[j];
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    objs[j] = obj;
                }

                var assetData = new AssetObjectData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    Path = assetPath,
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Guid = guid,
                    Object = asset,
                    Components = asset.GetComponentsInChildren<Component>(true),
                    Dependencies = objs
                };

                list.Add(assetData);
            }

            list.Sort((lhs, rhs) =>
            {
                var ret = lhs.FolderPath.CompareTo(rhs.FolderPath);
                if (ret != 0)
                    return ret;

                return lhs.Name.CompareTo(rhs.Name);
            });

            return list.ToArray();
        }

        public static T[] GetAssetTextureDataGeneric<T>()
            where T : AssetTextureData, new()
        {
            var guids = AssetDatabase.FindAssets("t:texture");
            var dict = new Dictionary<string, T>(guids.Length);
            var length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var texture = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (texture == null)
                    continue;

                var isResource = assetPath.Contains("/Resources/", StringComparison.OrdinalIgnoreCase);
                var textureData = new T()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    ResourceLoadPath = isResource ? assetPath.Substring(assetPath.IndexOf("Resources") + "Resources".Length + 1).Replace(System.IO.Path.GetExtension(assetPath), "") : "",
                    Path = assetPath,
                    Guid = guid,
                    Texture = texture,
                };

                dict.Add(assetPath, textureData);

                EditorUtility.DisplayProgressBar("텍스쳐 확인", string.Format("{0}/{1}", i, length), i / length);
            }

            guids = AssetDatabase.FindAssets("");
            length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                    continue;

                var assetData = new AssetObjectData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    Path = assetPath,
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Guid = guid,
                    Object = asset,
                };

                var dependencies = AssetDatabase.GetDependencies(assetPath);
                for (int j = 0; j < dependencies.COUNT(); j++)
                {
                    var path = dependencies[j];
                    T textureData;
                    if (dict.TryGetValue(path, out textureData) == false)
                    {
                        continue;
                    }
                    textureData.RefrenceAssetPrefabDatas.Add(assetData);
                }

                EditorUtility.DisplayProgressBar("텍스쳐 사용 확인", string.Format("{0}/{1}", i, length), i / length);
            }

            var array = dict.Values.ToArray();
            /*
            Array.Sort(array, (lhs, rhs) =>
            {
                var ret = lhs.FolderPath.CompareTo(rhs.FolderPath);
                if (ret != 0)
                    return ret;

                var lhsCount = lhs.RefrenceAssetPrefabDatas.COUNT();
                var rhsCount = rhs.RefrenceAssetPrefabDatas.COUNT();

                ret = rhsCount.CompareTo(lhsCount);
                if (ret != 0)
                    return ret;

                return lhs.Name.CompareTo(rhs.Name);
            });
            */
            EditorUtility.ClearProgressBar();
            return array;
        }

        public static AssetTextureData[] GetResourceLoadableTexturesData()
        {
            var guids = AssetDatabase.FindAssets("t:texture");
            var dict = new Dictionary<string, AssetTextureData>(guids.Length);
            var length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty() ||
                    assetPath.Contains("/Resources/", StringComparison.OrdinalIgnoreCase) == false)
                    continue;

                var texture = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (texture == null)
                    continue;


                var textureData = new AssetTextureData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    ResourceLoadPath = assetPath.Substring(assetPath.IndexOf("Resources") + "Resources".Length + 1).Replace(System.IO.Path.GetExtension(assetPath), ""),
                    Path = assetPath,
                    Guid = guid,
                    Texture = texture,
                };

                dict.Add(assetPath, textureData);

                EditorUtility.DisplayProgressBar("텍스쳐 확인", string.Format("{0}/{1}", i, length), i / length);
            }


            guids = AssetDatabase.FindAssets("t:Prefab");
            length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                    continue;

                var assetData = new AssetObjectData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    Path = assetPath,
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Guid = guid,
                    Object = asset,
                };

                var dependencies = AssetDatabase.GetDependencies(assetPath);
                for (int j = 0; j < dependencies.COUNT(); j++)
                {
                    var path = dependencies[j];
                    AssetTextureData textureData;
                    if (dict.TryGetValue(path, out textureData) == false)
                    {
                        continue;
                    }
                    textureData.RefrenceAssetPrefabDatas.Add(assetData);
                }

                EditorUtility.DisplayProgressBar("텍스쳐 사용 확인", string.Format("{0}/{1}", i, length), i / length);
            }

            var array = dict.Values.ToArray();

            Array.Sort(array, (lhs, rhs) =>
            {
                var ret = lhs.FolderPath.CompareTo(rhs.FolderPath);
                if (ret != 0)
                    return ret;

                var lhsCount = lhs.RefrenceAssetPrefabDatas.COUNT();
                var rhsCount = rhs.RefrenceAssetPrefabDatas.COUNT();

                ret = rhsCount.CompareTo(lhsCount);
                if (ret != 0)
                    return ret;

                return lhs.Name.CompareTo(rhs.Name);
            });

            EditorUtility.ClearProgressBar();
            return array;
        }

        public static AssetTextureData[] GetAssetTextureDatas()
        {
            var guids = AssetDatabase.FindAssets("t:texture");
            var dict = new Dictionary<string, AssetTextureData>(guids.Length);
            var length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var texture = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (texture == null)
                    continue;


                var textureData = new AssetTextureData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Path = assetPath,
                    Guid = guid,
                    Texture = texture,
                };

                dict.Add(assetPath, textureData);

                EditorUtility.DisplayProgressBar("텍스쳐 확인", string.Format("{0}/{1}", i, length), i / length);
            }


            guids = AssetDatabase.FindAssets("t:Prefab");
            length = (float)guids.Length;

            for (int i = 0; i < guids.Length; i++)
            {
                var guid = guids[i];
                if (guid.IsNullOrEmpty())
                    continue;

                var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (assetPath.IsNullOrEmpty())
                    continue;

                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                    continue;

                var assetData = new AssetObjectData()
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(assetPath),
                    Path = assetPath,
                    FolderPath = System.IO.Path.GetDirectoryName(assetPath),
                    Guid = guid,
                    Object = asset,
                };

                var dependencies = AssetDatabase.GetDependencies(assetPath);
                for (int j = 0; j < dependencies.COUNT(); j++)
                {
                    var path = dependencies[j];
                    AssetTextureData textureData;
                    if (dict.TryGetValue(path, out textureData) == false)
                    {
                        continue;
                    }
                    textureData.RefrenceAssetPrefabDatas.Add(assetData);
                }

                EditorUtility.DisplayProgressBar("텍스쳐 사용 확인", string.Format("{0}/{1}", i, length), i / length);
            }

            var array = dict.Values.ToArray();

            Array.Sort(array, (lhs, rhs) =>
            {
                var ret = lhs.FolderPath.CompareTo(rhs.FolderPath);
                if (ret != 0)
                    return ret;

                var lhsCount = lhs.RefrenceAssetPrefabDatas.COUNT();
                var rhsCount = rhs.RefrenceAssetPrefabDatas.COUNT();

                ret = rhsCount.CompareTo(lhsCount);
                if (ret != 0)
                    return ret;

                return lhs.Name.CompareTo(rhs.Name);
            });

            EditorUtility.ClearProgressBar();
            return array;
        }
    }
}
#endif
