﻿using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;

namespace Imago.Repository
{
    public interface IWikiRepository
    {
        string GetWikiUrl(SkillType skillType);
        string GetWikiUrl(SkillGroupType skillGroupType);
    }

    public class WikiRepository : IWikiRepository
    {
        private static Dictionary<SkillGroupType, string> _skillGroupTypeLookUp =
            new Dictionary<SkillGroupType, string>()
            {
                {SkillGroupType.Bewegung, "http://imago-rp.de/index.php/Bewegung_(Fertigkeitsgruppe)"},
                {SkillGroupType.Nahkampf, "http://imago-rp.de/index.php/Nahkampf_(Fertigkeitsgruppe)"},
                {SkillGroupType.Heimlichkeit, "http://imago-rp.de/index.php/Heimlichkeit_(Fertigkeitsgruppe)"},
                {SkillGroupType.Fernkampf, "http://imago-rp.de/index.php/Fernkampf_(Fertigkeitsgruppe)"},
                {SkillGroupType.Webkunst, "http://imago-rp.de/index.php/Weben_(Fertigkeitsgruppe)"},
                {SkillGroupType.Wissenschaft, "http://imago-rp.de/index.php/Wissenschaft_(Fertigkeitsgruppe)"},
                {SkillGroupType.Handwerk, "http://imago-rp.de/index.php/Handwerk_(Fertigkeitsgruppe)"},
                {SkillGroupType.Soziales, "http://imago-rp.de/index.php/Soziales_(Fertigkeitsgruppe)"}
            };

        private static Dictionary<SkillType, string> _skillTypeLookUp = new Dictionary<SkillType, string>()
        {
            {SkillType.Alchemie, "http://imago-rp.de/index.php/Alchemie"},
            {SkillType.Anatomie, "http://imago-rp.de/index.php/Anatomie"},
            {SkillType.Anfuehren, "http://imago-rp.de/index.php/Anf%C3%BChren"},
            {SkillType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Regeln)"},
            {SkillType.Ausdruck, "http://imago-rp.de/index.php/Ausdruck"},
            {SkillType.Ausweichen, "http://imago-rp.de/index.php/Ausweichen"},
            {SkillType.Bewusstsein, "http://imago-rp.de/index.php/Bewusstsein"},
            {SkillType.Blasrohre, "http://imago-rp.de/index.php/Blasrohre"},
            {SkillType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Regeln)"},
            {SkillType.Chaos, "http://imago-rp.de/index.php/Chaos"},
            {SkillType.Dolche, "http://imago-rp.de/index.php/Blasrohr_(Regeln)"},
            {SkillType.Einfalt, "http://imago-rp.de/index.php/Einfalt"},
            {SkillType.Einschuechtern, "http://imago-rp.de/index.php/Einsch%C3%BCchtern"},
            {SkillType.Ekstase, "http://imago-rp.de/index.php/Ekstase"},
            {SkillType.Empathie, "http://imago-rp.de/index.php/Empathie"},
            {SkillType.Gesellschafter, "http://imago-rp.de/index.php/Gesellschafter"},
            {SkillType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Regeln)"},
            {SkillType.Heiler, "http://imago-rp.de/index.php/Heiler"},
            {SkillType.Klettern, "http://imago-rp.de/index.php/Klettern"},
            {SkillType.Koerperbeherrschung, "http://imago-rp.de/index.php/K%C3%B6rperbeherrschung"},
            {SkillType.Kontrolle, "http://imago-rp.de/index.php/Kontrolle"},
            {SkillType.Laufen, "http://imago-rp.de/index.php/Laufen"},
            {SkillType.Literatur, "http://imago-rp.de/index.php/Literatur"},
            {SkillType.Leere, "http://imago-rp.de/index.php/Leere"},
            {SkillType.Manipulation, "http://imago-rp.de/index.php/Manipulation"},
            {SkillType.Materie, "http://imago-rp.de/index.php/Materie"},
            {SkillType.Mathematik, "http://imago-rp.de/index.php/Mathematik"},
            {SkillType.Naturkunde, "http://imago-rp.de/index.php/Naturkunde"},
            {SkillType.Philosophie, "http://imago-rp.de/index.php/Philosophie"},
            {SkillType.Physik, "http://imago-rp.de/index.php/Physik"},
            {SkillType.Reiten, "http://imago-rp.de/index.php/Reiten"},
            {SkillType.Schilde, "http://imago-rp.de/index.php/Schild_(Regeln)"},
            {SkillType.Schleichen, "http://imago-rp.de/index.php/Schleichen"},
            {SkillType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Regeln)"},
            {SkillType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Regeln)"},
            {SkillType.Schwimmen, "http://imago-rp.de/index.php/Schwimmen"},
            {SkillType.Sicherheit, "http://imago-rp.de/index.php/Sicherheit"},
            {SkillType.SozialeAdaption, "http://imago-rp.de/index.php/Soziale_Adaption"},
            {SkillType.Soziologie, "http://imago-rp.de/index.php/Soziologie"},
            {SkillType.Sphaerologie, "http://imago-rp.de/index.php/Sph%C3%A4rologie"},
            {SkillType.Sprache, "http://imago-rp.de/index.php/Sprachen"},
            {SkillType.Springen, "http://imago-rp.de/index.php/Springen"},
            {SkillType.SpurenLesen, "http://imago-rp.de/index.php/Spuren_lesen"},
            {SkillType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Regeln)"},
            {SkillType.Strategie, "http://imago-rp.de/index.php/Strategie"},
            {SkillType.Struktur, "http://imago-rp.de/index.php/Struktur"},
            {SkillType.Tanzen, "http://imago-rp.de/index.php/Tanzen"},
            {SkillType.Taschendiebstahl, "http://imago-rp.de/index.php/Taschendiebstahl"},
            {SkillType.Verbergen, "http://imago-rp.de/index.php/Verbergen"},
            {SkillType.Verkleiden, "http://imago-rp.de/index.php/Verkleiden"},
            {SkillType.Verstecken, "http://imago-rp.de/index.php/Verstecken"},
            {SkillType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Regeln)"},
            {SkillType.WirtschaftRecht, "http://imago-rp.de/index.php/Wirtschaft/Recht"},
            {SkillType.Wurfgeschosse, "http://imago-rp.de/index.php/Wurfgeschosse"},
            {SkillType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen"},
            {SkillType.Wundscher, "http://imago-rp.de/index.php/Wundscher"},
            {SkillType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Regeln)"}
        };

        public string GetWikiUrl(SkillType skillType)
        {
            if (_skillTypeLookUp.ContainsKey(skillType))
                return _skillTypeLookUp[skillType];

            return string.Empty;
        }

        public string GetWikiUrl(SkillGroupType skillGroupType)
        {
            if (_skillGroupTypeLookUp.ContainsKey(skillGroupType))
                return _skillGroupTypeLookUp[skillGroupType];

            return string.Empty;
        }
    }
}