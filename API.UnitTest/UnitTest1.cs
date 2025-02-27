namespace API.UnitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        //Arrange, ponemos toda la configuracion necesaria para una prueba, como inicializar variables, URLs, etc. 

        //Act, vamos a tener nuestra función

        //Assert, vamos a evaluar la prueba
        //Esto significa error, es decir, se esperaba un 1, pero se obtuvo un 0
        // Assert.Equal(1, 0);
        //Esto significa que paso la prueba, es decir, se esperaba un 1, pero se obtuvo un 1
        // Assert.Equal(1, 1);

    }
}
// reportgenerator -reports:"TestResults\4bdee2c4-fd40-441d-9fee-eefc4ed3892a\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html