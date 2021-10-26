using System;

namespace XeviousPlayer2
{
    public static class Gen
    {
        public const string OPENMEDIA_DIALOG_FILTER = "*.3g2; *.3gp; *.3gp2; *.3gpp; *.aac; *.adts; *.asf; *.avi; *.m4a; *.m4v; *.mkv; *.mov; *.mp3; *.mp4; *.mpeg; *.mpg; *.sami; *.smi; *.wav; *.webm; *.wma; *.wmv|";
        public static string PastaMp3 = "";
        public static int Lista = 0;

        public static string FA(string Texto)
        {
            return '"' + Texto + '"';
        }

        public static string TrataNome(string Nome, string Banda)
        {
            int codLetra = 0;
            string LetrasValidas = "áàéèêâíìîóòôúùüãõç";
            string CaractsInvalidos = " -._(){}&";
            char AspaD = (Char)34;
            char Aspa = (Char)44;
            Nome = RetiraLetras(Nome, CaractsInvalidos);
            Nome = RetiraLetras(Nome, CaractsInvalidos, true);
            Nome = Nome.Replace(AspaD, Aspa);
            for (int a = 0; a < Nome.Length; a++)
            {
                char Letra = Convert.ToChar(Nome[a]);
                codLetra = Convert.ToInt32(Letra);
                if ((codLetra < 32) || (codLetra > 122))
                {
                    if (LetrasValidas.IndexOf(Letra) == -1)
                    {
                        char[] letras = Nome.ToCharArray();
                        letras[a] = '_';
                        Nome = new string(letras);
                    }
                }
            }
            Nome = Nome.Replace(@"/", " ");
            // Verificar se os 3 primeiros caracteres são numericos, se for, retirar
            // Verificar se os 2 primeiros caracteres são numericos, se for, retirar
            if (Nome.Length>2)
                if (Nome.Substring(2, 2) == "0 ")
                    Nome = Nome.Substring(2);
            Nome = TiraNomeDaBanda(Nome, Banda);
            return Nome;
        }

        public static string RetNomePeloCaminho(string Caminho)
        {
            string[] Nomes = Caminho.Split('\\');
            string[] Nome = Nomes[Nomes.Length - 1].Split('.');
            return Nome[0];
        }

        private static string TiraNomeDaBanda(string Nome, string Banda)
        {
            int PosBanda = Nome.LastIndexOf(Banda);
            if (Banda.Length>0)
            {
                if ((PosBanda > -1) && (Nome.Length > Banda.Length))
                    if (PosBanda == 0)
                        if ((Nome.Length - Banda.Length) > 2)
                            Nome = Nome.Substring(Banda.Length + 2);
                        else
                            Nome = Nome.Substring(PosBanda - 1) + Nome.Substring(PosBanda + Banda.Length);
                if (Nome.Length>2)
                    if (Nome.ToLower().Substring(3) == "the")
                        Nome = Nome.Substring(3);
            }
            return Nome;
        }

        private static string RetiraLetras(string Texto, string letras, bool Finais = false)
        {
            bool Sair = false;
            bool Achou = false;
            do
            {
                Achou = false;
                for (int i = 0; i < letras.Length; i++)
                {
                    char Letra = letras[i];
                    if (Finais == false)
                    {
                        // if (Texto[1]!=null)
                        if (Texto!=null)
                            if (Texto[1] == Letra)
                            {
                                Texto = Texto.Substring(2);
                                Achou = true;
                            }
                    }
                    else
                    {
                        if (Texto[Texto.Length - 1] == Letra)
                        {
                            Texto = Texto.Substring(0, Texto.Length - 1);
                            Achou = true;
                        }
                    }
                }
                if (Achou == false)
                    Sair = true;
            } while (Sair == false);
            return Texto;
        }

    }

}
