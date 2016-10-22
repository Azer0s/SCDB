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
            var analayzed = new List<TypeWordCombo>();
            for (int i = 0; i < sentence.Length; i++)
            {
                analayzed.Add(new TypeWordCombo(tags[i], sentence[i]));
            }

            var spo = new SPO();
            if (analayzed[0].Type == "NNP" || analayzed[0].Type == "PRP")
            {
                spo.Subject = analayzed[0].Word;
            }

            if (analayzed[1].Type == "VBG" || analayzed[1].Type == "VBN" || analayzed[1].Type == "VPB" || analayzed[1].Type == "MD" || Cache.Instance.VerbExceptions.Contains(analayzed[1].Word))
            {
                spo.Predicate = analayzed[1].Word;
            }

            if (analayzed[1].Type == "VBZ" && !Cache.Instance.VerbExceptions.Contains(analayzed[1].Word))
            {
                spo.Predicate = GetNormalForm(analayzed[1].Word);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 2; i < analayzed.Count; i++)
            {
                sb.Append(analayzed[i].Word);
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
