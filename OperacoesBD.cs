using PVS.MediaPlayer;

namespace XeviousPlayer2
{
    public class OperacoesBD
    {
        public void AdicionaNoBD(Metadata data, long Tam, string lugar)
        {
            tbMusicas tbM = new tbMusicas();
            string Nome = "";
            if (data.Title == null)
            {
                Nome = Gen.RetNomePeloCaminho(lugar);
            }
            else
            {
                Nome = data.Title;
                if (Nome.Length < 2)
                {
                    Nome = Gen.RetNomePeloCaminho(lugar);
                }
            }
            tbM.Nome = Nome;
            tbM.Ano = data.Year == null ? 0 : int.Parse(data.Year);
            tbM.Tempo = (data.Duration.Minutes*60) + data.Duration.Seconds;
            int AnoTemp;
            int.TryParse(data.Year, out AnoTemp);
            tbM.Ano = AnoTemp;
            tbM.Banda = tbM.SetaBanda(data.Artist);
            tbM.SetaGenero(data.Genre);
            tbM.TemImagem = data.Image == null ? 0 : 1;
            tbM.Tamanho = Tam;
            tbM.Lugar = lugar;
            tbM.Nome = Gen.TrataNome(tbM.Nome, tbM.NomeBanda);
            tbM.Adiciona();
        }

    }
}
