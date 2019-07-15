using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace StatueStoreWebApplic.Models {
    public class ModelProdutos {

        public ModelProdutos(char sexo, string grupo = "", string filtro = "", string tamanho = "", string buscar = "") {

            RepoProdutos _repoProdutos = new RepoProdutos();
            _repoProdutos.filtrarPor = filtro;
            _repoProdutos.buscarPor = buscar;
            //Verifica se há um grupo

            if (!string.IsNullOrWhiteSpace(tamanho) && !tamanho.Equals("todos")) {
                if (!string.IsNullOrWhiteSpace(grupo)) { //caso houver
                    switch (sexo) {
                        case 'F':
                            produtos = _repoProdutos.FemininoPorGrupoETamanho(grupo, tamanho);
                            break;
                        case 'M':
                            produtos = _repoProdutos.MasculinoPorGrupoETamanho(grupo, tamanho);
                            break;
                    }
                }
                else //Caso não houver um grupo especificado
                {
                    switch (sexo) {
                        case 'F':
                            produtos = _repoProdutos.FemininoPorTamanho(tamanho);
                            break;
                        case 'M':
                            produtos = _repoProdutos.MasculinoPorTamanho(tamanho);
                            break;
                    }

                }
            }    
            else {
                if (!string.IsNullOrWhiteSpace(grupo)) { //caso houver
                    switch (sexo) {
                        case 'F':
                            produtos = _repoProdutos.TudoFemininoPorGrupo(grupo);
                            break;
                        case 'M':
                            produtos = _repoProdutos.tudoMasculinoPorGrupo(grupo);
                            break;
                    }
                }
                else //Caso não houver um grupo especificado
                {
                    switch (sexo) {
                        case 'F':
                            produtos = _repoProdutos.TudoFeminino();
                            break;
                        case 'M':
                            produtos = _repoProdutos.tudoMasculino();
                            break;
                    }

                }
            }



            //populando tamanhos
            if(!string.IsNullOrWhiteSpace(grupo))
                tamanhos = Tamanhos.GetAll(sexo, grupo);
            else
                tamanhos = Tamanhos.GetAll(sexo, "nenhum");
        }

        public IEnumerable<ProdutoDisplay> produtos;
        public IEnumerable<string> tamanhos;

    }
}


/*
 * 
 * SELECT nome FROM Produto WHERE idSubgrupo IN 
 * (SELECT idSubgrupo FROM Subgrupo WHERE idGrupo IN 
 * (SELECT idGrupo FROM Grupo WHERE nomeGrupo = 'Moletons'))
 * 
 * 
 */
