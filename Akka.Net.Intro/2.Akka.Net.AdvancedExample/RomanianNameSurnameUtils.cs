﻿using Akka.Net.AdvancedExample.SharedMessages;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Akka.Net.AdvancedExample
{
	public class RomanianNameSurnameUtils
	{
		private static readonly ConcurrentQueue<string> Surnames = new ConcurrentQueue<string>(new List<string>()
		{
			"Nita",
			"Pop",
			"Popa",
			"Popescu",
			"Ionescu",
			"Nemes",
			"Stan",
			"Dumitrescu",
			"Dima",
			"Gheorghiu",
			"Ionita",
			"Marin",
			"Tudor",
			"Dobre",
			"Barbu",
			"Nistor",
			"Florea",
			"Ene",
			"Dinu",
			"Georgescu",
			"Stoica",
			"Diaconu",
			"Diaconescu",
			"Mazilescu",
			"Ababei",
			"Aanei",
			"Nistor",
			"Mocanu",
			"Oprea",
			"Voinea",
			"Dochioiu",
			"Albu",
			"Tabacu",
			"Manole",
			"Cristea",
			"Toma",
			"Stanescu",
			"Preda",
			"Puscasu",
			"Tomescu",
			"Rusu",
			"Ceban",
			"Ciobanu",
			"Turcan",
			"Cebotari",
			"Sirbu",
			"Lungu",
			"Munteanu",
			"Rotari",
			"Ursu",
			"Gutu",
			"Rosca",
			"Melnic",
			"Cojocari"
		});

		private static readonly Dictionary<GenderEnum, ConcurrentQueue<string>> Names = new Dictionary<GenderEnum, ConcurrentQueue<string>>
		{
			{GenderEnum.Female ,new ConcurrentQueue<string>(new List<string>
			{
				"Ana",
				"Maria",
				"Mihaela",
				"Andreea",
				"Elena",
				"Alexandra",
				"Cristina",
				"Daniela",
				"Alina",
				"Ioana",
				"Nicoleta",
				"Georgiana",
				"Mariana",
				"Gabriela",
				"Adriana",
				"Ionela",
				"Florentina",
				"Anca",
				"Anamaria",
				"Simona",
				"Roxana",
				"Oana",
				"Irina",
				"Diana",
				"Mirela",
				"Iuliana",
				"Madalina",
				"Raluca",
				"Loredana",
				"Claudia",
				"Monica",
				"Ramona",
				"Corina",
				"Laura",
				"Liliana",
				"Valentina",
				"Iulia",
				"Florina",
				"Catalina",
				"Camelia",
				"Mircea",
				"Georgeta",
				"Adina",
				"Attila",
				"Vasilica",
				"Ileana",
				"Silvia",
				"Veronica",
				"Marinela",
				"Rodica",
				"Violeta",
				"Ancuta",
				"Viorica",
				"Emilia",
				"Timea",
				"Teodora",
				"Luminita",
				"Cornelia",
				"Magdalena",
				"Lavinia",
				"Andrea",
				"Csaba",
				"Stefania",
				"Aurelia",
				"Andra",
				"Bianca",
				"Petronela",
				"Paula",
				"Melinda",
				"Petrica",
				"Erika",
				"Angela",
				"Dorina",
				"Mihaita",
				"Lenuta",
				"Marilena",
				"Ecaterina",
				"Adelina",
				"Kinga",
				"Anisoara",
				"Lidia",
				"Gheorghita",
				"Sorina",
				"Geanina",
				"Mirabela",
				"Adela",
				"Csilla",
				"Elisabeta",
				"Annamaria",
				"Florica",
				"Zsuzsanna",
				"Dana",
				"Beata",
				"Reka",
				"Larisa",
				"Orsolya",
				"Monika",
				"Ruxandra",
				"Eva",
				"Tatiana",
				"Gina",
				"Ionica",
				"Manuela",
				"Felicia",
				"Marina",
				"Mioara",
				"Angelica",
				"Delia",
				"Emanuela",
				"Cristiana",
				"Eugenia",
				"Lacramioara",
				"Izabella",
				"Natalia",
				"Livia",
				"Denisa",
				"Doina",
				"Marcela",
				"Andrada",
				"Marioara",
				"Petruta",
				"Valerica",
				"Amalia",
				"Cosmina",
				"Otilia",
				"Victoria",
				"Renata",
				"Lucia",
				"Andreia",
				"Anita",
				"Giorgiana",
				"Steluta",
				"Luciana",
				"Lucica",
				"Niculina",
				"Sabina",
				"Crina",
				"Maricica",
				"Valeria",
				"Floarea",
				"Constanta",
				"Gabriella",
				"Marta",
				"Olga",
				"Eliza",
				"Krisztina",
				"Imola",
				"Melania",
				"Anna",
				"Gianina",
				"Anda",
				"Zsuzsa",
				"Aura",
				"Alexandrina",
				"Ionelia",
				"Aurora",
				"Margareta",
				"Stefana",
				"Aurica",
				"Luiza",
				"Victorita",
				"Cecilia",
				"Ligia",
				"Flavia",
				"Estera",
				"Ibolya",
				"Brigitta",
				"Liana",
				"Szidonia",
				"Edina",
				"Barna",
				"Hajnalka",
				"Olimpia",
				"Paraschiva",
				"Julia",
				"Rozalia"

			}) },
			{GenderEnum.Male, new ConcurrentQueue<string>(new List<string>
			{
				"Alexandru",
				"Adrian",
				"Andrei",
				"Mihai",
				"Ionut",
				"Florin",
				"Daniel",
				"Marian",
				"Marius",
				"Cristian",
				"Constantin",
				"Bogdan",
				"Vasile",
				"Gabriel",
				"Nicolae",
				"Gheorghe",
				"George",
				"Ioan",
				"Valentin",
				"Catalin",
				"Stefan",
				"Ion",
				"Iulian",
				"Ionel",
				"Lucian",
				"Cosmin",
				"Sorin",
				"Dumitru",
				"Ciprian",
				"Vlad",
				"Razvan",
				"Radu",
				"Viorel",
				"Ovidiu",
				"Robert",
				"Carmen",
				"Claudiu",
				"Alin",
				"Dan",
				"Costel",
				"Laurentiu",
				"Paul",
				"Dragos",
				"Silviu",
				"Liviu",
				"Petru",
				"Victor",
				"Zoltan",
				"Sergiu",
				"Zsolt",
				"Istvan",
				"Eugen",
				"Levente",
				"Ilie",
				"Octavian",
				"Cornel",
				"Tudor",
				"Danut",
				"Sebastian",
				"Aurel",
				"Katalin",
				"Nicusor",
				"Florian",
				"Eniko",
				"Mihail",
				"Laszlo",
				"Dorin",
				"Emil",
				"Georgian",
				"Szabolcs",
				"Emanuel",
				"Tiberiu",
				"Marin",
				"Tunde",
				"Teodor",
				"Costin",
				"Marcel",
				"Petre",
				"Calin",
				"Aurelian",
				"Ildiko",
				"Madalin",
				"Noemi",
				"Eduard",
				"Stelian",
				"Cristinel",
				"Emoke",
				"Florentin",
				"Emese",
				"Iosif",
				"Sandor",
				"Virgil",
				"Dorel",
				"Jozsef",
				"Traian",
				"Norbert",
				"Emilian",
				"Vladimir",
				"Lorand",
				"Tamas",
				"Erzsebet",
				"Grigore",
				"Corneliu",
				"Nicu",
				"Andras",
				"Tibor",
				"Szilard",
				"Arpad",
				"Pavel",
				"Raul",
				"Valeriu",
				"Doru",
				"Botond",
				"Beniamin",
				"Rares",
				"Tudorel",
				"Petrisor",
				"Agnes",
				"Costinel",
				"Georgel",
				"Remus",
				"Cezar",
				"Janos",
				"Samuel",
				"Nelu",
				"Neculai",
				"Eszter",
				"Mirel",
				"Leonard",
				"Ferenc",
				"Gabor",
				"David",
				"Hunor",
				"Peter",
				"Edit",
				"Florinel",
				"Marinel",
				"Imre",
				"Matei",
				"Flavius",
				"Simion",
				"Gelu",
				"Romeo",
				"Anton",
				"Denis",
				"Horatiu",

			})
			}
		};


		public static string GetSurname()
		{
			if (Surnames.TryDequeue(out string surmane))
			{
				return surmane;
			}

			return "Smith";
		}

		public static string GetName(GenderEnum gender)
		{
			if (Names[gender].TryDequeue(out string name))
			{
				return name;
			}

			return gender == GenderEnum.Male ? "John" : "Johana";
		}


	}
}