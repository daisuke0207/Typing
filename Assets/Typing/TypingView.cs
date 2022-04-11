using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Arsene.Typing
{
    /// <summary>
    /// TypingView 日本語用
    /// </summary>
    public class TypingView : MonoBehaviour
    {
        // 表示設定
        [SerializeField] bool _isVisiableFurigana = false;
        [SerializeField] Color _correctColor;
        [SerializeField] Color _missColor;

        // 表示テキスト
        [SerializeField] Text _furiganaText;
        [SerializeField] Text _questionText;
        [SerializeField] Text _answerText;

        // テキストデータアセット
        [SerializeField] TextAsset _furigana;
        [SerializeField] TextAsset _question;

        // 辞書
        [SerializeField] TypingDictionary _typeDictionary;

        // データリスト
        List<string> _fList = new List<string>();
        List<string> _qList = new List<string>();

        // 現在のデータ
        string _curFurigana = ""; // こなん
        string _curQuestion = ""; // コナン
        string _curAnswer = ""; // conan
        int _curAnswerIndex = 0; // 0 => c, 1 => o
        List<string> _romanSliceList = new List<string>();// co na n
        List<int> _furiganaCountList = new List<int>(); // 0 0 1 1 2
        List<int> _romNumList = new List<int>();

        // 正解判定フラグ
        bool _isCorrect = false;

        void Initialize()
        {
            SetList();
            if (!_isVisiableFurigana) _furiganaText.gameObject.SetActive(false);
        }

        void Start()
        {
            Initialize();
            // 問題出題
            Output();
        }

        void OnGUI()
        {
            _isCorrect = false;
            if (Keyboard.current.anyKey.wasPressedThisFrame)
            {
                int fCount = _furiganaCountList[_curAnswerIndex];
                // マイナスの正解
                if (_curAnswer[_curAnswerIndex].ToString() == "-")
                {
                    if (Keyboard.current[Key.Minus].wasPressedThisFrame)
                    {
                        _isCorrect = true;
                        Correct();

                        if (_curAnswerIndex >= _curAnswer.Length)
                        {
                            // 問題変更
                            Output();
                        }
                    }
                }
                // 回答通りの正解
                else if (((KeyControl)Keyboard.current[_curAnswer[_curAnswerIndex].ToString()]).wasPressedThisFrame)
                {
                    _isCorrect = true;
                    Correct();

                    if (_curAnswerIndex >= _curAnswer.Length)
                    {
                        // 問題変更
                        Output();
                    }
                }
                else if (((KeyControl)Keyboard.current[Key.N]).wasPressedThisFrame && fCount > 0 && _romanSliceList[fCount - 1] == "n")
                {
                    _romanSliceList[fCount - 1] = "nn";
                    _curAnswer = string.Join("", GetRomSliceListWithoutSkip());

                    ReCreateList(_romanSliceList);

                    _isCorrect = true;
                    Correct();

                    if (_curAnswerIndex >= _curAnswer.Length)
                    {
                        // 問題変更
                        Output();
                    }
                }
                else
                {
                    string curSliceFurigana = _curFurigana[fCount].ToString();

                    if (fCount < _curFurigana.Length - 1)
                    {
                        // 2文字を考慮した候補検索
                        string addNextChar = _curFurigana[fCount].ToString() + _curFurigana[fCount + 1].ToString();
                        CheckIrregularType(addNextChar, fCount, false);
                    }

                    if (!_isCorrect)
                    {
                        string fChar = _curFurigana[fCount].ToString();
                        CheckIrregularType(fChar, fCount, true);
                    }
                }

                if (!_isCorrect) Miss();
            }
        }

        /// <summary>
        /// テキストデータをリストに入れる
        /// </summary>
        void SetList()
        {
            string[] fArray = _furigana.text.Split('\n');
            _fList.AddRange(fArray);

            string[] qArray = _question.text.Split('\n');
            _qList.AddRange(qArray);
        }

        void AddSmallLetter()
        {
            int nextCharNum = _furiganaCountList[_curAnswerIndex] + 1;

            if (_curFurigana.Length - 1 < nextCharNum) return;

            string nextChar = _curFurigana[nextCharNum].ToString();
            string a = _typeDictionary.dic[nextChar][0];

            // aの0番目がx,l以外であれば処理をしない
            if (a[0] != 'x' || a[0] != 'l') return;

            // 挿入と表示の反映
            _romanSliceList.Insert(nextCharNum, a);
            _romanSliceList.RemoveAt(nextCharNum + 1);

            // 変更したリストを再表示
            ReCreateList(_romanSliceList);
            _curAnswer = string.Join("", GetRomSliceListWithoutSkip());
        }

        /// <summary>
        /// ふりがなをローマ字分割する
        /// </summary>
        /// <param name="str"></param>
        void SetRomanSliceList(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                string sliceRom = _typeDictionary.dic[str[i].ToString()][0];
                if (str[i].ToString() == "ゃ" || str[i].ToString() == "ゅ" || str[i].ToString() == "ょ")
                {
                    sliceRom = "SKIP";
                }
                else if (str[i].ToString() == "っ" && i + 1 < str.Length)
                {
                    sliceRom = _typeDictionary.dic[str[i + 1].ToString()][0][0].ToString();
                }
                else if (i + 1 < str.Length)
                {
                    // 次の文字も含めて辞書から探す
                    string addNextStr = str[i].ToString() + str[i + 1].ToString();
                    if (_typeDictionary.dic.ContainsKey(addNextStr))
                    {
                        sliceRom = _typeDictionary.dic[addNextStr][0];
                    }
                }
                _romanSliceList.Add(sliceRom);

                if (sliceRom == "SKIP") continue;

                for (int j = 0; j < sliceRom.Length; j++)
                {
                    _furiganaCountList.Add(i);
                    _romNumList.Add(j);
                }
            }
        }

        void ReCreateList(List<string> romList)
        {
            _furiganaCountList.Clear();
            _romNumList.Clear();

            for (int i = 0; i < romList.Count; i++)
            {
                string sliceRom = romList[i];
                if (sliceRom == "SKIP") continue;

                for (int j = 0; j < sliceRom.Length; j++)
                {
                    _furiganaCountList.Add(i);
                    _romNumList.Add(j);
                }
            }
        }


        void CheckIrregularType(string curSliceFurigana, int fCount, bool addSmallLetter)
        {
            if (_typeDictionary.dic.ContainsKey(curSliceFurigana))
            {
                List<string> stringList = _typeDictionary.dic[curSliceFurigana];
                for (int i = 0; i < stringList.Count; i++)
                {
                    string rom = stringList[i];
                    int romNum = _romNumList[_curAnswerIndex];

                    bool preCheck = true;
                    for (int j = 0; j < romNum; j++)
                    {
                        if (rom[j] != _romanSliceList[fCount][j]) preCheck = false;
                    }

                    if (preCheck && ((KeyControl)Keyboard.current[rom[romNum].ToString()]).wasPressedThisFrame)
                    {
                        _romanSliceList[fCount] = rom;
                        _curAnswer = string.Join("", _romanSliceList);

                        ReCreateList(_romanSliceList);

                        _isCorrect = true;
                        if (addSmallLetter) AddSmallLetter();

                        Correct();

                        if (_curAnswerIndex >= _curAnswer.Length)
                        {
                            // 問題変更
                            Output();
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// SKIPを除外したリストを返す
        /// </summary>
        /// <returns></returns>
        List<string> GetRomSliceListWithoutSkip()
        {
            List<string> returnList = new List<string>();
            foreach (string rom in _romanSliceList)
            {
                if (rom == "SKIP") continue;
                returnList.Add(rom);
            }
            return returnList;
        }

        /// <summary>
        /// タイピング出題
        /// </summary>
        void Output()
        {
            CurrentInfoReset();

            // 問題の選択
            int _questionIndex = Random.Range(0, _qList.Count - 1);

            // 問題情報を準備
            _curFurigana = _fList[_questionIndex];
            _curQuestion = _qList[_questionIndex];

            SetRomanSliceList(_curFurigana);
            _curAnswer = string.Join("", GetRomSliceListWithoutSkip());

            // テキストの変更
            _furiganaText.text = _curFurigana;
            _questionText.text = _curQuestion;
            _answerText.text = _curAnswer;
        }


        /// <summary>
        /// 現在の情報をリセットする
        /// </summary>
        void CurrentInfoReset()
        {
            _curAnswerIndex = 0;

            _romanSliceList.Clear();
            _furiganaCountList.Clear();
            _romNumList.Clear();
        }

        /// <summary>
        /// 正解した際の処理
        /// </summary>
        void Correct()
        {
            _curAnswerIndex++;
            _answerText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_correctColor)}>" + _curAnswer.Substring(0, _curAnswerIndex) + "</color>" + _curAnswer.Substring(_curAnswerIndex);
        }

        /// <summary>
        /// 間違えた際の処理
        /// </summary>
        void Miss()
        {
            _answerText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_correctColor)}>" + _curAnswer.Substring(0, _curAnswerIndex) + "</color>"
                + $"<color=#{ColorUtility.ToHtmlStringRGBA(_missColor)}>" + _curAnswer.Substring(_curAnswerIndex, 1) + "</color>"
                + _curAnswer.Substring(_curAnswerIndex + 1);
        }
    }
}
