using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Controllers
{
    public class ComboBoxController
    {
        #region [ PROPRIEDADES ]

        ComboBoxDAO dao = new ComboBoxDAO();

        #endregion

        #region [ MÉTODOS DE BUSCA ]

        public List<ComboBox> ComboBox_Log_Modulos(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
        {
            return dao.ComboBox_Log_Modulos(dataInicial, dataFinal, modulo, operacao, usuario, texto);
        }
        public List<ComboBox> ComboBox_Log_Usuarios(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
        {
            return dao.ComboBox_Log_Usuarios(dataInicial, dataFinal, modulo, operacao, usuario, texto);
        }
        public List<ComboBox> ComboBox_Log_Operacoes(DateTime dataInicial, DateTime dataFinal, string modulo, string operacao, string usuario, string texto)
        {
            return dao.ComboBox_Log_Operacoes(dataInicial, dataFinal, modulo, operacao, usuario, texto);
        }
        public List<ComboBox> ComboBoxCorredores()
        {
            return dao.ComboBoxCorredores();
        }
        public List<ComboBox> ComboBoxCorredoresACTPP()
        {
            return dao.ComboBoxCorredoresACTPP();
        }
        public List<ComboBox> ComboBoxPostosTrabalhoACTPP()
        {
            return dao.ComboBoxPostosTrabalhoACTPP();
        }
        public List<ComboBox> ComboBoxSituacaoControleRadios()
        {
            return dao.ComboBoxSituacaoControleRadios();
        }
        public List<ComboBox> ComboBoxTipoLocomotivas()
        {
            return dao.ComboBoxTipoLocomotivas();
        }

        public List<ComboBox> ComboBoxLocalidades(string Corredor)
        {
            return dao.ComboBoxLocalidades(Corredor);
        }

        public List<ComboBox> ComboBoxEstacoes(string Corredor)
        {
            return dao.ComboBoxEstacoes(Corredor);
        }

        public List<ComboBox> ComboBoxPerfis()
        {
            return dao.ComboBoxPerfis();
        }

        public List<ComboBox> ComboBoxPerfisACT()
        {
            return dao.ComboBoxPerfisACT();
        }

        public List<ComboBox> ComboBoxPostoTrabalho()
        {
            return dao.ComboBoxPostoTrabalho();
        }
   
        public List<ComboBox> ComboBoxGrupos()
        {
            return dao.ComboBoxGrupos();
        }
        public List<ComboBox> ComboBoxTrechos()
        {
            return dao.ComboBoxTrechos();
        }
        public List<ComboBox> ComboBoxMotivoParadaTrem()
        {
            return dao.ComboBoxMotivoParadaTrem();
        }
        public List<ComboBox> ComboBoxSBs()
        {
            return dao.ComboBoxSBs();
        }
        public List<ComboBox> ComboBoxTT_Corredores()
        {
            return dao.ComboBoxTT_Corredores();
        }
        public List<ComboBox> ComboBoxTT_Trechos()
        {
            return dao.ComboBoxTT_Trechos();
        }
        public List<ComboBox> ComboBoxTT_Rotas()
        {
            return dao.ComboBoxTT_Rotas();
        }

        public List<ComboBox> ComboBoxTT_SubRotas()
        {
            return dao.ComboBoxTT_SubRotas();
        }


        public List<ComboBox> ComboBoxTT_CorredoresComTT_RotasID(List<string> rotas_id)
        {
            return dao.ComboBoxTT_CorredoresComTT_RotasID(rotas_id);
        }
        public List<ComboBox> ComboBoxTT_CorredoresComTT_SubRotasID(List<string> subrotas_id)
        {
            return dao.ComboBoxTT_CorredoresComTT_SubRotasID(subrotas_id);
        }


        public List<ComboBox> ComboBoxTT_TrechosComTT_CorredoresID(List<string> corredores_id)
        {
            return dao.ComboBoxTT_TrechosComTT_CorredoresID(corredores_id);
        }
        public List<ComboBox> ComboBoxTT_TrechosComTT_RotasID(List<string> rotas_id)
        {
            return dao.ComboBoxTT_TrechosComTT_CorredoresID(rotas_id);
        }
        public List<ComboBox> ComboBoxTT_TrechosComTT_SubRotasID(List<string> subrotas_id)
        {
            return dao.ComboBoxTT_TrechosComTT_SubRotasID(subrotas_id);
        }


        public List<ComboBox> ComboBoxTT_RotasComTT_CorredoresID(List<string> corredor_id)
        {
            return dao.ComboBoxTT_RotasComTT_CorredoresID(corredor_id);
        }
        public List<ComboBox> ComboBoxTT_RotasComTT_TrechosID(List<string> trechos_id)
        {
            return dao.ComboBoxTT_RotasComTT_TrechosID(trechos_id);
        }
        public List<ComboBox> ComboBoxTT_RotasComTT_SubRotasID(List<string> subrotas_id)
        {
            return dao.ComboBoxTT_RotasComTT_SubRotasID(subrotas_id);
        }



        public List<ComboBox> ComboBoxTT_SubRotasComTT_CorredoresID(List<string> corredor_id)
        {
            return dao.ComboBoxTT_SubRotasComTT_CorredoresID(corredor_id);
        }
        public List<ComboBox> ComboBoxTT_SubRotasComTT_TrechosID(List<string> rotas_id)
        {
            return dao.ComboBoxTT_SubRotasComTT_TrechosID(rotas_id);
        }
        public List<ComboBox> ComboBoxTT_SubRotasComTT_RotasID(List<string> rotas_id)
        {
            return dao.ComboBoxTT_SubRotasComTT_RotasID(rotas_id);
        }


        public List<ComboBox> ComboBoxMotivosComGruposID(List<string> grupos_id)
        {
            return dao.ComboBoxMotivosComGruposID(grupos_id);
        }
        public List<ComboBox> ComboBoxGruposComMotivosID(List<string> motivos_id)
        {
            return dao.ComboBoxGruposComMotivosID(motivos_id);
        }

        #endregion

    }
}
