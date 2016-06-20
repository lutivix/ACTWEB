using LFSistemas.VLI.ACTWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LFSistemas.VLI.ACTWeb.Entities;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class Teste : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Usuarios> itens { get; set; }
        public List<Entities.NivelAcesso> dadosNivel { get; set; }

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //Pesquisar(null);
        }

        //protected void Pesquisar(string ordenacao)
        //{
        //    var usuarioController = new UsuarioController();
        //    itens = usuarioController.ObterTodos(new Entities.Usuarios()
        //    {
        //        Perfil_Abreviado = string.Empty,
        //        Matricula = string.Empty,
        //        Nome = string.Empty
        //    });

        //    //for (int i = 0; i < itens.Count; i++)
        //    //{
        //    //    itens[i].Senha = Uteis.Criptografar(itens[i].Senha, "a#3G6**@").ToString();
        //    //}
        //    for (int i = 0; i < itens.Count; i++)
        //    {
        //        itens[i].Senha = Uteis.Descriptografar(itens[i].Senha, "a#3G6**@");
        //    }

        //    if (itens.Count > 0)
        //    {
        //        this.RepeaterItens.DataSource = itens.OrderBy(o => o.Id).ToList();
        //        this.RepeaterItens.DataBind();
        //    }
        //    else
        //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

        //    lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        //}
    }
}