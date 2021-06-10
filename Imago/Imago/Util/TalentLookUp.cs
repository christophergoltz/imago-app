using System.Collections.Generic;
using Imago.Models;

namespace Imago.Util
{
    public static class TalentLookUp
    {
        public static Dictionary<SkillType, string> WikiTalentLookUp = new Dictionary<SkillType, string>()
        {
            {SkillType.Alchemie, "http://imago-rp.de/index.php/Alchemie"},
            {SkillType.Anatomie, "http://imago-rp.de/index.php/Anatomie"},
            {SkillType.Anfuehren, "http://imago-rp.de/index.php/Anf%C3%BChren"},
            {SkillType.Armbrueste, "http://imago-rp.de/index.php/Armbr%C3%BCste_(Fertigkeit)"},
            {SkillType.Ausdruck, "http://imago-rp.de/index.php/Ausdruck"},
            {SkillType.Ausweichen, "http://imago-rp.de/index.php/Ausweichen"},
            {SkillType.Bewusstsein, "http://imago-rp.de/index.php/Bewusstsein"},
            {SkillType.Blasrohre, "http://imago-rp.de/index.php/Blasrohre"},
            {SkillType.Boegen, "http://imago-rp.de/index.php/B%C3%B6gen_(Fertigkeit)"},
            {SkillType.Chaos, "http://imago-rp.de/index.php/Chaos"},
            {SkillType.Dolche, "http://imago-rp.de/index.php/Blasrohr_(Fertigkeit)"},
            {SkillType.Einfalt, "http://imago-rp.de/index.php/Einfalt"},
            {SkillType.Einschuechtern, "http://imago-rp.de/index.php/Einsch%C3%BCchtern"},
            {SkillType.Ekstase, "http://imago-rp.de/index.php/Ekstase"},
            {SkillType.Empathie, "http://imago-rp.de/index.php/Empathie"},
            {SkillType.Gesellschafter, "http://imago-rp.de/index.php/Gesellschafter"},
            {SkillType.Hiebwaffen, "http://imago-rp.de/index.php/Hiebwaffen_(Fertigkeit)"},
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
            {SkillType.Schilde, "http://imago-rp.de/index.php/Schild_(Fertigkeit)"},
            {SkillType.Schleichen, "http://imago-rp.de/index.php/Schleichen"},
            {SkillType.Schleuder, "http://imago-rp.de/index.php/Schleudern_(Fertigkeit)"},
            {SkillType.Schwerter, "http://imago-rp.de/index.php/Schwerter_(Fertigkeit)"},
            {SkillType.Schwimmen, "http://imago-rp.de/index.php/Schwimmen"},
            {SkillType.Sicherheit, "http://imago-rp.de/index.php/Sicherheit"},
            {SkillType.SozialeAdaption, "http://imago-rp.de/index.php/Soziale_Adaption"},
            {SkillType.Soziologie, "http://imago-rp.de/index.php/Soziologie"},
            {SkillType.Sphaerologie, "http://imago-rp.de/index.php/Sph%C3%A4rologie"},
            {SkillType.Sprache, "http://imago-rp.de/index.php/Sprachen"},
            {SkillType.Springen, "http://imago-rp.de/index.php/Springen"},
            {SkillType.SpurenLesen, "http://imago-rp.de/index.php/Spuren_lesen"},
            {SkillType.StaebeSpeere, "http://imago-rp.de/index.php/St%C3%A4be/Speere_(Fertigkeit)"},
            {SkillType.Strategie, "http://imago-rp.de/index.php/Strategie"},
            {SkillType.Struktur, "http://imago-rp.de/index.php/Struktur"},
            {SkillType.Tanzen, "http://imago-rp.de/index.php/Tanzen"},
            {SkillType.Taschendiebstahl, "http://imago-rp.de/index.php/Taschendiebstahl"},
            {SkillType.Verbergen, "http://imago-rp.de/index.php/Verbergen"},
            {SkillType.Verkleiden, "http://imago-rp.de/index.php/Verkleiden"},
            {SkillType.Verstecken, "http://imago-rp.de/index.php/Verstecken"},
            {SkillType.Waffenlos, "http://imago-rp.de/index.php/Waffenlos_(Fertigkeit)"},
            {SkillType.WirtschaftRecht, "http://imago-rp.de/index.php/Wirtschaft/Recht"},
            {SkillType.Wurfgeschosse, "http://imago-rp.de/index.php/Wurfgeschosse"},
            {SkillType.Wurfwaffen, "http://imago-rp.de/index.php/Wurfwaffen"},
            {SkillType.Wundscher, "http://imago-rp.de/index.php/Wundscher"},
            {SkillType.Zweihaender, "http://imago-rp.de/index.php/Zweih%C3%A4nder_(Fertigkeit)"}
        };
    }
}