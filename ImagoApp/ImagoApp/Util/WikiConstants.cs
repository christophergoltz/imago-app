using System.Collections.Generic;

namespace ImagoApp.Util
{
    public static class WikiConstants
    {
        public static readonly string WikiMainPageUrl = "http://imago-rp.de/index.php/Hauptseite";
        public static readonly string WikiUrlPrefix = "http://imago-rp.de/index.php/";

        public static readonly string ArmorUrl = "http://imago-rp.de/index.php/R%C3%BCstungen";
        public static readonly string MeleeWeaponUrl = "http://imago-rp.de/index.php/Nahkampfwaffen";
        public static readonly string RangedWeaponUrl = "http://imago-rp.de/index.php/Fernkampfwaffen";
        public static readonly string SpecialWeaponUrl = "http://imago-rp.de/index.php/Spezialwaffen";
        public static readonly string ShieldsUrl = "http://imago-rp.de/index.php/Schilde";

        public static readonly string ChangelogUrl = "http://imago-rp.de/index.php/Imago.App_(Changelog)";

        public static readonly Dictionary<Models.Enum.SkillGroupModelType, string> SkillGroupTypeLookUp = new Dictionary<Models.Enum.SkillGroupModelType, string>()
            {
                {Models.Enum.SkillGroupModelType.Bewegung, "http://imago-rp.de/index.php/Bewegung_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Nahkampf, "http://imago-rp.de/index.php/Nahkampf_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Heimlichkeit, "http://imago-rp.de/index.php/Heimlichkeit_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Fernkampf, "http://imago-rp.de/index.php/Fernkampf_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Webkunst, "http://imago-rp.de/index.php/Weben_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Wissenschaft, "http://imago-rp.de/index.php/Wissenschaft_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Handwerk, "http://imago-rp.de/index.php/Handwerk_(Fertigkeitsgruppe)"},
                {Models.Enum.SkillGroupModelType.Soziales, "http://imago-rp.de/index.php/Soziales_(Fertigkeitsgruppe)"}
            };

        public static readonly Dictionary<Models.SkillModelType, string> SkillTypeLookUp = new Dictionary<Models.SkillModelType, string>()
        {
            {Models.SkillModelType.Alchemie, "http://imago-rp.de/index.php/Alchemie_(Fertigkeit)"},
            {Models.SkillModelType.Anatomie, "http://imago-rp.de/index.php/Anatomie_(Fertigkeit)"},
            {Models.SkillModelType.Anfuehren, "http://imago-rp.de/index.php/Anf%C3%BChren_(Fertigkeit)"},
            {Models.SkillModelType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Fertigkeit)"},
            {Models.SkillModelType.Ausdruck, "http://imago-rp.de/index.php/Ausdruck_(Fertigkeit)"},
            {Models.SkillModelType.Ausweichen, "http://imago-rp.de/index.php/Ausweichen_(Fertigkeit)"},
            {Models.SkillModelType.Bewusstsein, "http://imago-rp.de/index.php/Bewusstsein"},
            {Models.SkillModelType.Blasrohre, "http://imago-rp.de/index.php/Blasrohr_(Fertigkeit)"},
            {Models.SkillModelType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Fertigkeit)"},
            {Models.SkillModelType.Chaos, "http://imago-rp.de/index.php/Chaos"},
            {Models.SkillModelType.Dolche, "http://imago-rp.de/index.php/Dolche_(Fertigkeit)"},
            {Models.SkillModelType.Einfalt, "http://imago-rp.de/index.php/Einfalt"},
            {Models.SkillModelType.Einschuechtern, "http://imago-rp.de/index.php/Einsch%C3%BCchtern_(Fertigkeit)"},
            {Models.SkillModelType.Ekstase, "http://imago-rp.de/index.php/Ekstase"},
            {Models.SkillModelType.Empathie, "http://imago-rp.de/index.php/Empathie_(Fertigkeit)"},
            {Models.SkillModelType.Gesellschafter, "http://imago-rp.de/index.php/Gesellschafter_(Fertigkeit)"},
            {Models.SkillModelType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Fertigkeit)"},
            {Models.SkillModelType.Heiler, "http://imago-rp.de/index.php/Heiler_(Fertigkeit)"},
            {Models.SkillModelType.Klettern, "http://imago-rp.de/index.php/Klettern_(Fertigkeit)"},
            {Models.SkillModelType.Koerperbeherrschung, "http://imago-rp.de/index.php/K%C3%B6rperbeherrschung_(Fertigkeit)"},
            {Models.SkillModelType.Kontrolle, "http://imago-rp.de/index.php/Kontrolle"},
            {Models.SkillModelType.Laufen, "http://imago-rp.de/index.php/Laufen_(Fertigkeit)"},
            {Models.SkillModelType.Literatur, "http://imago-rp.de/index.php/Literatur_(Fertigkeit)"},
            {Models.SkillModelType.Leere, "http://imago-rp.de/index.php/Leere"},
            {Models.SkillModelType.Manipulation, "http://imago-rp.de/index.php/Manipulation_(Fertigkeit)"},
            {Models.SkillModelType.Materie, "http://imago-rp.de/index.php/Materie"},
            {Models.SkillModelType.Mathematik, "http://imago-rp.de/index.php/Mathematik_(Fertigkeit)"},
            {Models.SkillModelType.Naturkunde, "http://imago-rp.de/index.php/Naturkunde_(Fertigkeit)"},
            {Models.SkillModelType.Philosophie, "http://imago-rp.de/index.php/Philosophie_(Fertigkeit)"},
            {Models.SkillModelType.Physik, "http://imago-rp.de/index.php/Physik_(Fertigkeit)"},
            {Models.SkillModelType.Reiten, "http://imago-rp.de/index.php/Reiten_(Fertigkeit)"},
            {Models.SkillModelType.Schilde, "http://imago-rp.de/index.php/Schild_(Fertigkeit)"},
            {Models.SkillModelType.Schleichen, "http://imago-rp.de/index.php/Schleichen_(Fertigkeit)"},
            {Models.SkillModelType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Fertigkeit)"},
            {Models.SkillModelType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Fertigkeit)"},
            {Models.SkillModelType.Schwimmen, "http://imago-rp.de/index.php/Schwimmen_(Fertigkeit)"},
            {Models.SkillModelType.Sicherheit, "http://imago-rp.de/index.php/Sicherheit_(Fertigkeit)"},
            {Models.SkillModelType.SozialeAdaption, "http://imago-rp.de/index.php/Soziale_Adaption_(Fertigkeit)"},
            {Models.SkillModelType.Soziologie, "http://imago-rp.de/index.php/Soziologie_(Fertigkeit)"},
            {Models.SkillModelType.Sphaerologie, "http://imago-rp.de/index.php/Sph%C3%A4rologie_(Fertigkeit)"},
            {Models.SkillModelType.Sprache, "http://imago-rp.de/index.php/Sprachen_(Fertigkeit)"},
            {Models.SkillModelType.Springen, "http://imago-rp.de/index.php/Springen_(Fertigkeit)"},
            {Models.SkillModelType.SpurenLesen, "http://imago-rp.de/index.php/Spuren_lesen_(Fertigkeit)"},
            {Models.SkillModelType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Fertigkeit)"},
            {Models.SkillModelType.Strategie, "http://imago-rp.de/index.php/Strategie_(Fertigkeit)"},
            {Models.SkillModelType.Struktur, "http://imago-rp.de/index.php/Struktur"},
            {Models.SkillModelType.Tanzen, "http://imago-rp.de/index.php/Tanzen_(Fertigkeit)"},
            {Models.SkillModelType.Taschendiebstahl, "http://imago-rp.de/index.php/Taschendiebstahl_(Fertigkeit)"},
            {Models.SkillModelType.Verbergen, "http://imago-rp.de/index.php/Verbergen_(Fertigkeit)"},
            {Models.SkillModelType.Verkleiden, "http://imago-rp.de/index.php/Verkleiden_(Fertigkeit)"},
            {Models.SkillModelType.Verstecken, "http://imago-rp.de/index.php/Verstecken_(Fertigkeit)"},
            {Models.SkillModelType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Fertigkeit)"},
            {Models.SkillModelType.WirtschaftRecht, "http://imago-rp.de/index.php/Wirtschaft/Recht_(Fertigkeit)"},
            {Models.SkillModelType.Wurfgeschosse, "http://imago-rp.de/index.php/Wurfgeschosse_(Fertigkeit)"},
            {Models.SkillModelType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen_(Fertigkeit)"},
            {Models.SkillModelType.Wundscher, "http://imago-rp.de/index.php/Wundscher_(Fertigkeit)"},
            {Models.SkillModelType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Fertigkeit)"}
        };

        public static readonly Dictionary<Models.SkillModelType, string> ParsableSkillTypeLookUp = new Dictionary<Models.SkillModelType, string>()
        {
            {Models.SkillModelType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Fertigkeit)"},
            {Models.SkillModelType.Blasrohre, "http://imago-rp.de/index.php/Blasrohr_(Fertigkeit)"},
            {Models.SkillModelType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Fertigkeit)"},
            {Models.SkillModelType.Dolche, "http://imago-rp.de/index.php/Dolche_(Fertigkeit)"},
            {Models.SkillModelType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Fertigkeit)"},
            {Models.SkillModelType.Schilde, "http://imago-rp.de/index.php/Schild_(Fertigkeit)"},
            {Models.SkillModelType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Fertigkeit)"},
            {Models.SkillModelType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Fertigkeit)"},
            {Models.SkillModelType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Fertigkeit)"},
            {Models.SkillModelType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Fertigkeit)"},
            {Models.SkillModelType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen_(Fertigkeit)"},
            {Models.SkillModelType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Fertigkeit)"}
        };
    }
}
