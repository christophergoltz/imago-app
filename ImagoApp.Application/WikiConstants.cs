using System.Collections.Generic;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application
{
    public static class WikiConstants
    {
        public const string WikiMainPageUrl = "http://imago-rp.de/index.php/Hauptseite";
        public const string WikiUrlPrefix = "http://imago-rp.de/index.php/";

        public const string ArmorUrl = "http://imago-rp.de/index.php/R%C3%BCstungen";
        public const string MeleeWeaponUrl = "http://imago-rp.de/index.php/Nahkampfwaffen";
        public const string RangedWeaponUrl = "http://imago-rp.de/index.php/Fernkampfwaffen";
        public const string SpecialWeaponUrl = "http://imago-rp.de/index.php/Spezialwaffen";
        public const string ShieldsUrl = "http://imago-rp.de/index.php/Schilde";

        public const string HealingUrl = "http://imago-rp.de/index.php/Heilung";
        public const string DailyGoodsUrl = "http://imago-rp.de/index.php/Alltagsg%C3%BCter";
        public const string AmmunitionUrl = "http://imago-rp.de/index.php/Munition";

        public const string ChangelogUrl = "https://github.com/christophergoltz/imago-app/blob/develop/CHANGELOG.md";
        public const string RoadmapUrl = "https://github.com/christophergoltz/imago-app/blob/develop/ROADMAP.md";
        public const string ImportantNotesUrl = "https://github.com/christophergoltz/imago-app/wiki/Wichtige-Hinweise";

        public static readonly Dictionary<SkillGroupModelType, string> SkillGroupTypeLookUp = new Dictionary<SkillGroupModelType, string>()
            {
                {SkillGroupModelType.Bewegung, "http://imago-rp.de/index.php/Bewegung_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Nahkampf, "http://imago-rp.de/index.php/Nahkampf_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Heimlichkeit, "http://imago-rp.de/index.php/Heimlichkeit_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Fernkampf, "http://imago-rp.de/index.php/Fernkampf_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Webkunst, "http://imago-rp.de/index.php/Weben_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Wissenschaft, "http://imago-rp.de/index.php/Wissenschaft_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Handwerk, "http://imago-rp.de/index.php/Handwerk_(Fertigkeitsgruppe)"},
                {SkillGroupModelType.Soziales, "http://imago-rp.de/index.php/Soziales_(Fertigkeitsgruppe)"}
            };

        public static readonly Dictionary<SkillModelType, string> SkillTypeLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Alchemie, "http://imago-rp.de/index.php/Alchemie_(Fertigkeit)"},
            {SkillModelType.Anatomie, "http://imago-rp.de/index.php/Anatomie_(Fertigkeit)"},
            {SkillModelType.Anfuehren, "http://imago-rp.de/index.php/Anf%C3%BChren_(Fertigkeit)"},
            {SkillModelType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Fertigkeit)"},
            {SkillModelType.Ausdruck, "http://imago-rp.de/index.php/Ausdruck_(Fertigkeit)"},
            {SkillModelType.Ausweichen, "http://imago-rp.de/index.php/Ausweichen_(Fertigkeit)"},
            {SkillModelType.Bewusstsein, "http://imago-rp.de/index.php/Bewusstsein"},
            {SkillModelType.Blasrohre, "http://imago-rp.de/index.php/Blasrohr_(Fertigkeit)"},
            {SkillModelType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Fertigkeit)"},
            {SkillModelType.Chaos, "http://imago-rp.de/index.php/Chaos"},
            {SkillModelType.Dolche, "http://imago-rp.de/index.php/Dolche_(Fertigkeit)"},
            {SkillModelType.Einfalt, "http://imago-rp.de/index.php/Einfalt"},
            {SkillModelType.Einschuechtern, "http://imago-rp.de/index.php/Einsch%C3%BCchtern_(Fertigkeit)"},
            {SkillModelType.Ekstase, "http://imago-rp.de/index.php/Ekstase"},
            {SkillModelType.Empathie, "http://imago-rp.de/index.php/Empathie_(Fertigkeit)"},
            {SkillModelType.Gesellschafter, "http://imago-rp.de/index.php/Gesellschafter_(Fertigkeit)"},
            {SkillModelType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Fertigkeit)"},
            {SkillModelType.Heiler, "http://imago-rp.de/index.php/Heiler_(Fertigkeit)"},
            {SkillModelType.Klettern, "http://imago-rp.de/index.php/Klettern_(Fertigkeit)"},
            {SkillModelType.Koerperbeherrschung, "http://imago-rp.de/index.php/K%C3%B6rperbeherrschung_(Fertigkeit)"},
            {SkillModelType.Kontrolle, "http://imago-rp.de/index.php/Kontrolle"},
            {SkillModelType.Laufen, "http://imago-rp.de/index.php/Laufen_(Fertigkeit)"},
            {SkillModelType.Literatur, "http://imago-rp.de/index.php/Literatur_(Fertigkeit)"},
            {SkillModelType.Leere, "http://imago-rp.de/index.php/Leere"},
            {SkillModelType.Manipulation, "http://imago-rp.de/index.php/Manipulation_(Fertigkeit)"},
            {SkillModelType.Materie, "http://imago-rp.de/index.php/Materie"},
            {SkillModelType.Mathematik, "http://imago-rp.de/index.php/Mathematik_(Fertigkeit)"},
            {SkillModelType.Naturkunde, "http://imago-rp.de/index.php/Naturkunde_(Fertigkeit)"},
            {SkillModelType.Philosophie, "http://imago-rp.de/index.php/Philosophie_(Fertigkeit)"},
            {SkillModelType.Physik, "http://imago-rp.de/index.php/Physik_(Fertigkeit)"},
            {SkillModelType.Reiten, "http://imago-rp.de/index.php/Reiten_(Fertigkeit)"},
            {SkillModelType.Schilde, "http://imago-rp.de/index.php/Schild_(Fertigkeit)"},
            {SkillModelType.Schleichen, "http://imago-rp.de/index.php/Schleichen_(Fertigkeit)"},
            {SkillModelType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Fertigkeit)"},
            {SkillModelType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Fertigkeit)"},
            {SkillModelType.Schwimmen, "http://imago-rp.de/index.php/Schwimmen_(Fertigkeit)"},
            {SkillModelType.Sicherheit, "http://imago-rp.de/index.php/Sicherheit_(Fertigkeit)"},
            {SkillModelType.SozialeAdaption, "http://imago-rp.de/index.php/Soziale_Adaption_(Fertigkeit)"},
            {SkillModelType.Soziologie, "http://imago-rp.de/index.php/Soziologie_(Fertigkeit)"},
            {SkillModelType.Sphaerologie, "http://imago-rp.de/index.php/Sph%C3%A4rologie_(Fertigkeit)"},
            {SkillModelType.Sprache, "http://imago-rp.de/index.php/Sprachen_(Fertigkeit)"},
            {SkillModelType.Springen, "http://imago-rp.de/index.php/Springen_(Fertigkeit)"},
            {SkillModelType.SpurenLesen, "http://imago-rp.de/index.php/Spuren_lesen_(Fertigkeit)"},
            {SkillModelType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Fertigkeit)"},
            {SkillModelType.Strategie, "http://imago-rp.de/index.php/Strategie_(Fertigkeit)"},
            {SkillModelType.Struktur, "http://imago-rp.de/index.php/Struktur"},
            {SkillModelType.Tanzen, "http://imago-rp.de/index.php/Tanzen_(Fertigkeit)"},
            {SkillModelType.Taschendiebstahl, "http://imago-rp.de/index.php/Taschendiebstahl_(Fertigkeit)"},
            {SkillModelType.Verbergen, "http://imago-rp.de/index.php/Verbergen_(Fertigkeit)"},
            {SkillModelType.Verkleiden, "http://imago-rp.de/index.php/Verkleiden_(Fertigkeit)"},
            {SkillModelType.Verstecken, "http://imago-rp.de/index.php/Verstecken_(Fertigkeit)"},
            {SkillModelType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Fertigkeit)"},
            {SkillModelType.WirtschaftRecht, "http://imago-rp.de/index.php/Wirtschaft/Recht_(Fertigkeit)"},
            {SkillModelType.Wurfgeschosse, "http://imago-rp.de/index.php/Wurfgeschosse_(Fertigkeit)"},
            {SkillModelType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen_(Fertigkeit)"},
            {SkillModelType.Wundscher, "http://imago-rp.de/index.php/Wundscher_(Fertigkeit)"},
            {SkillModelType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Fertigkeit)"}
        };


        public static readonly Dictionary<SkillModelType, string> ParsableWeaveTalentLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Chaos, "http://imago-rp.de/index.php/Chaos"},
            {SkillModelType.Struktur, "http://imago-rp.de/index.php/Struktur"},
            {SkillModelType.Leere, "http://imago-rp.de/index.php/Leere"},
            {SkillModelType.Materie, "http://imago-rp.de/index.php/Materie"},
            {SkillModelType.Einfalt, "http://imago-rp.de/index.php/Einfalt"},
            {SkillModelType.Bewusstsein, "http://imago-rp.de/index.php/Bewusstsein"},
            {SkillModelType.Kontrolle, "http://imago-rp.de/index.php/Kontrolle"},
            {SkillModelType.Ekstase, "http://imago-rp.de/index.php/Ekstase"},
        };

        public static readonly Dictionary<SkillModelType, string> ParsableSkillTypeLookUp = new Dictionary<SkillModelType, string>()
        {
            {SkillModelType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Fertigkeit)"},
            {SkillModelType.Blasrohre, "http://imago-rp.de/index.php/Blasrohr_(Fertigkeit)"},
            {SkillModelType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Fertigkeit)"},
            {SkillModelType.Dolche, "http://imago-rp.de/index.php/Dolche_(Fertigkeit)"},
            {SkillModelType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Fertigkeit)"},
            {SkillModelType.Schilde, "http://imago-rp.de/index.php/Schild_(Fertigkeit)"},
            {SkillModelType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Fertigkeit)"},
            {SkillModelType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Fertigkeit)"},
            {SkillModelType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Fertigkeit)"},
            {SkillModelType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Fertigkeit)"},
            {SkillModelType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen_(Fertigkeit)"},
            {SkillModelType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Fertigkeit)"}
        };
    }
}
