using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OpenNLP.Tools.PosTagger;

namespace Linguistics
{
    /// <summary>
    /// Natural language processor of the program.
    /// </summary>
    public class Structurizer
    {

        public static EnglishMaximumEntropyPosTagger PosTagger = new EnglishMaximumEntropyPosTagger("EnglishPOS.nbin");

        /// <summary>
        /// Tags a sentence and converts it to a SPO object.
        /// </summary>
        /// <param name="input">The sentence you want to analyze.</param>
        /// <param name="question">Whether the sentence is a question.</param>
        /// <returns>The sentence as a SPO object.</returns>
        public static SPO GetStructure(string input, bool question)
        {
            var splitSentence = new List<string>(input.Split(' '));

            for (int i = 0; i < splitSentence.Count; i++)
            {
                splitSentence.Remove("");
            }

            var sentence = splitSentence.ToArray();

            string[] tags = PosTagger.Tag(sentence);
            var analyzed = new List<TypeWordCombo>();
            for (int i = 0; i < sentence.Length; i++)
            {
                analyzed.Add(new TypeWordCombo(tags[i], sentence[i]));
            }

            var spo = new SPO();
            if ((analyzed[0].Type == "NNP" || analyzed[0].Type == "NNS" || analyzed[0].Type == "PRP") && !question)
            {
                spo.Subject = analyzed[0].Word;
                spo.IsExpression = true;
            }

            if ((analyzed[0].Type == "NNP" || analyzed[0].Type == "NNS" || analyzed[0].Type == "PRP" || analyzed[0].Word.ToLower().Equals("listall")) && question)
            {
                spo.Subject = analyzed[0].Word;
                spo.IsExpression = true;
            }

            if ((analyzed[0].Type == "WP" || analyzed[0].Type == "WRB" || analyzed[0].Type == "WDT" || analyzed[0].Type == "WP$") && question)
            {
                spo.Subject = analyzed[0].Word;
                spo.IsExpression = false;
            }

            if (analyzed[1].Type == "VBG" || analyzed[1].Type == "VBN" || analyzed[1].Type == "VBD" || analyzed[1].Type == "VBP" || analyzed[1].Type == "VB" || analyzed[1].Type == "MD" || analyzed[1].Type == "NN" || analyzed[0].Word.ToLower().Equals("listall") || Cache.Instance.VerbExceptions.Contains(analyzed[1].Word))
            {
                spo.Predicate = analyzed[1].Word;
            }

            if ((analyzed[1].Type == "VBZ" || analyzed[1].Type == "NNS") && !Cache.Instance.VerbExceptions.Contains(analyzed[1].Word))
            {
                spo.Predicate = GetNormalForm(analyzed[1].Word);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < analyzed.Count; i++)
            {
                sb.Append(analyzed[i].Word);
                sb.Append(" ");
            }
            spo.Object = sb.ToString();

            return spo;
        }

        /// <summary>
        /// Gets the infinitiv of a verb in the third person singular (present=.
        /// </summary>
        /// <param name="verb">The verb you want to convert.</param>
        /// <returns>The infinitiv of the verb.</returns>
        public static string GetNormalForm(string verb)
        {
            if (verb.Substring(verb.Length - 3) == "ies")
            {
                return verb.Substring(0, verb.Length - 3) + "y";
            }
            if (verb.Substring(verb.Length - 2) == "es")
            {
                return verb.Substring(0, verb.Length - 2);
            }
            if (verb.Substring(verb.Length - 1) == "s")
            {
                return verb.Substring(0, verb.Length - 1);
            }
            return null;
        }
    }
}
