# ConsumerDrivenContract.Net

In [Microservices](https://martinfowler.com/articles/microservices.html) World or any [Distributed System](https://en.wikipedia.org/wiki/Distributed_computing) we have **APIs** which consumed by many out of control **Consumers**.
Having a good and successful [Integration Tests](https://martinfowler.com/bliki/IntegrationTest.html) set is essential for ensuring that all providers and consumers can understand each other.

## Test Pyramid

Test Pyramid is a metaphor used to categorise all tests based on some specifications.
We have three test level:
* Unit Test
* Integration Test
* End-to-End Tests

***
Unit tests are test that trying to ensure that all unit peace of software acts as expected. At this level we test software piece in isolation manner without dealing with wiring of software pieces. Unit tests usally replace all dependencies with [Test Doubles](https://martinfowler.com/bliki/TestDouble.html)

![Unit Test](https://martinfowler.com/articles/practical-test-pyramid/unitTest.png)

***
Certainly, any software; deals with other sections, including the database, file, etc. When we write __unite tests__ we ignore this by using __test doubles__ . [Integration Test](https://martinfowler.com/bliki/IntegrationTest.html) tries to make sure that the software pieces(__which tested in isolation using unit test__) work together correctly. 

![Integration Testing](https://martinfowler.com/bliki/images/integrationTesting/sketch.png)

***

End-to-End test are write against the production version of the software(in production or production-like environments). Actually E2E tests, test the software against its interface.
![E2E tests](https://martinfowler.com/articles/practical-test-pyramid/e2etests.png)
