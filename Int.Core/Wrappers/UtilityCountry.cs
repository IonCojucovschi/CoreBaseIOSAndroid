//
// UtilityCountry.cs
//
// Author:
//       Sogurov Fiodor <f.songurov@software-dep.net>
//
// Copyright (c) 2016 Songurov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Collections.Generic;

namespace Int.Core.Wrappers
{
    public static class UtilityCountry
    {
        public static Dictionary<string, InfoCountry> GetSectionContry => new Dictionary<string, InfoCountry>
        {
            {"AQ", new InfoCountry {Sigla = "AQ", Regione = "Abruzzo", Provincia = "LAquila"}},
            {"CH", new InfoCountry {Sigla = "CH", Regione = "Abruzzo", Provincia = "Chieti"}},
            {"PE", new InfoCountry {Sigla = "PE", Regione = "Abruzzo", Provincia = "Pescara"}},
            {"TE", new InfoCountry {Sigla = "TE", Regione = "Abruzzo", Provincia = "Teramo"}},
            {"MT", new InfoCountry {Sigla = "MT", Regione = "Basilicata", Provincia = "Matera"}},
            {"PZ", new InfoCountry {Sigla = "PZ", Regione = "Basilicata", Provincia = "Potenza"}},
            {"CS", new InfoCountry {Sigla = "CS", Regione = "Calabria", Provincia = "Cosenza"}},
            {"CZ", new InfoCountry {Sigla = "CZ", Regione = "Calabria", Provincia = "Catanzaro"}},
            {"KR", new InfoCountry {Sigla = "KR", Regione = "Calabria", Provincia = "Crotone"}},
            {"RC", new InfoCountry {Sigla = "RC", Regione = "Calabria", Provincia = "Reggio Calabria"}},
            {"VV", new InfoCountry {Sigla = "VV", Regione = "Calabria", Provincia = "Vibo Valentia"}},
            {"AV", new InfoCountry {Sigla = "AV", Regione = "Campania", Provincia = "Avellino"}},
            {"BN", new InfoCountry {Sigla = "BN", Regione = "Campania", Provincia = "Benevento"}},
            {"CE", new InfoCountry {Sigla = "CE", Regione = "Campania", Provincia = "Caserta"}},
            {"NA", new InfoCountry {Sigla = "NA", Regione = "Campania", Provincia = "Napoli"}},
            {"SA", new InfoCountry {Sigla = "SA", Regione = "Campania", Provincia = "Salerno"}},
            {"BO", new InfoCountry {Sigla = "BO", Regione = "Emilia Romagna", Provincia = "Avellino"}},
            {"FC", new InfoCountry {Sigla = "FC", Regione = "Emilia Romagna", Provincia = "Forlì-Cesena"}},
            {"FE", new InfoCountry {Sigla = "FE", Regione = "Emilia Romagna", Provincia = "Ferrara"}},
            {"MO", new InfoCountry {Sigla = "MO", Regione = "Emilia Romagna", Provincia = "Modena"}},
            {"PC", new InfoCountry {Sigla = "PC", Regione = "Emilia Romagna", Provincia = "Piacenza"}},
            {"PR", new InfoCountry {Sigla = "PR", Regione = "Emilia Romagna", Provincia = "Parma"}},
            {"RA", new InfoCountry {Sigla = "RA", Regione = "Emilia Romagna", Provincia = "Ravenna"}},
            {"RE", new InfoCountry {Sigla = "RE", Regione = "Emilia Romagna", Provincia = "Reggio Emilia"}},
            {"RN", new InfoCountry {Sigla = "RN", Regione = "Emilia Romagna", Provincia = "Rimini"}},
            {"GO", new InfoCountry {Sigla = "GO", Regione = "Friuli Venezia Giulia", Provincia = "Gorizia"}},
            {"PN", new InfoCountry {Sigla = "PN", Regione = "Friuli Venezia Giulia", Provincia = "Pordenone"}},
            {"TS", new InfoCountry {Sigla = "TS", Regione = "Friuli Venezia Giulia", Provincia = "Trieste"}},
            {"UD", new InfoCountry {Sigla = "UD", Regione = "Friuli Venezia Giulia", Provincia = "Udine"}},
            {"FR", new InfoCountry {Sigla = "FR", Regione = "Lazio", Provincia = "Frosinone"}},
            {"LT", new InfoCountry {Sigla = "LT", Regione = "Lazio", Provincia = "Latina"}},
            {"RI", new InfoCountry {Sigla = "RI", Regione = "Lazio", Provincia = "Rieti"}},
            {"RM", new InfoCountry {Sigla = "RM", Regione = "Lazio", Provincia = "Roma"}},
            {"VT", new InfoCountry {Sigla = "VT", Regione = "Lazio", Provincia = "Viterbo"}},
            {"GE", new InfoCountry {Sigla = "GE", Regione = "Liguria", Provincia = "Genova"}},
            {"IM", new InfoCountry {Sigla = "IM", Regione = "Liguria", Provincia = "Imperia"}},
            {"SP", new InfoCountry {Sigla = "SP", Regione = "Liguria", Provincia = "La Spezia"}},
            {"SV", new InfoCountry {Sigla = "SV", Regione = "Liguria", Provincia = "Savona"}},
            {"BG", new InfoCountry {Sigla = "BG", Regione = "Lombardia", Provincia = "Bergamo"}},
            {"BS", new InfoCountry {Sigla = "BS", Regione = "Lombardia", Provincia = "Brescia"}},
            {"CO", new InfoCountry {Sigla = "CO", Regione = "Lombardia", Provincia = "Como"}},
            {"CR", new InfoCountry {Sigla = "CR", Regione = "Lombardia", Provincia = "Cremona"}},
            {"LC", new InfoCountry {Sigla = "LC", Regione = "Lombardia", Provincia = "Lecco"}},
            {"LO", new InfoCountry {Sigla = "LO", Regione = "Lombardia", Provincia = "Lodi"}},
            {"MB", new InfoCountry {Sigla = "MB", Regione = "Lombardia", Provincia = "Monza e Brianza"}},
            {"MI", new InfoCountry {Sigla = "MI", Regione = "Lombardia", Provincia = "Milano"}},
            {"MN", new InfoCountry {Sigla = "MN", Regione = "Lombardia", Provincia = "Mantova"}},
            {"PV", new InfoCountry {Sigla = "PV", Regione = "Lombardia", Provincia = "Pavia"}},
            {"SO", new InfoCountry {Sigla = "SO", Regione = "Lombardia", Provincia = "Sondrio"}},
            {"VA", new InfoCountry {Sigla = "VA", Regione = "Lombardia", Provincia = "Varese"}},
            {"AN", new InfoCountry {Sigla = "AN", Regione = "Marche", Provincia = "Ancona"}},
            {"AP", new InfoCountry {Sigla = "AP", Regione = "Marche", Provincia = "Ascoli Piceno"}},
            {"FM", new InfoCountry {Sigla = "FM", Regione = "Marche", Provincia = "Fermo"}},
            {"MC", new InfoCountry {Sigla = "MC", Regione = "Marche", Provincia = "Macerata"}},
            {"PU", new InfoCountry {Sigla = "PU", Regione = "Marche", Provincia = "Pesaro e Urbino"}},
            {"CB", new InfoCountry {Sigla = "CB", Regione = "Molise", Provincia = "Campobasso"}},
            {"IS", new InfoCountry {Sigla = "IS", Regione = "Molise", Provincia = "Isernia"}},
            {"AL", new InfoCountry {Sigla = "AL", Regione = "Piemonte", Provincia = "Alessandria"}},
            {"AT", new InfoCountry {Sigla = "AT", Regione = "Piemonte", Provincia = "Asti"}},
            {"BI", new InfoCountry {Sigla = "BI", Regione = "Piemonte", Provincia = "Biella"}},
            {"CN", new InfoCountry {Sigla = "CN", Regione = "Piemonte", Provincia = "Cuneo"}},
            {"NO", new InfoCountry {Sigla = "NO", Regione = "Piemonte", Provincia = "Novara"}},
            {"TO", new InfoCountry {Sigla = "TO", Regione = "Piemonte", Provincia = "Torino"}},
            {"VB", new InfoCountry {Sigla = "VB", Regione = "Piemonte", Provincia = "Verbano Cusio Ossola"}},
            {"VC", new InfoCountry {Sigla = "VC", Regione = "Piemonte", Provincia = "Vercelli"}},
            {"BA", new InfoCountry {Sigla = "BA", Regione = "Puglia", Provincia = "Bari"}},
            {"BR", new InfoCountry {Sigla = "BR", Regione = "Puglia", Provincia = "Brindisi"}},
            {"BT", new InfoCountry {Sigla = "BT", Regione = "Puglia", Provincia = "Barletta-Andria-Trani"}},
            {"FG", new InfoCountry {Sigla = "FG", Regione = "Puglia", Provincia = "Foggia"}},
            {"LE", new InfoCountry {Sigla = "LE", Regione = "Puglia", Provincia = "Lecce"}},
            {"TA", new InfoCountry {Sigla = "TA", Regione = "Puglia", Provincia = "Taranto"}},
            {"CA", new InfoCountry {Sigla = "CA", Regione = "Sardegna", Provincia = "Cagliari"}},
            {"CI", new InfoCountry {Sigla = "CI", Regione = "Sardegna", Provincia = "Carbonia-Iglesias "}},
            {"NU", new InfoCountry {Sigla = "NU", Regione = "Sardegna", Provincia = "Nuoro"}},
            {"OG", new InfoCountry {Sigla = "OG", Regione = "Sardegna", Provincia = "Ogliastra"}},
            {"OR", new InfoCountry {Sigla = "OR", Regione = "Sardegna", Provincia = "Oristano"}},
            {"OT", new InfoCountry {Sigla = "OT", Regione = "Sardegna", Provincia = "Olbia - Tempio"}},
            {"SS", new InfoCountry {Sigla = "SS", Regione = "Sardegna", Provincia = "Sassari"}},
            {"VS", new InfoCountry {Sigla = "VS", Regione = "Sardegna", Provincia = "Medio Campidano"}},
            {"AG", new InfoCountry {Sigla = "AG", Regione = "Sicilia", Provincia = "Agrigento"}},
            {"CL", new InfoCountry {Sigla = "CL", Regione = "Sicilia", Provincia = "Caltanisetta"}},
            {"CT", new InfoCountry {Sigla = "CT", Regione = "Sicilia", Provincia = "Catania"}},
            {"EN", new InfoCountry {Sigla = "EN", Regione = "Sicilia", Provincia = "Enna"}},
            {"ME", new InfoCountry {Sigla = "ME", Regione = "Sicilia", Provincia = "Messina"}},
            {"PA", new InfoCountry {Sigla = "PA", Regione = "Sicilia", Provincia = "Palermo"}},
            {"RG", new InfoCountry {Sigla = "RG", Regione = "Sicilia", Provincia = "Ragusa"}},
            {"SR", new InfoCountry {Sigla = "SR", Regione = "Sicilia", Provincia = "Siracusa"}},
            {"TP", new InfoCountry {Sigla = "TP", Regione = "Sicilia", Provincia = "Trapani"}},
            {"BZ", new InfoCountry {Sigla = "BZ", Regione = "Trentino Alto Adige", Provincia = "Bolzano"}},
            {"TN", new InfoCountry {Sigla = "TN", Regione = "Trentino Alto Adige", Provincia = "Trento"}},
            {"AR", new InfoCountry {Sigla = "AR", Regione = "Toscana", Provincia = "Arezzo"}},
            {"FI", new InfoCountry {Sigla = "FI", Regione = "Toscana", Provincia = "Firenze"}},
            {"GR", new InfoCountry {Sigla = "GR", Regione = "Toscana", Provincia = "Grosseto"}},
            {"LI", new InfoCountry {Sigla = "LI", Regione = "Toscana", Provincia = "Livorno"}},
            {"LU", new InfoCountry {Sigla = "LU", Regione = "Toscana", Provincia = "Lucca"}},
            {"MS", new InfoCountry {Sigla = "MS", Regione = "Toscana", Provincia = "Massa Carrara"}},
            {"PI", new InfoCountry {Sigla = "PI", Regione = "Toscana", Provincia = "Pisa"}},
            {"PO", new InfoCountry {Sigla = "PO", Regione = "Toscana", Provincia = "Prato"}},
            {"PT", new InfoCountry {Sigla = "PT", Regione = "Toscana", Provincia = "Pistoia"}},
            {"SI", new InfoCountry {Sigla = "SI", Regione = "Toscana", Provincia = "Siena"}},
            {"PG", new InfoCountry {Sigla = "PG", Regione = "Umbria", Provincia = "Perugia"}},
            {"TR", new InfoCountry {Sigla = "TR", Regione = "Umbria", Provincia = "Terni"}},
            {"AO", new InfoCountry {Sigla = "AO", Regione = "Valle d'Aosta", Provincia = "Aosta"}},
            {"BL", new InfoCountry {Sigla = "BL", Regione = "Veneto", Provincia = "Belluno"}},
            {"PD", new InfoCountry {Sigla = "PD", Regione = "Veneto", Provincia = "Padova"}},
            {"RO", new InfoCountry {Sigla = "RO", Regione = "Veneto", Provincia = "Rovigo"}},
            {"TV", new InfoCountry {Sigla = "TV", Regione = "Veneto", Provincia = "Treviso"}},
            {"VE", new InfoCountry {Sigla = "VE", Regione = "Veneto", Provincia = "Venezia"}},
            {"VI", new InfoCountry {Sigla = "VI", Regione = "Veneto", Provincia = "Vicenza"}},
            {"VR", new InfoCountry {Sigla = "VR", Regione = "Veneto", Provincia = "Verona"}}
        };
    }

    public class InfoCountry
    {
        public string Sigla { get; set; }
        public string Provincia { get; set; }
        public string Regione { get; set; }
    }
}