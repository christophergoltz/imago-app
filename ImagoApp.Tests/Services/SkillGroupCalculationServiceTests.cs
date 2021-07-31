using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class SkillGroupCalculationServiceTests
    {
        //[Theory]
        //[ClassData(typeof(SkillGroupIncreaseEnoughExperienceTestData))]
        //public void GibEpAnFertigkeitsKategorie_FertigkeitsKategorieHasGenugEp_SteigerungWertInc(
        //       int aktuellerSteigerungsWert, int verfuegbareEp, int erwarteterMinimalerSteigerungsWert)
        //{
        //    var testFert = ImagoEntitaetFactory.GetNewEntitaet(ImagoFertigkeitsKategorie.Soziales);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    var kat = spieler.SozialesKategorie;
        //    kat.SteigerungsWert = aktuellerSteigerungsWert;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnFertigkeitsKategorie((ImagoFertigkeitsKategorie)testFert.Identifier, verfuegbareEp);
        //    var result = kat.SteigerungsWert;

        //    Assert.True(erwarteterMinimalerSteigerungsWert <= result);
        //}

        //[Theory]
        //[ClassData(typeof(SkillGroupIncreaseNotEnoughExperienceTestData))]
        //public void GibEpAnFertigkeitsKategorie_FertigkeitsKategorieHasNotGenugEp_SteigerungWertNoInc(
        //    int aktuellerSteigerungsWert, int verfuegbareEp, int erwarteterSteigerungsWert)
        //{
        //    var testFert = ImagoEntitaetFactory.GetNewEntitaet(ImagoFertigkeitsKategorie.Soziales);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    var kat = spieler.SozialesKategorie;
        //    kat.SteigerungsWert = aktuellerSteigerungsWert;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnFertigkeitsKategorie((ImagoFertigkeitsKategorie)testFert.Identifier, verfuegbareEp);
        //    var result = kat.SteigerungsWert;

        //    Assert.Equal(erwarteterSteigerungsWert, result);
        //}

        //[Theory]
        //[ClassData(typeof(SkillGroupIncreaseMultipleIncreasesTestData))]
        //public void
        //    GibEpAnFertigkeitsKategorie_FertigkeitsKategorieHasGenugEpFuerMehrAlsEineSteigerung_SteigerungsWertKonkret(
        //        int aktuellerSteigerungsWert, int verfuegbareEp, int erwarteterSteigerungsWert)
        //{
        //    var testFert = ImagoEntitaetFactory.GetNewEntitaet(ImagoFertigkeitsKategorie.Soziales);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    var kat = spieler.SozialesKategorie;
        //    kat.SteigerungsWert = aktuellerSteigerungsWert;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnFertigkeitsKategorie((ImagoFertigkeitsKategorie)testFert.Identifier, verfuegbareEp);
        //    var result = kat.SteigerungsWert;

        //    Assert.Equal(erwarteterSteigerungsWert, result);
        //}

    }
}
