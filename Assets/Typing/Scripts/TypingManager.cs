using System.Collections.Generic;
using Arsene.Typing.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

namespace Arsene.Typing
{
    /// <summary>
    /// TypingManager 日本語用
    /// </summary>
    public class TypingManager : MonoBehaviour
    {
        // タイピングデータ
        [SerializeField] TypingQuestionObject typingQuestionObject;
        readonly TypingDictionary _typingDictionary = new TypingDictionary();

        // データを保持するリスト
        List<string> _fList = new List<string>();
        List<string> _qList = new List<string>();

        // 現在の問題データ
        string _curF = ""; // こなん
        string _curQ = ""; // コナン
        string _curA = ""; // conan => 進行中に変更される

        // 現在の問題の一文字を参照するための変数
        int _curAIndex = 0;
        // 回答を分割したリスト
        List<string> _romSliceList = new List<string>(); // co na n  => 進行中に変更される
        // 回答文字数に合わせたふりがなのインデックス
        List<int> _fIndexList = new List<int>(); // 0 0 1 1 2  => 進行中に変更される
        // 分割されたローマ字単位のインデックス
        List<int> _romIndexList = new List<int>(); // 0 1 0 1 0  => 進行中に変更される

        // UI
        // 参照
        [SerializeField] GameObject _qPanel;
        // 表示設定
        [SerializeField] bool _isVisiableFurigana = false;
        [SerializeField] Color _correctColor;
        [SerializeField] Color _missColor;

        // 表示テキスト
        [SerializeField] Text _fText;
        [SerializeField] Text _qText;
        [SerializeField] Text _aText;

        void Awake()
        {
            Initialize();
        }

        public async void OnEnable()
        {
            await UniTask.WaitUntil(() => Keyboard.current != null);
            Keyboard.current.onTextInput += OnTextInput;
        }

        void Start()
        {
            GameStart();
        }

        public void OnDisable()
        {
            Keyboard.current.onTextInput -= OnTextInput;
            // Keyboard.current.
        }

        public void OnTextInput(char c)
        {
            bool isCorrect = false;
            int fIndex = _fIndexList[_curAIndex];

            // 回答と一致したときの処理
            if (_curA[_curAIndex] == c)
            {
                isCorrect = true;
            }
            // 省略可能なnをタイプしたとき
            else if (fIndex > 0 && _curF[fIndex - 1] == 'ん' && c == 'n' && _curA[_curAIndex - 1] == 'n')
            {
                // 回答更新
                _romSliceList[fIndex - 1] = "nn";
                _curA = string.Join("", GetRomSliceListWithoutSkip(_romSliceList));
                ReCreateIndexList(_romSliceList);

                isCorrect = true;
            }
            // 柔軟な入力判定
            else
            {
                // 2文字で柔軟チェック
                if (_curF.Length > fIndex + 1)
                {
                    string addNextChar = _curF[fIndex].ToString() + _curF[fIndex + 1].ToString();
                    isCorrect = CheckIfFlexibleType(addNextChar, fIndex, c);
                }

                // 2文字がなければ1文字で柔軟チェック
                if (!isCorrect) isCorrect = CheckIfFlexibleType(_curF[fIndex].ToString(), fIndex, c);
            }

            if (isCorrect)
            {
                CorrectChar();
                if (_curAIndex >= _curA.Length)
                {
                    Complete();
                }
                return;
            }

            MissChar();
            return;
        }

        void Initialize()
        {
            // テキストデータをリストに入れる
            _fList.AddRange(typingQuestionObject.fTextAsset.text.Split('\n'));
            _qList.AddRange(typingQuestionObject.qTextAsset.text.Split('\n'));

            if (!_isVisiableFurigana) _fText.gameObject.SetActive(false);
        }

        void GameStart()
        {
            OutputQuestion();
        }

        /// <summary>
        /// タイピング文字を出題する
        /// </summary>
        void OutputQuestion()
        {
            // 問題の選択
            int qIndex = Random.Range(0, _qList.Count - 1);

            // 問題情報を格納
            _curF = _fList[qIndex];
            _curQ = _qList[qIndex];
            (_curA, _romSliceList) = CreateRomanAnswerAndSliceFromKana(_curF);

            // テキストの変更
            _fText.text = _curF;
            _qText.text = _curQ;
            _aText.text = _curA;
        }

        /// <summary>
        /// 現在の問題が完了したときの処理
        /// </summary>
        void Complete()
        {
            CurrentTypeReset();

            // 再度問題出題
            OutputQuestion();
        }

        /// <summary>
        /// 一文字正解したときの処理
        /// </summary>
        void CorrectChar()
        {
            _curAIndex++;
            _aText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_correctColor)}>" + _curA.Substring(0, _curAIndex) + "</color>" + _curA.Substring(_curAIndex);
        }

        /// <summary>
        /// 一文字誤答したときの処理
        /// </summary>
        void MissChar()
        {
            _aText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_correctColor)}>" + _curA.Substring(0, _curAIndex) + "</color>"
                + $"<color=#{ColorUtility.ToHtmlStringRGBA(_missColor)}>" + _curA.Substring(_curAIndex, 1) + "</color>"
                + _curA.Substring(_curAIndex + 1);
        }

        /// <summary>
        /// ふりがなからローマ字の回答とローマ字のスライスリストを作成する
        /// 回答の正規化を行っている
        /// </summary>
        /// <param name="strF">ふりがな</param>
        /// <returns>string = ローマ字の回答, List<string> = ローマ字スライスリスト</returns>
        (string, List<string>) CreateRomanAnswerAndSliceFromKana(string strF)
        {
            List<string> sliceRomList = new List<string>();
            for (int i = 0; i < strF.Length; i++)
            {
                bool isPrevAndCurPair = false;
                string sliceRom = _typingDictionary.Dic[strF[i].ToString()][0];
                // 捨て仮名時の処理（ぁ、ぃ、ぅ、ぇ、ぉ、ゃ、ゅ、ょ）
                bool isSmallKana = "ぁぃぅぇぉゃゅょっ".IndexOf(strF[i]) >= 0;
                if (isSmallKana)
                {
                    // 小さい「つ」のとき
                    if (strF[i] == 'っ' && strF.Length > i + 1)
                    {
                        // 母音かどうか
                        bool isVowel = "aeiouAEIOU".IndexOf(strF[i + 1]) >= 0;
                        if (!isVowel)
                        {
                            // 次の回答の1番目のローマ字を追加する
                            sliceRom = _typingDictionary.Dic[strF[i + 1].ToString()][0][0].ToString();
                        }
                    }
                    else
                    {
                        // 前の文字と一緒に辞書から一致するローマ字を探す
                        string prevAndCurStr = strF[i - 1].ToString() + strF[i].ToString();
                        if (_typingDictionary.Dic.ContainsKey(prevAndCurStr))
                        {
                            // 前の文字のローマ字を削除 かつ 融合された組み合わせを入れる
                            sliceRomList.RemoveAt(sliceRomList.Count - 1);
                            sliceRom = _typingDictionary.Dic[prevAndCurStr][0];
                            isPrevAndCurPair = true;
                        }
                    }
                }
                else if (strF[i] == 'ん')
                {
                    // 「ん」の後ろが母音、ナ行、ヤ行、にゃ、にぃ、にゅ、にぇ、にょのときと「ん」が文末のときは省略できない。
                    List<char> cannotSkipList = new List<char> { 'a', 'i', 'u', 'e', 'o', 'y', 'n' };
                    if (strF.Length > i + 1 && !cannotSkipList.Contains(_typingDictionary.Dic[strF[i + 1].ToString()][0][0]))
                    {
                        sliceRom = "n";
                    }
                }

                sliceRomList.Add(sliceRom);

                if (isPrevAndCurPair) sliceRomList.Add("SKIP");

                // タイピングインデックス情報の設定
                for (int j = 0; j < sliceRom.Length; j++)
                {
                    _fIndexList.Add(i);
                    _romIndexList.Add(j);
                }
            }

            string answer = string.Join("", GetRomSliceListWithoutSkip(sliceRomList));
            return (answer, sliceRomList);
        }

        /// <summary>
        /// 柔軟な入力であるかをチェックする
        /// <param name="curSliceF">柔軟チェックしたいスライスふりがな</param>
        /// <param name="fIndex">目的のふりがなのインデックス</param>
        /// <param name="inputChar">入力ローマ字</param>
        /// </summary>
        /// <returns>true/false = 柔軟な入力あり/柔軟に入力なし</returns>
        bool CheckIfFlexibleType(string curSliceF, int fIndex, char inputChar)
        {
            bool existsFlexible = false;

            if (_typingDictionary.Dic.ContainsKey(curSliceF))
            {
                List<string> romFlexibleList = _typingDictionary.Dic[curSliceF];
                for (int i = 0; i < romFlexibleList.Count; i++)
                {
                    string rom = romFlexibleList[i];
                    int romIndex = _romIndexList[_curAIndex];

                    bool preCheck = true;
                    for (int j = 0; j < romIndex; j++)
                    {
                        if (rom.Length < j + 1 || _romSliceList.Count < fIndex + 1 || _romSliceList[fIndex].Length < j + 1) continue;
                        if (rom[j] != _romSliceList[fIndex][j]) preCheck = false;
                    }

                    // 柔軟チェック
                    if (preCheck && rom.Length >= romIndex && rom[romIndex] == inputChar)
                    {
                        // ローマ字スライスと回答更新
                        _romSliceList[fIndex] = rom;
                        _curA = string.Join("", GetRomSliceListWithoutSkip(_romSliceList));
                        ReCreateIndexList(_romSliceList);

                        if (curSliceF.Length == 1) AddSmallRoman();

                        existsFlexible = true;
                    }
                }
            }

            return existsFlexible;
        }

        /// <summary>
        /// ふりがなインデックスリストとローマ字インデックスリストの再設定
        /// ケース: 回答が変更された際に再設定が必要
        /// </summary>
        /// <param name="romList"></param>
        void ReCreateIndexList(List<string> romList)
        {
            _fIndexList.Clear();
            _romIndexList.Clear();

            for (int i = 0; i < romList.Count; i++)
            {
                string sliceRom = romList[i];

                for (int j = 0; j < sliceRom.Length; j++)
                {
                    _fIndexList.Add(i);
                    _romIndexList.Add(j);
                }
            }
        }

        /// <summary>
        /// 現在の情報をリセットする
        /// </summary>
        void CurrentTypeReset()
        {
            // 情報をリセット
            _curAIndex = 0;
            _fIndexList.Clear();
            _romSliceList.Clear();
            _romIndexList.Clear();
        }

        /// <summary>
        /// 捨て仮名をローマ字回答を追加
        /// </summary>
        void AddSmallRoman()
        {
            int nextRomIndex = _fIndexList[_curAIndex] + 1;

            if (_curF.Length < nextRomIndex + 1) return;

            string nextChar = _curF[nextRomIndex].ToString();
            string nextARom = _typingDictionary.Dic[nextChar][0];

            // aの0番目がx,l以外であれば処理をしない
            if (nextARom[0] != 'x' && nextARom[0] != 'l') return;

            // 挿入と表示の反映
            _romSliceList.Insert(nextRomIndex, nextARom);

            // 更新
            ReCreateIndexList(_romSliceList);
            _curA = string.Join("", GetRomSliceListWithoutSkip(_romSliceList));
        }

        /// <summary>
        /// SKIPを除外したリストを返す
        /// </summary>
        /// <returns></returns>
        List<string> GetRomSliceListWithoutSkip(List<string> romList)
        {
            List<string> returnList = new List<string>();
            foreach (string rom in romList)
            {
                if (rom == "SKIP") continue;
                returnList.Add(rom);
            }
            return returnList;
        }

        /// <summary>
        /// タイピングテキストモードの表示にする
        /// <param name="scale">テキストスケール</param>
        /// </summary>
        public void SetOnlyTextView()
        {
            _qPanel.transform.localScale = new Vector3(3f, 3f, 1f);
        }

        /// <summary>
        /// Viewをデフォルトにする
        /// </summary>
        public void ResetView()
        {
            _qPanel.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
