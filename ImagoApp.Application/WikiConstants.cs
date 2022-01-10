using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application
{
    public static class WikiConstants
    {
        public const string WikiMainPageUrl = "https://imago-rp.de/wiki/Hauptseite";
        public const string WikiUrlPrefix = "https://imago-rp.de/wiki/";

        public const string ArmorUrl = "https://imago-rp.de/wiki/R%C3%BCstungen";
        public const string MeleeWeaponUrl = "https://imago-rp.de/wiki/Nahkampfwaffen";
        public const string RangedWeaponUrl = "https://imago-rp.de/wiki/Fernkampfwaffen";
        public const string SpecialWeaponUrl = "https://imago-rp.de/wiki/Spezialwaffen";
        public const string ShieldsUrl = "https://imago-rp.de/wiki/Schilde";
        public const string WeaveTalentUrl = "https://imago-rp.de/wiki/Webk%C3%BCnste_(App-Tabelle)";

        public const string HealingUrl = "https://imago-rp.de/wiki/Heilung";
        public const string DailyGoodsUrl = "https://imago-rp.de/wiki/Alltagsg%C3%BCter";
        public const string AmmunitionUrl = "https://imago-rp.de/wiki/Munition";

        public const string ChangelogUrl = "https://github.com/christophergoltz/imago-app/blob/develop/CHANGELOG.md";
        public const string RoadmapUrl = "https://github.com/christophergoltz/imago-app/blob/develop/ROADMAP.md";
        public const string ImportantNotesUrl = "https://github.com/christophergoltz/imago-app/wiki/Wichtige-Hinweise";

        public static readonly Dictionary<SkillGroupModelType, string> SkillGroupTypeLookUp = new Dictionary<SkillGroupModelType, string>()
            {
                {SkillGroupModelType.Bewegung, "https://imago-rp.de/wiki/Bewegung_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Nahkampf, "https://imago-rp.de/wiki/Nahkampf_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Heimlichkeit, "https://imago-rp.de/wiki/Heimlichkeit_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Fernkampf, "https://imago-rp.de/wiki/Fernkampf_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Webkunst, "https://imago-rp.de/wiki/Weben_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Wissenschaft, "https://imago-rp.de/wiki/Wissenschaft_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Handwerk, "https://imago-rp.de/wiki/Handwerk_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Soziales, "https://imago-rp.de/wiki/Soziales_(Fertigkeitsgruppe)"}
            };

        public static readonly Dictionary<SkillModelType, string> SkillTypeLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Alchemie, "https://imago-rp.de/wiki/Alchemie_(Fertigkeit)"},
            {SkillModelType.Anatomie, "https://imago-rp.de/wiki/Anatomie_(Fertigkeit)"},
            {SkillModelType.Anfuehren, "https://imago-rp.de/wiki/Anf%C3%BChren_(Fertigkeit)"},
            {SkillModelType.Armbrueste, "https://imago-rp.de/wiki/Armbr%C3%BCste_(Fertigkeit)"},
            {SkillModelType.Ausdruck, "https://imago-rp.de/wiki/Ausdruck_(Fertigkeit)"},
            {SkillModelType.Ausweichen, "https://imago-rp.de/wiki/Ausweichen_(Fertigkeit)"},
            {SkillModelType.Bewusstsein, "https://imago-rp.de/wiki/Bewusstsein"},
            {SkillModelType.Blasrohre, "https://imago-rp.de/wiki/Blasrohr_(Fertigkeit)"},
            {SkillModelType.Boegen, "https://imago-rp.de/wiki/B%C3%B6gen_(Fertigkeit)"},
            {SkillModelType.Chaos, "https://imago-rp.de/wiki/Chaos"},
            {SkillModelType.Dolche, "https://imago-rp.de/wiki/Dolche_(Fertigkeit)"},
            {SkillModelType.Einfalt, "https://imago-rp.de/wiki/Einfalt"},
            {SkillModelType.Einschuechtern, "https://imago-rp.de/wiki/Einsch%C3%BCchtern_(Fertigkeit)"},
            {SkillModelType.Ekstase, "https://imago-rp.de/wiki/Ekstase"},
            {SkillModelType.Empathie, "https://imago-rp.de/wiki/Empathie_(Fertigkeit)"},
            {SkillModelType.Gesellschafter, "https://imago-rp.de/wiki/Gesellschafter_(Fertigkeit)"},
            {SkillModelType.Hiebwaffen, "https://imago-rp.de/wiki/Hiebwaffen_(Fertigkeit)"},
            {SkillModelType.Heiler, "https://imago-rp.de/wiki/Heiler_(Fertigkeit)"},
            {SkillModelType.Klettern, "https://imago-rp.de/wiki/Klettern_(Fertigkeit)"},
            {SkillModelType.Koerperbeherrschung, "https://imago-rp.de/wiki/K%C3%B6rperbeherrschung_(Fertigkeit)"},
            {SkillModelType.Kontrolle, "https://imago-rp.de/wiki/Kontrolle"},
            {SkillModelType.Laufen, "https://imago-rp.de/wiki/Laufen_(Fertigkeit)"},
            {SkillModelType.Literatur, "https://imago-rp.de/wiki/Literatur_(Fertigkeit)"},
            {SkillModelType.Leere, "https://imago-rp.de/wiki/Leere"},
            {SkillModelType.Manipulation, "https://imago-rp.de/wiki/Manipulation_(Fertigkeit)"},
            {SkillModelType.Materie, "https://imago-rp.de/wiki/Materie"},
            {SkillModelType.Mathematik, "https://imago-rp.de/wiki/Mathematik_(Fertigkeit)"},
            {SkillModelType.Naturkunde, "https://imago-rp.de/wiki/Naturkunde_(Fertigkeit)"},
            {SkillModelType.Philosophie, "https://imago-rp.de/wiki/Philosophie_(Fertigkeit)"},
            {SkillModelType.Physik, "https://imago-rp.de/wiki/Physik_(Fertigkeit)"},
            {SkillModelType.Reiten, "https://imago-rp.de/wiki/Reiten_(Fertigkeit)"},
            {SkillModelType.Schilde, "https://imago-rp.de/wiki/Schild_(Fertigkeit)"},
            {SkillModelType.Schleichen, "https://imago-rp.de/wiki/Schleichen_(Fertigkeit)"},
            {SkillModelType.Schleuder, "https://imago-rp.de/wiki/Schleudern_(Fertigkeit)"},
            {SkillModelType.Schwerter, "https://imago-rp.de/wiki/Schwerter_(Fertigkeit)"},
            {SkillModelType.Schwimmen, "https://imago-rp.de/wiki/Schwimmen_(Fertigkeit)"},
            {SkillModelType.Sicherheit, "https://imago-rp.de/wiki/Sicherheit_(Fertigkeit)"},
            {SkillModelType.SozialeAdaption, "https://imago-rp.de/wiki/Soziale_Adaption_(Fertigkeit)"},
            {SkillModelType.Soziologie, "https://imago-rp.de/wiki/Soziologie_(Fertigkeit)"},
            {SkillModelType.Sphaerologie, "https://imago-rp.de/wiki/Sph%C3%A4rologie_(Fertigkeit)"},
            {SkillModelType.Sprache, "https://imago-rp.de/wiki/Sprachen_(Fertigkeit)"},
            {SkillModelType.Springen, "https://imago-rp.de/wiki/Springen_(Fertigkeit)"},
            {SkillModelType.SpurenLesen, "https://imago-rp.de/wiki/Spuren_lesen_(Fertigkeit)"},
            {SkillModelType.StaebeSpeere, "https://imago-rp.de/wiki/St%C3%A4be/Speere_(Fertigkeit)"},
            {SkillModelType.Strategie, "https://imago-rp.de/wiki/Strategie_(Fertigkeit)"},
            {SkillModelType.Struktur, "https://imago-rp.de/wiki/Struktur"},
            {SkillModelType.Tanzen, "https://imago-rp.de/wiki/Tanzen_(Fertigkeit)"},
            {SkillModelType.Taschendiebstahl, "https://imago-rp.de/wiki/Taschendiebstahl_(Fertigkeit)"},
            {SkillModelType.Verbergen, "https://imago-rp.de/wiki/Verbergen_(Fertigkeit)"},
            {SkillModelType.Verkleiden, "https://imago-rp.de/wiki/Verkleiden_(Fertigkeit)"},
            {SkillModelType.Verstecken, "https://imago-rp.de/wiki/Verstecken_(Fertigkeit)"},
            {SkillModelType.Waffenlos, "https://imago-rp.de/wiki/Waffenlos_(Fertigkeit)"},
            {SkillModelType.WirtschaftRecht, "https://imago-rp.de/wiki/Wirtschaft/Recht_(Fertigkeit)"},
            {SkillModelType.Wurfgeschosse, "https://imago-rp.de/wiki/Wurfgeschosse_(Fertigkeit)"},
            {SkillModelType.Wurfwaffen, "https://imago-rp.de/wiki/Wurfwaffen_(Fertigkeit)"},
            {SkillModelType.Wundscher, "https://imago-rp.de/wiki/Wundscher_(Fertigkeit)"},
            {SkillModelType.Zweihaender, "https://imago-rp.de/wiki/Zweih%C3%A4nder_(Fertigkeit)"}
        };


        public static readonly Dictionary<SkillModelType, string> ParsableWeaveTalentLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Chaos, "https://imago-rp.de/wiki/Chaos"},
            {SkillModelType.Struktur, "https://imago-rp.de/wiki/Struktur"},
            {SkillModelType.Leere, "https://imago-rp.de/wiki/Leere"},
            {SkillModelType.Materie, "https://imago-rp.de/wiki/Materie"},
            {SkillModelType.Einfalt, "https://imago-rp.de/wiki/Einfalt"},
            {SkillModelType.Bewusstsein, "https://imago-rp.de/wiki/Bewusstsein"},
            {SkillModelType.Kontrolle, "https://imago-rp.de/wiki/Kontrolle"},
            {SkillModelType.Ekstase, "https://imago-rp.de/wiki/Ekstase"},
        };

        public static readonly Dictionary<SkillModelType, string> ParsableSkillTypeLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Armbrueste, "https://imago-rp.de/wiki/Armbr%C3%BCste_(Fertigkeit)"},
            {SkillModelType.Blasrohre, "https://imago-rp.de/wiki/Blasrohr_(Fertigkeit)"},
            {SkillModelType.Boegen, "https://imago-rp.de/wiki/B%C3%B6gen_(Fertigkeit)"},
            {SkillModelType.Dolche, "https://imago-rp.de/wiki/Dolche_(Fertigkeit)"},
            {SkillModelType.Hiebwaffen, "https://imago-rp.de/wiki/Hiebwaffen_(Fertigkeit)"},
            {SkillModelType.Schilde, "https://imago-rp.de/wiki/Schild_(Fertigkeit)"},
            {SkillModelType.Schleuder, "https://imago-rp.de/wiki/Schleudern_(Fertigkeit)"},
            {SkillModelType.Schwerter, "https://imago-rp.de/wiki/Schwerter_(Fertigkeit)"},
            {SkillModelType.StaebeSpeere, "https://imago-rp.de/wiki/St%C3%A4be/Speere_(Fertigkeit)"},
            {SkillModelType.Waffenlos, "https://imago-rp.de/wiki/Waffenlos_(Fertigkeit)"},
            {SkillModelType.Wurfwaffen, "https://imago-rp.de/wiki/Wurfwaffen_(Fertigkeit)"},
            {SkillModelType.Zweihaender, "https://imago-rp.de/wiki/Zweih%C3%A4nder_(Fertigkeit)"}
        };
    }
}
