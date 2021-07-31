using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class AttributeCalculationServiceTests
    {
        //[Theory]
        //[ClassData(typeof(AttributeIncreaseEnoughExperienceTestData))]
        //public void GibEpAnAttribut_AttributHasGenugEp_SteigerungWertInc(int aktuellerSw, int verfuegbareEp,
        //    int erwarteterMinimalerSw)
        //{
        //    var testAttr = ImagoEntitaetFactory.GetNewEntitaet(ImagoAttribut.Staerke);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    //attr is null
        //    var attr = spieler.Attribute.FirstOrDefault(a => a.Identifier.Equals(testAttr));
        //    attr.SteigerungsWert = aktuellerSw;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnAttribut((ImagoAttribut)testAttr.Identifier, verfuegbareEp);
        //    var result = attr.SteigerungsWert;

        //    Assert.True(erwarteterMinimalerSw <= result);
        //}

        //[Theory]
        //[ClassData(typeof(AttributeIncreaseNotEnoughExperienceTestData))]
        //public void GibEpAnAttribut_AttributHasNotGenugEp_SteigerungWertNoInc(int aktuellerSw, int verfuegbareEp,
        //    int erwarteterSw)
        //{
        //    var testAttr = ImagoEntitaetFactory.GetNewEntitaet(ImagoAttribut.Staerke);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    var attr = spieler.Attribute.FirstOrDefault(a => a.Identifier.Equals(testAttr));
        //    attr.SteigerungsWert = aktuellerSw;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnAttribut((ImagoAttribut)testAttr.Identifier, verfuegbareEp);
        //    var result = attr.SteigerungsWert;

        //    Assert.Equal(erwarteterSw, result);
        //}

        //[Theory]
        //[ClassData(typeof(AttributeIncreaseMultipleIncreasesTestData))]
        //public void GibEpAnAttribut_AttributHasGenugEpFuerMehrAlsEineSteigerung_SteigerungsWertKonkret(
        //    int aktuellerSteigerungsWert, int verfuegbareEp, int erwarteterSteigerungsWert)
        //{
        //    var testAttr = ImagoEntitaetFactory.GetNewEntitaet(ImagoAttribut.Staerke);

        //    var spieler = ImagoEntitaetFactory.CreateSpieler();
        //    var attr = spieler.Attribute.FirstOrDefault(a => a.Identifier.Equals(testAttr));
        //    attr.SteigerungsWert = aktuellerSteigerungsWert;

        //    var service = new SpielerService(spieler);
        //    service.GibEpAnAttribut((ImagoAttribut)testAttr.Identifier, verfuegbareEp);
        //    var result = attr.SteigerungsWert;

        //    Assert.Equal(erwarteterSteigerungsWert, result);
        //}
    }
}
