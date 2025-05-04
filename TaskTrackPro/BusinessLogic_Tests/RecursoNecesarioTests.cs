using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class RecursoNecesarioTests
{
    private static readonly Recurso RECURSO_VALIDO = new Recurso("Computadora", "tipo", "desripcion", false, 8);

    [TestMethod]
    public void Constructor_ValoresValidos_SeAsignaCorrectamente()
    {
        var recursoNecesario = new RecursoNecesario(RECURSO_VALIDO, 2);

        Assert.AreEqual(RECURSO_VALIDO, recursoNecesario.Recurso);
        Assert.AreEqual(2, recursoNecesario.CantidadNecesaria);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Constructor_RecursoNulo_DeberiaLanzarExcepcion()
    {
        var recursoNecesario = new RecursoNecesario(null, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_CantidadMenorIgualA0_DeberiaLanzarExcepcion()
    {
        var recursoNecesario = new RecursoNecesario(RECURSO_VALIDO, 0);
    }
}