using System.Collections.Generic;

namespace Arsene.Typing
{
    /// <summary>
    /// 日本語と英字のの対応表
    /// </summary>
    public class TypingDictionary
    {
        // NOTE: Google日本語入力の対応にする
        public readonly Dictionary<string, List<string>> Dic = new Dictionary<string, List<string>>() {
            {"あ", new List<string>{"a"}},{"い", new List<string>{"i"}},{"う", new List<string>{"u", "wu"}},
            {"え", new List<string>{"e"}},{"お", new List<string>{"o"}},
            {"か", new List<string>{"ka","ca"}},{"き", new List<string>{"ki"}},{"く", new List<string>{"ku","cu","qu"}},
            {"け", new List<string>{"ke"}},{"こ", new List<string>{"ko","co"}},
            {"さ", new List<string>{"sa"}},{"し", new List<string>{"si","ci","shi"}},{"す", new List<string>{"su"}},
            {"せ", new List<string>{"se","ce"}},{"そ", new List<string>{"so"}},
            {"た", new List<string>{"ta"}},{"ち", new List<string>{"ti","chi","cyi"}},{"つ", new List<string>{"tu","tsu"}},
            {"て", new List<string>{"te"}},{"と", new List<string>{"to"}},
            {"な", new List<string>{"na"}},{"に", new List<string>{"ni"}},{"ぬ", new List<string>{"nu"}},
            {"ね", new List<string>{"ne"}},{"の", new List<string>{"no"}},
            {"は", new List<string>{"ha"}},{"ひ", new List<string>{"hi"}},{"ふ", new List<string>{"hu","fu"}},
            {"へ", new List<string>{"he"}},{"ほ", new List<string>{"ho"}},
            {"ま", new List<string>{"ma"}},{"み", new List<string>{"mi"}},{"む", new List<string>{"mu"}},
            {"め", new List<string>{"me"}},{"も", new List<string>{"mo"}},
            {"や", new List<string>{"ya"}},{"ゆ", new List<string>{"yu"}},{"よ", new List<string>{"yo"}},
            {"ら", new List<string>{"ra"}},{"り", new List<string>{"ri"}},{"る", new List<string>{"ru"}},
            {"れ", new List<string>{"re"}},{"ろ", new List<string>{"ro"}},
            {"わ", new List<string>{"wa"}},{"を", new List<string>{"wo"}},{"ん", new List<string>{"nn", "xn"}},
            {"が", new List<string>{"ga"}},{"ぎ", new List<string>{"gi"}},{"ぐ", new List<string>{"gu"}},
            {"げ", new List<string>{"ge"}},{"ご", new List<string>{"go"}},
            {"ざ", new List<string>{"za"}},{"じ", new List<string>{"zi","ji"}},{"ず", new List<string>{"zu"}},
            {"ぜ", new List<string>{"ze"}},{"ぞ", new List<string>{"zo"}},
            {"だ", new List<string>{"da"}},{"ぢ", new List<string>{"di"}},{"づ", new List<string>{"du"}},
            {"で", new List<string>{"de"}},{"ど", new List<string>{"do"}},
            {"ば", new List<string>{"ba"}},{"び", new List<string>{"bi"}},{"ぶ", new List<string>{"bu"}},
            {"べ", new List<string>{"be"}},{"ぼ", new List<string>{"bo"}},
            {"ぱ", new List<string>{"pa"}},{"ぴ", new List<string>{"pi"}},{"ぷ", new List<string>{"pu"}},
            {"ぺ", new List<string>{"pe"}},{"ぽ", new List<string>{"po"}},
            {"ぁ", new List<string>{"xa","la"}},{"ぃ", new List<string>{"xi","li"}},{"ぅ", new List<string>{"xu","lu"}},
            {"ぇ", new List<string>{"xe","le"}},{"ぉ", new List<string>{"xo","lo"}},
            {"っ", new List<string>{"xtu","ltu", "xtsu", "ltsu"}},
            {"ゃ", new List<string>{"xya","lya"}},{"ゅ", new List<string>{"xyu","lyu"}},{"ょ", new List<string>{"xyo","lyo"}},
            {"きゃ", new List<string>{"kya"}},{"きぃ", new List<string>{"kyi"}},{"きゅ", new List<string>{"kyu"}},
            {"きぇ", new List<string>{"kye"}},{"きょ",new List<string>{"kyo"}},
            {"しゃ", new List<string>{"sya","sha"}},{"しぃ",new List<string>{"syi"}},{"しゅ", new List<string>{"syu","shu"}},
            {"しぇ", new List<string>{"sye","she"}},{"しょ", new List<string>{"syo","sho"}},
            {"ちゃ", new List<string>{"tya","cha","cya"}},{"ちぃ", new List<string>{"tyi","chi","cyi"}},
            {"ちゅ", new List<string>{"tyu","chu","cyu"}},{"ちぇ", new List<string>{"tye","che","cye"}},{"ちょ", new List<string>{"tyo","cho","cyo"}},
            {"にゃ", new List<string>{"nya"}},{"にぃ", new List<string>{"nyi"}},{"にゅ",new List<string>{"nyu"}},
            {"にぇ", new List<string>{"nye"}},{"にょ", new List<string>{"nyo"}},
            {"ひゃ", new List<string>{"hya"}},{"ひぃ", new List<string>{"hyi"}},{"ひゅ", new List<string>{"hyu"}},
            {"ひぇ", new List<string>{"hye"}},{"ひょ", new List<string>{"hyo"}},
            {"みゃ", new List<string>{"mya"}},{"みぃ", new List<string>{"myi"}},{"みゅ", new List<string>{"myu"}},
            {"みぇ", new List<string>{"mye"}},{"みょ", new List<string>{"myo"}},
            {"りゃ", new List<string>{"rya"}},{"りぃ", new List<string>{"ryi"}},{"りゅ", new List<string>{"ryu"}},
            {"りぇ", new List<string>{"rye"}},{"りょ", new List<string>{"ryo"}},
            {"ぎゃ", new List<string>{"gya"}},{"ぎぃ", new List<string>{"gyi"}},{"ぎゅ", new List<string>{"gyu"}},
            {"ぎぇ", new List<string>{"gye"}},{"ぎょ", new List<string>{"gyo"}},
            {"じゃ", new List<string>{"ja","zya"}},{"じぃ", new List<string>{"zhi"}},{"じゅ", new List<string>{"ju","zyu"}},
            {"じぇ", new List<string>{"je","zye"}},{"じょ", new List<string>{"jo","zyo"}},
            {"ぢゃ", new List<string>{"dya"}},{"ぢぃ", new List<string>{"dyi"}},{"ぢゅ", new List<string>{"dyu"}},
            {"ぢぇ", new List<string>{"dye"}},{"ぢょ", new List<string>{"dyo"}},
            {"びゃ", new List<string>{"bya"}},{"びぃ", new List<string>{"byi"}},{"びゅ", new List<string>{"byu"}},
            {"びぇ", new List<string>{"bye"}},{"びょ", new List<string>{"byo"}},
            {"てゃ", new List<string>{"tha"}},{"てぃ", new List<string>{"thi"}},{"てゅ", new List<string>{"thu"}},
            {"てぇ", new List<string>{"the"}},{"てょ", new List<string>{"tho"}},
            {"うぁ", new List<string>{"wha"}},{"うぃ", new List<string>{"whi","wi"}},{"うぇ", new List<string>{"whe","we"}},{"うぉ", new List<string>{"who"}},
            {"でゃ", new List<string>{"dha"}},{"でぃ", new List<string>{"dhi"}},{"でゅ", new List<string>{"dhu"}},
            {"でぇ", new List<string>{"dhe"}},{"でょ", new List<string>{"dho"}},
            {"くぁ", new List<string>{"qa"}},{"くぃ", new List<string>{"qi","qyi"}},
            {"くぇ", new List<string>{"qe","qye"}},{"くぉ", new List<string>{"qo"}},
            {"ふぁ", new List<string>{"fa"}},{"ふぃ", new List<string>{"fi"}},{"ふぇ", new List<string>{"fe"}},{"ふぉ", new List<string>{"fo"}},
            {"ゔぁ", new List<string>{"va"}},{"ゔぃ", new List<string>{"vi"}},{"ゔ", new List<string>{"vu"}},
            {"ゔぇ", new List<string>{"ve"}},{"ゔぉ", new List<string>{"vo"}},
            {"ぴゃ", new List<string>{"pya"}},{"ぴぃ", new List<string>{"pyi"}},{"ぴゅ", new List<string>{"pyu"}},
            {"ぴぇ", new List<string>{"pye"}},{"ぴょ", new List<string>{"pyo"}},
            {"、", new List<string>{","}},{"。", new List<string>{"."}},{"「", new List<string>{"["}},{"」", new List<string>{"]"}},
            {"ー", new List<string>{"-"}},{"〜", new List<string>{"~"}},{"！", new List<string>{"!"}},{"？", new List<string>{"?"}},
        };
    }
}
