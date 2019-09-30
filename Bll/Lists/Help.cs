﻿using System.Collections.Generic;

namespace Bll.Lists {
  public class Help {
    private const string link = "https://optimize-manual.herokuapp.com/";

    public Dictionary<int, string> HRef;
    public Help() {
      this.HRef = new Dictionary<int, string> {
        { 0, link },
        { 1, $"{link}#conceito" },
        { 2, $"{link}#login" },
        { 3, $"{link}#menu-empresa" },
        { 4, $"{link}#menu-operacoes" },
        { 5, $"{link}#menu-linhas" },
        { 6, $"{link}#menu-pesquisa" },
        { 10, $"{link}#empresa" },
        { 11, $"{link}#usuarios-empresa" },
        { 12, $"{link}#periodos" },
        { 13, $"{link}#jurisdicoes" },
        { 14, $"{link}#classe-linha" },
        { 15, $"{link}#operacoes" },
        { 16, $"{link}#tarifa" },
        { 17, $"{link}#classe-tarifa" },
        { 18, $"{link}#custos" },
        { 19, $"{link}#terminais" },
        { 20, $"{link}#instalacoes" },
        { 21, $"{link}#finalidades" },
        { 22, $"{link}#troncos" },
        { 23, $"{link}#linhas-tronco" },
        { 24, $"{link}#corredores" },
        { 25, $"{link}#linhas-corredor" },
        { 26, $"{link}#pontos" },
        { 27, $"{link}#veiculos" },
        { 28, $"{link}#chassis" },
        { 29, $"{link}#carrocerias" },
        { 30, $"{link}#idade-media" },
        { 31, $"{link}#linhas" },
        { 32, $"{link}#linhas-terminal" },
        { 33, $"{link}#atendimentos" },
        { 34, $"{link}#horarios" },
        { 35, $"{link}#total-viagens" },
        { 36, $"{link}#itinerarios" },
        { 37, $"{link}#itinerario-att" },
        { 38, $"{link}#pontos-att" },
        { 39, $"{link}#tipicos" },
        { 40, $"{link}#oferta" },
        { 41, $"{link}#fator" },
        { 42, $"{link}#indice" },
        { 43, $"{link}#pesquisas" },
        { 44, $"{link}#linhas-pesquisa" },
        { 45, $"{link}#viagens" },
        { 46, $"{link}#dimensionamento" },
        { 47, $"{link}#sinotico" },
        { 48, $"{link}#" },
        { 49, $"{link}#" },
        { 50, $"{link}#" },
        { 51, $"{link}#" },
        { 52, $"{link}#" },
      };
    }
  }
}
