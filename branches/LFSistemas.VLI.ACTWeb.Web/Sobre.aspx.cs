using System;
using System.Collections.Generic;
using System.Linq;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public class versao
    {
        public int ID { get; set; }
        public string VERSAO { get; set; }
        public string MODULO { get; set; }
        public string IMPLANTACAO { get; set; }
    }
    public partial class Sobre : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<versao> v = new List<versao>();
                v.Add(new versao() { ID = 1, VERSAO = "1.0.0", MODULO = "Usuário", IMPLANTACAO = "02/09/2014" });
                v.Add(new versao() { ID = 2, VERSAO = "1.0.1", MODULO = "Apoio", IMPLANTACAO = "23/09/2014" });
                v.Add(new versao() { ID = 3, VERSAO = "1.0.2", MODULO = "Envio de Macro: 50, 61 e 200", IMPLANTACAO = "25/11/2014" });
                v.Add(new versao() { ID = 4, VERSAO = "1.0.3", MODULO = "Nível de Acesso", IMPLANTACAO = "26/11/2014" });
                v.Add(new versao() { ID = 5, VERSAO = "1.0.4", MODULO = "Filto Corredor no Apoio", IMPLANTACAO = "08/01/2015" });
                v.Add(new versao() { ID = 6, VERSAO = "1.0.5", MODULO = "Locomotivas e Restrições", IMPLANTACAO = "23/02/2015" });
                v.Add(new versao() { ID = 7, VERSAO = "1.0.6", MODULO = "Indicadores", IMPLANTACAO = "10/04/2015" });
                v.Add(new versao() { ID = 8, VERSAO = "1.0.7", MODULO = "Paineis", IMPLANTACAO = "14/04/2015" });
                v.Add(new versao() { ID = 9, VERSAO = "1.0.8", MODULO = "SAVI", IMPLANTACAO = "14/04/2015" });
                dgVersoes.DataSource = v.OrderByDescending(o => o.VERSAO).ToList();
                dgVersoes.DataBind();
            }
        }
    }
}