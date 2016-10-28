using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using OpenNLP.Tools.PosTagger;

namespace Linguistics
{
    public class Structurizer
    {
        public static SPO GetStructure(string input)
        {
            var splitSentence = new List<string>(input.Split(' '));

            for (int i = 0; i < splitSentence.Count; i++)
            {
                splitSentence.Remove("");
            }

            var sentence = splitSentence.ToArray();

            EnglishMaximumEntropyPosTagger posTagger = new EnglishMaximumEntropyPosTagger("EnglishPOS.nbin");
            string[] tags = posTagger.Tag(sentence);
            var analyzed = new List<TypeWordCombo>();
            for (int i = 0; i < sentence.Length; i++)
            {
                analyzed.Add(new TypeWordCombo(tags[i], sentence[i]));
            }

            var spo = new SPO();
            if (analyzed[0].Type == "NNP" || analyzed[0].Type == "NNS" || analyzed[0].Type == "PRP")
            {
                spo.Subject = analyzed[0].Word;
            }

            //TODO: check for negation
            if (analyzed[1].Type == "VBG" || analyzed[1].Type == "VBN" || analyzed[1].Type == "VBP" || analyzed[1].Type == "MD" || analyzed[1].Type == "NN" || Cache.Instance.VerbExceptions.Contains(analyzed[1].Word))
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
