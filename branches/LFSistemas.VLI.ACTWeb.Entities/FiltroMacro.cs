using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por filtrar macros
    /// </summary>
    public class FiltroMacro
    {
        #region [ PROPRIEDADES ]

        public string NumeroLocomotiva { get; set; }
        public string NumeroTrem { get; set; }
        public string NumeroMacro { get; set; }
        public string CodigoOS { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Mais { get; set; }
        public int? Espaco { get; set; }
        public string Motivo { get; set; }
        public string Expressao { get; set; }
        public string Corredores { get; set; }
        public string PrefixoTrem { get; set; }
        public bool naoLidas { get; set; }
        public string cabines { get; set; }
        

        #endregion
    }
    public static class Botao
    {
        public static bool pesquisaPorBotao;

        public static DateTime ultimoDataIni;

        public static DateTime ultimoDataFim;

        public static string cabinesSelecionadas;

        public static bool registroNaoLocalizadoBotao;

        public static bool registroNaoLocalizadoAtualização;

        public static DateTime getultimoDataIni()
        {
            return ultimoDataIni;
        }

        public static DateTime getultimoDataFim()
        {
            return ultimoDataFim;
        }

        public static string getcabinesSelecionadas()
        {
            return cabinesSelecionadas;
        }

        public static bool getregistroNaoLocalizadoBotao()
        {
            return registroNaoLocalizadoBotao;
        }

        public static bool getregistroNaoLocalizadoAtualização()
        {
            return registroNaoLocalizadoAtualização;
        }
        public static void Atualizar(DateTime di, DateTime df, string cabines)
        {

            ultimoDataIni = di;
            ultimoDataFim = df;
            cabinesSelecionadas = cabines;

        }

        public static void Atualizar(int origem, string cabines)
        {

            if (origem == 1)
            {
                pesquisaPorBotao = false;
            }
            else
            {
                pesquisaPorBotao = true;
            }

            cabinesSelecionadas = cabines;

        }

        public static int UltimaAtualizacaoOrigem()
        {   
            
            if (pesquisaPorBotao)
            {
                return 2;
            }
            else
            {
                return 1;
            }

        }

    }
}
