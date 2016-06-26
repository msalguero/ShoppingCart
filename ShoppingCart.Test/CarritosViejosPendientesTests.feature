Feature: CarritosViejosPendientesTests
	Revisar los carritos viejos pendientes

Scenario: Revisar carritos viejos pendientes
Given una lista de carritos pendientes y pagados
| Id | Usuario   | Estado   | FechaDeCreacion |
| 1  | victor    | sinPagar | 25/06/2016      |
| 2  | ricardo   | sinPagar | 25/06/2016      |
| 3  | sebastian | pagado   | 25/06/2016      |
| 3  | edwin     | pagado   | 25/06/2016      |
Given el usuario victor
When Al ejecutar el reporte de carritos pendientes
Then devolvera los carritos que tengan mas de un mes de creados y que aun esten pendientes
