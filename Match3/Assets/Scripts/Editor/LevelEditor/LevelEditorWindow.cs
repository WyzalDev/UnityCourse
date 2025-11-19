// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using Core.Data;

namespace Editor.LevelEditor
{
    public class LevelEditorWindow : EditorWindow
    {
        private const string LevelsPath = "Levels";
        private const string ElementsTexturePath = "Assets/Textures/Board";

        private const int GemsTypeNumber = 5;
        private const int HeightLimit = 11;
        private const int WidthLimit = 7;

        private const string ValidationErrorMessageEmptyElements = "Remove empty elements";
        private const string ValidationErrorMessageFieldNotCreated = "Field with width/height = 0 not acceptable";
        private const string ValidationErrorMessageLevelNameIsEmpty = "Level name field need to be filled";
        private const string ValidationErrorMessageLevelNameAlreadyExists = "Level with that name already exists";
        private const string ValidationErrorMessageMovesLimitationIs0 = "Moves limitation value must greater be than 0";
        private const string ValidationErrorMessageGoalRequiredValueIs0 = "Goal required value must be greater than 0";
        private const string ValidationErrorMessageGoalScoreMultiplierIs0 = "Goal score mul must be greater than 0";

        private LevelConfig _levelConfig;

        private int _seed;
        private int _movesLimitation;

        private int _requiredValue;
        private GoalType? _goalType;
        private int _scoreMultiplier;
        private ElementType _goalElementType;

        private int _fieldHeight;
        private int _fieldWidth;
        private ElementType[][] _field;

        private string _levelName = string.Empty;
        private string _loadLevelName;

        private GUIStyle _centeredTextStyle;
        private GUIStyle _foldOutStyle;
        private GUIStyle _centeredRedTextStyle;
        private GUIStyle _centeredTextFieldStyle;
        private GUIStyle _centeredButtonStyle;
        private GUIStyle _fieldButtonStyle;
        private GUIStyle _leftFieldButtonStyle;
        private GUIStyle _rightFieldButtonStyle;
        private GUIStyle _centeredDropdownStyle;

        private Dictionary<ElementType, Texture2D> _elementTextures;

        private bool _isLevelLoaded;
        private bool _isLoadingFileNotExists;
        private bool _isCreatingFileAlreadyExistsOrNull;

        private bool _isGoalFoldoutExpanded;
        private bool _isFieldFoldoutExpanded;

        private bool _isValidationSucces;
        private string _validationErrorMessage;

        [MenuItem("Tools/Custom/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>();
        }

        private void CreateGUI()
        {
            Restore();
        }

        private void OnGUI()
        {
            TrySetStyles();
            TrySetTextures();

            if (_isLevelLoaded)
                OnLevelLoaded();
            else
                OnLevelNotLoaded();
        }

        private void TrySetStyles()
        {
            _centeredTextStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            _foldOutStyle ??= new GUIStyle(GUI.skin.FindStyle("Foldout"))
            {
                margin = new RectOffset(25, 0, 0, 0)
            };

            _centeredRedTextStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(50, 50, 10, 10),
                normal = { textColor = Color.red },
                hover = { textColor = Color.red },
                active = { textColor = Color.red },
                focused = { textColor = Color.red }
            };

            _centeredTextFieldStyle ??= new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(50, 50, 10, 10)
            };

            _centeredButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(50, 50, 10, 10),
            };

            _fieldButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(5, 5, 5, 5),
            };

            _leftFieldButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(50, 5, 5, 5),
            };

            _rightFieldButtonStyle ??= new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(5, 50, 5, 5),
            };

            _centeredDropdownStyle ??= new GUIStyle(GUI.skin.FindStyle("Dropdown"))
            {
                alignment = TextAnchor.MiddleCenter,
                margin = new RectOffset(50, 50, 10, 10),
            };
        }

        private void TrySetTextures()
        {
            if (_elementTextures != null)
                return;

            var enumMembers = Enum.GetValues(typeof(ElementType)) as ElementType[];

            if (enumMembers == null)
                return;

            _elementTextures = new Dictionary<ElementType, Texture2D>();

            foreach (var elementType in enumMembers)
                _elementTextures[elementType] =
                    AssetDatabase.LoadAssetAtPath<Texture2D>($"{ElementsTexturePath}/{elementType.ToString()}.png");
        }

        private void OnLevelNotLoaded()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Create Or Load Level", _centeredTextStyle);

            GUILayout.BeginHorizontal();
            _loadLevelName = EditorGUILayout.TextField("Level Name", _loadLevelName, _centeredTextFieldStyle);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Level", _centeredButtonStyle))
                CreateLevel();

            if (GUILayout.Button("Load Level", _centeredButtonStyle))
                LoadLevel();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (_isCreatingFileAlreadyExistsOrNull)
                GUILayout.Label("Name is empty or Level already exists", _centeredRedTextStyle);
            else
                GUILayout.FlexibleSpace();

            if (_isLoadingFileNotExists)
                GUILayout.Label("Level with current Name not exists", _centeredRedTextStyle);
            else
                GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void CreateLevel()
        {
            if (_loadLevelName is null || _loadLevelName.Length == 0 ||
                File.Exists($"Assets/Resources/{LevelsPath}/{_loadLevelName}.txt"))
            {
                _isCreatingFileAlreadyExistsOrNull = true;
                _isLoadingFileNotExists = false;
                return;
            }

            _levelConfig = new LevelConfig();

            SetConfigValues(true);
            _levelName = _loadLevelName;
            _isLevelLoaded = true;
        }

        private void LoadLevel()
        {
            if (_loadLevelName is null || _loadLevelName.Length == 0 ||
                !File.Exists($"Assets/Resources/{LevelsPath}/{_loadLevelName}.txt"))
            {
                _isLoadingFileNotExists = true;
                _isCreatingFileAlreadyExistsOrNull = false;
                return;
            }

            try
            {
                var loadedConfig = File.ReadAllText($"Assets/Resources/{LevelsPath}/{_loadLevelName}.txt");

                _levelConfig = JsonUtility.FromJson<LevelConfig>(loadedConfig);
                SetConfigValues();

                _levelName = _loadLevelName;
                _isLevelLoaded = true;

                Debug.Log($"Loaded File {LevelsPath}/{_loadLevelName}.txt");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load Level {_loadLevelName} \n {e.Message}");
            }
        }

        private void SetConfigValues(bool isRestore = false)
        {
            _seed = isRestore ? 0 : _levelConfig.Seed;
            _movesLimitation = isRestore ? 0 : _levelConfig.MovesLimitation;
            _requiredValue = isRestore ? 0 : _levelConfig.GoalsConfig.RequiredValue;
            _goalType = isRestore ? 0 : _levelConfig.GoalsConfig.GoalType;
            _scoreMultiplier = isRestore ? 0 : _levelConfig.GoalsConfig.ScoreMultiplier;
            _goalElementType = isRestore ? ElementType.None : _levelConfig.GoalsConfig.GoalElementType;
            _fieldHeight = isRestore ? 0 : _levelConfig.FieldConfig.Height;
            _fieldWidth = isRestore ? 0 : _levelConfig.FieldConfig.Width;

            if (isRestore)
            {
                _field = null;
                return;
            }

            _field = new ElementType[_fieldHeight][];
            for (var i = 0; i < _fieldHeight; i++)
            {
                _field[i] = new ElementType[_fieldWidth];
                for (var j = 0; j < _fieldWidth; j++)
                    _field[i][j] = _levelConfig.FieldConfig.Field[i].FieldPart[j];
            }
        }

        private void OnLevelLoaded()
        {
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();

            _levelName = EditorGUILayout.TextField("Level Name", _levelName, _centeredTextFieldStyle);
            _seed = EditorGUILayout.IntField("Seed", _seed, _centeredTextFieldStyle);
            _movesLimitation =
                EditorGUILayout.IntField("MovesLimitation", _movesLimitation, _centeredTextFieldStyle);

            ShowGoalsMenu();
            ShowFieldMenu();

            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Back", _centeredButtonStyle))
            {
                Restore();
                _isLevelLoaded = false;
            }

            if (GUILayout.Button("Save Level", _centeredButtonStyle))
            {
                _isValidationSucces = ValidateBeforeSave();
                if (_isValidationSucces)
                    SaveLevel();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (_isValidationSucces)
                GUILayout.FlexibleSpace();
            else
                GUILayout.Label(_validationErrorMessage, _centeredRedTextStyle);
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }

        private void ShowGoalsMenu()
        {
            _isGoalFoldoutExpanded =
                EditorGUILayout.Foldout(_isGoalFoldoutExpanded, "Goal Settings", _foldOutStyle);

            if (!_isGoalFoldoutExpanded)
                return;

            GUILayout.BeginHorizontal();
            if (EditorGUILayout.DropdownButton(new GUIContent(_goalType.ToString()), FocusType.Passive,
                    _centeredDropdownStyle))
            {
                var genericMenu = new GenericMenu();

                genericMenu.AddItem(new GUIContent("Destroy gems of certain type"),
                    _goalType == GoalType.DestroyGems,
                    GoalsDropDownCallback, GoalType.DestroyGems);

                genericMenu.AddItem(new GUIContent("Destroy obstacles of certain type"),
                    _goalType == GoalType.DestroyObstacles, GoalsDropDownCallback, GoalType.DestroyObstacles);

                genericMenu.AddItem(new GUIContent("Reach certain score"), _goalType == GoalType.GetScore,
                    GoalsDropDownCallback, GoalType.GetScore);

                genericMenu.ShowAsContext();
            }

            switch (_goalType)
            {
                case GoalType.DestroyGems:
                    if (!_goalElementType.IsGem())
                        _goalElementType = ElementType.RedGem;

                    if (GUILayout.Button(_elementTextures[_goalElementType], _centeredButtonStyle,
                            GUILayout.Height(30), GUILayout.Width(30)))
                        ShowGemGoalTypeGenericMenu();

                    _requiredValue =
                        EditorGUILayout.IntField("Required", _requiredValue, _centeredTextFieldStyle);
                    break;
                case GoalType.DestroyObstacles:
                    if (!_goalElementType.IsObstacle())
                        _goalElementType = ElementType.Rock;

                    if (GUILayout.Button(_elementTextures[_goalElementType], _centeredButtonStyle,
                            GUILayout.Height(30), GUILayout.Width(30)))
                        ShowObstacleGoalTypeGenericMenu();

                    _requiredValue =
                        EditorGUILayout.IntField("Required", _requiredValue, _centeredTextFieldStyle);
                    break;
                case GoalType.GetScore:
                    _requiredValue =
                        EditorGUILayout.IntField("Required", _requiredValue, _centeredTextFieldStyle);

                    _scoreMultiplier = EditorGUILayout.IntField("Score Multiplier", _scoreMultiplier,
                        _centeredTextFieldStyle);
                    break;
                default:
                    GUILayout.FlexibleSpace();
                    GUILayout.FlexibleSpace();
                    break;
            }

            GUILayout.EndHorizontal();
        }

        private void GoalsDropDownCallback(object userData)
        {
            if (userData is not GoalType goalType)
                return;

            _goalType = goalType;
        }

        private void ShowFieldMenu()
        {
            _isFieldFoldoutExpanded =
                EditorGUILayout.Foldout(_isFieldFoldoutExpanded, "Field Settings", _foldOutStyle);

            if (!_isFieldFoldoutExpanded)
                return;

            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            _fieldHeight = EditorGUILayout.IntSlider("Height", _fieldHeight, 0, HeightLimit);
            GUILayout.Space(50);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            _fieldWidth = EditorGUILayout.IntSlider("Width", _fieldWidth, 0, WidthLimit);
            GUILayout.Space(50);
            GUILayout.EndHorizontal();

            FieldCorrection();

            ShowFillRandomlyWithGemsButton();

            for (var i = 0; i < _fieldHeight; i++)
            {
                GUILayout.BeginHorizontal();
                for (var j = 0; j < _fieldWidth; j++)
                {
                    var style = _fieldButtonStyle;

                    if (j == _fieldWidth - 1)
                        style = _rightFieldButtonStyle;

                    if (j == 0)
                        style = _leftFieldButtonStyle;

                    if (GUILayout.Button(_elementTextures[_field[i][j]], style,
                            GUILayout.Height(30), GUILayout.Width(30)))
                        ShowFieldElementGenericMenu(i, j);
                }
                GUILayout.EndHorizontal();
            }
        }

        private void FieldCorrection()
        {
            if (_fieldWidth == 0 || _fieldHeight == 0)
                return;

            if (_field == null)
            {
                AdjustField();
                return;
            }

            var isFieldSizeChanged = _fieldHeight != _field.Length || _fieldWidth != _field[0].Length;

            if (isFieldSizeChanged)
                AdjustField();
        }

        private void AdjustField()
        {
            var field = new ElementType[_fieldHeight][];

            for (var i = 0; i < _fieldHeight; i++)
            {
                field[i] = new ElementType[_fieldWidth];
                for (var j = 0; j < _fieldWidth; j++)
                {
                    if (_field != null && i < _field.Length && j < _field[0].Length)
                        field[i][j] = _field[i][j];
                    else
                        field[i][j] = ElementType.None;
                }
            }

            _field = field;
        }

        private void ShowFillRandomlyWithGemsButton()
        {
            if (_fieldWidth == 0 || _fieldHeight == 0 || _field == null)
                return;

            if (GUILayout.Button("Fill with gems randomly", _centeredButtonStyle))
                FillRandomlyWithGems();
        }

        private void FillRandomlyWithGems()
        {
            for (var i = 0; i < _fieldHeight; i++)
            {
                for (var j = 0; j < _fieldWidth; j++)
                    _field[i][j] = (ElementType)Random.Range(1, GemsTypeNumber + 1);
            }
        }

        private void ShowFieldElementGenericMenu(int i, int j)
        {
            var menu = new GenericMenu();

            var enumMembers = Enum.GetValues(typeof(ElementType)) as ElementType[];

            if (enumMembers == null)
                return;

            for (var index = 1; index < enumMembers.Length; index++)
                menu.AddItem(new GUIContent(enumMembers[index].ToString()), _field[i][j] == enumMembers[index],
                    FieldElementGenericMenuCallback, new GenericMenuElementData
                    {
                        i = i,
                        j = j,
                        type = enumMembers[index]
                    });

            menu.ShowAsContext();
        }

        private void FieldElementGenericMenuCallback(object userData)
        {
            if (userData is not GenericMenuElementData data)
                return;

            _field[data.i][data.j] = data.type;
        }

        private void ShowGemGoalTypeGenericMenu()
        {
            var menu = new GenericMenu();

            var enumMembers = Enum.GetValues(typeof(ElementType)) as ElementType[];

            if (enumMembers == null)
                return;

            for (var i = 1; i <= GemsTypeNumber + 1; i++)
                menu.AddItem(new GUIContent(enumMembers[i].ToString()), _goalElementType == enumMembers[i],
                    GoalElementTypeGenericMenuCallback, enumMembers[i]);

            menu.ShowAsContext();
        }

        private void ShowObstacleGoalTypeGenericMenu()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent(nameof(ElementType.Rock)), _goalElementType == ElementType.Rock,
                GoalElementTypeGenericMenuCallback, ElementType.Rock);

            menu.AddItem(new GUIContent(nameof(ElementType.Ice)), _goalElementType == ElementType.Ice,
                GoalElementTypeGenericMenuCallback, ElementType.Ice);

            menu.ShowAsContext();
        }

        private void GoalElementTypeGenericMenuCallback(object userData)
        {
            if (userData is not ElementType data)
                return;

            _goalElementType = data;
        }

        private bool ValidateBeforeSave()
        {
            if (string.IsNullOrEmpty(_levelName))
            {
                _validationErrorMessage = ValidationErrorMessageLevelNameIsEmpty;
                return false;
            }

            if (!string.Equals(_levelName, _loadLevelName))
            {
                if (File.Exists($"Assets/Resources/{LevelsPath}/{_levelName}.txt"))
                {
                    _validationErrorMessage = ValidationErrorMessageLevelNameAlreadyExists;
                    return false;
                }
            }

            if (_movesLimitation == 0)
            {
                _validationErrorMessage = ValidationErrorMessageMovesLimitationIs0;
                return false;
            }

            if (_requiredValue == 0)
            {
                _validationErrorMessage = ValidationErrorMessageGoalRequiredValueIs0;
                return false;
            }

            if (_goalType == GoalType.GetScore && _scoreMultiplier == 0)
            {
                _validationErrorMessage = ValidationErrorMessageGoalScoreMultiplierIs0;
                return false;
            }

            if (_field == null || _fieldHeight == 0 || _fieldWidth == 0)
            {
                _validationErrorMessage = ValidationErrorMessageFieldNotCreated;
                return false;
            }

            for (var i = 0; i < _fieldHeight; ++i)
            {
                for (var j = 0; j < _fieldWidth; j++)
                {
                    if (_field[i][j] != ElementType.None)
                        continue;

                    _validationErrorMessage = ValidationErrorMessageEmptyElements;
                    return false;
                }
            }

            return true;
        }

        private void SaveLevel()
        {
            var newGoalsConfig = new GoalsConfig(_requiredValue, _goalType.GetValueOrDefault(), _scoreMultiplier,
                _goalElementType);
            var newFieldConfig = new FieldConfig(_fieldHeight, _fieldWidth, _field);
            var newConfig = new LevelConfig(_movesLimitation, _seed, newGoalsConfig, newFieldConfig);

            if (!string.Equals(_levelName, _loadLevelName))
            {
                try
                {
                    File.Delete($"Assets/Resources/{LevelsPath}/{_loadLevelName}.txt");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }

            try
            {
                File.WriteAllText($"Assets/Resources/{LevelsPath}/{_levelName}.txt",
                    JsonUtility.ToJson(newConfig));

                Debug.Log(string.Equals(_levelName, _loadLevelName)
                    ? $"Saved File {LevelsPath}/{_levelName}.txt"
                    : $"Renamed ({LevelsPath}/{_loadLevelName}.txt and Saved File {LevelsPath}/{_levelName}.txt"
                );
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            _isLevelLoaded = false;
            Restore();
        }

        private void Restore()
        {
            _levelName = string.Empty;
            SetConfigValues(true);
            _levelConfig = null;
            _isCreatingFileAlreadyExistsOrNull = false;
            _isLoadingFileNotExists = false;
            _isValidationSucces = false;
            _validationErrorMessage = string.Empty;
        }

        [Serializable]
        private class GenericMenuElementData
        {
            public int i;
            public int j;
            public ElementType type;
        }
    }
}